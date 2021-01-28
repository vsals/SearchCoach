// <copyright file="leader-board-tab.test.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Provider } from "@fluentui/react-northstar";
import LeaderBoardTab from "../../leader-board-tab/leader-board-tab";
import { render } from "react-dom";
import { act } from "react-dom/test-utils";
import pretty from "pretty";

jest.mock("../../../api/leader-board-api");

jest.mock("react-i18next", () => ({
    useTranslation: () => ({
        t: (key: any) => key,
        i18n: { changeLanguage: jest.fn() },
    }),

    withTranslation: () => (Component: any) => {
        Component.defaultProps = {
            ...Component.defaultProps,
            t: (key: any) => key,
        };
        return Component;
    },
}));
jest.mock("@microsoft/teams-js", () => ({
    initialize: () => {
        return true;
    },
    getContext: (callback: any) =>
        callback(
            Promise.resolve({ teamId: "ewe", entityId: "sdsd", locale: "en-US" })
        ),
}));

let container: any = null;

beforeEach(() => {
    // setup a DOM element as a render target
    container = document.createElement("div");
    // container *must* be attached to document so events work correctly.
    document.body.appendChild(container);
});

describe("LeaderBoardTab", () => {
    it("renders snapshots", () => {
        act(() => {
            render(
                <Provider>
                    <LeaderBoardTab/>
                </Provider>,
                container
            );
        });

        expect(pretty(container.innerHTML)).toMatchSnapshot();
    });

    it("LeaderBoard Table Validation", () => {
        const leaderBoardDataTable = document.querySelector(
            "[data-testid=leader-board-table-data]"
        );
        expect(leaderBoardDataTable).not.toBe(null);
    });
});
