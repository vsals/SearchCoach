// <copyright file="QuestionAnswerMapperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Mappers
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.EntityModels;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class that contains test methods for question answer model mapper.
    /// </summary>
    [TestClass]
    public class QuestionAnswerMapperTest
    {
        private QuestionAnswersMapper questionAnswerMapper;

        /// <summary>
        /// Initialize all test variables.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.questionAnswerMapper = new QuestionAnswersMapper();
        }

        /// <summary>
        ///  Test case to check if mapped view model data is not null and valid.
        /// </summary>
        [TestMethod]
        public void QuestionCardMapToViewModelNotNullValid()
        {
            // ARRANGE
            var questionAnswersEntity = QuestionsAnswersMapperData.QuestionAnswersEntity;
            var questionAnswersViewModel = QuestionsAnswersMapperData.QuestionAnswersViewModel;

            // ACT
            var result = this.questionAnswerMapper.MapToViewModel(
                questionAnswersEntity,
                QuestionsAnswersMapperData.UserName,
                QuestionsAnswersMapperData.TeamId,
                QuestionsAnswersMapperData.SentByUserId,
                QuestionsAnswersMapperData.ApplicationManifestId);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Question, questionAnswersViewModel.Question);
            Assert.AreEqual(result.Option4, questionAnswersViewModel.Option4);
            Assert.AreEqual(result.Option1, questionAnswersViewModel.Option1);
            Assert.AreEqual(result.Option3, questionAnswersViewModel.Option3);
            Assert.AreEqual(result.Option2, questionAnswersViewModel.Option2);
            Assert.AreEqual(result.CorrectOption, questionAnswersViewModel.CorrectOption);
            Assert.AreEqual(result.TeamId, questionAnswersViewModel.TeamId);
            Assert.AreEqual(result.IsCorrectAnswer, questionAnswersViewModel.IsCorrectAnswer);
            Assert.AreEqual(result.SearchPageRedirectionPath, questionAnswersViewModel.SearchPageRedirectionPath);
            Assert.AreEqual(result.Notes, questionAnswersViewModel.Notes);
            Assert.AreEqual(result.UserName, questionAnswersViewModel.UserName);
            Assert.AreEqual(result.SentByUserId, questionAnswersViewModel.SentByUserId);
            Assert.AreEqual(result.QuestionId, questionAnswersViewModel.QuestionId);
        }

        /// <summary>
        ///  Test case to check if mapped view model data is not null and valid.
        /// </summary>
        [TestMethod]
        public void MapToViewModelNotNullValidFailure()
        {
            // ARRANGE
            var questionAnswersEntity = QuestionsAnswersMapperData.QuestionAnswersEntity;
            var questionAnswersViewModel = QuestionsAnswersMapperData.QuestionAnswersViewModel;

            // ACT
            var result = this.questionAnswerMapper.MapToViewModel(
                questionAnswersEntity,
                QuestionsAnswersMapperData.UserName,
                QuestionsAnswersMapperData.AnswerText,
                QuestionsAnswersMapperData.SelectedOption,
                QuestionsAnswersMapperData.ApplicationManifestId,
                isCorrectAnswer: false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Question, questionAnswersViewModel.Question);
            Assert.AreEqual(result.Option4, questionAnswersViewModel.Option4);
            Assert.AreEqual(result.Option1, questionAnswersViewModel.Option1);
            Assert.AreEqual(result.Option3, questionAnswersViewModel.Option3);
            Assert.AreEqual(result.Option2, questionAnswersViewModel.Option2);
            Assert.AreEqual(result.SelectedOption, questionAnswersViewModel.SelectedOption);
            Assert.AreEqual(result.CorrectOption, questionAnswersViewModel.CorrectOption);
            Assert.AreEqual(result.Answer, questionAnswersViewModel.Answer);
        }

        /// <summary>
        ///  Test case to check if mapped view model data is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapToViewModelNullCheck()
        {
            // ARRANGE
            QuestionAnswersEntity questionAnswersEntity = null;

            // ACT
            this.questionAnswerMapper.MapToViewModel(
                questionAnswersEntity,
                QuestionsAnswersMapperData.UserName,
                QuestionsAnswersMapperData.AnswerText,
                QuestionsAnswersMapperData.SelectedOption,
                QuestionsAnswersMapperData.ApplicationManifestId,
                isCorrectAnswer: false);
        }

        /// <summary>
        ///  Test case to check if mapped view model data is null.
        /// </summary>
        [ExpectedException(typeof(ArgumentNullException))]
        public void QuestionCardMapToViewModelNullCheck()
        {
            // ARRANGE
            QuestionAnswersEntity questionAnswersEntity = null;

            // ACT
            this.questionAnswerMapper.MapToViewModel(
                questionAnswersEntity,
                QuestionsAnswersMapperData.UserName,
                QuestionsAnswersMapperData.TeamId,
                QuestionsAnswersMapperData.SentByUserId,
                QuestionsAnswersMapperData.ApplicationManifestId);
        }

        /// <summary>
        ///  Test case to check if mapped data model is not null and valid.
        /// </summary>
        [TestMethod]
        public void MapToDataModelNotNullValid()
        {
            // ARRANGE
            var questionAnswersViewModel = QuestionsAnswersMapperData.QuestionAnswersViewModel;

            // ACT
            var result = this.questionAnswerMapper.MapToDataModel(
                questionAnswersViewModel);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Question, questionAnswersViewModel.Question);
            Assert.AreEqual(result.Option4, questionAnswersViewModel.Option4);
            Assert.AreEqual(result.Option1, questionAnswersViewModel.Option1);
            Assert.AreEqual(result.Option3, questionAnswersViewModel.Option3);
            Assert.AreEqual(result.Option2, questionAnswersViewModel.Option2);
            Assert.AreEqual(result.CorrectOption, questionAnswersViewModel.CorrectOption);
            Assert.AreEqual(result.TeamId, questionAnswersViewModel.TeamId);
            Assert.AreEqual(result.IsCorrectAnswer, questionAnswersViewModel.IsCorrectAnswer);
            Assert.AreEqual(result.SearchPageRedirectionPath, questionAnswersViewModel.SearchPageRedirectionPath);
            Assert.AreEqual(result.Notes, questionAnswersViewModel.Notes);
            Assert.AreEqual(result.UserName, questionAnswersViewModel.UserName);
            Assert.AreEqual(result.SentByUserId, questionAnswersViewModel.SentByUserId);
            Assert.AreEqual(result.QuestionId, questionAnswersViewModel.QuestionId);
        }

        /// <summary>
        ///  Test case to check if mapped data model is not null and valid.
        /// </summary>
        [TestMethod]
        public void MapToDataModelNotNullValidFailure()
        {
            // ARRANGE
            var questionAnswersViewModel = QuestionsAnswersMapperData.QuestionAnswersViewModel;

            // ACT
            var result = this.questionAnswerMapper.MapToDataModel(
               questionAnswersViewModel);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Question, questionAnswersViewModel.Question);
            Assert.AreEqual(result.Option4, questionAnswersViewModel.Option4);
            Assert.AreEqual(result.Option1, questionAnswersViewModel.Option1);
            Assert.AreEqual(result.Option3, questionAnswersViewModel.Option3);
            Assert.AreEqual(result.Option2, questionAnswersViewModel.Option2);
            Assert.AreEqual(result.SelectedOption, questionAnswersViewModel.SelectedOption);
            Assert.AreEqual(result.CorrectOption, questionAnswersViewModel.CorrectOption);
            Assert.AreEqual(result.Answer, questionAnswersViewModel.Answer);
        }

        /// <summary>
        ///  Test case to check if mapped view model data is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void MapToDataModelNullCheck()
        {
            // ACT
            this.questionAnswerMapper.MapToDataModel(null);
        }
    }
}