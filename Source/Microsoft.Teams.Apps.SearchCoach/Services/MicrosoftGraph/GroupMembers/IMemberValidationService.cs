// <copyright file="IMemberValidationService.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.GroupMembers
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for member validation Service.
    /// </summary>
    public interface IMemberValidationService
    {
        /// <summary>
        /// Method to validate whether current user is a valid member of provided team's group.
        /// </summary>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <param name="groupId">The group id of the team that the validator uses to check if the user is a member of the team. </param>
        /// <param name="authorizationHeaders">HttpRequest authorization headers.</param>
        /// <returns>Returns true if current user is a valid member of provided team's group.</returns>
        Task<bool> ValidateMemberAsync(string userAadObjectId, string groupId, string authorizationHeaders);
    }
}