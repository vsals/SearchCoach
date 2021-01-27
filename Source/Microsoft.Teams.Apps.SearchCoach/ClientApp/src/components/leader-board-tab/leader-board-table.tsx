// <copyright file="leader-board-table.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from 'react';
import { Table, Text } from '@fluentui/react-northstar';
import "../../components/leader-board-tab/leader-board-tab.css";
import { useTranslation } from 'react-i18next';
import { ILeaderBoardUserData } from "../../models/ILeaderBoardUserData";

interface ILeaderBoardUserTableProps {
    userResponsesData: Array<ILeaderBoardUserData>
}

const LeaderBoardUserTable: React.FunctionComponent<ILeaderBoardUserTableProps> = props => {

    const localize = useTranslation().t;
    const header = {
        items: [
            {
                content: (<Text content={localize("name")} className="user-name-header" />),
                className: "questions-header"
            },
            {
                content: (<Text content={localize("numberOfCorrectAnswerHeading")} />),
                className: "questions-header"
            },
            {
                content: (<Text content={localize("numberOfQuestionsAttemptedHeading")} />),
                className: "questions-header"
            }
        ],
    }

    const rows = props.userResponsesData.map((member, index) => {

        return {
            key: index,
            items: [
                {
                    content: (<Text className="user-name" content={member.userName} />),
                    className: "user-response-name"
                },
                {
                    content: (<Text content={member.rightAnswers} />),
                    className: "user-response-data-count"
                },
                {
                    content: (<Text content={member.questionsAttempted} />),
                    className: "user-response-data-count"
                }
            ],
        }
    });

    return (
        <Table header={header} rows={rows} className="user-data-table" data-testid="leaderboard-table-data" />
    );
}

export default LeaderBoardUserTable