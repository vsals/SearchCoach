// <copyright file="QuestionAnswersMapper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.ModelMappers
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Models.DataModels;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// A model class that contains methods related to question model mappings.
    /// </summary>
    public class QuestionAnswersMapper : IQuestionAnswersMapper
    {
        /// <summary>
        /// Search coach tab entity Id.
        /// </summary>
        public const string SearchCoachTabEntityId = "SearchCoach";

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
            string applicationManifestId)
        {
            questionAnswersEntity = questionAnswersEntity ?? throw new ArgumentNullException(nameof(questionAnswersEntity));

            return new QuestionAnswersViewModel()
            {
                Question = questionAnswersEntity.Question,
                Option1 = questionAnswersEntity.Option1,
                Option2 = questionAnswersEntity.Option2,
                Option3 = questionAnswersEntity.Option3,
                Option4 = questionAnswersEntity.Option4,
                CorrectOption = questionAnswersEntity.CorrectOption,
                Notes = questionAnswersEntity.Notes,
                UserName = userName,
                QuestionId = questionAnswersEntity.QuestionId,
                SearchPageRedirectionPath = $"https://teams.microsoft.com/l/entity/{applicationManifestId}/{SearchCoachTabEntityId}",
                TeamId = teamId,
                SentByUserId = Guid.Parse(sentByUserId),
                IsSubmitted = false,
                IsCorrectAnswer = true,
                IsQuestionAttempted = false,
            };
        }

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
            bool isCorrectAnswer)
        {
            questionAnswersEntity = questionAnswersEntity ?? throw new ArgumentNullException(nameof(questionAnswersEntity));

            return new QuestionAnswersViewModel()
            {
                Question = questionAnswersEntity.Question,
                Option1 = questionAnswersEntity.Option1,
                Option2 = questionAnswersEntity.Option2,
                Option3 = questionAnswersEntity.Option3,
                Option4 = questionAnswersEntity.Option4,
                CorrectOption = questionAnswersEntity.CorrectOption,
                Notes = questionAnswersEntity.Notes,
                UserName = userName,
                QuestionId = questionAnswersEntity.QuestionId,
                SearchPageRedirectionPath = $"https://teams.microsoft.com/l/entity/{applicationManifestId}/{SearchCoachTabEntityId}",
                Answer = answerText,
                IsCorrectAnswer = isCorrectAnswer,
                SelectedOption = selectedOption,
                IsSubmitted = true,
            };
        }

        /// <summary>
        /// Gets question answer data model from view model object.
        /// </summary>
        /// <param name="questionAnswersViewModel">Question answer card data model.</param>
        /// <returns>Returns a question answer card data model object.</returns>
        public QuestionAnswerCardData MapToDataModel(
            QuestionAnswersViewModel questionAnswersViewModel)
        {
            questionAnswersViewModel = questionAnswersViewModel ?? throw new ArgumentNullException(nameof(questionAnswersViewModel));

            return new QuestionAnswerCardData()
            {
                ThankNoteText = Strings.QuestionAnswerCardThankNoteText,
                CorrectAnswerMessageText = Strings.QuestionAnswerCardCorrectAnswerMessageText,
                WrongAnswerMessageText = Strings.QuestionAnswerCardWrongAnswerMessageText,
                SubmitNoteText = Strings.QuestionAnswerCardSubmitNoteText,
                OpenUrlButtonText = Strings.QuestionAnswerCardOpenUrlButtonText,
                TextBoxLabelText = Strings.QuestionAnswerCardTextBoxLabelText,
                ShowCardTextBoxText = Strings.QuestionAnswerCardShowCardTextBoxText,
                SubmitActionButtonText = Strings.QuestionAnswerCardSubmitActionButtonText,
                UserSendMessageText = $"{questionAnswersViewModel.UserName} {Strings.QuestionAnswerCardUserSendMessageText}",
                Answer = questionAnswersViewModel.Answer,
                CorrectOption = questionAnswersViewModel.CorrectOption,
                IsCorrectAnswer = questionAnswersViewModel.IsCorrectAnswer,
                IsSubmitted = questionAnswersViewModel.IsSubmitted,
                Notes = questionAnswersViewModel.Notes,
                Option1 = questionAnswersViewModel.Option1,
                Option2 = questionAnswersViewModel.Option2,
                Option3 = questionAnswersViewModel.Option3,
                Option4 = questionAnswersViewModel.Option4,
                Question = questionAnswersViewModel.Question,
                QuestionId = questionAnswersViewModel.QuestionId,
                SearchPageRedirectionPath = questionAnswersViewModel.SearchPageRedirectionPath,
                SelectedOption = questionAnswersViewModel.SelectedOption,
                TeamId = questionAnswersViewModel.TeamId,
                SentByUserId = questionAnswersViewModel.SentByUserId,
                UserName = questionAnswersViewModel.UserName,
            };
        }
    }
}