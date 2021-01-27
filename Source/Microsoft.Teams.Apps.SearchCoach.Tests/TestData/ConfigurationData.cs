// <copyright file="ConfigurationData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using Microsoft.Extensions.Options;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;

    /// <summary>
    /// Class that contains test data for configuration settings.
    /// </summary>
    public static class ConfigurationData
    {
        /// <summary>
        /// Test data for bot settings.
        /// </summary>
        public static readonly IOptions<BotSettings> BotOptions = Options.Create(new BotSettings()
        {
            MicrosoftAppId = string.Empty,
            MicrosoftAppPassword = string.Empty,
            TenantId = "12345",
            AppBasePath = "https://test.azurewebsites.net",
            CardCacheDurationInHour = 1,
            ManifestId = "12345",
        });

        /// <summary>
        /// Test data for storage settings.
        /// </summary>
        public static readonly StorageSettings StorageSettings = new StorageSettings()
        {
            ConnectionString = "abc",
        };

        /// <summary>
        /// Test data for Telemetry settings.
        /// </summary>
        public static readonly TelemetrySettings TelemetrySettings = new TelemetrySettings()
        {
            InstrumentationKey = "abc",
        };
    }
}