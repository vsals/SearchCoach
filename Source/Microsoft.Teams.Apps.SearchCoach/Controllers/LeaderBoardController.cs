// <copyright file="LeaderBoardController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace Microsoft.Teams.Apps.SearchCoach.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Users;

    /// <summary>
    /// Initializes a new instance of the <see cref="LeaderBoardController"/> class.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("api/leaderboard")]
    public class LeaderBoardController : BaseSearchCoachController
    {
        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Provider to deal with user response provider methods.
        /// </summary>
        private readonly IUserResponseStorageProvider userResponseProvider;

        /// <summary>
        /// Instance of user service to get user's details.
        /// </summary>
        private readonly IUsersService usersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="LeaderBoardController"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="userResponseProvider">Provider for fetching user responses details from storage table.</param>
        /// <param name="usersService">Instance of user service to get user's details.</param>
        public LeaderBoardController(
            ILogger<LeaderBoardController> logger,
            TelemetryClient telemetryClient,
            IUserResponseStorageProvider userResponseProvider,
            IUsersService usersService)
            : base(telemetryClient)
        {
            this.logger = logger;
            this.userResponseProvider = userResponseProvider;
            this.usersService = usersService;
        }

        /// <summary>
        /// Get user's responses details from storage to show on leader-board tab.
        /// </summary>
        /// <param name="teamId">Team id to fetch user's responses details for that particular team.</param>
        /// <returns>A collection of user's responses details.</returns>
        [HttpGet]
        [Route("{teamId}")]
        public async Task<IActionResult> GetUsersResponsesAsync(string teamId)
        {
            try
            {
                this.logger.LogInformation("User's responses - HTTP Get call initiated.");
                this.RecordEvent("User's responses - HTTP Get call initiated.", RequestStatus.Initiated);

                if (string.IsNullOrEmpty(teamId))
                {
                    this.logger.LogError("User's responses - Team id is null or empty.");
                    this.RecordEvent("User responses - HTTP Get call failed.", RequestStatus.Failed);
                    return this.BadRequest($"Team id can not be null or empty.");
                }

                // Fetch user's responses details from storage for a particular team.
                var usersResponses = await this.userResponseProvider.GetUsersResponsesAsync(teamId);

                // Get user's display names for all distinct users of the team.
                var usersDisplayNameDetails = await this.usersService.GetUserDisplayNamesAsync(
                    this.UserAadId.ToString(),
                    this.Request.Headers["Authorization"].ToString(),
                    usersResponses.Select(userResponse => userResponse.UserId.ToString()).Distinct());

                // Get user's responses for total correct answers and total attempted questions.
                var usersResponsesData = usersResponses
                    .GroupBy(userResponseEntity => userResponseEntity.UserId)
                    .Select(userResponse => new UserResponseData()
                    {
                        RightAnswers = userResponse.Where(response => response.IsCorrectAnswer).Count(),
                        UserName = usersDisplayNameDetails.Where(userDetail => userDetail.UserId == userResponse.Key).FirstOrDefault().DisplayName,
                        QuestionsAttempted = userResponse.Where(c => c.IsQuestionAttempted).Count(),
                    });

                this.logger.LogInformation("User's responses - HTTP Get call succeeded.");
                this.RecordEvent("User's responses - HTTP Get call succeeded.", RequestStatus.Succeeded);

                return this.Ok(usersResponsesData);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, $"User's responses - HTTP Get call failed, for team Id: {teamId} and userId: {this.UserAadId}");
                this.RecordEvent("User's responses - HTTP Get call failed.", RequestStatus.Failed);
                throw;
            }
        }
    }
}