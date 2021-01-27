// <copyright file="IQuestionAnswersStorageProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// Interface of question answers provider.
    /// </summary>
    public interface IQuestionAnswersStorageProvider
    {
        /// <summary>
        /// Get question answers collection.
        /// </summary>
        /// <returns>A task that represents a collection of question answers entities.</returns>
        Task<IEnumerable<QuestionAnswersEntity>> GetQuestionAnswersEntitiesAsync();

        /// <summary>
        /// Get question answers entity details.
        /// </summary>
        /// <param name="questionId">Unique question id to fetch question answers entity details.</param>
        /// <returns>A question answers entity details.</returns>
        Task<QuestionAnswersEntity> GetQuestionAnswersEntityAsync(string questionId);
    }
}