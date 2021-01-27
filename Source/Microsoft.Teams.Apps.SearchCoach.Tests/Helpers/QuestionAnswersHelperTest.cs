// <copyright file="QuestionAnswersHelperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Helpers
{
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Class that contains test methods for question answers helper.
    /// </summary>
    [TestClass]
    public class QuestionAnswersHelperTest
    {
        private Mock<ILogger<QuestionAnswersHelper>> logger;
        private IOptions<BotSettings> botOptions;
        private Mock<IMemoryCache> memoryCache;
        private Mock<IQuestionAnswersStorageProvider> questionAnswersStorageProvider;
        private QuestionAnswersHelper questionAnswersHelper;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<QuestionAnswersHelper>>();
            this.questionAnswersStorageProvider = new Mock<IQuestionAnswersStorageProvider>();
            this.memoryCache = new Mock<IMemoryCache>();
            this.botOptions = ConfigurationData.BotOptions;

            this.questionAnswersHelper = new QuestionAnswersHelper(
                this.logger.Object,
                this.memoryCache.Object,
                this.botOptions,
                this.questionAnswersStorageProvider.Object);
        }

        /// <summary>
        ///  Test case to check if question answers entities data is not null and valid.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task QuestionAnswersDataNotNullValidAsync()
        {
            // ARRANGE
            this.questionAnswersStorageProvider
                .Setup(x => x.GetQuestionAnswersEntitiesAsync())
                .Returns(Task.FromResult(QuestionAnswersHelperData.QuestionAnswers));

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            // ACT
            var result = await this.questionAnswersHelper.GetQuestionAnswersEntitiesAsync();

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count(), 3);
        }

        /// <summary>
        ///  Test case to check if question answers entities data is null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task QuestionAnswersDataNullAsync()
        {
            // ARRANGE
            this.questionAnswersStorageProvider
                .Setup(x => x.GetQuestionAnswersEntitiesAsync())
                .Returns(Task.FromResult(QuestionAnswersHelperData.QuestionAnswersNullData));

            this.memoryCache
                .Setup(x => x.CreateEntry(It.IsAny<object>()))
                .Returns(Mock.Of<ICacheEntry>);

            // ACT
            var result = await this.questionAnswersHelper.GetQuestionAnswersEntitiesAsync();

            // ASSERT
            Assert.IsNull(result);
        }
    }
}