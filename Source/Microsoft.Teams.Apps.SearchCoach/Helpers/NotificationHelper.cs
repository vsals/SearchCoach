// <copyright file="NotificationHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Common.BackgroundService;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Polly;
    using Polly.Contrib.WaitAndRetry;
    using Polly.Retry;

    /// <summary>
    /// Class that handles card create/update helper methods.
    /// </summary>
    public class NotificationHelper
    {
        /// <summary>
        /// Represents retry delay.
        /// </summary>
        private const int RetryDelay = 1000;

        /// <summary>
        /// Represents retry count.
        /// </summary>
        private const int RetryCount = 2;

        /// <summary>
        /// Retry policy with jitter, retry twice with a jitter delay of up to 1 sec. Retry for HTTP 429(transient error)/502 bad gateway.
        /// </summary>
        /// <remarks>
        /// Reference: https://github.com/Polly-Contrib/Polly.Contrib.WaitAndRetry#new-jitter-recommendation.
        /// </remarks>
        private readonly AsyncRetryPolicy retryPolicy = Policy.Handle<ErrorResponseException>(
            ex => ex.Response.StatusCode == HttpStatusCode.TooManyRequests || ex.Response.StatusCode == HttpStatusCode.BadGateway)
            .WaitAndRetryAsync(Backoff.DecorrelatedJitterBackoffV2(TimeSpan.FromMilliseconds(RetryDelay), RetryCount));

        /// <summary>
        /// Microsoft App credentials.
        /// </summary>
        private readonly MicrosoftAppCredentials microsoftAppCredentials;

        /// <summary>
        /// Used to run a background task using background service.
        /// </summary>
        private readonly BackgroundTaskWrapper backgroundTaskWrapper;

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<NotificationHelper> logger;

        /// <summary>
        /// Search Coach Bot adapter to get context.
        /// </summary>
        private readonly BotFrameworkAdapter botAdapter;

        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Provider to deal with user response details from storage table.
        /// </summary>
        private readonly IUserResponseStorageProvider userResponseProvider;

        /// <summary>
        /// The instance of user response mapper class to work with user response model.
        /// </summary>
        private readonly IUserResponseMapper userResponseMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationHelper"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="microsoftAppCredentials">Instance for Microsoft app credentials.</param>
        /// <param name="backgroundTaskWrapper">Instance of backgroundTaskWrapper to run a background task using IHostedService.</param>
        /// <param name="botAdapter">Search coach bot adapter.</param>
        /// <param name="botOptions">A set of key/value application configuration properties for activity handler.</param>
        /// <param name="userResponseProvider">Provider for fetching information about user response details from storage table.</param>
        /// <param name="userResponseMapper">The instance of user response mapper class to work with models.</param>
        public NotificationHelper(
            ILogger<NotificationHelper> logger,
            MicrosoftAppCredentials microsoftAppCredentials,
            BackgroundTaskWrapper backgroundTaskWrapper,
            BotFrameworkAdapter botAdapter,
            IOptions<BotSettings> botOptions,
            IUserResponseStorageProvider userResponseProvider,
            IUserResponseMapper userResponseMapper)
        {
            this.logger = logger;
            this.microsoftAppCredentials = microsoftAppCredentials;
            this.backgroundTaskWrapper = backgroundTaskWrapper;
            this.botAdapter = botAdapter;
            this.botOptions = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
            this.userResponseProvider = userResponseProvider;
            this.userResponseMapper = userResponseMapper;
        }

        /// <summary>
        /// Gets team member details and sends question card to members of the team.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="teamId">Team id to get members list.</param>
        /// <param name="servicePath">Service URL to send card.</param>
        /// <param name="questionId">Question id of currently send question card.</param>
        /// <param name="questionCard">Question card containing the question details.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns the task that sends question card to all members of the team.</returns>
        public async Task SendQuestionCardToTeamMembersAsync(
            ITurnContext<IInvokeActivity> turnContext,
            string teamId,
            string servicePath,
            string questionId,
            IMessageActivity questionCard,
            CancellationToken cancellationToken)
        {
            try
            {
                turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));

                var teamDetails = await this.GetTeamMembersAsync(teamId, turnContext.Activity.ServiceUrl);
                if (teamDetails == null)
                {
                    this.logger.LogError("No team members found.");
                    return;
                }

                // Send question card to all members of the team. Enqueue task to task wrapper and it will be executed by background service.
                this.backgroundTaskWrapper.Enqueue(this.SendQuestionCardToMembersAsync(
                    turnContext,
                    teamDetails,
                    questionCard,
                    servicePath,
                    cancellationToken));

                this.logger.LogInformation("Question quiz card sent successfully.");

                // Store all members of the team in storage after sending question card.
                this.backgroundTaskWrapper.Enqueue(this.UpsertAllTeamMembersAsync(turnContext, teamDetails, questionId));
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"An error occurred in {nameof(this.SendQuestionCardToTeamMembersAsync)}");
                throw;
            }
        }

        /// <summary>
        /// Method to send question card to members of the team.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="teamMembers">Team members of the team.</param>
        /// <param name="questionCard">Question card containing the question details.</param>
        /// <param name="serviceUrl">Service URL of team context to send card in chat.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>Returns a task to send question card to the team members.</returns>
        private async Task SendQuestionCardToMembersAsync(
            ITurnContext<IInvokeActivity> turnContext,
            IEnumerable<TeamsChannelAccount> teamMembers,
            IMessageActivity questionCard,
            string serviceUrl,
            CancellationToken cancellationToken)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            var teamsChannelId = turnContext.Activity.TeamsGetChannelId();
            var teamInfo = turnContext.Activity.TeamsGetTeamInfo();

            var credentials = new MicrosoftAppCredentials(this.microsoftAppCredentials.MicrosoftAppId, this.microsoftAppCredentials.MicrosoftAppPassword);
            ConversationReference conversationReference = null;

            foreach (var teamMember in teamMembers)
            {
                var conversationParameters = new ConversationParameters
                {
                    IsGroup = false,
                    Bot = turnContext.Activity.Recipient,
                    Members = new ChannelAccount[] { teamMember },
                    TenantId = turnContext.Activity.Conversation.TenantId,
                };

                try
                {
                    // Retry for HTTP 429(transient error) / 502 bad gateway.
                    await this.retryPolicy.ExecuteAsync(async () =>
                    {
                        // Creates a conversation on the specified channel and send notification card on that conversation.
                        await ((BotFrameworkAdapter)turnContext.Adapter).CreateConversationAsync(
                            teamsChannelId,
                            serviceUrl,
                            credentials,
                            conversationParameters,
                            async (conversationTurnContext, conversationCancellationToken) =>
                            {
                                conversationReference = conversationTurnContext.Activity.GetConversationReference();
                                await ((BotFrameworkAdapter)turnContext.Adapter).ContinueConversationAsync(
                                    this.microsoftAppCredentials.MicrosoftAppId,
                                    conversationReference,
                                    async (conversationContext, conversationCancellation) =>
                                    {
                                        this.logger.LogInformation($"Sending question card to team member: {teamMember.AadObjectId}");
                                        await conversationContext.SendActivityAsync(questionCard, conversationCancellation);
                                    },
                                    cancellationToken);
                            }, cancellationToken);
                    });
                }
                catch (Exception ex)
                {
                    this.logger.LogError(ex, $"Error while sending question card to members of the team with id : {teamInfo.Id}.");
                    throw;
                }
            }
        }

        /// <summary>
        /// Get list of members present in a team.
        /// </summary>
        /// <param name="teamId">Id of a team</param>
        /// <param name="serviceUrl">Service URL of team context to get team details.</param>
        /// <returns>List of members of a team.</returns>
        private async Task<IEnumerable<TeamsChannelAccount>> GetTeamMembersAsync(
            string teamId,
            string serviceUrl)
        {
            try
            {
                IEnumerable<TeamsChannelAccount> teamsChannelAccounts = new List<TeamsChannelAccount>();
                var conversationReference = new ConversationReference
                {
                    ChannelId = teamId,
                    ServiceUrl = serviceUrl,
                };

                await this.botAdapter.ContinueConversationAsync(
                    this.botOptions.Value.MicrosoftAppId,
                    conversationReference,
                    async (context, token) =>
                    {
                        teamsChannelAccounts = await TeamsInfo.GetTeamMembersAsync(context, teamId, CancellationToken.None);
                    },
                    default);

                this.logger.LogInformation("Fetching team members from team roster is successful.");
                return teamsChannelAccounts;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error occurred while getting team member list.");
                throw;
            }
        }

        /// <summary>
        /// Store all members of a team in storage.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="teamMembers">List of members of a team.</param>
        /// <param name="questionId">Question id of currently send card.</param>
        /// <returns>Returns a task to represents a  question card to the team members.</returns>
        private async Task UpsertAllTeamMembersAsync(
            ITurnContext<IInvokeActivity> turnContext,
            IEnumerable<TeamsChannelAccount> teamMembers,
            string questionId)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            var activity = turnContext.Activity;

            // Get team information from current activity.
            var teamDetails = turnContext.Activity.TeamsGetTeamInfo();
            var userResponseEntities = new List<UserResponseEntity>();

            // Construct user response entities via getting team members details.
            foreach (var teamMember in teamMembers)
            {
                var entity = this.userResponseMapper.MapToEntity(
                    teamDetails.Id,
                    teamMember.AadObjectId,
                    questionId,
                    activity.From.AadObjectId);

                // Add entity in collection.
                userResponseEntities.Add(entity);
            }

            try
            {
                // Bulk insert in table storage.
                await this.userResponseProvider.BatchInsertOrMergeAsync(userResponseEntities);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception ex)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                this.logger.LogError(ex, $"Error while bulk inserting team members details in table storage.");
            }
        }
    }
}