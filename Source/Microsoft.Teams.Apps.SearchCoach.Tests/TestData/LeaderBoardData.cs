// <copyright file="LeaderBoardData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.TestData
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Teams.Apps.SearchCoach.Models.Entity;

    /// <summary>
    /// Class that contains test data for user responses to show on leader-board tab.
    /// </summary>
    public static class LeaderBoardData
    {
        /// <summary>
        /// Fetch user responses for a particular team to show on leader-board tab.
        /// </summary>
        public static readonly string TeamId = "00:0000000000000000000000000000000@abc.tacv0";

        /// <summary>
        /// Azure Active Directory id of user.
        /// </summary>
        public static readonly string UserId = "00000000-0000-0000-0000-000000000001";

        /// <summary>
        /// A collection of user responses for test.
        /// </summary>
        public static readonly List<UserResponseEntity> UserResponseEntities = new List<UserResponseEntity>()
        {
            new UserResponseEntity()
            {
                TeamId = "00:0000000000000000000000000000000@abc.tacv0",
                ResponseId = "1_00000000-0000-0000-0000-000000000000",
                QuestionId = "1",
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000001"),
                RespondedOn = DateTime.Now,
                Response = "Test response",
                IsQuestionAttempted = false,
                IsCorrectAnswer = false,
                SentOn = DateTime.Now,
                SentByUserId = Guid.Empty,
            },
            new UserResponseEntity()
            {
                TeamId = "00:0000000000000000000000000000000@abc.tacv0",
                ResponseId = "2_00000000-0000-0000-0000-000000000000",
                QuestionId = "2",
                UserId = Guid.Parse("00000000-0000-0000-0000-000000000002"),
                RespondedOn = DateTime.Now,
                Response = "Test response",
                IsQuestionAttempted = false,
                IsCorrectAnswer = false,
                SentOn = DateTime.Now,
                SentByUserId = Guid.Empty,
            },
        };

        /// <summary>
        /// A collection of user object id's.
        /// </summary>
        public static readonly IEnumerable<string> UserObjectIds = new List<string>()
        {
            "00000000-0000-0000-0000-000000000001",
            "00000000-0000-0000-0000-000000000002",
        };

        /// <summary>
        /// A collection of user details.
        /// </summary>
        public static readonly Dictionary<Guid, string> UsersDetails = new Dictionary<Guid, string>()
        {
            { Guid.Parse("00000000-0000-0000-0000-000000000001"), "ABC" },
            { Guid.Parse("00000000-0000-0000-0000-000000000002"), "XYZ" },
        };

        /// <summary>
        /// Fetch team member's details based on group id of team.
        /// </summary>
        public static readonly string GroupId = "00000000-0000-0000-0000-000000000009";
    }
}