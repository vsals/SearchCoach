// <copyright file="BingSearchHelperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Helpers
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Class to test Bing search helper methods.
    /// </summary>
    [TestClass]
#pragma warning disable CA1001 // Types that own disposable fields should be disposable
    public class BingSearchHelperTest
#pragma warning restore CA1001 // Types that own disposable fields should be disposable
    {
        private Mock<ILogger<BingSearchHelper>> logger;
        private IOptions<BingSearchSettings> bingSearchSettings;
        private HttpClient httpClient;
        private BingSearchHelper bingSearchHelper;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.logger = new Mock<ILogger<BingSearchHelper>>();
            this.bingSearchSettings = BingSearchHelperData.BingSearchSettings;
            this.httpClient = new HttpClient();

            this.bingSearchHelper = new BingSearchHelper(
                this.logger.Object,
                this.httpClient,
                this.bingSearchSettings);
        }

        /// <summary>
        /// Test case to check if country code is valid.
        /// </summary>
        [TestMethod]
        public void CountryCodeIsValid()
        {
            // ARRANGE
            var country = BingSearchHelperData.CountryName;

            // ACT
            var result = this.bingSearchHelper.IsValidCountry(country);

            // ASSERT
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test case to check if country code is not valid.
        /// </summary>
        [TestMethod]
        public void CountryCodeIsNotValid()
        {
            // ARRANGE
            var country = BingSearchHelperData.InValidCountryName;

            // ACT
            var result = this.bingSearchHelper.IsValidCountry(country);

            // ASSERT
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test case to check if default country code value is valid.
        /// </summary>
        [TestMethod]
        public void DefaultCountryCodeIsValid()
        {
            // ARRANGE
            var country = BingSearchHelperData.DefaultMarketValue;

            // ACT
            var result = this.bingSearchHelper.IsValidCountry(country);

            // ASSERT
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test case to check country code is null.
        /// </summary>
        [TestMethod]
        public void CountryCodeNullCheck()
        {
            // ARRANGE
            string country = null;

            // ACT
            var result = this.bingSearchHelper.IsValidCountry(country);

            // ASSERT
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test case to check if domain value is valid.
        /// </summary>
        [TestMethod]
        public void DomainValueIsValid()
        {
            // ARRANGE
            var country = BingSearchHelperData.DomainValue;

            // ACT
            var result = this.bingSearchHelper.IsValidDomain(country);

            // ASSERT
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Test case to check if domain value is not valid.
        /// </summary>
        [TestMethod]
        public void DomainValueIsInValid()
        {
            // ARRANGE
            var country = BingSearchHelperData.InValidDomainValue;

            // ACT
            var result = this.bingSearchHelper.IsValidDomain(country);

            // ASSERT
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test case to null check for domain value.
        /// </summary>
        [TestMethod]
        public void DomainValueNullCheck()
        {
            // ARRANGE
            string country = null;

            // ACT
            var result = this.bingSearchHelper.IsValidDomain(country);

            // ASSERT
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Test case to check if search query model is not null and valid.
        /// </summary>
        [TestMethod]
        public void ConstructSearchQueryModelNotNullValid()
        {
            // ARRANGE
            var searchQuery = BingSearchHelperData.SearchQuery;

            // ACT
            var result = this.bingSearchHelper.ConstructSearchQueryModel(
                BingSearchResultData.SearchString,
                BingSearchResultData.MarketValue,
                BingSearchResultData.FreshnessValue,
                BingSearchHelperData.DomainList);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.AppId, searchQuery.AppId);
            Assert.AreEqual(result.Count, searchQuery.Count);
            Assert.AreEqual(result.Domains, searchQuery.Domains);
            Assert.AreEqual(result.Freshness, searchQuery.Freshness);
            Assert.AreEqual(result.Market, searchQuery.Market);
            Assert.AreEqual(result.Offset, searchQuery.Offset);
            Assert.AreEqual(result.SearchString, searchQuery.SearchString);
        }

        /// <summary>
        /// Test case to check if invalid values are passed method returns null.
        /// </summary>
        /// <returns>A task that represents the work queued to execute.</returns>
        [TestMethod]
        public async Task GetBingSearchResultsExceptionTestAsync()
        {
            // ARRANGE
            this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", BingSearchHelperData.BingSearchSettings.Value.ApiKey);

            // ACT
            var result = await this.bingSearchHelper.GetBingSearchResultsAsync(BingSearchHelperData.SearchQuery).ConfigureAwait(false);

            // ASSERT
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test case to check if passing null data to get Bing search result returns null.
        /// </summary>
        [TestMethod]
        public void GetBingSearchResultsNullCheck()
        {
            // ACT
            var result = this.bingSearchHelper.GetBingSearchResultsAsync(null);

            // ASSERT
            Assert.IsNull(result.Result);
        }
    }
}