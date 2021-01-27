// <copyright file="QuestionsCardData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.DataModels
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Questions list card data model class.
    /// </summary>
    public class QuestionsCardData
    {
        /// <summary>
        /// Gets or sets a collection of question entities.
        /// </summary>
        [JsonProperty("QuestionList")]
        public IEnumerable<QuestionData> Questions { get; set; }

        /// <summary>
        /// Gets or sets question title text to show on questions card.
        /// </summary>
        [JsonProperty("questionsCardBodyTitleText")]
        public string TitleText { get; set; }

        /// <summary>
        /// Gets or sets text to show on questions card button.
        /// </summary>
        [JsonProperty("questionsCardSendButtonText")]
        public string ButtonText { get; set; }
    }
}