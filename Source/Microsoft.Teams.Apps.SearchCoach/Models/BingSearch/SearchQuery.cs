// <copyright file="SearchQuery.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.BingSearch
{
    using System.Collections.Generic;

    /// <summary>
    /// Bing search query model.
    /// </summary>
    public class SearchQuery
    {
        /// <summary>
        /// Gets or sets search text for Bing API.
        /// </summary>
        public string SearchString { get; set; }

        /// <summary>
        /// Gets or sets search domains for Bing API.
        /// </summary>
#pragma warning disable CA2227 // Collection properties should be read only
        public List<string> Domains { get; set; }
#pragma warning restore CA2227 // Collection properties should be read only

        /// <summary>
        /// Gets or sets a value indicating whether error is there or not.
        /// </summary>
        public bool Error { get; set; }

        /// <summary>
        /// Gets or sets count.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets offset.
        /// Offset determines the number of records to be skipped before returning results.
        /// Default value is 0.
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Gets or sets time freshness of contents.
        /// </summary>
        public string Freshness { get; set; }

        /// <summary>
        /// Gets or sets application id.
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// Gets or sets market value.
        /// Market value indicates the country-code selected through location filter.
        /// </summary>
        public string Market { get; set; }
    }
}