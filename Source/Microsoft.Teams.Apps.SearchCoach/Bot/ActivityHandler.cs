// <copyright file="ActivityHandler.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Bot
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Teams;
    using Microsoft.Bot.Schema;
    using Microsoft.Bot.Schema.Teams;
    using Microsoft.Extensions.Localization;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// The StarterTeamsActivityHandler is responsible for reacting to incoming events from Teams sent from BotFramework.
    /// </summary>
    public sealed class ActivityHandler : TeamsActivityHandler
    {
        /// <summary>
        /// Logger implementation to send logs to the logger service.
        /// </summary>
        private readonly ILogger<ActivityHandler> logger;

        /// <summary>
        /// The current cultures' string localizer.
        /// </summary>
        private readonly IStringLocalizer<Strings> localizer;

        /// <summary>
        /// Instance of Application Insights Telemetry client.
        /// </summary>
        private readonly TelemetryClient telemetryClient;

        /// <summary>
        /// State management object for maintaining user conversation state.
        /// </summary>
        private readonly BotState userState;

        /// <summary>
        /// Provider for fetching information about team details from storage table.
        /// </summary>
        private readonly ITeamStorageProvider teamStorageProvider;

        /// <summary>
        /// Provider for storing user information in storage table once the bot is installed in personal scope.
        /// </summary>
        private readonly IUserDetailProvider userDetailProvider;

        /// <summary>
        /// Instance of class that handles card notification helper methods.
        /// </summary>
        private readonly NotificationHelper notificationCardHelper;

        /// <summary>
        /// Provider to deal with user response details from storage table.
        /// </summary>
        private readonly IUserResponseStorageProvider userResponseProvider;

        /// <summary>
        /// The instance of question mapper class to work with question view model.
        /// </summary>
        private readonly IQuestionAnswersMapper questionMapper;

        /// <summary>
        /// Instance of class that handles question card helper methods.
        /// </summary>
        private readonly CardHelper cardHelper;

        /// <summary>
        /// Instance to work with question answers data.
        /// </summary>
        private readonly IQuestionAnswersStorageProvider questionAnswersStorageProvider;

        /// <summary>
        /// The instance of user response mapper class to work with user response model.
        /// </summary>
        private readonly IUserResponseMapper userResponseMapper;

        /// <summary>
        /// Instance of class that handles question answers helper methods.
        /// </summary>
        private readonly QuestionAnswersHelper questionAnswersHelper;

        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityHandler"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="localizer">The current cultures' string localizer.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client. </param>
        /// <param name="teamStorageProvider">Provider for fetching information about team details from storage table.</param>
        /// <param name="userDetailProvider">Provider instance to work with user data.</param>
        /// <param name="userState">State management object for maintaining user conversation state.</param>
        /// <param name="questionAnswersStorageProvider">Provider instance to work with question answers data.</param>
        /// <param name="notificationCardHelper">Instance of class that handles card notification helper methods.</param>
        /// <param name="userResponseProvider">Provider for fetching information about user response details from storage table.</param>
        /// <param name="questionMapper">The instance of question mapper class to work with models.</param>
        /// <param name="cardHelper">Instance of class that handles question card helper methods.</param>
        /// <param name="questionAnswersHelper">Instance of class that handles question answers helper methods.</param>
        /// <param name="userResponseMapper">The instance of user response mapper class to work with models.</param>
        /// <param name="botOptions">A set of key/value application configuration properties for activity handler.</param>
        public ActivityHandler(
            ILogger<ActivityHandler> logger,
            IStringLocalizer<Strings> localizer,
            TelemetryClient telemetryClient,
            ITeamStorageProvider teamStorageProvider,
            IUserDetailProvider userDetailProvider,
            UserState userState,
            IQuestionAnswersStorageProvider questionAnswersStorageProvider,
            NotificationHelper notificationCardHelper,
            IUserResponseStorageProvider userResponseProvider,
            CardHelper cardHelper,
            QuestionAnswersHelper questionAnswersHelper,
            IQuestionAnswersMapper questionMapper,
            IUserResponseMapper userResponseMapper,
            IOptions<BotSettings> botOptions)
        {
            this.logger = logger;
            this.localizer = localizer;
            this.telemetryClient = telemetryClient;
            this.teamStorageProvider = teamStorageProvider;
            this.userDetailProvider = userDetailProvider;
            this.userState = userState;
            this.questionAnswersStorageProvider = questionAnswersStorageProvider;
            this.notificationCardHelper = notificationCardHelper;
            this.userResponseProvider = userResponseProvider;
            this.questionMapper = questionMapper;
            this.cardHelper = cardHelper;
            this.questionAnswersHelper = questionAnswersHelper;
            this.userResponseMapper = userResponseMapper;
            this.botOptions = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
        }

        /// <summary>
        /// Handles an incoming activity.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        /// <remarks>
        /// Reference link: https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.activityhandler.onturnasync?view=botbuilder-dotnet-stable.
        /// </remarks>
        public override Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            this.RecordEvent(nameof(this.OnTurnAsync), turnContext);

            return base.OnTurnAsync(turnContext, cancellationToken);
        }

        /// <summary>
        /// Handle when a message is addressed to the bot.
        /// </summary>
        /// <param name="turnContext">The turn context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A Task resolving to either a login card or the adaptive card of the Reddit post.</returns>
        /// <remarks>
        /// For more information on bot messaging in Teams, see the documentation
        /// https://docs.microsoft.com/en-us/microsoftteams/platform/bots/how-to/conversations/conversation-basics?tabs=dotnet#receive-a-message .
        /// </remarks>
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            var activity = turnContext.Activity;
            this.RecordEvent(nameof(this.OnMessageActivityAsync), turnContext);

            var submittedAnswerResponse = activity.Value?.ToString();

            // Check if answer is submitted by user and it is not a message activity.
            if (submittedAnswerResponse != null && string.IsNullOrEmpty(activity.Text))
            {
                var submittedAnswerDetails = JsonConvert.DeserializeObject<SubmitAnswerData>(JObject.Parse(submittedAnswerResponse).ToString());

                // Get submitted answer response from storage.
                UserResponseEntity userResponseData = await this.userResponseProvider.GetUserResponseAsync(
                    submittedAnswerDetails.TeamId,
                    submittedAnswerDetails.QuestionId,
                    activity.From.AadObjectId);

                // Get question answers entity from storage.
                var questionAnswersEntity = await this.questionAnswersStorageProvider.GetQuestionAnswersEntityAsync(submittedAnswerDetails.QuestionId);
                bool isAnswerCorrect = questionAnswersEntity.CorrectOption == submittedAnswerDetails.SelectedAnswerValue;

                var questionViewModelData = this.questionMapper.MapToViewModel(
                    questionAnswersEntity,
                    activity.From.Name,
                    isAnswerCorrect ? this.localizer.GetString("CorrectAnswerText") : this.localizer.GetString("WrongAnswerText"),
                    submittedAnswerDetails.SelectedAnswerValue,
                    this.botOptions.Value.ManifestId,
                    isAnswerCorrect);

                // Get updated card and refresh the current card.
                Attachment questionAnswerCard = this.cardHelper.GetQuestionAnswerCard(questionViewModelData);
                IMessageActivity updateCard = MessageFactory.Attachment(questionAnswerCard);
                updateCard.Id = activity.ReplyToId;
                await turnContext.UpdateActivityAsync(updateCard, cancellationToken);

                // Get updated entity object.
                var userResponseEntity = this.userResponseMapper.UpdateMapToEntity(
                    userResponseData,
                    submittedAnswerDetails.SelectedAnswerValue,
                    activity.From.AadObjectId,
                    isAnswerCorrect);

                // Update user response entity.
                var response = await this.userResponseProvider.UpdateUserResponseAsync(userResponseEntity);
                if (response)
                {
                    this.logger.LogInformation("Question data updated successfully in storage.");
                    this.logger.LogInformation("User response submitted successfully.");
                }
                else
                {
                    this.logger.LogInformation("Error while updating question data in storage.");
                }
            }
        }

        /// <summary>
        /// When OnTurn method receives a submit invoke activity on bot turn, it calls this method.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="action">Provides context for a turn of a bot and.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task that represents a task module response.</returns>
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionFetchTaskAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            try
            {
                turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
                this.RecordEvent(nameof(this.OnTeamsMessagingExtensionFetchTaskAsync), turnContext);
                var activity = turnContext.Activity;

                // Show error message on Task module if opening it from personal scope.
                if (activity.Conversation.ConversationType == ConversationTypes.Personal)
                {
                    Attachment personalScopeErrorCard = this.cardHelper.GetErrorMessageCard(this.localizer.GetString("PersonalScopeErrorMessage"));
                    return this.GetTaskModuleErrorResponse(personalScopeErrorCard);
                }

                // Gets the TeamsInfo object from the current activity.
                var teamsDetails = activity.TeamsGetTeamInfo();

                // Get current team details from storage.
                var teamDetail = await this.teamStorageProvider.GetTeamDetailAsync(teamsDetails.Id);

                if (teamDetail == null)
                {
                    // Show error message on Task module if opening from teams scope where bot is not installed.
                    Attachment personalScopeErrorCard = this.cardHelper.GetErrorMessageCard(this.localizer.GetString("TeamScopeErrorMessage"));
                    return this.GetTaskModuleErrorResponse(personalScopeErrorCard);
                }

                // Get a collection of question answers entities.
                var questionAnswersEntities = this.questionAnswersHelper.GetQuestionAnswersEntitiesAsync()?.Result;
                Attachment questionAnswerCard = this.cardHelper.GetQuestionsCard(questionAnswersEntities);

                return this.GetTaskModuleResponse(questionAnswerCard);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while fetching task module.");
                throw;
            }
        }

        /// <summary>
        /// Invoked when members other than this bot (like a user) are removed from the conversation.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        protected override async Task OnConversationUpdateActivityAsync(ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            try
            {
                turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
                this.RecordEvent(nameof(this.OnConversationUpdateActivityAsync), turnContext);

                var activity = turnContext.Activity;
                this.logger.LogInformation($"conversationType: {activity.Conversation.ConversationType}, membersAdded: {activity.MembersAdded?.Count}, membersRemoved: {activity.MembersRemoved?.Count}");

                if (activity.Conversation.ConversationType == ConversationTypes.Personal)
                {
                    if (activity.MembersAdded != null && activity.MembersAdded.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.HandleBotInstallEventInPersonalScopeAsync(turnContext);
                    }
                }
                else if (activity.Conversation.ConversationType == ConversationTypes.Channel)
                {
                    if (activity.MembersAdded != null && activity.MembersAdded.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.HandleBotInstallEventInTeamScopeAsync(turnContext);
                    }
                    else if (activity.MembersRemoved != null && activity.MembersRemoved.Any(member => member.Id == activity.Recipient.Id))
                    {
                        await this.HandleBotUninstallEventInTeamScopeAsync(turnContext);
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while updating conversation activity actions.");
                throw;
            }
        }

        /// <summary>
        ///  Handle message extension submit action task received by the bot.
        /// </summary>
        /// <param name="turnContext">Context object containing information cached for a single turn of conversation with a user.</param>
        /// <param name="action">Messaging extension action value payload.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>Response of messaging extension action.</returns>
        /// <remarks>
        /// Reference link: https://docs.microsoft.com/en-us/dotnet/api/microsoft.bot.builder.teams.teamsactivityhandler.onteamsmessagingextensionfetchtaskasync?view=botbuilder-dotnet-stable.
        /// </remarks>
        protected override async Task<MessagingExtensionActionResponse> OnTeamsMessagingExtensionSubmitActionAsync(
            ITurnContext<IInvokeActivity> turnContext,
            MessagingExtensionAction action,
            CancellationToken cancellationToken)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            action = action ?? throw new ArgumentNullException(nameof(action));
            this.RecordEvent(nameof(this.OnTeamsMessagingExtensionFetchTaskAsync), turnContext);

            var activity = turnContext.Activity;

            var questionDetails = JsonConvert.DeserializeObject<QuestionSubmitActionData>(JObject.Parse(activity.Value?.ToString())["data"].ToString());
            var questionId = questionDetails.SelectedQuestionId; // Get selected question id from quiz questions list.

            // Gets the TeamsInfo object from the current activity.
            var teamDetails = activity.TeamsGetTeamInfo();

            var questionAnswersEntity = await this.questionAnswersStorageProvider.GetQuestionAnswersEntityAsync(questionId);
            if (questionAnswersEntity == null)
            {
                this.logger.LogInformation("Question answers entity not found.");
                return null;
            }

            var userResponseDetails = await this.userResponseProvider.GetUserResponseEntityAsync(teamDetails.Id, questionId);
            if (userResponseDetails != null)
            {
                // Show error message on Task module if sending same question multiple times for current team.
                Attachment errorCard = this.cardHelper.GetErrorMessageCard(string.Format(CultureInfo.InvariantCulture, this.localizer.GetString("QuestionAlreadySentErrorMessageText"), questionAnswersEntity.Question));
                return this.GetTaskModuleErrorResponse(errorCard);
            }

            // Get question with answer options card.
            var questionAnswerCard = this.cardHelper.QuestionCard(
                questionAnswersEntity,
                activity.From.Name,
                teamDetails.Id,
                activity.From.AadObjectId);

            // Get all team members and send notification question card to all.
            await this.notificationCardHelper.SendQuestionCardToTeamMembersAsync(
                turnContext,
                teamDetails.Id,
                activity.ServiceUrl,
                questionId,
                MessageFactory.Attachment(questionAnswerCard),
                cancellationToken);

            this.logger.LogInformation("Question quiz card sent successfully.");

            return null;
        }

        /// <summary>
        /// Sent welcome card to personal chat.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        private async Task HandleBotInstallEventInPersonalScopeAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            this.RecordEvent(nameof(this.HandleBotInstallEventInPersonalScopeAsync), turnContext);

            var activity = turnContext.Activity;
            this.logger.LogInformation($"Bot added in personal {turnContext.Activity.Conversation.Id}");

            var userStateAccessors = this.userState.CreateProperty<UserConversationState>(nameof(UserConversationState));
            var userConversationState = await userStateAccessors.GetAsync(turnContext, () => new UserConversationState());

            // check if welcome card is already sent.
            if (userConversationState.IsWelcomeCardSent)
            {
                return;
            }

            var welcomeCard = this.cardHelper.GetPersonalScopeWelcomeCard();
            await turnContext.SendActivityAsync(MessageFactory.Attachment(welcomeCard));

            userConversationState.IsWelcomeCardSent = true;
            await userStateAccessors.SetAsync(turnContext, userConversationState);

            // Add user details in storage.
            await this.userDetailProvider.AddUserDetailAsync(
                activity.Conversation.Id,
                activity.From.AadObjectId,
                activity.ServiceUrl);
        }

        /// <summary>
        /// Send a welcome card if bot is installed in Team scope.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        private async Task HandleBotInstallEventInTeamScopeAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            try
            {
                turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
                this.RecordEvent(nameof(this.HandleBotInstallEventInTeamScopeAsync), turnContext);

                var activity = turnContext.Activity;
                var welcomeCard = this.cardHelper.GetTeamsScopeWelcomeCard();
                await turnContext.SendActivityAsync(MessageFactory.Attachment(welcomeCard));

                // Get team information and construct entity model.
                var teamsDetails = activity.TeamsGetTeamInfo();
                TeamEntity teamEntity = new TeamEntity
                {
                    TeamId = teamsDetails.Id,
                    BotInstalledOn = DateTime.UtcNow,
                    ServicePath = activity.ServiceUrl,
                };

                // Store team information in storage.
                bool operationStatus = await this.teamStorageProvider.UpsertTeamDetailAsync(teamEntity);

                if (!operationStatus)
                {
                    this.logger.LogError("Unable to store bot installation details of a team in storage.");
                    await turnContext.SendActivityAsync("Unable to store bot installation details of a team in storage. Please re-install the app.");
                    throw new Exception("Unable to store bot installation details of a team in storage. Please re-install the app.");
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while sending welcome card to teams scope.");
                throw;
            }
        }

        /// <summary>
        /// Remove user details from storage if bot is uninstalled from Team scope.
        /// </summary>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        /// <returns>A task that represents a response.</returns>
        private async Task HandleBotUninstallEventInTeamScopeAsync(ITurnContext<IConversationUpdateActivity> turnContext)
        {
            turnContext = turnContext ?? throw new ArgumentNullException(nameof(turnContext));
            this.RecordEvent(nameof(this.HandleBotUninstallEventInTeamScopeAsync), turnContext);

            var activity = turnContext.Activity;
            this.logger.LogInformation($"Bot removed from team {activity.Conversation.Id}");

            var teamsChannelData = activity.GetChannelData<TeamsChannelData>();
            var teamId = teamsChannelData.Team.Id;

            // Get team information from storage.
            var teamEntity = await this.teamStorageProvider.GetTeamDetailAsync(teamId);
            if (teamEntity == null)
            {
                this.logger.LogError($"Team details not found for team id: {teamId} to delete it.");
                return;
            }

            // Delete team information from storage when bot is uninstalled.
            bool result = await this.teamStorageProvider.DeleteTeamDetailAsync(teamEntity);
            if (!result)
            {
                this.logger.LogError("Unable to remove team details from Azure storage.");
            }
        }

        /// <summary>
        /// Records event data to Application Insights telemetry client.
        /// </summary>
        /// <param name="eventName">Name of the event.</param>
        /// <param name="turnContext">Provides context for a turn in a bot.</param>
        private void RecordEvent(string eventName, ITurnContext turnContext)
        {
            this.telemetryClient.TrackEvent(eventName, new Dictionary<string, string>
            {
                { "userId", turnContext.Activity.From.AadObjectId },
                { "tenantId", turnContext.Activity.Conversation.TenantId },
            });
        }

        /// <summary>
        /// Get messaging extension action response object to show collection of question answers.
        /// </summary>
        /// <param name="questionAnswerCard">Question answer card as input.</param>
        /// <returns>MessagingExtensionActionResponse object.</returns>
        private MessagingExtensionActionResponse GetTaskModuleResponse(
          Attachment questionAnswerCard)
        {
            return new MessagingExtensionActionResponse
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo
                    {
                        Card = questionAnswerCard,
                        Height = 300,
                        Width = 500,
                        Title = this.localizer.GetString("QuestionListTaskModuleTitle"),
                    },
                },
            };
        }

        /// <summary>
        /// Get messaging extension action response object.
        /// </summary>
        /// <param name="errorCard">Error card as attachment.</param>
        /// <returns>MessagingExtensionActionResponse object.</returns>
        private MessagingExtensionActionResponse GetTaskModuleErrorResponse(
            Attachment errorCard)
        {
            return new MessagingExtensionActionResponse
            {
                Task = new TaskModuleContinueResponse()
                {
                    Value = new TaskModuleTaskInfo
                    {
                        Card = errorCard,
                        Height = 150,
                        Width = 300,
                        Title = this.localizer.GetString("TaskModuleErrorTitle"),
                    },
                },
            };
        }
    }
}