// <copyright file="IBingSearchProvider.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Providers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Teams.Apps.SearchCoach.Models.BingSearch;

    /// <summary>
    /// Interface for Bing search provider methods.
    /// </summary>
    public interface IBingSearchProvider
    {
        /// <summary>
        /// Get Bing search results.
        /// </summary>
        /// <param name="searchQuery">Search filter model.</param>
        /// <returns>A task that returns list of webpages results.</returns>
        Task<IEnumerable<BingWebPagesResult>> GetBingSearchResultsAsync(SearchQuery searchQuery);
    }
}