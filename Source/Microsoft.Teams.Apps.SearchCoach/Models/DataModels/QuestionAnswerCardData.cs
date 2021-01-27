// <copyright file="QuestionAnswerCardData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.DataModels
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Question answer card data model class.
    /// </summary>
    public class QuestionAnswerCardData
    {
        /// <summary>
        /// Gets or sets thank note text value.
        /// </summary>
        [JsonProperty("thankNoteText")]
        public string ThankNoteText { get; set; }

        /// <summary>
        /// Gets or sets wrong answer message text value.
        /// </summary>
        [JsonProperty("wrongAnswerMessageText")]
        public string WrongAnswerMessageText { get; set; }

        /// <summary>
        /// Gets or sets correct answer message value.
        /// </summary>
        [JsonProperty("correctAnswerMessageText")]
        public string CorrectAnswerMessageText { get; set; }

        /// <summary>
        /// Gets or sets submit note text value.
        /// </summary>
        [JsonProperty("submitNoteText")]
        public string SubmitNoteText { get; set; }

        /// <summary>
        /// Gets or sets show card text value.
        /// </summary>
        [JsonProperty("showCardTextBoxText")]
        public string ShowCardTextBoxText { get; set; }

        /// <summary>
        /// Gets or sets text box label value.
        /// </summary>
        [JsonProperty("textBoxLabelText")]
        public string TextBoxLabelText { get; set; }

        /// <summary>
        /// Gets or sets open URL button text value.
        /// </summary>
        [JsonProperty("openUrlButtonText")]
        public string OpenUrlButtonText { get; set; }

        /// <summary>
        /// Gets or sets submit action button text value.
        /// </summary>
        [JsonProperty("submitActionButtonText")]
        public string SubmitActionButtonText { get; set; }

        /// <summary>
        /// Gets or sets user send message text value.
        /// </summary>
        [JsonProperty("userSendMessageText")]
        public string UserSendMessageText { get; set; }

        /// <summary>
        /// Gets or sets question text.
        /// </summary>
        [Required]
        [JsonProperty("question")]
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets question option first.
        /// </summary>
        [Required]
        [JsonProperty("option1")]
        public string Option1 { get; set; }

        /// <summary>
        /// Gets or sets question option second.
        /// </summary>
        [Required]
        [JsonProperty("option2")]
        public string Option2 { get; set; }

        /// <summary>
        /// Gets or sets question option third.
        /// </summary>
        [Required]
        [JsonProperty("option3")]
        public string Option3 { get; set; }

        /// <summary>
        /// Gets or sets question option fourth.
        /// </summary>
        [Required]
        [JsonProperty("option4")]
        public string Option4 { get; set; }

        /// <summary>
        /// Gets or sets correct option of question.
        /// </summary>
        [Required]
        [JsonProperty("correctOption")]
        public string CorrectOption { get; set; }

        /// <summary>
        /// Gets or sets encoded HTML content to show while answering the question.
        /// </summary>
        [MaxLength(200)]
        [JsonProperty("notes")]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets Azure Active Directory id of user who created question answers entry.
        /// </summary>
        [JsonProperty("createdBy")]
        public Guid CreatedBy { get; set; }

        /// <summary>
        /// Gets or sets user display name who send the question card.
        /// </summary>
        [JsonProperty("userName")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets unique question id.
        /// </summary>
        [JsonProperty("questionId")]
        public string QuestionId { get; set; }

        /// <summary>
        /// Gets or sets search page redirect URL for data search.
        /// </summary>
        [JsonProperty("searchPageRedirectionPath")]
        public string SearchPageRedirectionPath { get; set; }

        /// <summary>
        /// Gets or sets team id from where card is sent to user.
        /// </summary>
        [JsonProperty("teamId")]
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets user who sent the question card.
        /// </summary>
        [JsonProperty("sentByUserId")]
        public Guid SentByUserId { get; set; }

        /// <summary>
        /// Gets or sets selected option by user for question card.
        /// </summary>
        [JsonProperty("selectedOption")]
        public string SelectedOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets answer is correct or not.
        /// </summary>
        [JsonProperty("isCorrectAnswer")]
        public bool IsCorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets answer text replied by user.
        /// </summary>
        [JsonProperty("answer")]
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user responded on question or not.
        /// </summary>
        [JsonProperty("isSubmitted")]
        public bool IsSubmitted { get; set; }
    }
}
