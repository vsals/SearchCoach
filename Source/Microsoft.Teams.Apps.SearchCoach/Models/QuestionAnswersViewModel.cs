// <copyright file="QuestionAnswersViewModel.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents question answers view model for UI data binding.
    /// </summary>
    public class QuestionAnswersViewModel
    {
        /// <summary>
        /// Gets or sets question text.
        /// </summary>
        [Required]
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets question option first.
        /// </summary>
        [Required]
        public string Option1 { get; set; }

        /// <summary>
        /// Gets or sets question option second.
        /// </summary>
        [Required]
        public string Option2 { get; set; }

        /// <summary>
        /// Gets or sets question option third.
        /// </summary>
        [Required]
        public string Option3 { get; set; }

        /// <summary>
        /// Gets or sets question option fourth.
        /// </summary>
        [Required]
        public string Option4 { get; set; }

        /// <summary>
        /// Gets or sets correct option of question.
        /// </summary>
        [Required]
        public string CorrectOption { get; set; }

        /// <summary>
        /// Gets or sets encoded HTML content to show while answering the question.
        /// </summary>
        [MaxLength(200)]
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets user display name who send the question card.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets unique question id.
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// Gets or sets search page redirect URL for data search.
        /// </summary>
        public string SearchPageRedirectionPath { get; set; }

        /// <summary>
        /// Gets or sets team id from where card is sent to user.
        /// </summary>
        public string TeamId { get; set; }

        /// <summary>
        /// Gets or sets user who sent the question card.
        /// </summary>
        public Guid SentByUserId { get; set; }

        /// <summary>
        /// Gets or sets selected option by user for question card.
        /// </summary>
        public string SelectedOption { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets answer is correct or not.
        /// </summary>
        public bool IsCorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets answer text replied by user.
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether user responded on question or not.
        /// </summary>
        public bool IsSubmitted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether question is attempted by user.
        /// </summary>
        public bool IsQuestionAttempted { get; set; }
    }
}