// <copyright file="QuestionAnswersHelperData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System.Collections.Generic;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// Class that contains test data for question answers helper methods.
    /// </summary>
    public static class QuestionAnswersHelperData
    {
        /// <summary>
        /// A collection of question answers entity.
        /// </summary>
        public static readonly IEnumerable<QuestionAnswersEntity> QuestionAnswers = new List<QuestionAnswersEntity>
        {
            new QuestionAnswersEntity
            {
                Question = "Part XVIII of the constitution deals with",
                QuestionId = "1",
            },
            new QuestionAnswersEntity
            {
                Question = "(- 30) × 20 is equal to",
                QuestionId = "2",
            },
            new QuestionAnswersEntity
            {
                Question = "The fundamental duties in Indian constitution is inspired by",
                QuestionId = "3",
            },
        };

        /// <summary>
        /// A collection of question answers entity with HTML contents.
        /// </summary>
        public static readonly IEnumerable<QuestionAnswersEntity> QuestionAnswersWithHtmlContent = new List<QuestionAnswersEntity>
        {
            new QuestionAnswersEntity
            {
                Question = "Enter a string having '&', '<', '>' or '\"' in it: ",
                QuestionId = "1",
            },
            new QuestionAnswersEntity
            {
                Question = "Part XVIII of the constitution deals with < (- 30) × 20 is equal to",
                QuestionId = "2",
            },
            new QuestionAnswersEntity
            {
                Question = "Part XVIII of the constitution deals with '<', '>' or '\"' in it: ",
                QuestionId = "3",
            },
        };

        /// <summary>
        /// A collection of question answers entity.
        /// </summary>
        public static readonly IEnumerable<QuestionAnswersEntity> QuestionAnswersNullData = null;
    }
}