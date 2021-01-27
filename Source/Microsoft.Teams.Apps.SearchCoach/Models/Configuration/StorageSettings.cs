// <copyright file="StorageSettings.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Configuration
{
    /// <summary>
    /// Class which will help to provide Microsoft Azure storage settings.
    /// </summary>
    public class StorageSettings
    {
        /// <summary>
        /// Gets or sets Microsoft Azure storage connection string.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}