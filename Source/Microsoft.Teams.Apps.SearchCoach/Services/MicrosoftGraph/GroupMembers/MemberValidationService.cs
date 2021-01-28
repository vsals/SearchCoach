// <copyright file="MemberValidationService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.GroupMembers
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Authentication;

    /// <summary>
    /// Class handles methods to validate member of a team's group.
    /// </summary>
    public class MemberValidationService : IMemberValidationService
    {
        /// <summary>
        /// Sends logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<MemberValidationService> logger;

        /// <summary>
        /// Instance of access token helper to get valid token to access Microsoft Graph.
        /// </summary>
        private readonly ITokenHelper accessTokenHelper;

        /// <summary>
        /// Instance of IOptions to read team's group data from azure application configuration.
        /// </summary>
        private readonly IGroupMembersService groupMembersService;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberValidationService"/> class.
        /// </summary>
        /// <param name="accessTokenHelper">Instance of access token helper to get valid token to access Microsoft Graph.</param>
        /// <param name="logger">Logger instance to send logs to the Application Insights service.</param>
        /// <param name="groupMembersService">Instance of group member service.</param>
        public MemberValidationService(
            ITokenHelper accessTokenHelper,
            ILogger<MemberValidationService> logger,
            IGroupMembersService groupMembersService)
        {
            this.accessTokenHelper = accessTokenHelper;
            this.logger = logger;
            this.groupMembersService = groupMembersService;
        }

        /// <summary>
        /// Method to validate whether current user is a member of team's group.
        /// </summary>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <param name="groupId">The group id of the team that the validator uses to check if the user is a member of the team. </param>
        /// <param name="authorizationHeaders">HttpRequest authorization headers.</param>
        /// <returns>Returns true if current user is a member of team's group.</returns>
        public async Task<bool> ValidateMemberAsync(string userAadObjectId, string groupId, string authorizationHeaders)
        {
            var accessToken = await this.accessTokenHelper.GetAccessTokenAsync(userAadObjectId, authorizationHeaders);
            if (string.IsNullOrEmpty(accessToken))
            {
                this.logger.LogError("Token to access graph API is null.");
                return false;
            }

            // Check whether current user is a member of team's group.
            return await this.groupMembersService.GetGroupMemberAsync(groupId, userAadObjectId, accessToken);
        }
    }
}