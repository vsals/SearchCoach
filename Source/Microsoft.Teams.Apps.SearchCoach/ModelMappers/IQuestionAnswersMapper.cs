// <copyright file="IQuestionAnswersMapper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.ModelMappers
{
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Models.DataModels;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// Interface for handling operations related to question answers view model mappings.
    /// </summary>
    public interface IQuestionAnswersMapper
    {
        /// <summary>
        /// Gets question view model from entity model.
        /// </summary>
        /// <param name="questionAnswersEntity">Question answers entity model object.</param>
        /// <param name="userName">Current user display name.</param>
        /// <param name="teamId">Team id in which user is part of.</param>
        /// <param name="sentByUserId">Azure Active Directory id of user who send the question card.</param>
        /// <param name="applicationManifestId">Application manifest id to get the logo of the application.</param>
        /// <returns>Returns a question view model object.</returns>
        public QuestionAnswersViewModel MapToViewModel(
            QuestionAnswersEntity questionAnswersEntity,
            string userName,
            string teamId,
            string sentByUserId,
            string applicationManifestId);

        /// <summary>
        /// Gets question view model from entity model.
        /// </summary>
        /// <param name="questionAnswersEntity">Question answers entity model object.</param>
        /// <param name="userName">Current user display name to show on question card.</param>
        /// <param name="answerText">Answer text to show on question card.</param>
        /// <param name="selectedOption">Option selected by the user.</param>
        /// <param name="applicationManifestId">Application manifest id to get the logo of the application.</param>
        /// <param name="isCorrectAnswer">Result to show whether given answer is correct or not.</param>
        /// <returns>Returns a question view model object.</returns>
        public QuestionAnswersViewModel MapToViewModel(
            QuestionAnswersEntity questionAnswersEntity,
            string userName,
            string answerText,
            string selectedOption,
            string applicationManifestId,
            bool isCorrectAnswer);

        /// <summary>
        /// Gets question answer data model from view model object.
        /// </summary>
        /// <param name="questionAnswersViewModel">Question answer card data model.</param>
        /// <returns>Returns a question answer card data model object.</returns>
        public QuestionAnswerCardData MapToDataModel(
            QuestionAnswersViewModel questionAnswersViewModel);
    }
}