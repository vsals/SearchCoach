// <copyright file="SubmitAnswerData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Submitted answer data model class.
    /// </summary>
    public class SubmitAnswerData
    {
        /// <summary>
        /// Gets or sets selected answer value for a submitted answer response.
        /// </summary>
        [JsonProperty("choiceset")]
        public string SelectedAnswerValue { get; set; }

        /// <summary>
        /// Gets or sets team id for which user is a part of.
        /// </summary>
        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets question id of submitted answer response.
        /// </summary>
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }
    }
}