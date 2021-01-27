// <copyright file="leader-board-tab.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import * as microsoftTeams from '@microsoft/teams-js';
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { getUserResponsesDetails } from "../../api/user-response-api";
import { Text, Flex, Loader } from '@fluentui/react-northstar';
import LeaderBoardUserTable from "../leader-board-tab/leader-board-table";
import { ILeaderBoardUserData } from "../../models/ILeaderBoardUserData";

import "../../components/leader-board-tab/leader-board-tab.css";

interface ILeaderBoardState {
    isLoading: boolean;
    screenWidth: number;
    userData: Array<ILeaderBoardUserData>
}

interface ILeaderBoardProps extends WithTranslation {
}

class LeaderBoardTab extends React.Component<ILeaderBoardProps, ILeaderBoardState> {
    localize: TFunction;
    monthList: Array<string> | undefined;
    teamId: string;

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.teamId = "";
        window.addEventListener("resize", this.update);
        this.state = {
            isLoading: true,
            screenWidth: window.innerWidth,
            userData: []
        }
    }

    /**
    * Used to initialize Microsoft Teams sdk.
    */
    componentDidMount() {
        microsoftTeams.initialize();
        microsoftTeams.getContext((context: microsoftTeams.Context) => {
            this.teamId = context.teamId!;
            this.update();
            this.getLeaderBoardDetails();
        });
    }

    /**
    * get screen width real time.
    */
    update = () => {
        this.setState({
            screenWidth: window.innerWidth
        });
    };

    /**
    * Fetch user detail data.
    */
    getLeaderBoardDetails = async () => {
        let response = await getUserResponsesDetails(this.teamId);

        if (response.status === 200 && response.data) {
            this.setState({
                userData: response.data,
                isLoading: false
            })
        }
    }

    /**
    * Renders the component
    */
    public render(): JSX.Element {
        return (
            <div>
                {this.getWrapperPage()}
            </div>
        );
    }

    /**
   *Get wrapper for page which acts as container for all child components
   */
    private getWrapperPage = () => {
        if (this.state.isLoading) {
            return (
                <div className="container-div">
                    <div className="container-subdiv">
                        <div className="loader">
                            <Loader />
                        </div>
                    </div>
                </div>
            );
        } else {
            if (this.state.userData.length > 0) {
                return (
                    <div>
                        <Flex>
                            <Text className="leader-board-heading" content={this.localize("allStudentText")} weight="bold" />
                        </Flex>
                        <LeaderBoardUserTable userResponsesData={this.state.userData} />
                    </div>
                );
            }
            else {
                return (
                    <div>
                        <Flex>
                            <Text className="leader-board-error-heading" content={this.localize("leaderBoardTabDataNotFoundText")} weight="bold" />
                        </Flex>
                    </div>
                );
            }
        }
    }
}

export default withTranslation()(LeaderBoardTab)