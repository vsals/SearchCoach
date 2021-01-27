// <copyright file="DataFreshness.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Common
{
    /// <summary>
    /// A class that holds constants related to data freshness.
    /// Freshness indicates how recently the data has got updated.
    /// </summary>
    public static class DataFreshness
    {
        /// <summary>
        /// Freshness of data being past 24 hours.
        /// </summary>
        public const string Day = "Day";

        /// <summary>
        /// Freshness of data being past week.
        /// </summary>
        public const string Week = "Week";

        /// <summary>
        /// Freshness of data being past month.
        /// </summary>
        public const string Month = "Month";
    }
}