// <copyright file="search-page.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Flex, Text, Image } from "@fluentui/react-northstar";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import bingIcon from "../../assets/bing-icon.png";
import SearchBox from "../search-box/search-box-page";
import SearchFilterButtons from "../search-filter/search-filter-buttons";
import { ISelectedDropdownItem } from "../../constants/search-filter-interface";
import SearchCoachResultsPage from "../search-coach-results/search-coach-results"
import resources from "../../constants/resources"

import "./search-coach.css";

interface ISearchPageState {
    isLoading: boolean;
    screenWidth: number;
    searchString: string;
    filteredCountryValue: ISelectedDropdownItem;
    showResult: boolean;
    freshness: string;
    domainValues: string;
}

/** This component contains search landing page content. */
class SearchCoachLandingPage extends React.Component<WithTranslation, ISearchPageState> {
    localize: TFunction;
    monthList: Array<string> | undefined;
    selectedDomainNames: string;

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        window.addEventListener("resize", this.update);
        this.selectedDomainNames = "";
        this.state = {
            isLoading: false,
            screenWidth: window.innerWidth,
            searchString: "",
            filteredCountryValue: { key: resources.countries[0].id, header: resources.countries[0].name },
            showResult: false,
            freshness: "1",
            domainValues: ""
        }
    }

    componentDidMount() {
        this.setState({ isLoading: true });
        this.update();
    }

    /**
    * Get screen width real time.
    */
    update = () => {
        this.setState({
            screenWidth: window.innerWidth
        });
    };

    /** 
    * This event handler handles operator filter change.
    * @param value {String} The operator value.
    */
    handleOperatorChange = (value: string) => {
        if (this.state.searchString) {
            this.setState({
                searchString: this.state.searchString + " " + value + " "
            });
        }
    }

    /** 
    * This event handler handles search text change.
    * @param searchText {String} The search text value.
    */
    handleSearchChange = (searchText: string) => {
        this.setState({
            searchString: searchText
        });
    }

    /** 
    * This event handler handles location filter change.
    * @param selectedCountry {ISelectedDropdownItem} The location filter selected country value.
    */
    handleCountryChange = (selectedCountry: ISelectedDropdownItem) => {
        this.setState({
            filteredCountryValue: selectedCountry
        });
    }

    /** This event handler handles search click.*/
    handleSearchClick = () => {
        this.setState({
            showResult: true
        });
    }

    /** 
    * This event handler handles freshness value change.
    * @param value {String} The selected freshness value.
    */
    handleFreshnessChange = (value: string) => {
        this.setState({
            freshness: value
        })
    }

    /**
    * The event handler is called when the domain values are selected/deselected for parent component for maintaining state for this component.
    * @param selectedDomains {String} The multiple domain values string selected from filter with delimiter as (;).
    */
    handleDomainValueChange = (selectedDomains: string) => {
        this.setState({
            domainValues: selectedDomains
        })
    }

    /** Renders the component */
    render() {
        return (
            <>
                {
                    !this.state.showResult &&
                    <div className="search-container-text">
                        <div className="search-container">
                            <Flex gap="gap.small" vAlign="center" padding="padding.medium" className="search-icon-text">
                                <Image src={bingIcon} className="bing-image-width" />
                                <Text className="search-page-text" weight="semibold" content={this.localize("searchCoachTitle")} />
                            </Flex>
                            <SearchBox isSearchResultPage={false} queryText={this.state.searchString} onSearchTextChanged={this.handleSearchChange} onSearchClick={this.handleSearchClick} />
                            <SearchFilterButtons onOperatorAdded={this.handleOperatorChange} isSearchResultPage={false} onCountryChange={this.handleCountryChange} selectedCountry={this.state.filteredCountryValue} onFreshnessChange={this.handleFreshnessChange} onDomainValuesChange={this.handleDomainValueChange} selectedDomains={this.state.domainValues} />
                        </div>
                    </div>
                }
                {
                    this.state.showResult &&
                    <SearchCoachResultsPage freshness={this.state.freshness} searchText={this.state.searchString} selectedCountry={this.state.filteredCountryValue.key} domainValues={this.state.domainValues} handleCountryChange={this.handleCountryChange} handleDomainValueChange={this.handleDomainValueChange} handleFreshnessChange={this.handleFreshnessChange} handleOperatorChange={this.handleOperatorChange} handleSearchChange={this.handleSearchChange} handleSearchClick={this.handleSearchClick} />
                }
            </>
        );
    }
}

export default withTranslation()(SearchCoachLandingPage)