// <copyright file="search-filter-buttons.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { WithTranslation, withTranslation } from "react-i18next";
import { Flex } from "@fluentui/react-northstar";
import { TFunction } from "i18next";
import PopUpMenuWrapper from "../popup-menu/popup-menu-wrapper";
import { ISelectedDropdownItem } from "../../constants/search-filter-interface";
import resources from "../../constants/resources";
import PopUpOperatorContent from "../popup-menu/popup-menu-operator-content";
import PopUpLocationContent from "../popup-menu/popup-menu-location-content";

import "./search-filter.css";

interface ISearchFilterButtonsState {
}

interface ISearchFilterButtonProps extends WithTranslation {
    onOperatorAdded: (value: string) => void;
    isSearchResultPage: boolean;
    onCountryChange: (value: ISelectedDropdownItem) => void;
    selectedCountry: ISelectedDropdownItem;
    onFreshnessChange: (value: string) => void;
    onDomainValuesChange: (selectedDomains: string) => void;
    selectedDomains: string;
}
/** This component contains search filter content. */
class SearchFilterButtons extends React.Component<ISearchFilterButtonProps, ISearchFilterButtonsState> {
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
        const searchResultClassName = this.props.isSearchResultPage ? "search-filter-box-container" : "search-box-container";

        return (
            <div className={searchResultClassName}>
                <div className="search-filter-button-container">
                    <Flex>
                        <Flex.Item align="start" size="size.smaller">
                            <PopUpMenuWrapper activeButton={resources.operatorText} {...this.props} onDomainValuesChange={this.props.onDomainValuesChange} popUpContent={<PopUpOperatorContent {...this.props} />} buttonContent={this.localize("operatorsButton")} />
                        </Flex.Item>
                        <Flex.Item align="start" size="size.smaller">
                            <PopUpMenuWrapper activeButton={resources.locationText} {...this.props} onDomainValuesChange={this.props.onDomainValuesChange} popUpContent={<PopUpLocationContent {...this.props} />} buttonContent={this.localize("locationButton")}  />
                        </Flex.Item>
                        <Flex.Item align="start" size="size.smaller">
                            <PopUpMenuWrapper activeButton={resources.domainText} {...this.props} onDomainValuesChange={this.props.onDomainValuesChange} buttonContent={this.localize("domainsButton")}/>
                        </Flex.Item>
                        <Flex.Item align="start" size="size.smaller">
                            <PopUpMenuWrapper activeButton={resources.anytimeText} {...this.props} onDomainValuesChange={this.props.onDomainValuesChange} />
                        </Flex.Item>
                    </Flex>
                </div>
            </div>
        );
    }
}

export default withTranslation()(SearchFilterButtons)