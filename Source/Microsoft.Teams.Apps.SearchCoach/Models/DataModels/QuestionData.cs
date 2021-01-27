// <copyright file="QuestionData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.DataModels
{
    using Newtonsoft.Json;

    /// <summary>
    /// Question data model class.
    /// </summary>
    public class QuestionData
    {
        /// <summary>
        /// Gets or sets question text value.
        /// </summary>
        [JsonProperty("title")]
        public string QuestionText { get; set; }

        /// <summary>
        /// Gets or sets question id value.
        /// </summary>
        [JsonProperty("value")]
        public string QuestionId { get; set; }
    }
}