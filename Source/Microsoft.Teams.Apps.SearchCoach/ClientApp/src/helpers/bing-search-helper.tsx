//-----------------------------------------------------------------------
// <copyright file="bing-search-helper.tsx" company="Microsoft">
// © Microsoft. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

import moment from 'moment';
import { TFunction } from 'i18next';
import usaImage from "../assets/usa.png";
import franceImage from "../assets/france.png";
import germanyImage from "../assets/germany.png";
import russiaImage from "../assets/russia.png";
import italyImage from "../assets/italy.png";
import koreaImage from "../assets/korea.png";
import japanImage from "../assets/japan.png";
import indiaImage from "../assets/india.png";

export interface ICountryInfo {
    country: string;
    countryImage: string;
}

export interface ICategoryInfo {
    category: string;
}

/** This function extracts the hostname from search result url
 * @param url The search result url
 */
export function extractHostname(url: string): string {
    let hostname;
    const urlObj = new URL(url);
    hostname= urlObj.hostname;

    // remove www.
    if (hostname.indexOf("www.") > -1) {
        hostname = hostname.substring(hostname.indexOf(".") + 1)
    }
    return hostname;
}

/** This function extracts the category value from search result url
 * @param url The search result url
 * @param localize The function is passed here for localizing the category names
 */
export function extractCategory(url: string, localize: TFunction): ICategoryInfo {
    const domainUrl = extractHostname(url);
    const tld = domainUrl.split('.').pop();
    const categoryInfo = {} as ICategoryInfo;

    switch (tld) {
        case 'com':
        case 'org':
        case 'net':
            categoryInfo.category = localize('nonGovernmentLabelText');
            break;
        case 'edu':
            categoryInfo.category = localize('educationLabelText');
            break;
        case 'gov':
            categoryInfo.category = localize('governmentLabelText');
            break;
        case 'mil':
            categoryInfo.category = localize('militaryLabelText');
            break;
        default:
            categoryInfo.category = localize('unknownLabelText');
            break;
    }

    return categoryInfo;
}

/** This function extracts the country value from search result url
 * @param url The search result url
 * @param localize The function is passed here for localizing the country names
 */
export function extractCountry(url: string, localize: TFunction): ICountryInfo {
    const domainUrl = extractHostname(url);
    const tld = domainUrl.split('.').pop();
    const countryInfo = {} as ICountryInfo;

    switch (tld) {
         
        case 'in':
            countryInfo.country = localize('indiaText');
            countryInfo.countryImage = indiaImage;
            break;
        case 'de':
            countryInfo.country = localize('germanyText');
            countryInfo.countryImage = germanyImage;
            break;
        case 'fr':
            countryInfo.country = localize('franceText');
            countryInfo.countryImage = franceImage;
            break;
        case 'ru':
            countryInfo.country = localize('russiaText');
            countryInfo.countryImage = russiaImage;
            break;
        case 'kr':
            countryInfo.country = localize('southKoreaText');
            countryInfo.countryImage = koreaImage;
            break;
        case 'jp':
            countryInfo.country = localize('japanText');
            countryInfo.countryImage = japanImage;
            break;
        case 'it':
            countryInfo.country = localize('italyText');
            countryInfo.countryImage = italyImage;
            break;
        case 'edu':
        case 'gov':
        case 'mil':
            countryInfo.country = localize('unitedStatesText');
            countryInfo.countryImage = usaImage;
            break;
        case 'com':
        case 'org':
        case 'net':
        default:
            countryInfo.country = localize('unknownLabelText');
            countryInfo.countryImage = "unknown.png";
            break;
    }

    return countryInfo;
}

/** This function gets the human-readable time string value
 * @param time The date-time value at which the search result article was updated
 * This parameter is passed as date but comes here as string value
 */
export function getReadableTimeString(time: string): string {

    const searchResultTime = new Date(time);
    return moment([searchResultTime.getFullYear(), searchResultTime.getMonth(), searchResultTime.getDay()]).fromNow(); 
}