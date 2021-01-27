// <copyright file="UserResponseEntity.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Models.Entity
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents user response entity used for storage and retrieval.
    /// </summary>
    public class UserResponseEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets team id to track user response.
        /// </summary>
        [Required]
        public string TeamId
        {
            get { return this.PartitionKey; }
            set { this.PartitionKey = value; }
        }

        /// <summary>
        /// Gets or sets unique response id of a question like: QuestionId_userId  for each created response.
        /// </summary>
        [Required]
        public string ResponseId
        {
            get { return this.RowKey; }
            set { this.RowKey = value; }
        }

        /// <summary>
        /// Gets or sets question id of user responded answer.
        /// </summary>
        [Required]
        public string QuestionId { get; set; }

        /// <summary>
        /// Gets or sets Azure Active Directory id if user who will submit the answer of the question.
        /// </summary>
        [Required]
        public Guid UserId { get; set; }

        /// <summary>
        /// Gets or sets date-time when user responded for a question.
        /// </summary>
        public DateTime RespondedOn { get; set; }

        /// <summary>
        /// Gets or sets responded answer by user.
        /// </summary>
        [Required]
        public string Response { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether question is attempted by user.
        /// </summary>
        public bool IsQuestionAttempted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether responded answer is correct or not.
        /// </summary>
        public bool IsCorrectAnswer { get; set; }

        /// <summary>
        /// Gets or sets date-time when question card is sent to user.
        /// </summary>
        public DateTime SentOn { get; set; }

        /// <summary>
        /// Gets or sets Azure Active Directory id if user who send the question card.
        /// </summary>
        [Required]
        public Guid SentByUserId { get; set; }
    }
}