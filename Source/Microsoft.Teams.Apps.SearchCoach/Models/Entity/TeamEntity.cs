// <copyright file="TeamEntity.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Entity
{
    using System;
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// Class contains team details where application is installed.
    /// </summary>
    public class TeamEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets team id where application is installed.
        /// </summary>
        public string TeamId
        {
            get
            {
                return this.PartitionKey;
            }

            set
            {
                this.PartitionKey = value;
                this.RowKey = value;
            }
        }

        /// <summary>
        /// Gets or sets the date time when the application is installed.
        /// </summary>
        public DateTime BotInstalledOn { get; set; }

        /// <summary>
        /// Gets or sets service URL where responses to particular activity should be sent.
        /// </summary>
        public string ServicePath { get; set; }
    }
}