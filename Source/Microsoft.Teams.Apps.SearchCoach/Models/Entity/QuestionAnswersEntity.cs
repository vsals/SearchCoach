// <copyright file="QuestionAnswersEntity.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.EntityModels
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents question answers entity used for storage and retrieval.
    /// </summary>
    public class QuestionAnswersEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets unique question id of question answers entry.
        /// </summary>
        [Required]
        public string QuestionId
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        /// <summary>
        /// Gets or sets question text of question answers entry.
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
    }
}