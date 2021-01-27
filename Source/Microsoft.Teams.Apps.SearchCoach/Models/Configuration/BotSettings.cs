// <copyright file="BotSettings.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Configuration
{
    /// <summary>
    /// A class which helps to provide Bot settings for application.
    /// </summary>
    public class BotSettings
    {
        /// <summary>
        /// Gets or sets application base Uri which helps in generating customer token.
        /// </summary>
        public string AppBasePath { get; set; }

        /// <summary>
        /// Gets or sets application tenant id.
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets application id.
        /// </summary>
        public string MicrosoftAppId { get; set; }

        /// <summary>
        /// Gets or sets application password.
        /// </summary>
        public string MicrosoftAppPassword { get; set; }

        /// <summary>
        /// Gets or sets manifest schema id.
        /// </summary>
        public string ManifestId { get; set; }

        /// <summary>
        /// Gets or sets card cache interval.
        /// </summary>
        public double CardCacheDurationInHour { get; set; }
    }
}