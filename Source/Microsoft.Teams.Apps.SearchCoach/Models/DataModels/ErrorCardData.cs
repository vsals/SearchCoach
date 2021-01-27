// <copyright file="ErrorCardData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.DataModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// Error card data model class.
    /// </summary>
    public class ErrorCardData
    {
        /// <summary>
        /// Gets or sets error message text to show on card.
        /// </summary>
        [JsonProperty("errorMessage")]
        public string ErrorMessageText { get; set; }
    }
}