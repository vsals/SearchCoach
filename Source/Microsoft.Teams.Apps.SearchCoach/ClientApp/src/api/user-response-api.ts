// <copyright file="user-response-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axios from "./axios-decorator";

/**
* Get user responses details to show on leader-board tab.
*@param teamId {String} Team Id for which user responses needs to be fetched.
*/
export const getUserResponsesDetails = async (teamId: string): Promise<any> => {
    let url = `/api/leaderboard/${teamId}`;

    return await axios.get(url);
}