// <copyright file="UserDetailEntity.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Entity
{
    using Microsoft.WindowsAzure.Storage.Table;

    /// <summary>
    /// User detail entity class.
    /// It is responsible for storing Azure Active Directory id of user and conversation id for sending notification to users.
    /// The value will be added when bot is installed by user/in a team.
    /// </summary>
    public class UserDetailEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets Azure Active Directory id of user.
        /// </summary>
        public string UserAadObjectId
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
        /// Gets or sets channel id for the user or bot on particular channel.
        /// </summary>
        public string UserConversationId { get; set; }

        /// <summary>
        /// Gets or sets service URL where responses to particular activity should be sent.
        /// </summary>
        public string ServicePath { get; set; }
    }
}