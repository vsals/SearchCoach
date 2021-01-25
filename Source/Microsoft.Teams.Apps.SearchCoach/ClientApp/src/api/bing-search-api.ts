// <copyright file="bing-search-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axiosDecorator from "../api/axios-decorator";
import { AxiosResponse } from "axios";

/**
* Get bing search results.
* @param searchText {String} Bing search text.
* @param selectedCountry {String} Bing search selected country.
* @param freshness {String} Bing search selected freshness of data.
* @param selectedDomainValues {String} Bing search selected domain values.
*/
export const getBingSearchResults = async (
    searchText: string,
    selectedCountry: string,
    freshness: string,
    // This value contains list of domain values with delimiter (;).
    selectedDomainValues: string): Promise<AxiosResponse> => {

    const url = `/api/search?searchText=​​​​${encodeURIComponent(searchText.trim())}&selectedCountry=​​​​${selectedCountry}&freshness=​​​​${freshness}&domainValues=${selectedDomainValues}`;

    return await axiosDecorator.get(url);
}