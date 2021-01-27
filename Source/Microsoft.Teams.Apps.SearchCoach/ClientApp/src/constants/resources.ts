// <copyright file="resources.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import { IConstantSelectedItem } from "constants/search-filter-interface";

export default class Resources {

    // themes
    public static readonly body: string = "body";
    public static readonly theme: string = "theme";
    public static readonly default: string = "default";
    public static readonly light: string = "light";
    public static readonly dark: string = "dark";
    public static readonly contrast: string = "contrast";

    // screen size
    public static readonly screenWidthLarge: number = 1200;
    public static readonly screenWidthSmall: number = 1000;

    // countries list to be shown in location filter
    public static readonly countries: IConstantSelectedItem[] = [
        { name: "No Filter", id: "nf" } as IConstantSelectedItem,
        { name: "United States", id: "en-US" } as IConstantSelectedItem,
        { name: "Japan", id: "ja-JP" } as IConstantSelectedItem,
        { name: "France", id: "fr-FR" } as IConstantSelectedItem,
        { name: "Germany", id: "de-DE" } as IConstantSelectedItem,
        { name: "Italy", id: "it-IT" } as IConstantSelectedItem,
        { name: "South Korea", id: "ko-KR" } as IConstantSelectedItem,
        { name: "Russia", id: "ru-RU" } as IConstantSelectedItem
    ];

    // domains list to be shown in domain filter
    public static readonly domains: IConstantSelectedItem[] = [
        { name: ".com", id: "1" } as IConstantSelectedItem,
        { name: ".org", id: "2" } as IConstantSelectedItem,
        { name: ".edu", id: "3" } as IConstantSelectedItem,
        { name: ".net", id: "4" } as IConstantSelectedItem,
        { name: ".gov", id: "5" } as IConstantSelectedItem,
        { name: ".mil", id: "6" } as IConstantSelectedItem,
    ];

    // filter buttons
    public static readonly operatorText: string = "operators";
    public static readonly locationText: string = "location";
    public static readonly domainText: string = "alldomains";
    public static readonly anytimeText: string = "anytime";

    //marking unknown category or country
    public static readonly unknownText: string = "Unknown";

    //operator filters
    public static readonly andOperatorText: string = "AND";
    public static readonly orOperatorText: string = "OR";
    public static readonly notOperatorText: string = "NOT";

    // Average network timeout in milliseconds.
    public static readonly axiosDefaultTimeout: number = 10000;
}