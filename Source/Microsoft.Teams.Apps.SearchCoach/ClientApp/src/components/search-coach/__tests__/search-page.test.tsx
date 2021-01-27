// <copyright file="search-page.test.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Provider } from "@fluentui/react-northstar";
import SearchLandingPage from "../../search-coach/search-page";
import { render } from "react-dom";
import { act } from "react-dom/test-utils";
import pretty from "pretty";

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

beforeAll(() => {
    // setup a DOM element as a render target
    container = document.createElement("div");
    // container *must* be attached to document so events work correctly.
    document.body.appendChild(container);
    act(() => {
        render(
            <Provider>
                <SearchLandingPage />
            </Provider>,
            container
        );
    });
});

describe("SearchLandingPage", () => {
    it("renders snapshots", () => {
        expect(pretty(container.innerHTML)).toMatchSnapshot();
    });

    it("Operator Popup Validation", () => {
        const operatorFilterButton = document.querySelector(
            "[data-testid=operator-filter-button]"
        );

        act(() => {
            operatorFilterButton?.dispatchEvent(
                new MouseEvent("click", { bubbles: true })
            );
        });

        const operatorFilterPopup = document.querySelector(
            "[data-testid=operator-filter-popup]"
        );
        expect(operatorFilterPopup).not.toBe(null);
    });
});