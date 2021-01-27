// <copyright file="search-coach-results.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { Flex, Text, Label, Loader, FlexItem, Image } from "@fluentui/react-northstar";
import { HoverCard, HoverCardType } from "office-ui-fabric-react/lib/HoverCard";
import { getBingSearchResults } from "../../api/bing-search-api";
import { extractCategory, extractCountry, getReadableTimeString, ICountryInfo, ICategoryInfo } from "../../helpers/bing-search-helper";
import bingIcon from "../../assets/bing-icon-yellow.png";
import SearchBox from "../search-box/search-box-page";
import SearchFilterButtons from "../search-filter/search-filter-buttons";
import { WithTranslation, withTranslation } from "react-i18next";
import { TFunction } from "i18next";
import { ISelectedDropdownItem } from "../../constants/search-filter-interface";
import resources from "../../constants/resources";

import "./search-coach-results.css"

interface ISearchResultProps extends WithTranslation {
    searchText: string
    selectedCountry: string
    freshness: string
    domainValues: string
    handleSearchChange: (searchText: string) => void
    handleSearchClick: () => void;
    handleOperatorChange: (value: string) => void;
    handleCountryChange: (selectedCountry: ISelectedDropdownItem) => void;
    handleFreshnessChange: (value: any) => void;
    handleDomainValueChange: (selectedDomains: string) => void;
}

interface ISearchResultsState {
    isLoading: boolean;
    screenWidth: number;
    searchResultsDetails: ISearchResultsDetails[];
    showHelp: boolean;
    searchString: string;
    showResult: boolean;
    filteredCountryValue: ISelectedDropdownItem;
    freshness: string;
    domainValues: string;
    showNoResultsFound: boolean;
}

interface ISearchResultsDetails {
    id: string;
    title: string;
    link: string;
    dateLastCrawled: Date;
    snippet: string;
    dateLastPublished: Date;
    isNavigational: boolean;
    isFamilyFriendly: boolean;
    language: string;
    domain: string;
    country: string;
}

/** This component contains search results page content */
class SearchCoachResultsPage extends React.Component<ISearchResultProps, ISearchResultsState> {
    localize: TFunction;

    constructor(props: any) {
        super(props);
        this.localize = this.props.t;
        this.state = {
            isLoading: true,
            screenWidth: window.innerWidth,
            searchResultsDetails: [],
            showHelp: false,
            searchString: "",
            showResult: false,
            filteredCountryValue: { key: this.props.selectedCountry, header: resources.countries.filter(element => element.id === this.props.selectedCountry)[0].name },
            freshness: "All",
            domainValues: "",
            showNoResultsFound: false
        }
    }

    componentDidMount() {
        this.setState({ isLoading: true, searchString: this.props.searchText });
        this.getBingSearchResults(this.props.searchText, this.props.selectedCountry, this.props.freshness, this.props.domainValues);
        window.addEventListener("resize", this.update);
    }

    /**
    * get screen width real time.
    */
    update = () => {
        this.setState({
            screenWidth: window.innerWidth
        });
    };

    /** This event handler handles search title click
    * @param searchLink {String} The searched link
    */
    handleTitleClick(searchLink: string) {
        window.open(searchLink, '_blank');
    }

    /** This event handler handles location filter change for this component
    * @param selectedCountry {ISelectedDropdownItem} The location filter selected country value
    */
    handleCountryChange = (selectedCountry: ISelectedDropdownItem) => {
        this.setState({
            filteredCountryValue: selectedCountry
        });

        this.props.handleCountryChange(selectedCountry);
    }

    /** The event handler handles search click for this component */
    handleSearchClick = () => {
        this.setState({
            isLoading: true
        });
        this.getBingSearchResults(this.props.searchText, this.props.selectedCountry, this.props.freshness, this.props.domainValues);
    }

    /** Fetch bing search results 
     * @param searchText {String} The searched text
     * @param selectedCountry {String} The selected country value
     * @param freshness {String} The selected freshness value
     * @param domainValues {String} The selected domain values
     */
    getBingSearchResults = async (searchText: string, selectedCountry: string, freshness: string, domainValues: string) => {
        let response = await getBingSearchResults(searchText, selectedCountry, freshness, domainValues);

        if (response.status === 200 && response.data) {

            this.setState(
                {
                    searchResultsDetails: response.data,
                    searchString: searchText
                });

            // Here we have managed the case whether no results would be returned from bing api
            if (response.data.length === 0) {
                this.setState({
                    showNoResultsFound: true
                });
            }
        }

        this.setState({
            isLoading: false
        });
    }

