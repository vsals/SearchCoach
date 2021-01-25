// <copyright file="BingSearchHelper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Bing search helper class to work with Bing search API methods.
    /// </summary>
    public class BingSearchHelper : ISearchHelper, IBingSearchProvider
    {
        /// <summary>
        /// Country codes to validate selected country code value.
        /// "nf" indicates No Filter condition.
        /// </summary>
        private readonly string[] countryCodes = { "en-US", "ja-JP", "fr-FR", "de-DE", "it-IT", "ru-RU", "ko-KR" };

        /// <summary>
        /// Domain values to validate selected domain value.
        /// </summary>
        private readonly string[] domainValues = { ".com", ".org", ".mil", ".gov", ".edu", ".net" };

        /// <summary>
        /// Instance to send logs to the logger service.
        /// </summary>
        private readonly ILogger<BingSearchHelper> logger;

        /// <summary>
        /// Provides a base class for sending HTTP requests and receiving HTTP responses from a resource identified by a URI.
        /// </summary>
        private readonly HttpClient httpClient;

        /// <summary>
        /// Instance to mark the default market value.
        /// </summary>
        private readonly string defaultMarketValue = "nf";

        /// <summary>
        /// Instance to fetch the number of search results.
        /// </summary>
        private readonly int topSearchResult = 20;

        /// <summary>
        /// Instance to mark the default offset value for search results.
        /// </summary>
        private readonly int defaultOffsetValue;

        /// <summary>
        /// A set of key/value application configuration properties for Bing search settings.
        /// </summary>
        private readonly IOptions<BingSearchSettings> bingSearchSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="BingSearchHelper"/> class.
        /// </summary>
        /// <param name="logger">Logger implementation to send logs to the logger service.</param>
        /// <param name="httpClient">Instance of HttpClient.</param>
        /// <param name="bingSearchSettings">A set of key/value application configuration properties for Bing search.</param>
        public BingSearchHelper(
            ILogger<BingSearchHelper> logger,
            HttpClient httpClient,
            IOptions<BingSearchSettings> bingSearchSettings)
        {
            this.logger = logger;
            this.httpClient = httpClient;
            this.bingSearchSettings = bingSearchSettings ?? throw new ArgumentNullException(nameof(bingSearchSettings));
        }

        /// <summary>
        /// Validate country code value is valid or not.
        /// </summary>
        /// <param name="selectedCountryCode">Selected country code value.</param>
        /// <returns>Returns whether selected country is valid or not.</returns>
        public bool IsValidCountry(string selectedCountryCode)
        {
            try
            {
                if (selectedCountryCode == null)
                {
                    this.logger.LogError("Country code is null or empty.");
                    return false;
                }

                // Check if the selected country code value belongs to the list of valid country codes or not.
                bool isSelectedCountryCodeValid = this.countryCodes.Where(x => selectedCountryCode.Contains(x, StringComparison.InvariantCultureIgnoreCase)).Any();

                if (!isSelectedCountryCodeValid)
                {
                    this.logger.LogError("Country code is invalid.");
                    return false;
                }
                else
                {
                    // Selected country code is valid.
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while validating country code.");
                return false;
            }
        }

        /// <summary>
        /// Validate domain value is valid or not.
        /// </summary>
        /// <param name="selectedDomainValue">Selected domain value.</param>
        /// <returns>Returns whether selected domain is valid or not.</returns>
        public bool IsValidDomain(string selectedDomainValue)
        {
            try
            {
                if (selectedDomainValue == null)
                {
                    this.logger.LogError("Domain value is null or empty.");
                    return false;
                }

                // Check if the selected domain value belongs to the list of valid domains or not.
                bool isSelectedDomainValueValid = this.domainValues.Where(domain => domain.Contains(selectedDomainValue, StringComparison.InvariantCultureIgnoreCase)).Any();

                if (!isSelectedDomainValueValid)
                {
                    this.logger.LogError("Domain value data is invalid.");
                    return false;
                }
                else
                {
                    // Selected domain value is valid.
                    return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while validating domain value.");
                return false;
            }
        }

        /// <summary>
        /// Construct search query model data.
        /// </summary>
        /// <param name="searchText">Bing searched text from user.</param>
        /// <param name="selectedCountry">Bing selected country from location filter.</param>
        /// <param name="freshness">Bing selected freshness value from freshness filter.</param>
        /// <param name="domainValues">Bing search selected domains from domain filter.</param>
        /// <returns>Returns search query model data.</returns>
        public SearchQuery ConstructSearchQueryModel(
            string searchText,
            string selectedCountry,
            string freshness,
            List<string> domainValues)
        {
            try
            {
                return new SearchQuery()
                {
                    AppId = this.bingSearchSettings.Value.ApiKey,
                    SearchString = searchText?.Trim(),
                    Count = this.topSearchResult,
                    Offset = this.defaultOffsetValue,
                    SafeSearch = this.bingSearchSettings.Value.SafeSearch,
                    Market = selectedCountry,
                    Freshness = freshness,
                    Domains = domainValues,
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while constructing search query model data.");
                return null;
            }
        }

        /// <summary>
        /// Get Bing search results.
        /// </summary>
        /// <param name="searchQuery">Bing search filter model.</param>
        /// <returns>A task that returns list of Bing search results.</returns>
        public async Task<IEnumerable<BingWebPagesResult>> GetBingSearchResultsAsync(SearchQuery searchQuery)
        {
            try
            {
                if (searchQuery == null)
                {
                    this.logger.LogError("SearchQuery object is null or empty.");
                    return null;
                }

                this.httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", this.bingSearchSettings.Value.ApiKey);

                var bingSearchResults = new List<BingWebPagesResult>();

                // If market value is default ("nf"), then we would consider it as "en-US".
                if (searchQuery.Market == this.defaultMarketValue)
                {
                    searchQuery.Market = this.bingSearchSettings.Value.DefaultCountryCode;
                }

                string requestUri = this.ConstructRequestURI(searchQuery);

                HttpResponseMessage response = await this.httpClient.GetAsync(new Uri(requestUri));
                string contentString = await response.Content.ReadAsStringAsync();
                JObject siteListDataResponse = JObject.Parse(contentString);

                // Get webpages details.
                List<BingWebPagesResult> bingSearchWebPages = new List<BingWebPagesResult>();

                if (siteListDataResponse["webPages"]["value"] != null)
                {
                    var webPagesResult = siteListDataResponse["webPages"]["value"].ToString();
                    if (webPagesResult != null)
                    {
                        bingSearchWebPages = JsonConvert.DeserializeObject<List<BingWebPagesResult>>(webPagesResult);
                    }
                }
                else
                {
                    this.logger.LogInformation("Bing search webpages results are not available.");
                }

                bingSearchResults.AddRange(bingSearchWebPages);

                return bingSearchResults;
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex, "Error while fetching Bing search results.");
                return null;
            }
        }

        /// <summary>
        /// Method to construct Bing Search request Uri.
        /// We have passed in key values for freshness instead of UI facing strings
        /// 1 -> All , 2 => Day, 3 => Week , 4 => Month.
        /// Refer https://docs.microsoft.com/en-us/rest/api/cognitiveservices-bingsearch/bing-web-api-v7-reference
        /// for more details.
        /// </summary>
        /// <param name="searchQuery">Search query object to construct request Uri.</param>
        /// <returns>Returns a request Uri value for Bing Search.</returns>
        internal string ConstructRequestURI(SearchQuery searchQuery)
        {
            string freshnessQueryValue = string.Empty;
            if (!string.IsNullOrEmpty(searchQuery.Freshness))
            {
                switch (searchQuery.Freshness)
                {
                    case "1":
                        freshnessQueryValue = string.Empty;
                        break;
                    case "2":
                        freshnessQueryValue = DataFreshness.Day;
                        break;
                    case "3":
                        freshnessQueryValue = DataFreshness.Week;
                        break;
                    case "4":
                        freshnessQueryValue = DataFreshness.Month;
                        break;
                    default:
                        freshnessQueryValue = DataFreshness.Month;
                        break;
                }
            }

            var searchString = searchQuery.SearchString;

            if (searchQuery.Domains.Count > 0)
            {
                string selectedDomainText = "site:" + string.Join(" OR site:", searchQuery.Domains);

                if (!string.IsNullOrEmpty(selectedDomainText))
                {
                    searchString += " (" + selectedDomainText + ")";
                }
            }

            // Kept safe search as "strict" to block all adult content webpages.
            string requestUri = this.bingSearchSettings.Value.ApiUrl
                + "?mkt=" + searchQuery.Market
                + "&count=" + this.topSearchResult
                + "&freshness=" + freshnessQueryValue
                + "&safeSearch=" + this.bingSearchSettings.Value.SafeSearch
                + "&offset=" + searchQuery.Offset.ToString(CultureInfo.InvariantCulture)
                + "&q=" + Uri.EscapeDataString(searchString);

            return requestUri;
        }
    }
}