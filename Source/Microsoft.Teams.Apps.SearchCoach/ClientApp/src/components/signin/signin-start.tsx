import React, { useEffect } from "react";
import * as microsoftTeams from "@microsoft/teams-js";
import { getAuthenticationConsentMetadata } from "../../api/authentication-metadata-api";

/** This component contains sing-in-start content */
const SignInSimpleStart: React.FunctionComponent = () => {

    useEffect(() => {
        microsoftTeams.initialize();
        microsoftTeams.getContext(context => {
            const windowLocationOriginDomain = window.location.hostname;
            const login_hint = context.upn ? context.upn : "";

            getAuthenticationConsentMetadata(windowLocationOriginDomain, login_hint).then(result => {
                window.location.assign(result.data);
            });
        });
    });

    return (
        <></>
    );
};

export default SignInSimpleStart;