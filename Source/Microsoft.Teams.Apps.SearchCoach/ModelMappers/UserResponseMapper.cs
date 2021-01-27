// <copyright file="UserResponseMapper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.ModelMappers
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// A model class that contains methods related to user response model mappings.
    /// </summary>
    public class UserResponseMapper : IUserResponseMapper
    {
        /// <summary>
        /// Gets user response entity model.
        /// </summary>
        /// <param name="teamId">Team id in which user is part of.</param>
        /// <param name="userAadObjectId">Azure Active Directory id of team member.</param>
        /// <param name="questionId">Unique id of question.</param>
        /// <param name="currentUserObjectId">Azure Active Directory id of current user who send the question card.</param>
        /// <returns>Returns a user response model object.</returns>
        public UserResponseEntity MapToEntity(
            string teamId,
            string userAadObjectId,
            string questionId,
            string currentUserObjectId)
        {
            return new UserResponseEntity
            {
                TeamId = teamId,
                ResponseId = $"{questionId}_{userAadObjectId}",
                QuestionId = questionId,
                UserId = Guid.Parse(userAadObjectId),
                RespondedOn = DateTime.Now,
                Response = string.Empty,
                SentOn = DateTime.Now,
                SentByUserId = Guid.Parse(currentUserObjectId),
                IsCorrectAnswer = false,
                IsQuestionAttempted = false,
            };
        }

        /// <summary>
        /// Gets user response updated entity model.
        /// </summary>
        /// <param name="userResponseData">User response entity data.</param>
        /// <param name="submittedAnswerOption">Answer option submitted by current user.</param>
        /// <param name="currentUserObjectId">Azure Active Directory id of current user who submitted the answer.</param>
        /// <param name="isAnswerCorrect">Indicates whether submitted answer is correct or not.</param>
        /// <returns>Returns a user response model object.</returns>
        public UserResponseEntity UpdateMapToEntity(
            UserResponseEntity userResponseData,
            string submittedAnswerOption,
            string currentUserObjectId,
            bool isAnswerCorrect)
        {
            userResponseData = userResponseData ?? throw new ArgumentNullException(nameof(userResponseData));

            return new UserResponseEntity
            {
                ETag = "*",
                TeamId = userResponseData.TeamId,
                ResponseId = userResponseData.ResponseId,
                QuestionId = userResponseData.QuestionId,
                SentByUserId = userResponseData.SentByUserId,
                UserId = Guid.Parse(currentUserObjectId),
                Response = submittedAnswerOption,
                IsCorrectAnswer = isAnswerCorrect,
                RespondedOn = DateTime.Now,
                SentOn = userResponseData.SentOn,
                IsQuestionAttempted = true,
            };
        }
    }
}