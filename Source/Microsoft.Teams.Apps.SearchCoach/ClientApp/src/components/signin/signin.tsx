import * as React from "react";
import { RouteComponentProps } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Button } from "@fluentui/react-northstar";
import * as microsoftTeams from "@microsoft/teams-js";

/**
 * This component contains the sign-in content
 */
const SignInPage: React.FunctionComponent<RouteComponentProps> = () => {
    const { t } = useTranslation();
    function onSignIn() {
        microsoftTeams.initialize();
        microsoftTeams.authentication.authenticate({
            url: window.location.origin + "/signin-simple-start",
            successCallback: () => {
                window.location.href = "/search-landing-page";
            },
            failureCallback: (reason) => {
                console.log("Login failed: " + reason);
                window.location.href = "/error";
            }
        });
    }

    return (
        <div className="sign-in-content-container">
            <Button content={t('signInButtonText')} primary className="sign-in-button" onClick={onSignIn} />
        </div>
    );
};

export default SignInPage;