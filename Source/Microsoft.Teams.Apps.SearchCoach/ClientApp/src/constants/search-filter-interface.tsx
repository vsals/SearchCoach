// <copyright file="search-filter-interface.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

// The interface used for maintaining the list of countries and domains
export interface IConstantSelectedItem {
	name: string;
	id: string;
}

/** The interface used for maintaining the selected 
 * drop-down value for location filters
 */
export interface ISelectedDropdownItem {
    header: string,
    key: string,
}