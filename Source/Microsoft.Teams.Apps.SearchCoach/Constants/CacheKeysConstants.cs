// <copyright file="CacheKeysConstants.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Constants
{
    /// <summary>
    /// Constants to list keys used by cache layers in application.
    /// </summary>
    public static class CacheKeysConstants
    {
        /// <summary>
        /// Cache key for teams scope welcome card template.
        /// </summary>
        public const string WelcomeCardTeamScopeJsonTemplate = "_WCTSTemplate";

        /// <summary>
        /// Cache key for personal scope welcome card template.
        /// </summary>
        public const string WelcomeCardJsonTemplate = "_WCTemplate";

        /// <summary>
        /// Cache key for error card template.
        /// </summary>
        public const string ErrorCardJsonTemplate = "_ECTemplate";

        /// <summary>
        /// Cache key for question answers entities.
        /// </summary>
        public const string QuestionAnswersEntityKey = "_QAEntity";

        /// <summary>
        /// Cache key for answer card template.
        /// </summary>
        public const string QuestionAnswerCardJsonTemplate = "_QATemplate";

        /// <summary>
        /// Cache key for questions list card template.
        /// </summary>
        public const string QuestionsListCardJsonTemplate = "_QLCTemplate";
    }
}