// <copyright file="BingSearchSettings.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Configuration
{
    /// <summary>
    /// A class which helps to provide Bing search settings.
    /// </summary>
    public class BingSearchSettings
    {
        /// <summary>
        /// Gets or sets Bing search API URL.
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets Bing search API Key.
        /// </summary>
        public string ApiKey { get; set; }

        /// <summary>
        /// Gets or sets Bing API safe search type.
        /// Safe search is used to filters webpages for adult content.
        /// Values could be "Off","Moderate","Strict".
        /// "Off" : Allow adult content , "Moderate" : "Allow webpages with adult content, but not images or videos"
        /// "Strict" : Don't allow any adult content webpages nor images or videos.
        /// </summary>
        public string SafeSearch { get; set; }

        /// <summary>
        /// Gets or sets default value for location filter (country-code) in Bing search.
        /// </summary>
        public string DefaultCountryCode { get; set; }
    }
}