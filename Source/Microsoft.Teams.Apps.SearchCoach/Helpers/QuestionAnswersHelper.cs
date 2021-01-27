// <copyright file="QuestionAnswersHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Constants;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;
    using Microsoft.Teams.Apps.SearchCoach.Providers;

    /// <summary>
    /// Class that handles question answers helper methods .
    /// </summary>
    public class QuestionAnswersHelper
    {
        /// <summary>
        /// Represents the default card cache duration in Hours.
        /// </summary>
        private const double DefaultCardCacheDurationInHour = 1;

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<QuestionAnswersHelper> logger;

        /// <summary>
        /// Cache for storing authorization result.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// Provider instance to work with question answers data.
        /// </summary>
        private readonly IQuestionAnswersStorageProvider questionAnswersStorageProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionAnswersHelper"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="memoryCache">MemoryCache instance for caching authorization result.</param>
        /// <param name="botOptions">A set of key/value application configuration properties for activity handler.</param>
        /// <param name="questionAnswersStorageProvider">Provider instance to work with question answers data.</param>
        public QuestionAnswersHelper(
            ILogger<QuestionAnswersHelper> logger,
            IMemoryCache memoryCache,
            IOptions<BotSettings> botOptions,
            IQuestionAnswersStorageProvider questionAnswersStorageProvider)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.botOptions = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
            this.questionAnswersStorageProvider = questionAnswersStorageProvider;
        }

        /// <summary>
        /// Get question answers entities.
        /// </summary>
        /// <returns>Returns a collection of question answers entities.</returns>
        public async Task<IEnumerable<QuestionAnswersEntity>> GetQuestionAnswersEntitiesAsync()
        {
            this.logger.LogInformation($"{nameof(this.GetQuestionAnswersEntitiesAsync)} initiated.");

            bool isCacheEntryExists = this.memoryCache.TryGetValue(
                CacheKeysConstants.QuestionAnswersEntityKey,
                out IEnumerable<QuestionAnswersEntity> questionAnswersEntities);

            if (!isCacheEntryExists)
            {
                // If cache duration is not specified then by default cache for 2 hours.
                var cacheDurationInHour = TimeSpan.FromHours(this.botOptions.Value.CardCacheDurationInHour);
                cacheDurationInHour = cacheDurationInHour.Hours <= 0 ? TimeSpan.FromHours(DefaultCardCacheDurationInHour) : cacheDurationInHour;

                questionAnswersEntities = await this.questionAnswersStorageProvider.GetQuestionAnswersEntitiesAsync();
                this.memoryCache.Set(CacheKeysConstants.QuestionAnswersEntityKey, questionAnswersEntities, cacheDurationInHour);
            }

            this.logger.LogInformation($"{nameof(this.GetQuestionAnswersEntitiesAsync)} succeeded.");

            return questionAnswersEntities;
        }
    }
}