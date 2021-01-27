// <copyright file="CardHelpersData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    /// <summary>
    /// Class that contains test data for card helper methods.
    /// </summary>
    public static class CardHelpersData
    {
        /// <summary>
        /// Test data for error message card.
        /// </summary>
        public static readonly string ErrorMessageText = "bot is not installed";

        /// <summary>
        /// Test data for error message card that contains HTML tags.
        /// </summary>
        public static readonly string ErrorMessageTextWithHtml = "Enter a string having '&', '<', '>' or '\"' in it: <html> ";

        /// <summary>
        /// Error message card file path.
        /// </summary>
        public static readonly string ErrorMessageCardFilePath = ".\\TestData\\Cards\\ErrorCard_TestResult.json";

        /// <summary>
        /// Error message card file path that contains HTML encoded contents.
        /// </summary>
        public static readonly string HtmlEncodedErrorMessageCardFilePath = ".\\TestData\\Cards\\ErrorCard_HtmlEncoded_TestResult.json";

        /// <summary>
        /// Teams scope welcome card file path.
        /// </summary>
        public static readonly string TeamsScopeWelcomeCardFilePath = ".\\TestData\\Cards\\WelcomeCardTeamsScope_TestResult.json";

        /// <summary>
        /// Personal scope welcome card file path.
        /// </summary>
        public static readonly string PersonalScopeWelcomeCardFilePath = ".\\TestData\\Cards\\WelcomeCardPersonalScope_TestResult.json";

        /// <summary>
        /// Question list card file path.
        /// </summary>
        public static readonly string QuestionListCardFilePath = ".\\TestData\\Cards\\QuestionListCard_TestResult.json";

        /// <summary>
        /// Question list card file path that contains HTML encoded contents.
        /// </summary>
        public static readonly string HtmlEncodedQuestionListCardFilePath = ".\\TestData\\Cards\\QuestionListCard_HtlmEncoded_TestResult.json";

        /// <summary>
        /// Question answer card file path.
        /// </summary>
        public static readonly string QuestionAnswerCardFilePath = ".\\TestData\\Cards\\QuestionAnswerCard_TestResult.json";

        /// <summary>
        /// Question answer card file path that contains HTML encoded contents.
        /// </summary>
        public static readonly string HtmlEncodedQuestionAnswerCardFilePath = ".\\TestData\\Cards\\QuestionAnswerCard_HtmlEncoded_TestResult.json";
    }
}