// <copyright file="bing-search-helper.test.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import { extractHostname, extractCategory, extractCountry, getReadableTimeString, ICountryInfo, ICategoryInfo } from "../bing-search-helper"
import { act } from "react-dom/test-utils";
import { useTranslation } from 'react-i18next';

let url = "https://4a7b540e91d7.ngrok.io";
let nonGovernmentDomainTestUrl = "https://www.google.com:80";
let educationalDomainTestUrl = "https://www.rasmussen.edu/";
let unknownDomainTestUrl = "https://www.indiannavy.in";
let germanDomainTestUrl = "https://www.smartvie.de";
let categoryInfo: ICategoryInfo;
let countryInfo: ICountryInfo;
let currentTime = new Date(Date.now());
let timeAnYearAgo = currentTime.setFullYear(currentTime.getFullYear() - 1);
let timeValue:string;
let hostName:string;
let { t } = useTranslation();

describe("BingSearchHelper", () => {

    it("Validates Extract HostName Method", () => {
        act(() => {
            hostName = extractHostname(url);
        });
        expect(hostName).toBe("4a7b540e91d7.ngrok.io");
    });

    it("Validates Extract Category Method", () => {
        act(() => {
            categoryInfo = extractCategory(nonGovernmentDomainTestUrl, t);
        });
        expect(categoryInfo.category).toBe(t('nonGovernmentLabelText'));

        act(() => {
            categoryInfo = extractCategory(educationalDomainTestUrl, t);
        });
        expect(categoryInfo.category).toBe(t('educationLabelText'));

        act(() => {
            categoryInfo = extractCategory(unknownDomainTestUrl, t);
        });
        expect(categoryInfo.category).toBe(t('unknownLabelText'));
    });

    it("Validates Extract Country Method", () => {
        act(() => {
            countryInfo = extractCountry(educationalDomainTestUrl, t);
        });
        expect(countryInfo.country).toBe(t('unitedStatesText'));

        act(() => {
            countryInfo = extractCountry(nonGovernmentDomainTestUrl, t);
        });
        expect(countryInfo.country).toBe(t('unknownLabelText'));

        act(() => {
            countryInfo = extractCountry(germanDomainTestUrl, t);
        });
        expect(countryInfo.country).toBe(t('germanyText'));
    });

    it("Validates Get Time Value Method", () => {
        act(() => {
            timeValue = getReadableTimeString(currentTime.toString());
        });
        expect(timeValue).toBe("today");

        act(() => {
            timeValue = getReadableTimeString(timeAnYearAgo.toString());
        });
        expect(timeValue).toBe("an year ago");
    });
});