// <copyright file="IGroupMembersService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.GroupMembers
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for Group Members Service.
    /// </summary>
    public interface IGroupMembersService
    {
        /// <summary>
        /// Get a member of a team's group.
        /// </summary>
        /// <param name="groupId">Group id of the team to find and get member id.</param>
        /// <param name="userAadObjectId">Azure Active Directory user object id</param>
        /// <param name="accessToken">User authorization access token.</param>
        /// <returns>A task that returns true if user exists in team's group otherwise false.</returns>
        Task<bool> GetGroupMemberAsync(string groupId, string userAadObjectId, string accessToken);
    }
}