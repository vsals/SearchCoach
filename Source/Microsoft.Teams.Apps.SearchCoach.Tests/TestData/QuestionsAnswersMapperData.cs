// <copyright file="QuestionsAnswersMapperData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.Models;
    using Microsoft.Teams.Apps.SearchCoach.Models.DataModels;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;

    /// <summary>
    /// Class that contains test data for question answers mapper methods.
    /// </summary>
    public static class QuestionsAnswersMapperData
    {
        /// <summary>
        /// Test data of question answers view model.
        /// </summary>
        public static readonly QuestionAnswersViewModel QuestionAnswersViewModel = new QuestionAnswersViewModel()
        {
            Question = "Who gave the call for Green Revolution?",
            Option1 = "M.S Swaminathan",
            Option2 = "Verghese Kurien",
            Option3 = "Tribhuvandas Patel",
            Option4 = "H.M. Dayala",
            CorrectOption = "M.S Swaminathan",
            Notes = "Father of the Green Revolution in India and renowned farm scientist M S Swaminathan has given the call for 'evergreen revolution', “which implies productivity improvement in perpetuity without ecological and social harm",
            SelectedOption = "Verghese Kurien",
            UserName = "John",
            Answer = "M.S Swaminathan",
            QuestionId = "1",
            IsCorrectAnswer = false,
            SearchPageRedirectionPath = "https://teams.microsoft.com/l/entity/00000000-0000-0000-0000-000000000000/SearchCoach",
            SentByUserId = Guid.Empty,
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
        };

        /// <summary>
        /// Test data of question answers view model with HTML contents.
        /// </summary>
        public static readonly QuestionAnswersViewModel QuestionAnswersViewModelHtmlContent = new QuestionAnswersViewModel()
        {
            Question = "Who gave the call for '&', '<', '>' or '\"' Green Revolution?",
            Option1 = "M.S Swaminathan",
            Option2 = "Verghese Kurien",
            Option3 = "Tribhuvandas Patel",
            Option4 = "H.M. Dayala",
            CorrectOption = "M.S Swaminathan",
            Notes = "Father of the Green Revolution in India and renowned farm scientist M S Swaminathan has given the call for 'evergreen revolution', “which implies productivity improvement in perpetuity without ecological and social harm",
            SelectedOption = "Verghese Kurien",
            UserName = "John",
            Answer = "M.S Swaminathan",
            QuestionId = "1",
            IsCorrectAnswer = true,
            SearchPageRedirectionPath = "https://teams.microsoft.com/l/entity/00000000-0000-0000-0000-000000000000/SearchCoach",
            SentByUserId = Guid.Empty,
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
        };

        /// <summary>
        /// Test data for question answers entity.
        /// </summary>
        public static readonly QuestionAnswersEntity QuestionAnswersEntity = new QuestionAnswersEntity()
        {
            QuestionId = "1",
            Question = "Who gave the call for Green Revolution?",
            Option1 = "M.S Swaminathan",
            Option2 = "Verghese Kurien",
            Option3 = "Tribhuvandas Patel",
            Option4 = "H.M. Dayala",
            CorrectOption = "M.S Swaminathan",
            Notes = "Father of the Green Revolution in India and renowned farm scientist M S Swaminathan has given the call for 'evergreen revolution', “which implies productivity improvement in perpetuity without ecological and social harm",
        };

        /// <summary>
        /// Test data for user name to show on card.
        /// </summary>
        public static readonly string UserName = "John";

        /// <summary>
        /// Test data for correct answer text.
        /// </summary>
        public static readonly string AnswerText = "M.S Swaminathan";

        /// <summary>
        /// Test data for selected option for response.
        /// </summary>
        public static readonly string SelectedOption = "Verghese Kurien";

        /// <summary>
        /// Test data for TeamID.
        /// </summary>
        public static readonly string TeamId = "00:0000000000000000000000000000000@abc.tacv0";

        /// <summary>
        /// Test data for Azure Active directory id of user who send the question card.
        /// </summary>
        public static readonly string SentByUserId = "00000000-0000-0000-0000-000000000000";

        /// <summary>
        /// test data for application manifest id.
        /// </summary>
        public static readonly string ApplicationManifestId = "00000000-0000-0000-0000-000000000000";

        /// <summary>
        /// Test data of question answers data model with HTML contents.
        /// </summary>
        public static readonly QuestionAnswerCardData QuestionAnswerCardDataHtmlContent = new QuestionAnswerCardData()
        {
            Question = "Who gave the call for '&', '<', '>' or '\"' Green Revolution?",
            Option1 = "M.S Swaminathan",
            Option2 = "Verghese Kurien",
            Option3 = "Tribhuvandas Patel",
            Option4 = "H.M. Dayala",
            CorrectOption = "M.S Swaminathan",
            Notes = "Father of the Green Revolution in India and renowned farm scientist M S Swaminathan has given the call for 'evergreen revolution', “which implies productivity improvement in perpetuity without ecological and social harm",
            SelectedOption = "Verghese Kurien",
            UserName = "John",
            Answer = "M.S Swaminathan",
            QuestionId = "1",
            IsCorrectAnswer = true,
            SearchPageRedirectionPath = "https://teams.microsoft.com/l/entity/00000000-0000-0000-0000-000000000000/SearchCoach",
            SentByUserId = Guid.Empty,
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
            ThankNoteText = Strings.QuestionAnswerCardThankNoteText,
            CorrectAnswerMessageText = Strings.QuestionAnswerCardCorrectAnswerMessageText,
            WrongAnswerMessageText = Strings.QuestionAnswerCardWrongAnswerMessageText,
            SubmitNoteText = Strings.QuestionAnswerCardSubmitNoteText,
            OpenUrlButtonText = Strings.QuestionAnswerCardOpenUrlButtonText,
            TextBoxLabelText = Strings.QuestionAnswerCardTextBoxLabelText,
            ShowCardTextBoxText = Strings.QuestionAnswerCardShowCardTextBoxText,
            SubmitActionButtonText = Strings.QuestionAnswerCardSubmitActionButtonText,
            UserSendMessageText = $"{"John"} {Strings.QuestionAnswerCardUserSendMessageText}",
            IsSubmitted = false,
        };

        /// <summary>
        /// Test data of question answers data model.
        /// </summary>
        public static readonly QuestionAnswerCardData QuestionAnswerCardData = new QuestionAnswerCardData()
        {
            Question = "Who gave the call for Green Revolution?",
            Option1 = "M.S Swaminathan",
            Option2 = "Verghese Kurien",
            Option3 = "Tribhuvandas Patel",
            Option4 = "H.M. Dayala",
            CorrectOption = "M.S Swaminathan",
            Notes = "Father of the Green Revolution in India and renowned farm scientist M S Swaminathan has given the call for 'evergreen revolution', “which implies productivity improvement in perpetuity without ecological and social harm",
            SelectedOption = "Verghese Kurien",
            UserName = "John",
            Answer = "M.S Swaminathan",
            QuestionId = "1",
            IsCorrectAnswer = true,
            SearchPageRedirectionPath = "https://teams.microsoft.com/l/entity/00000000-0000-0000-0000-000000000000/SearchCoach",
            SentByUserId = Guid.Empty,
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
            ThankNoteText = Strings.QuestionAnswerCardThankNoteText,
            CorrectAnswerMessageText = Strings.QuestionAnswerCardCorrectAnswerMessageText,
            WrongAnswerMessageText = Strings.QuestionAnswerCardWrongAnswerMessageText,
            SubmitNoteText = Strings.QuestionAnswerCardSubmitNoteText,
            OpenUrlButtonText = Strings.QuestionAnswerCardOpenUrlButtonText,
            TextBoxLabelText = Strings.QuestionAnswerCardTextBoxLabelText,
            ShowCardTextBoxText = Strings.QuestionAnswerCardShowCardTextBoxText,
            SubmitActionButtonText = Strings.QuestionAnswerCardSubmitActionButtonText,
            UserSendMessageText = $"{"John"} {Strings.QuestionAnswerCardUserSendMessageText}",
            IsSubmitted = false,
        };
    }
}