// <copyright file="BingSearchController.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.ApplicationInsights;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Teams.Apps.SearchCoach.Common;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.Providers;

    /// <summary>
    /// Bing controller to handle Bing search API operations.
    /// </summary>
    [ApiController]
    [Route("api/search")]
    [Authorize]
    public class BingSearchController : BaseSearchCoachController
    {
        /// <summary>
        /// Instance to send logs to the Application Insights service.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Instance to work with Bing search API helper methods.
        /// </summary>
        private readonly ISearchHelper bingSearchHelper;

        /// <summary>
        /// Instance of Bing search provider to work with Bing search methods.
        /// </summary>
        private readonly IBingSearchProvider bingSearchProvider;

        /// <summary>
        /// Instance to mark the default market value.
        /// </summary>
        private readonly string defaultMarketValue = "nf";

        /// <summary>
        /// Initializes a new instance of the <see cref="BingSearchController"/> class.
        /// </summary>
        /// <param name="logger">Instance to send logs to the Application Insights service.</param>
        /// <param name="telemetryClient">The Application Insights telemetry client.</param>
        /// <param name="bingSearchHelper">Instance of Bing search API helper.</param>
        /// <param name="bingSearchProvider">Instance of Bing search provider to work with Bing search methods.</param>
        public BingSearchController(
        ILogger<BingSearchController> logger,
        TelemetryClient telemetryClient,
        ISearchHelper bingSearchHelper,
        IBingSearchProvider bingSearchProvider)
        : base(telemetryClient)
        {
            this.logger = logger;
            this.bingSearchHelper = bingSearchHelper;
            this.bingSearchProvider = bingSearchProvider;
        }

        /// <summary>
        /// Get Bing search results.
        /// </summary>
        /// <param name="searchText">Bing searched text from user.</param>
        /// <param name="selectedCountry">Bing selected country from location filter.</param>
        /// <param name="freshness">Bing selected freshness value from freshness filter.</param>
        /// <param name="domainValues">Bing search selected domains from domain filter.</param>
        /// <returns>A collection of Bing search results.</returns>
        [HttpGet]
        public async Task<IActionResult> GetSearchResultAsync(
            string searchText,
            string selectedCountry,
            string freshness,
            string domainValues)
        {
            try
            {
                this.RecordEvent("Bing search - HTTP Get call initiated.", RequestStatus.Initiated);

                if (string.IsNullOrEmpty(searchText))
                {
                    this.logger.LogError("Search text is either null or empty.");
                    return this.BadRequest("Search text cannot be null or empty.");
                }

                if ((string.IsNullOrEmpty(selectedCountry) || !selectedCountry.Contains(this.defaultMarketValue, StringComparison.InvariantCultureIgnoreCase)) && !this.bingSearchHelper.IsValidCountry(selectedCountry))
                {
                    this.logger.LogError("Selected country code value is null or empty or invalid.");
                    return this.BadRequest("Selected country code value is null or empty or invalid.");
                }

                var selectedDomains = new List<string>();
                if (!string.IsNullOrEmpty(domainValues))
                {
                    selectedDomains = domainValues.Split(';').ToList();

                    foreach (var domain in selectedDomains)
                    {
                        if (!this.bingSearchHelper.IsValidDomain(domain))
                        {
                            this.logger.LogError("Selected domain value is null or empty or invalid.");
                            return this.BadRequest("Selected domain value is null or empty or invalid.");
                        }
                    }
                }

                // Gets the search query model from search filter selected values.
                var searchQuery = this.bingSearchHelper.ConstructSearchQueryModel(
                    searchText,
                    selectedCountry,
                    freshness,
                    selectedDomains);

                // Gets search results by calling Bing API with search filter parameters.
                var searchResults = await this.bingSearchProvider.GetBingSearchResultsAsync(searchQuery);

                if (searchResults == null)
                {
                    this.logger.LogInformation("Bing search results not found for current search criteria.");
                    this.logger.LogInformation($"Search Text as :{searchText}, Selected Country as :{selectedCountry}, Selected Freshness value as :{freshness}, Selected Domain values as :{domainValues}");
                    this.RecordEvent("Bing search - HTTP Get call failed.", RequestStatus.Failed);
                    return this.NotFound("Bing search results not found for current search criteria.");
                }

                this.RecordEvent("Bing search - HTTP Get call succeeded.", RequestStatus.Succeeded);

                return this.Ok(searchResults);
            }
            catch (Exception ex)
            {
                this.RecordEvent("Get Bing Search Results has been failed.", RequestStatus.Failed);
                this.logger.LogError(ex, "Get Bing Search Results has been failed.");
                throw;
            }
        }
    }
}