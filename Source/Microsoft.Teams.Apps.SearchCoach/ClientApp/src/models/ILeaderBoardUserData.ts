// <copyright file="ILeaderBoardUserData.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

// Interface to deal with user response data for leader-board tab.
export interface ILeaderBoardUserData {
    userName: string;
    rightAnswers: number;
    questionsAttempted: number;
}