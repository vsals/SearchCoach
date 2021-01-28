// <copyright file="UserResponseMapperTest.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Mappers
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;
    using Microsoft.Teams.Apps.SearchCoach.Tests.TestData;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Class that contains all the test cases for user response model mappers operations.
    /// </summary>
    [TestClass]
    public class UserResponseMapperTest
    {
        private UserResponseMapper userResponseMapper;

        /// <summary>
        /// Method for testing UpdateMap method from mapper.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            this.userResponseMapper = new UserResponseMapper();
        }

        /// <summary>
        /// Test case to check if mapped entity data is not null and valid.
        /// </summary>
        [TestMethod]
        public void MapToEntityNotNullValid()
        {
            // ARRANGE
            var userResponseEntity = UserResponseMapperData.UserResponseEntity;

            // ACT
            var result = this.userResponseMapper.MapToEntity(
                QuestionsAnswersMapperData.TeamId,
                UserResponseMapperData.UserAadObjectId,
                UserResponseMapperData.QuestionId,
                UserResponseMapperData.CurrentUserObjectId);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.QuestionId, userResponseEntity.QuestionId);
            Assert.AreEqual(result.IsCorrectAnswer, userResponseEntity.IsCorrectAnswer);
            Assert.AreEqual(result.UserId, userResponseEntity.UserId);
            Assert.AreEqual(result.Response, userResponseEntity.Response);
            Assert.AreEqual(result.SentByUserId, userResponseEntity.SentByUserId);
            Assert.AreEqual(result.TeamId, userResponseEntity.TeamId);
            Assert.AreEqual(result.ResponseId, userResponseEntity.ResponseId);
        }

        /// <summary>
        ///  Test case to check if update entity data is not null and valid..
        /// </summary>
        [TestMethod]
        public void UpdateMapToEntityNotNullValid()
        {
            // ARRANGE
            var userResponseEntity = UserResponseMapperData.UserResponseEntity;

            // ACT
            var result = this.userResponseMapper.UpdateMapToEntity(UserResponseMapperData.UserResponseEntity, QuestionsAnswersMapperData.SelectedOption, UserResponseMapperData.CurrentUserObjectId, false);

            // ASSERT
            Assert.IsNotNull(result);
            Assert.AreEqual(result.QuestionId, userResponseEntity.QuestionId);
            Assert.AreEqual(result.IsCorrectAnswer, userResponseEntity.IsCorrectAnswer);
            Assert.AreEqual(result.SentByUserId, userResponseEntity.SentByUserId);
            Assert.AreEqual(result.TeamId, userResponseEntity.TeamId);
            Assert.AreEqual(result.ResponseId, userResponseEntity.ResponseId);
        }

        /// <summary>
        ///  Test case to check if update entity data is null.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void UpdateMapToEntityNullCheckTest()
        {
            // ARRANGE
            UserResponseEntity userResponseEntity = null;

            // ACT
            this.userResponseMapper.UpdateMapToEntity(
                userResponseEntity,
                QuestionsAnswersMapperData.SelectedOption,
                UserResponseMapperData.CurrentUserObjectId,
                isAnswerCorrect: false);
        }
    }
}