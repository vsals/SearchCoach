// <copyright file="TelemetrySettings.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Configuration
{
    /// <summary>
    /// Class which will help to provide Telemetry settings for Search Coach application.
    /// </summary>
    public class TelemetrySettings
    {
        /// <summary>
        /// Gets or sets Application Insights instrumentation key.
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}