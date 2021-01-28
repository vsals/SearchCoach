// <copyright file="BingSearchHelperData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System.Collections.Generic;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;

    /// <summary>
    /// Class that contains test data for bing search helper.
    /// </summary>
    public static class BingSearchHelperData
    {
        /// <summary>
        /// Test data for Bing search settings.
        /// </summary>
        public static readonly IOptions<BingSearchSettings> BingSearchSettings = Options.Create(new BingSearchSettings()
        {
            ApiKey = "1029fc86eb314c46afa608d341300000",
            ApiUrl = "https://www.test.com",
            SafeSearch = "safe",
        });

        /// <summary>
        /// Test data for valid country name text.
        /// </summary>
        public static readonly string CountryName = "en-US";

        /// <summary>
        /// Test data for Invalid country name text.
        /// </summary>
        public static readonly string InValidCountryName = "abc";

        /// <summary>
        /// Test data for default market value text.
        /// </summary>
        public static readonly string DefaultMarketValue = "en-US";

        /// <summary>
        /// Test data for valid domain value text.
        /// </summary>
        public static readonly string DomainValue = ".com";

        /// <summary>
        /// Test data for Invalid domain value text.
        /// </summary>
        public static readonly string InValidDomainValue = "abc";

        /// <summary>
        /// Test data for Invalid domain value text.
        /// </summary>
        public static readonly List<string> DomainList = new List<string>()
        {
            ".com",
            ".org",
        };

        /// <summary>
        /// A search query object to be passed in test.
        /// </summary>
        public static readonly SearchQuery SearchQuery = new SearchQuery
        {
            SearchString = "COVID",
            Domains = DomainList,
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = "Day",
            AppId = BingSearchSettings.Value.ApiKey,
            Market = DefaultMarketValue,
        };
    }
}