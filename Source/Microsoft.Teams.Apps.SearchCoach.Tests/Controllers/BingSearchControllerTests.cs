// <copyright file="BingSearchControllerTests.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Controllers;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Tests.Fakes;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// The BingSearchControllerTests contains test cases related to Bing Search API controller.
    /// </summary>
    [TestClass]
    public class BingSearchControllerTests
    {
        private Mock<ILogger<BingSearchController>> logger;
        private TelemetryClient telemetryClient;
        private BingSearchController bingSearchController;
        private Mock<ISearchHelper> bingSearchHelper;
        private Mock<IBingSearchProvider> bingSearchProvider;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<BingSearchController>>();
            this.telemetryClient = new TelemetryClient();
            this.bingSearchHelper = new Mock<ISearchHelper>();
            this.bingSearchProvider = new Mock<IBingSearchProvider>();

            this.bingSearchController = new BingSearchController(
                this.logger.Object,
                this.telemetryClient,
                this.bingSearchHelper.Object,
                this.bingSearchProvider.Object);

            this.bingSearchController.ControllerContext = new ControllerContext();
            this.bingSearchController.ControllerContext.HttpContext =
                FakeHttpContext.GetMockHttpContextWithUserClaims();
        }

        /// <summary>
        /// Test case to check if correct input is passed method returns valid search results.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchValidInputValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(
                    BingSearchResultData.SearchQuery))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(BingSearchResultData.SearchString, BingSearchResultData.MarketValue, BingSearchResultData.FreshnessValue, BingSearchResultData.DomainValue).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Test case to check if invalid domain values are passed method returns bad request.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchInvalidDomainBadRequestAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(
                    BingSearchResultData.SearchQueryWithInvalidDomain))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValue))
                .Returns(true);

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(BingSearchResultData.SearchString, BingSearchResultData.MarketValue, BingSearchResultData.FreshnessValue, BingSearchResultData.DomainValue).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status400BadRequest);
        }

        /// <summary>
        /// Test case to check if search text contains HTML method returns valid results or not.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchTextWithHtmlValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(
                    BingSearchResultData.SearchQueryWithSearchTextAsHtml))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValue))
                .Returns(true);

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(BingSearchResultData.SearchStringWithHtmlContent, BingSearchResultData.MarketValue, BingSearchResultData.FreshnessValue, BingSearchResultData.DomainValue).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }

        /// <summary>
        /// Test case to check if search text contains scripts method returns valid results or not.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task BingSearchTextWithScriptsValidContentsAsync()
        {
            // ARRANGE
            this.bingSearchProvider
                .Setup(x => x.GetBingSearchResultsAsync(
                    BingSearchResultData.SearchQueryWithSearchTextAsScript))
                .Returns(Task.FromResult(new List<BingWebPagesResult>().AsEnumerable()));

            this.bingSearchHelper
                .Setup(x => x.IsValidCountry(BingSearchResultData.MarketValue))
                .Returns(true);

            this.bingSearchHelper
                .Setup(x => x.IsValidDomain(BingSearchResultData.DomainValue))
                .Returns(true);

            // ACT
            var result = (ObjectResult)await this.bingSearchController.GetSearchResultAsync(BingSearchResultData.SearchStringWithScriptContent, BingSearchResultData.MarketValue, BingSearchResultData.FreshnessValue, BingSearchResultData.DomainValue).ConfigureAwait(false);

            // ASSERT
            Assert.AreEqual(result.StatusCode, StatusCodes.Status200OK);
        }
    }
}