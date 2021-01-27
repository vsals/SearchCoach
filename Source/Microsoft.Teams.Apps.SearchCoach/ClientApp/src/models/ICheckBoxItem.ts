// <copyright file="ICheckBoxItem.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

/** This interface is used for maintaining the state of domain check-box values. */
export interface ICheckBoxItem {
    // unique value to identity each item.
    key: string;
    // name of the check-box item.
    title: string;
    // explains whether the check-box is checked or unchecked.
    isChecked: boolean;
}