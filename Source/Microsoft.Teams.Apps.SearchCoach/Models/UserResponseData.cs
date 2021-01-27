// <copyright file="UserResponseData.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>
namespace Microsoft.Teams.Apps.SearchCoach.Models
{
    /// <summary>
    /// Model class that contains user response data to show on leader-board tab.
    /// </summary>
    public class UserResponseData
    {
        /// <summary>
        /// Gets or sets the user display name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the number of answers replied correctly by user.
        /// </summary>
        public int RightAnswers { get; set; }

        /// <summary>
        /// Gets or sets the number of questions attempted by user.
        /// </summary>
        public int QuestionsAttempted { get; set; }
    }
}