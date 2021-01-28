// <copyright file="search-box-page.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { WithTranslation, withTranslation } from "react-i18next";
import { Button, Flex, Input } from "@fluentui/react-northstar";
import { SearchIcon } from "@fluentui/react-icons-northstar";
import { TFunction } from "i18next";

import "./search-box.css";

interface ISearchBoxState {
}

interface ISearchBoxProps extends WithTranslation {
    isSearchResultPage: boolean
    queryText: string;
    onSearchTextChanged: (searchText: string) => void;
    onSearchClick: () => void;
}

/** This component contains search box content. */
class SearchBox extends React.Component<ISearchBoxProps, ISearchBoxState> {
    localize: TFunction;
    monthList: string[] | undefined;

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
    }

    componentDidMount() {
        this.setState({ isLoading: true });
    }

    /** Renders the component. */
    render() {
        const searchResultClassName = this.props.isSearchResultPage ? "search-results-box-container" : "search-box-container";

        return (
            <div className={searchResultClassName}>
                <div className="search-box">
                    <Flex>
                        <Flex.Item>
                            <Input className="searchbox-dark" fluid
                                icon={<Button icon={<SearchIcon />}
                                    onClick={this.props.onSearchClick}
                                    text iconOnly title={this.localize('searchTitleText')} />}
                                onChange={(event: any) => this.props.onSearchTextChanged(event.target.value)}
                                value={this.props.queryText}
                                placeholder={this.localize('searchBoxPlaceholder')} />
                        </Flex.Item>
                    </Flex>
                </div>
            </div>
        );
    }
}

export default withTranslation()(SearchBox)