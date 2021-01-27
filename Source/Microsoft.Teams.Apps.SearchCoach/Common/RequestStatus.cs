// <copyright file="RequestStatus.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Common
{
    /// <summary>
    /// Record logging request status.
    /// </summary>
    public enum RequestStatus
    {
        /// <summary>
        /// This represents the request is initiated.
        /// </summary>
        Initiated,

        /// <summary>
        /// This represents the request is completed.
        /// </summary>
        Succeeded,

        /// <summary>
        /// This represents the request is failed.
        /// </summary>
        Failed,
    }
}