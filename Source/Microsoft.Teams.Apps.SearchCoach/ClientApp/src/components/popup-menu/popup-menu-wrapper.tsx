// <copyright file="popup-menu-wrapper.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from "react";
import { Button, Popup } from "@fluentui/react-northstar";
import PopUpDomainContent from "./popup-menu-domain-content";
import PopUpFreshnessContent from "./popup-menu-freshness-content";
import { ISelectedDropdownItem, IConstantSelectedItem } from "../../constants/search-filter-interface";
import resources from "../../constants/resources";
import { ICheckBoxItem } from "../../models/ICheckBoxItem";

import "./popup-menu.css";

interface IPopupMenuWrapperProps {
    activeButton: string;
    onOperatorAdded: (value: string) => void;
    onCountryChange: (value: ISelectedDropdownItem) => void;
    selectedCountry: ISelectedDropdownItem;
    onFreshnessChange: (value: string) => void;
    onDomainValuesChange: (selectedDomains: string) => void;
    selectedDomains: string;
    // This parameter is used to pass the popup content from parent component.
    popUpContent?: React.ReactNode;
    // This parameter is used to pass the button content from parent component.
    buttonContent?: string;
}

/** 
* This component is a wrapper over all the popup filters.
* @param props {IPopupMenuWrapperProps} The props for this component.
*/
const PopUpMenuWrapper: React.FunctionComponent<IPopupMenuWrapperProps> = (props: IPopupMenuWrapperProps) => {

    const [popup, onPopUpChange] = React.useState({ isPopUpOpen: false });
    const initialDomainValuesList: ICheckBoxItem[] = [];
    const [domainValues, onDomainValueChange] = React.useState(initialDomainValuesList);

    /**
    * The event handler handles popup open/close change.
    * @param isOpen {Boolean} Marks whether it is open/close.
    */
    const onPopupOpenChange = (isOpen: boolean) => {
        onPopUpChange({ isPopUpOpen: isOpen });

        resources.domains.map((value: IConstantSelectedItem) => {
            initialDomainValuesList.push({ key: value.id, title: value.name, isChecked: false });
        });

        if (props.selectedDomains) {
            const splitDomains = props.selectedDomains.split(';');

            splitDomains.map((selectedValue: string) => {
                const filteredIndex = initialDomainValuesList.findIndex(value => value.title === selectedValue);

                if (filteredIndex !== -1) {
                    initialDomainValuesList[filteredIndex].isChecked = true;
                }
            });
        }

        onDomainValueChange(initialDomainValuesList);
    }

    /**
    * The event handler handles domain values checked/unchecked list
    * and passes changed state back to parent component.
    * @param domainListValues {ICheckBoxItem[]} The filter domain list values.
    */
    const handleDomainValueChange = (domainListValues: ICheckBoxItem[]) => {
        onDomainValueChange(domainListValues);
        let selectedDomains: string = "";

        // Here we are mapping the domain list values and combining them in a string with delimiter (;).
        domainListValues.map((checkbox: ICheckBoxItem) => {
            if (checkbox.isChecked) {
                if (selectedDomains === "") {
                    selectedDomains += checkbox.title;
                } else {
                    selectedDomains += ";" + checkbox.title;
                }
            }
        });

        props.onDomainValuesChange(selectedDomains);
    }

    /** 
    * We have kept the condition for freshness popup separately as here the popup sub
    * component is called directly and buttonContent and popupContent are not passed.
    */
    if (props.activeButton === resources.anytimeText) {
        return (
            <PopUpFreshnessContent {...props} />
        );
    }

    /** 
    * We have kept the condition for domain popup separately as here popupContent cannot be
    * passed as the functions called within the content like onDomainValueChange are defined
    * in this component.
    */
    else if (props.activeButton === resources.domainText) {
        return (
            <Popup
                trigger={<Button text content={props.buttonContent} size="small" />}
                content={<PopUpDomainContent domains={resources.domains} onDomainValueChange={handleDomainValueChange} selectedDomainValuesList={domainValues} />}
                align={props.activeButton === resources.domainText ? "center" : "start"}
                position="below"
                pointing={true}
                onOpenChange={({ open }: any) => onPopupOpenChange(open)}
                trapFocus
                open={popup.isPopUpOpen}
            />
        );
    }
    else {
        return (
            <Popup
                trigger={<Button text content={props.buttonContent} size="small" />}
                content={props.popUpContent}
                align={props.activeButton === resources.domainText ? "center" : "start"}
                position="below"
                pointing={true}
                onOpenChange={({ open }: any) => onPopupOpenChange(open)}
                trapFocus
                open={popup.isPopUpOpen}
            />
        );
    }
}

export default PopUpMenuWrapper