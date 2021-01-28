// <copyright file="CardHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Web;
    using AdaptiveCards;
    using AdaptiveCards.Templating;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Bot.Schema;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Models.DataModels;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// Class that handles card create/update helper methods.
    /// </summary>
    public class CardHelper
    {
        /// <summary>
        /// Represents the question list card file name.
        /// </summary>
        private const string QuestionsCardFileName = "QuestionListCard.json";

        /// <summary>
        /// Represents the welcome card personal scope file name.
        /// </summary>
        private const string WelcomeCardTeamsScopeFileName = "WelcomeCardTeamsScope.json";

        /// <summary>
        /// Represents the personal scope error card file name.
        /// </summary>
        private const string ErrorCardFileName = "ErrorCard.json";

        /// <summary>
        /// Represents the question answer card file name.
        /// </summary>
        private const string QuestionAnswerCardFileName = "QuestionAnswerCard.json";

        /// <summary>
        /// Represents the welcome card personal scope file name.
        /// </summary>
        private const string WelcomeCardPersonalScopeFileName = "WelcomeCardPersonalScope.json";

        /// <summary>
        /// Search coach tab entity Id.
        /// </summary>
        private const string SearchCoachTabEntityId = "SearchCoach";

        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger<CardHelper> logger;

        /// <summary>
        /// Cache for storing authorization result.
        /// </summary>
        private readonly IMemoryCache memoryCache;

        /// <summary>
        /// Information about the web hosting environment an application is running in.
        /// </summary>
        private readonly IWebHostEnvironment hostingEnvironment;

        /// <summary>
        /// A set of key/value application configuration properties for Activity settings.
        /// </summary>
        private readonly IOptions<BotSettings> botOptions;

        /// <summary>
        /// The instance of question mapper class to work with question view model.
        /// </summary>
        private readonly IQuestionAnswersMapper questionMapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CardHelper"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="memoryCache">MemoryCache instance for caching authorization result.</param>
        /// <param name="hostingEnvironment">Information about the web hosting environment an application is running in.</param>
        /// <param name="botOptions">A set of key/value application configuration properties for activity handler.</param>
        /// <param name="questionMapper">The instance of question mapper class to work with models.</param>
        public CardHelper(
            ILogger<CardHelper> logger,
            IMemoryCache memoryCache,
            IWebHostEnvironment hostingEnvironment,
            IOptions<BotSettings> botOptions,
            IQuestionAnswersMapper questionMapper)
        {
            this.logger = logger;
            this.memoryCache = memoryCache;
            this.hostingEnvironment = hostingEnvironment;
            this.botOptions = botOptions ?? throw new ArgumentNullException(nameof(botOptions));
            this.questionMapper = questionMapper;
        }

        /// <summary>
        /// Get question answer card as attachment.
        /// </summary>
        /// <param name="question">Question view model object.</param>
        /// <returns>Returns a attachment of question answer card.</returns>
        public Attachment GetQuestionAnswerCard(QuestionAnswersViewModel question)
        {
            question = question ?? throw new ArgumentNullException(nameof(question));

            this.logger.LogInformation("Get question answer card initiated.");
            var questionAnswerCardContents = this.questionMapper.MapToDataModel(question);

            // Get question answers card template.
            var cardTemplate = this.GetCardTemplate(CacheKeysConstants.QuestionAnswerCardJsonTemplate, QuestionAnswerCardFileName);

            var template = new AdaptiveCardTemplate(cardTemplate);
            var card = template.Expand(questionAnswerCardContents);

            AdaptiveCard adaptiveCard = AdaptiveCard.FromJson(card).Card;
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
            };

            return attachment;
        }

        /// <summary>
        /// Get question's list card to show on Task Module.
        /// </summary>
        /// <param name="questionAnswersEntities">Collection of question answers entities.</param>
        /// <returns>Returns attachment of questions card.</returns>
        public Attachment GetQuestionsCard(IEnumerable<QuestionAnswersEntity> questionAnswersEntities)
        {
            this.logger.LogInformation("Get questions card initiated.");

            var questions = questionAnswersEntities.Select(questionAnswers => new QuestionData
            {
                QuestionText = HttpUtility.HtmlEncode(questionAnswers.Question),
                QuestionId = questionAnswers.QuestionId,
            }).ToList();

            var questionCardContents = new QuestionsCardData()
            {
                Questions = questions,
                TitleText = Strings.QuestionsCardBodyTitleText,
                ButtonText = Strings.QuestionsCardSendButtonText,
            };

            var cardTemplate = this.GetCardTemplate(CacheKeysConstants.QuestionsListCardJsonTemplate, QuestionsCardFileName);

            var template = new AdaptiveCardTemplate(cardTemplate);
            var card = template.Expand(questionCardContents);

            AdaptiveCard adaptiveCard = AdaptiveCard.FromJson(card).Card;
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
            };

            this.logger.LogInformation("Get questions card succeeded.");

            return attachment;
        }

        /// <summary>
        /// Get personal scope welcome card.
        /// </summary>
        /// <returns>Returns personal scope welcome card as attachment.</returns>
        public Attachment GetPersonalScopeWelcomeCard()
        {
            this.logger.LogInformation("Get personal scope welcome card initiated.");
            var tabDeepLinkUrl = $"https://teams.microsoft.com/l/entity/{this.botOptions.Value.ManifestId}/{SearchCoachTabEntityId}";

            var welcomeCardContents = new WelcomeCardData()
            {
                TitleText = Strings.PersonalScopeWelcomeCardTitleText,
                HeadingText = Strings.PersonalScopeWelcomeCardHeadingText,
                DeepLinkButtonText = Strings.PersonalScopeWelcomeCardDeepLinkButtonText,
                TabDeepLinkUrl = tabDeepLinkUrl,
            };

            // Get welcome card template.
            var welcomeCardTemplate = this.GetCardTemplate(CacheKeysConstants.WelcomeCardJsonTemplate, WelcomeCardPersonalScopeFileName);

            var template = new AdaptiveCardTemplate(welcomeCardTemplate);
            var card = template.Expand(welcomeCardContents);

            AdaptiveCard adaptiveCard = AdaptiveCard.FromJson(card).Card;
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
            };

            this.logger.LogInformation("Get personal scope welcome card succeeded.");

            return attachment;
        }

        /// <summary>
        /// Get error card to show error messages.
        /// </summary>
        /// <param name="errorMessage"> Error message to show on card.</param>
        /// <returns>Returns attachment of error card.</returns>
        public Attachment GetErrorMessageCard(string errorMessage)
        {
            errorMessage = errorMessage ?? throw new ArgumentNullException(nameof(errorMessage));
            this.logger.LogInformation("Get error message card initiated.");

            var errorCardContents = new ErrorCardData()
            {
                ErrorMessageText = HttpUtility.HtmlEncode(errorMessage),
            };

            var cardTemplate = this.GetCardTemplate(CacheKeysConstants.ErrorCardJsonTemplate, ErrorCardFileName);

            var template = new AdaptiveCardTemplate(cardTemplate);
            var card = template.Expand(errorCardContents);

            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = AdaptiveCard.FromJson(card).Card,
            };

            this.logger.LogInformation("Get error message card succeeded.");

            return attachment;
        }

        /// <summary>
        /// Get teams scope welcome card.
        /// </summary>
        /// <returns>Returns teams scope welcome card as attachment.</returns>
        public Attachment GetTeamsScopeWelcomeCard()
        {
            this.logger.LogInformation("Get teams scope welcome card initiated.");

            var welcomeCardContents = new WelcomeCardData()
            {
                TitleText = Strings.TeamScopeWelcomeCardTitleText,
                HeadingText = Strings.TeamScopeWelcomeCardHeadingText,
            };

            // Get welcome card template.
            var welcomeCardTemplate = this.GetCardTemplate(CacheKeysConstants.WelcomeCardTeamScopeJsonTemplate, WelcomeCardTeamsScopeFileName);

            var template = new AdaptiveCardTemplate(welcomeCardTemplate);
            var card = template.Expand(welcomeCardContents);

            AdaptiveCard adaptiveCard = AdaptiveCard.FromJson(card).Card;
            Attachment attachment = new Attachment()
            {
                ContentType = AdaptiveCard.ContentType,
                Content = adaptiveCard,
            };

            this.logger.LogInformation("Get teams scope welcome card succeeded.");

            return attachment;
        }

        /// <summary>
        /// Get question answers card template file name.
        /// </summary>
        /// <returns>Returns question answers card template file name.</returns>
        public string FetchQuestionAnswersCardFileName()
        {
            return QuestionAnswerCardFileName;
        }

        /// <summary>
        /// Get question answer card.
        /// </summary>
        /// <param name="questionAnswersEntity">Question answers entity object.</param>
        /// <param name="userName">Current user display name.</param>
        /// <param name="teamId">Unique id representing a team.</param>
        /// <param name="aadObjectId">Azure Active Directory id of current user.</param>
        /// <returns>Returns attachment as question answer card.</returns>
        public Attachment QuestionCard(
            QuestionAnswersEntity questionAnswersEntity,
            string userName,
            string teamId,
            string aadObjectId)
        {
            var questionViewModelData = this.questionMapper.MapToViewModel(
                questionAnswersEntity,
                userName,
                teamId,
                aadObjectId,
                this.botOptions.Value.ManifestId);

            return this.GetQuestionAnswerCard(questionViewModelData);
        }

        /// <summary>
        /// Get card template from memory.
        /// </summary>
        /// <param name="cardCacheKey">Card cache key.</param>
        /// <param name="cardJsonTemplateFileName">File name of JSON adaptive card template with file extension as .json to be provided.</param>
        /// <returns>Returns JSON adaptive card template string.</returns>
        private string GetCardTemplate(string cardCacheKey, string cardJsonTemplateFileName)
        {
            this.logger.LogInformation("Get card template initiated.");

            bool isCacheEntryExists = this.memoryCache.TryGetValue(cardCacheKey, out string cardTemplate);

            if (!isCacheEntryExists)
            {
                var cardJsonFilePath = Path.Combine(this.hostingEnvironment.ContentRootPath, $".\\Cards\\{cardJsonTemplateFileName}");
                cardTemplate = File.ReadAllText(cardJsonFilePath);
                this.memoryCache.Set(cardCacheKey, cardTemplate);
            }

            this.logger.LogInformation("Get card template succeeded.");

            return cardTemplate;
        }
    }
}