// <copyright file="UserResponseStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Implements storage provider which stores user response data in Azure table storage.
    /// </summary>
    public class UserResponseStorageProvider : BaseStorageProvider, IUserResponseStorageProvider
    {
        /// <summary>
        /// Represents user response entity name.
        /// </summary>
        private const string UserResponseEntityName = "UserResponseCollection";

        /// <summary>
        /// Logger implementation to send logs to the logger service.
        /// </summary>
        private readonly ILogger<BaseStorageProvider> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserResponseStorageProvider"/> class.
        /// Handles storage read write operations.
        /// </summary>
        /// <param name="connectionString">azure storage connection string.</param>
        /// <param name="logger">Sends logs to the logger service.</param>
        public UserResponseStorageProvider(
            string connectionString,
            ILogger<BaseStorageProvider> logger)
            : base(connectionString, UserResponseEntityName, logger)
        {
            this.logger = logger;

            // Initialize the table while project startup to import user response collection data manually.
            this.EnsureInitializedAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get user response data from storage.
        /// </summary>
        /// <param name="teamId">Team id to fetch the answer details.</param>
        /// <param name="questionId">Question id for which user submitted the response.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of current user.</param>
        /// <returns>A task that represent a object to hold user response data.</returns>
        public async Task<UserResponseEntity> GetUserResponseAsync(string teamId, string questionId, string userAadObjectId)
        {
            // When there is no response created by user and Messaging Extension is open, table initialization is required.
            await this.EnsureInitializedAsync();

            if (string.IsNullOrEmpty(teamId) || string.IsNullOrEmpty(questionId) || string.IsNullOrEmpty(userAadObjectId))
            {
                return null;
            }

            var operation = TableOperation.Retrieve<UserResponseEntity>(teamId, $"{questionId}_{userAadObjectId}");
            var data = await this.CloudTable.ExecuteAsync(operation);

            return data.Result as UserResponseEntity;
        }

        /// <summary>
        /// Updates user response data in storage.
        /// </summary>
        /// <param name="entity">Holds user response data.</param>
        /// <returns>A task that represents user response data is updated.</returns>
        public async Task<bool> UpdateUserResponseAsync(UserResponseEntity entity)
        {
            await this.EnsureInitializedAsync();
            TableOperation addOrUpdateOperation = TableOperation.Replace(entity);
            var result = await this.CloudTable.ExecuteAsync(addOrUpdateOperation);

            return result.HttpStatusCode == (int)HttpStatusCode.NoContent;
        }

        /// <summary>
        /// Get user response entity based on team id and question id.
        /// </summary>
        /// <param name="teamId">Team id to fetch user response details.</param>
        /// <param name="questionId">Question id to fetch a particular question details.</param>
        /// <returns>A user response entity object for a particular question id for a team.</returns>
        public async Task<UserResponseEntity> GetUserResponseEntityAsync(string teamId, string questionId)
        {
            // When there is no response created by user and Messaging Extension is open, table initialization is required.
            await this.EnsureInitializedAsync();

            if (string.IsNullOrEmpty(questionId) || string.IsNullOrEmpty(teamId))
            {
                return null;
            }

            string partitionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, teamId);
            string questionIdCondition = TableQuery.GenerateFilterCondition(nameof(UserResponseEntity.QuestionId), QueryComparisons.Equal, questionId);
            var combinedPartitionFilter = TableQuery.CombineFilters(partitionKeyCondition, TableOperators.And, questionIdCondition);

            TableQuery<UserResponseEntity> query = new TableQuery<UserResponseEntity>().Where(combinedPartitionFilter);
            var queryResult = await this.CloudTable.ExecuteQuerySegmentedAsync(query, null);

            return queryResult?.FirstOrDefault();
        }

        /// <summary>
        /// Get user's responses for a particular team.
        /// </summary>
        /// <param name="teamId">Team id to fetch user's responses details.</param>
        /// <returns>A task that represents a collection of user's responses entities.</returns>
        public async Task<IEnumerable<UserResponseEntity>> GetUsersResponsesAsync(string teamId)
        {
            await this.EnsureInitializedAsync();
            string partitionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, teamId);

            TableQuery<UserResponseEntity> query = new TableQuery<UserResponseEntity>().Where(partitionKeyCondition);
            TableContinuationToken continuationToken = null;
            var usersResponses = new List<UserResponseEntity>();

            do
            {
                var queryResult = await this.CloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                if (queryResult?.Results != null)
                {
                    usersResponses.AddRange(queryResult.Results);
                    continuationToken = queryResult.ContinuationToken;
                }
            }
            while (continuationToken != null);

            return usersResponses;
        }

        /// <summary>
        /// Insert or merge a batch of entities in Azure table storage.
        /// A batch can contain up to 100 entities.
        /// </summary>
        /// <param name="entities">Entities to be inserted or merged in Azure table storage.</param>
        /// <returns>A task that represents the work queued to execute.</returns>
        public async Task BatchInsertOrMergeAsync(IEnumerable<UserResponseEntity> entities)
        {
            try
            {
                var array = entities.ToArray();
                for (var i = 0; i <= array.Length / 100; i++)
                {
                    var lowerBound = i * 100;
                    var upperBound = Math.Min(lowerBound + 99, array.Length - 1);
                    if (lowerBound > upperBound)
                    {
                        break;
                    }

                    var batchOperation = new TableBatchOperation();
                    for (var j = lowerBound; j <= upperBound; j++)
                    {
                        batchOperation.InsertOrMerge(array[j]);
                    }

                    await this.CloudTable.ExecuteBatchAsync(batchOperation);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}