// <copyright file="IUserResponseStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Interface for user response storage provider.
    /// </summary>
    public interface IUserResponseStorageProvider
    {
        /// <summary>
        /// Get user response entity based on team id and question id.
        /// </summary>
        /// <param name="teamId">Team id to fetch user response details.</param>
        /// <param name="questionId">Question id to fetch a particular question details.</param>
        /// <returns>A user response entity object for a particular question id for a team.</returns>
        Task<UserResponseEntity> GetUserResponseEntityAsync(string teamId, string questionId);

        /// <summary>
        /// Insert or merge a batch of entities in Azure table storage.
        /// A batch can contain up to 100 entities.
        /// Get user response data from storage.
        /// </summary>
        /// <param name="teamId">Team id to fetch the answer details.</param>
        /// <param name="questionId">Question id for which user submitted the response.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <returns>A task that represent a object to hold user response data.</returns>
        Task<UserResponseEntity> GetUserResponseAsync(string teamId, string questionId, string userAadObjectId);

        /// <summary>
        /// Updates user response data in storage.
        /// </summary>
        /// <param name="entity">Holds user response data.</param>
        /// <returns>A task that represents user response data is updated.</returns>
        Task<bool> UpdateUserResponseAsync(UserResponseEntity entity);

        /// <summary>
        /// Get question response for a team.
        /// </summary>
        /// <param name="entities">Entities to be inserted or merged in Azure table storage.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        Task BatchInsertOrMergeAsync(IEnumerable<UserResponseEntity> entities);

        /// <summary>
        /// Get user's responses for a particular team.
        /// </summary>
        /// <param name="teamId">Team id to fetch user's responses details.</param>
        /// <returns>A task that represents a collection of user's responses entities.</returns>
        Task<IEnumerable<UserResponseEntity>> GetUsersResponsesAsync(string teamId);
    }
}