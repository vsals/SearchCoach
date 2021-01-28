// <copyright file="leader-board-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axios from "./axios-decorator";
import { ILeaderBoardUserData } from "../models/ILeaderBoardUserData";
import { AxiosResponse } from "axios";

/**
* Get user responses details to show on leader-board tab.
* @param teamId {String} Team Id for which user responses needs to be fetched.
* @param groupId {String} Group id of the current team to fetch team members to check if current user is exists in the team who is trying to access the leader-board tab data.
*/
export const getUserResponsesDetails = async (teamId: string, groupId: string): Promise<AxiosResponse<ILeaderBoardUserData[]>> => {
    let url = `/api/leaderboard/${teamId}?groupId=${groupId}`;

    return await axios.get(url);
}