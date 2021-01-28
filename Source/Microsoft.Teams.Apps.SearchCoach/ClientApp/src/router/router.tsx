// <copyright file="router.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Suspense } from "react";
import { BrowserRouter, Route, Switch } from "react-router-dom";
import Redirect from "components/redirect";
import ErrorPage from "components/error-page";
import SearchCoachLandingPage from "components/search-coach/search-page";
import SearchCoachResultsPage from "components/search-coach-results/search-coach-results";
import SignInPage from "components/signin/signin";
import SignInSimpleStart from "components/signin/signin-start";
import SignInSimpleEnd from "components/signin/signin-end";
import LeaderBoardTab from "../components/leader-board-tab/leader-board-tab";

export const AppRoute: React.FunctionComponent<{}> = () => {

    return (
        <Suspense fallback={<div className="container-div"><div className="container-subdiv"></div></div>}>
            <BrowserRouter>
                <Switch>
                    <Route exact path="/error" component={ErrorPage} />
                    <Route exact path="/search-landing-page" component={SearchCoachLandingPage} />
                    <Route exact path="/search-coach-results" component={SearchCoachResultsPage} />
                    <Route exact path="/signin" component={SignInPage} />
                    <Route exact path="/signin-simple-start" component={SignInSimpleStart} />
                    <Route exact path="/signin-simple-end" component={SignInSimpleEnd} />
                    <Route exact path="/leader-board-tab" component={LeaderBoardTab} />
                    <Route component={Redirect} />
                </Switch>
            </BrowserRouter>
        </Suspense>
    );
}