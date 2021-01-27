// <copyright file="signin.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Text, Button } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";

/**
 * This component contains the sign-in content
 */
const SignInPage: React.FunctionComponent<RouteComponentProps> = props => {
    const { t } = useTranslation();
    const { history } = props;

    const errorMessage = t('signInTextMessage');

    function onSignIn() {
        microsoftTeams.initialize();
        microsoftTeams.authentication.authenticate({
            url: window.location.origin + "/signin-simple-start",
            successCallback: () => {
                history.push('/search-landing-page');
            },
            failureCallback: (reason) => {
                console.log("Login failed: " + reason);
                history.push('/error');
            }
        });
    }

    return (
        <div className="sign-in-content-container">
            <div>
            </div>
            <Text
                content={errorMessage}
                size="medium"
            />
            <div className="space"></div>
            <Button content="Sign in" primary className="sign-in-button" onClick={onSignIn} />
        </div>
    );
};

export default SignInPage;