    /** This renders plain card and is used as a functional subcomponent */
    renderDomainPlainCard = (): JSX.Element => {
        return (
            <div className="toplevel-domain-popup">
                <Flex>
                    <FlexItem>
                        <div className="search-result-domain-popup">
                            <Text className="domain-popup-heading" content={this.localize("domainTypeHeading")} />
                        </div>
                    </FlexItem>
                </Flex>
                <Flex>
                    <FlexItem>
                        <div className="search-result-domain-popup">
                            <Text content={this.localize("domainTypeHelpContent")} />
                        </div>
                    </FlexItem>
                </Flex>
            </div>
        )
    }

    /** This renders search results description */
    renderSearchResultsDescription = (countryInfo: ICountryInfo, categoryInfo: ICategoryInfo, value: ISearchResultsDetails): JSX.Element => {

        return (
            <Flex gap="gap.small" wrap>
                <Flex.Item>
                    <div className="last-updated-date">
                        <Label className="last-updated-date-lbl" content={this.localize("lastUpdatedText")} />
                        <Label className="last-updated-date-text" content={getReadableTimeString(value.dateLastCrawled.toString())} />
                    </div>
                </Flex.Item>
                <Flex.Item>
                    <HoverCard
                        cardDismissDelay={200}
                        type={HoverCardType.plain}
                        plainCardProps={{ onRenderPlainCard: this.renderDomainPlainCard }}
                    >
                        <div className="last-updated-date top-level-domain">
                            <Label className="last-updated-date-lbl" content={this.localize("domainTypeText")} />
                            <Label className="last-updated-date-text" content={categoryInfo.category} />
                        </div>
                    </HoverCard>
                </Flex.Item>
                <Flex.Item>
                    <div className="last-updated-date">
                        <Label className="last-updated-date-lbl" content={this.localize("countryLabelText")} />
                        {countryInfo.country !== resources.unknownText && <Label image={countryInfo.countryImage} className="last-updated-date-text" content={countryInfo.country} />}
                        {countryInfo.country === resources.unknownText && <Label className="last-updated-date-text" content={countryInfo.country} />}
                    </div>
                </Flex.Item>
            </Flex>
        );
    }

    /**
     * Render search results. 
    */
    renderSearchPage() {
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
        }
        else {
            return (
                <>
                    {
                        this.state.searchResultsDetails.map((value: ISearchResultsDetails) => {

                            const categoryInfo = extractCategory(value.link, this.localize);
                            const countryInfo = extractCountry(value.link, this.localize);

                            return (
                                <div className="search-result-container">
                                    <Flex gap="gap.small" wrap>
                                        <Flex.Item>
                                            <Text onClick={() => this.handleTitleClick(value.link)} className="search-title" content={value.title} />
                                        </Flex.Item>
                                    </Flex>
                                    <Flex gap="gap.small" wrap>
                                        <Flex.Item>
                                            <Text className="search-url-text" content={value.link} />
                                        </Flex.Item>
                                    </Flex>
                                    <Flex gap="gap.small" wrap>
                                        <Flex.Item>
                                            <Text title={value.snippet} className="search-description" content={value.snippet} />
                                        </Flex.Item>
                                    </Flex>

                                    {this.renderSearchResultsDescription(countryInfo, categoryInfo, value)}

                                </div>
                            )
                        })
                    }
                </>
            );
        }
    }


    /**
   * Renders the component.
   */
    public render(): JSX.Element {

        return (
            <div className="search-container-text">
                <div className="search-result-searchbox-container">
                    <div>
                        <Flex gap="gap.small" vAlign="center" padding="padding.medium" className="search-result-searchbox-padding">
                            <Flex.Item>
                                <Image src={bingIcon} className="bing-image-width" />
                            </Flex.Item>
                            <Flex.Item>
                                <SearchBox isSearchResultPage={true} queryText={this.props.searchText} onSearchTextChanged={this.props.handleSearchChange} onSearchClick={this.handleSearchClick} />
                            </Flex.Item>
                        </Flex>
                        <SearchFilterButtons onOperatorAdded={this.props.handleOperatorChange} isSearchResultPage={true} onCountryChange={this.handleCountryChange} selectedCountry={this.state.filteredCountryValue} onFreshnessChange={this.props.handleFreshnessChange} onDomainValuesChange={this.props.handleDomainValueChange} selectedDomains={this.props.domainValues} />
                        <Flex gap="gap.small" wrap>
                            <Flex.Item>
                                <div className="seperator-results"></div>
                            </Flex.Item>
                        </Flex>
                        {
                            !this.state.showNoResultsFound &&
                            this.renderSearchPage()
                        }
                        {
                            this.state.showNoResultsFound &&
                            <div className="search-no-results">{this.localize("noSearchResultsFound")}</div>
                        }
                    </div>
                </div>
            </div>
        );
    }
}

export default withTranslation()(SearchCoachResultsPage)