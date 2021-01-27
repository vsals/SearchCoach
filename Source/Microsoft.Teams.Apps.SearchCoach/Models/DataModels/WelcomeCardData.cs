// <copyright file="WelcomeCardData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.DataModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// Welcome card data model class.
    /// </summary>
    public class WelcomeCardData
    {
        /// <summary>
        /// Gets or sets title text to show on welcome card.
        /// </summary>
        [JsonProperty("titleText")]
        public string TitleText { get; set; }

        /// <summary>
        /// Gets or sets heading text to show on welcome card.
        /// </summary>
        [JsonProperty("headingText")]
        public string HeadingText { get; set; }

        /// <summary>
        /// Gets or sets deep-link button text to show on welcome card.
        /// </summary>
        [JsonProperty("deepLinkButtonText")]
        public string DeepLinkButtonText { get; set; }

        /// <summary>
        /// Gets or sets deep-link URL text to show on welcome card.
        /// </summary>
        [JsonProperty("tabDeepLinkUrl")]
        public string TabDeepLinkUrl { get; set; }
    }
}