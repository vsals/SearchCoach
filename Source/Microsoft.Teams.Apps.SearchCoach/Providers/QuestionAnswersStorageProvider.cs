// <copyright file="QuestionAnswersStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Implements storage provider which stores question answers data in Azure table storage.
    /// </summary>
    public class QuestionAnswersStorageProvider : BaseStorageProvider, IQuestionAnswersStorageProvider
    {
        /// <summary>
        /// Represents question answers entity name.
        /// </summary>
        private const string QuestionAnswersEntityName = "QuestionAnswersCollection";

        /// <summary>
        /// Represents question answers entity partition key value.
        /// </summary>
        private const string PartitionKeyValue = "Class";

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionAnswersStorageProvider"/> class.
        /// Handles storage read write operations.
        /// </summary>
        /// <param name="connectionString">azure storage connection string.</param>
        /// <param name="logger">Sends logs to the logger service.</param>
        public QuestionAnswersStorageProvider(
            string connectionString,
            ILogger<BaseStorageProvider> logger)
            : base(connectionString, QuestionAnswersEntityName, logger)
        {
            // Initialize the table while project startup to import question answers collection data manually.
            this.EnsureInitializedAsync().ConfigureAwait(false);
        }

        /// <summary>
        /// Get question answers collection.
        /// </summary>
        /// <returns>A task that represents a collection of question answers entities.</returns>
        public async Task<IEnumerable<QuestionAnswersEntity>> GetQuestionAnswersEntitiesAsync()
        {
            await this.EnsureInitializedAsync();
            string partitionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKeyValue);

            TableQuery<QuestionAnswersEntity> query = new TableQuery<QuestionAnswersEntity>().Where(partitionKeyCondition);
            TableContinuationToken continuationToken = null;
            var questionAnswersCollection = new List<QuestionAnswersEntity>();

            do
            {
                var queryResult = await this.CloudTable.ExecuteQuerySegmentedAsync(query, continuationToken);
                if (queryResult?.Results != null)
                {
                    questionAnswersCollection.AddRange(queryResult.Results);
                    continuationToken = queryResult.ContinuationToken;
                }
            }
            while (continuationToken != null);

            return questionAnswersCollection;
        }

        /// <summary>
        /// Get question answers entity details.
        /// </summary>
        /// <param name="questionId">Unique question id to fetch question answers entity details.</param>
        /// <returns>A question answers entity details.</returns>
        public async Task<QuestionAnswersEntity> GetQuestionAnswersEntityAsync(string questionId)
        {
            await this.EnsureInitializedAsync();

            if (string.IsNullOrEmpty(questionId))
            {
                return null;
            }

            string partitionKeyCondition = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKeyValue);
            string questionIdCondition = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, questionId);
            var combinedPartitionFilter = TableQuery.CombineFilters(partitionKeyCondition, TableOperators.And, questionIdCondition);

            TableQuery<QuestionAnswersEntity> query = new TableQuery<QuestionAnswersEntity>().Where(combinedPartitionFilter);
            var queryResult = await this.CloudTable.ExecuteQuerySegmentedAsync(query, null);

            return queryResult?.FirstOrDefault();
        }
    }
}