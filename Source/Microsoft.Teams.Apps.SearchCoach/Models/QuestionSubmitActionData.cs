// <copyright file="QuestionSubmitActionData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Submitted question action data model class.
    /// </summary>
    public class QuestionSubmitActionData
    {
        /// <summary>
        /// Gets or sets selected value id from a choice-set.
        /// </summary>
        [JsonProperty("choiceset")]
        public string SelectedQuestionId { get; set; }
    }
}