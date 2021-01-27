// <copyright file="UserResponseMapperData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Class that contains test data for user response mapper.
    /// </summary>
    public static class UserResponseMapperData
    {
        /// <summary>
        /// Test data for user response entity.
        /// </summary>
        public static readonly UserResponseEntity UserResponseEntity = new UserResponseEntity()
        {
            QuestionId = "1",
            IsCorrectAnswer = false,
            UserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            RespondedOn = DateTime.Now,
            SentByUserId = Guid.Parse("00000000-0000-0000-0000-000000000000"),
            TeamId = "00:0000000000000000000000000000000@abc.tacv0",
            ResponseId = $"{"1"}_{"00000000-0000-0000-0000-000000000000"}", // QuestionId_UserId
            Response = string.Empty,
            SentOn = DateTime.Now,
        };

        /// <summary>
        /// Test data for Azure Active Directory id of user.
        /// </summary>
        public static readonly string UserAadObjectId = "00000000-0000-0000-0000-000000000000";

        /// <summary>
        /// Test data for Azure Active Directory id of current user.
        /// </summary>
        public static readonly string CurrentUserObjectId = "00000000-0000-0000-0000-000000000000";

        /// <summary>
        /// Test data for QuestionId.
        /// </summary>
        public static readonly string QuestionId = "1";
    }
}