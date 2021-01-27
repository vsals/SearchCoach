// <copyright file="BingWebPagesResult.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.BingSearch
{
    using System;
    using Newtonsoft.Json;

    /// <summary>
    /// Bing search webpages model.
    /// </summary>
    public class BingWebPagesResult
    {
        /// <summary>
        /// Gets or sets id of web page.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [JsonProperty("name")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets URL of web page.
        /// </summary>
        [JsonProperty("url")]
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets last date crawled.
        /// </summary>
        [JsonProperty("dateLastCrawled")]
        public DateTime DateLastCrawled { get; set; }

        /// <summary>
        /// Gets or sets description.
        /// </summary>
        [JsonProperty("snippet")]
        public string Snippet { get; set; }

        /// <summary>
        /// Gets or sets published date.
        /// </summary>
        [JsonProperty("datePublished")]
        public DateTime DateLastPublished { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether navigation URL.
        /// </summary>
        [JsonProperty("isNavigational")]
        public bool IsNavigational { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether family friend.
        /// </summary>
        [JsonProperty("isFamilyFriendly")]
        public bool IsFamilyFriendly { get; set; }

        /// <summary>
        /// Gets or sets language.
        /// </summary>
        [JsonProperty("language")]
        public string Language { get; set; }
    }
}