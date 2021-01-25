// <copyright file="BingSearchResultData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;

    /// <summary>
    /// Class that contains test data for bing search results.
    /// </summary>
    public static class BingSearchResultData
    {
        /// <summary>
        /// Search string value to be used in test case.
        /// </summary>
        public static readonly string SearchString = "COVID";

        /// <summary>
        /// Search string value with HTML content to be used in test case.
        /// </summary>
        public static readonly string SearchStringWithHtmlContent = "COVID <html></html>";

        /// <summary>
        /// Search string value with JavaScript content to be used in test case.
        /// </summary>
        public static readonly string SearchStringWithScriptContent = "COVID <script></script>";

        /// <summary>
        /// Market value to be used in test case.
        /// </summary>
        public static readonly string MarketValue = "en-US";

        /// <summary>
        /// Domain value to be used in test case.
        /// </summary>
        public static readonly string DomainValue = ".com";

        /// <summary>
        /// Invalid Domain value to be used in test case.
        /// </summary>
        public static readonly string InvalidDomainValue = ".us";

        /// <summary>
        /// Freshness value to be used in test case.
        /// </summary>
        public static readonly string FreshnessValue = "Day";

        /// <summary>
        /// Safe search value to be used in test case.
        /// </summary>
        public static readonly string SafeSearch = "safe";

        /// <summary>
        /// The AppId value to be used in test case.
        /// </summary>
        public static readonly string AppId = "12345";

        /// <summary>
        /// A search query object to be passed in test.
        /// </summary>
        public static readonly SearchQuery SearchQuery = new SearchQuery
        {
            SearchString = SearchString,
            Domains = new List<string> { DomainValue },
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            SafeSearch = SafeSearch,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having HTML content in searched text to be passed in test.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithSearchTextAsHtml = new SearchQuery
        {
            SearchString = SearchStringWithHtmlContent,
            Domains = new List<string> { DomainValue },
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            SafeSearch = SafeSearch,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having JavaScript content in searched text to be passed in test.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithSearchTextAsScript = new SearchQuery
        {
            SearchString = SearchStringWithScriptContent,
            Domains = new List<string> { DomainValue },
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            SafeSearch = SafeSearch,
            AppId = AppId,
            Market = MarketValue,
        };

        /// <summary>
        /// A search query object having invalid domain value to be passed in test.
        /// </summary>
        public static readonly SearchQuery SearchQueryWithInvalidDomain = new SearchQuery
        {
            SearchString = SearchString,
            Domains = new List<string> { InvalidDomainValue },
            Error = false,
            Count = 20,
            Offset = 0,
            Freshness = FreshnessValue,
            SafeSearch = SafeSearch,
            AppId = AppId,
            Market = MarketValue,
        };
    }
}
