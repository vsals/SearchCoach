// <copyright file="CardHelperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Helpers
{
    using System;
    using System.IO;
    using AdaptiveCards;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Class that contains test methods for card helper.
    /// </summary>
    [TestClass]
    public class CardHelperTest
    {
        private Mock<ILogger<CardHelper>> logger;
        private IOptions<BotSettings> botOptions;
        private Mock<IMemoryCache> memoryCache;
        private Mock<IWebHostEnvironment> hostingEnvironment;
        private CardHelper cardHelper;
        private Mock<IQuestionAnswersMapper> questionAnswersMapper;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<CardHelper>>();
            this.memoryCache = new Mock<IMemoryCache>();
            this.hostingEnvironment = new Mock<IWebHostEnvironment>();
            this.botOptions = ConfigurationData.BotOptions;
            this.questionAnswersMapper = new Mock<IQuestionAnswersMapper>();

            this.cardHelper = new CardHelper(
                this.logger.Object,
                this.memoryCache.Object,
                this.hostingEnvironment.Object,
                this.botOptions,
                this.questionAnswersMapper.Object);
        }

        /// <summary>
        ///  Test case to check if questions card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void QuestionsCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.QuestionListCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetQuestionsCard(QuestionAnswersHelperData.QuestionAnswers);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Title, expectedCardData.Card.Actions[0].Title);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Type, expectedCardData.Card.Actions[0].Type);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);

            Assert.AreEqual(
                ((AdaptiveChoiceSetInput)((AdaptiveCard)result.Content).Body[1]).Choices[0].Title,
                ((AdaptiveChoiceSetInput)expectedCardData.Card.Body[1]).Choices[0].Title);
        }

        /// <summary>
        ///  Test case to check if questions card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void QuestionsCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedQuestionListCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetQuestionsCard(QuestionAnswersHelperData.QuestionAnswersWithHtmlContent);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Title, expectedCardData.Card.Actions[0].Title);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Type, expectedCardData.Card.Actions[0].Type);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);

            Assert.AreEqual(
                ((AdaptiveChoiceSetInput)((AdaptiveCard)result.Content).Body[1]).Choices[0].Title,
                ((AdaptiveChoiceSetInput)expectedCardData.Card.Body[1]).Choices[0].Title);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct questions card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetQuestionsCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetQuestionsCard(null);
        }

        /// <summary>
        ///  Test case to check if personal scope welcome card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void PersonalScopeWelcomeCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.PersonalScopeWelcomeCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetPersonalScopeWelcomeCard();

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Title, expectedCardData.Card.Actions[0].Title);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Type, expectedCardData.Card.Actions[0].Type);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[1]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[1]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[1]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[1]).Text);
        }

        /// <summary>
        /// Test case to check if teams scope welcome card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void TeamsScopeWelcomeCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.TeamsScopeWelcomeCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetTeamsScopeWelcomeCard();

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[1]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[1]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[1]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[1]).Text);
        }

        /// <summary>
        ///  Test case to check if error message card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void ErrorMessageCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.ErrorMessageCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetErrorMessageCard(CardHelpersData.ErrorMessageText);

            // ASSERT
            Assert.IsNotNull(result);

            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);
        }

        /// <summary>
        ///  Test case to check if error message card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void ErrorMessageCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedErrorMessageCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetErrorMessageCard(CardHelpersData.ErrorMessageTextWithHtml);

            // ASSERT
            Assert.IsNotNull(result);

            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
               ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
               ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct error message card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetErrorMessageCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetErrorMessageCard(null);
        }

        /// <summary>
        ///  Test case to check if questions answer card is not null and have valid contents.
        /// </summary>
        [TestMethod]
        public void QuestionsAnswerCardNotNullValidContent()
        {
            // ARRANGE
            this.hostingEnvironment
           .Setup(m => m.ContentRootPath)
           .Returns(".");

            this.memoryCache
            .Setup(x => x.CreateEntry(It.IsAny<object>()))
            .Returns(Mock.Of<ICacheEntry>);
            var cardTemplate = File.ReadAllText(CardHelpersData.QuestionAnswerCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            this.questionAnswersMapper
                .Setup(m => m.MapToDataModel(QuestionsAnswersMapperData.QuestionAnswersViewModel))
                .Returns(QuestionsAnswersMapperData.QuestionAnswerCardData);

            // ACT
            var result = this.cardHelper.GetQuestionAnswerCard(QuestionsAnswersMapperData.QuestionAnswersViewModel);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Title, expectedCardData.Card.Actions[0].Title);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Type, expectedCardData.Card.Actions[0].Type);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Text,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Text);

            Assert.AreEqual(
                ((AdaptiveChoiceSetInput)((AdaptiveCard)result.Content).Body[2]).Choices[0].Title,
                ((AdaptiveChoiceSetInput)expectedCardData.Card.Body[2]).Choices[0].Title);
        }

        /// <summary>
        ///  Test case to check if questions answer card is not null and contains valid HTML encoded data.
        /// </summary>
        [TestMethod]
        public void QuestionAnswerCardNotNullValidHtmlContent()
        {
            // ARRANGE
            this.hostingEnvironment
                .Setup(m => m.ContentRootPath)
                .Returns(".");

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            this.questionAnswersMapper
                .Setup(m => m.MapToDataModel(QuestionsAnswersMapperData.QuestionAnswersViewModelHtmlContent))
                .Returns(QuestionsAnswersMapperData.QuestionAnswerCardDataHtmlContent);

            var cardTemplate = File.ReadAllText(CardHelpersData.HtmlEncodedQuestionAnswerCardFilePath);
            var expectedCardData = AdaptiveCard.FromJson(cardTemplate);

            // ACT
            var result = this.cardHelper.GetQuestionAnswerCard(QuestionsAnswersMapperData.QuestionAnswersViewModelHtmlContent);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions.Count, expectedCardData.Card.Actions.Count);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Title, expectedCardData.Card.Actions[0].Title);
            Assert.AreEqual(((AdaptiveCard)result.Content).Actions[0].Type, expectedCardData.Card.Actions[0].Type);
            Assert.AreEqual(((AdaptiveCard)result.Content).Body.Count, expectedCardData.Card.Body.Count);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[0]).Type,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[0]).Type);

            Assert.AreEqual(
                ((AdaptiveChoiceSetInput)((AdaptiveCard)result.Content).Body[2]).Choices[0].Title,
                ((AdaptiveChoiceSetInput)expectedCardData.Card.Body[2]).Choices[0].Title);

            Assert.AreEqual(
                ((AdaptiveTextBlock)((AdaptiveCard)result.Content).Body[1]).Text,
                ((AdaptiveTextBlock)expectedCardData.Card.Body[1]).Text);
        }

        /// <summary>
        ///  Test case to throw exception while passing null data to construct question answer card.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetQuestionAnswerCardArgumentNullException()
        {
            // ACT
            this.cardHelper.GetQuestionAnswerCard(null);
        }
    }
}