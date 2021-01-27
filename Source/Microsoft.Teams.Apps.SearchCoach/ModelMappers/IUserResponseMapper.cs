// <copyright file="IUserResponseMapper.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.ModelMappers
{
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Interface for handling operations related to user response model mappings.
    /// </summary>
    public interface IUserResponseMapper
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
            string currentUserObjectId);

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
            bool isAnswerCorrect);
    }
}