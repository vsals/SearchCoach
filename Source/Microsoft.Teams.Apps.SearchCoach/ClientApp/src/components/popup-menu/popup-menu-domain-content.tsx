// <copyright file="popup-menu-domain-content.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { useTranslation } from "react-i18next";
import { Text, Flex, Checkbox } from "@fluentui/react-northstar";
import { ICheckBoxItem } from "../../models/ICheckBoxItem";
import { IConstantSelectedItem } from "../../constants/search-filter-interface";

import "./popup-menu.css";

interface IPopUpDomainContentProps {
    // This property describes the constant domain values picked from resources.
    domains: IConstantSelectedItem[];
    onDomainValueChange: (domainValueState: ICheckBoxItem[]) => void;
    // This property describes the selected domains through filter.
    selectedDomainValuesList: ICheckBoxItem[];
}

/** 
* This component contains domain filter popup content.
* @param props {IPopUpDomainContentProps} The props for this component.
*/
const PopUpDomainContent: React.FunctionComponent<IPopUpDomainContentProps> = (props: IPopUpDomainContentProps) => {
    const { t } = useTranslation();
    const [selectedCheckboxes, setSelectedCheckboxes] = React.useState(props.selectedDomainValuesList);
    const data = props.selectedDomainValuesList;

    React.useEffect(() => {
        setFilterCheckboxes(props.selectedDomainValuesList);
    }, [props.selectedDomainValuesList]);

    /**
    * The event handler is called when the domain values are selected/deselected.
    * for maintaining state for this component and passes changed state back to parent component.
    * @param key {String} The domain value key.
    * @param checked {Boolean} Marks whether the domain value check-box is checked/unchecked.
    */
    const handleDomainValueChange = (key: string, checked: boolean) => {
        const checkboxList = data.map((checkbox: ICheckBoxItem) => { return checkbox.key === key ? { ...checkbox, isChecked: checked } : checkbox });
        setFilterCheckboxes(checkboxList);
        props.onDomainValueChange(checkboxList);
    };

    /**
    * This function sets the domain check-boxes checked/unchecked value.
    * @param filterItems {ICheckBoxItem[]} The domain filter values.
    */
    const setFilterCheckboxes = (filterItems: ICheckBoxItem[]) => {

        if (filterItems) {
            let items = [...filterItems];
            setSelectedCheckboxes(items);
        } else {
            let items: ICheckBoxItem[] = [];
            items = props.domains.map((value: IConstantSelectedItem) => ({ key: value.id, title: value.name, isChecked: false }));
            setSelectedCheckboxes(items);
        }
    }

    return (
        <div className="domain-popup">
          
            <Flex gap="gap.medium" className="domain-container">
                <Flex className="domain-button-container">
                    <Checkbox key="0" onChange={() => handleDomainValueChange(selectedCheckboxes[0].key, !selectedCheckboxes[0].isChecked)} checked={(selectedCheckboxes[0].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Text align="start" content={selectedCheckboxes[0].title} size="small" weight="semibold" />
                        <Text align="start" content={t("comCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
                <Flex className="domain-button-container">
                    <Checkbox key="1" onChange={() =>handleDomainValueChange(selectedCheckboxes[1].key, !selectedCheckboxes[1].isChecked)} checked={(selectedCheckboxes[1].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Text align="start" content={selectedCheckboxes[1].title} size="small" weight="semibold" />
                        <Text align="start" content={t("orgCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
            </Flex>
            <Flex gap="gap.medium" className="domain-container">
                <Flex className="domain-button-container">
                    <Checkbox key="2" onChange={() => handleDomainValueChange(selectedCheckboxes[2].key, !selectedCheckboxes[2].isChecked)} checked={(selectedCheckboxes[2].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Text align="start" content={selectedCheckboxes[2].title} size="small" weight="semibold" />
                        <Text align="start" content={t("eduCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
                <Flex className="domain-button-container">
                    <Checkbox key="3" onChange={() => handleDomainValueChange(selectedCheckboxes[3].key, !selectedCheckboxes[3].isChecked)} checked={(selectedCheckboxes[3].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Text align="start" content={selectedCheckboxes[3].title} size="small" weight="semibold" />
                        <Text align="start" content={t("netCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
            </Flex>
            <Flex gap="gap.medium" className="domain-container">
                <Flex className="domain-button-container">
                    <Checkbox key="4" onChange={() => handleDomainValueChange(selectedCheckboxes[4].key, !selectedCheckboxes[4].isChecked)} checked={(selectedCheckboxes[4].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center"><Text align="start" content={selectedCheckboxes[4].title} size="small" weight="semibold" />
                        <Text align="start" content={t("govCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
                <Flex className="domain-button-container">
                    <Checkbox key="5" onChange={() => handleDomainValueChange(selectedCheckboxes[5].key, !selectedCheckboxes[5].isChecked)} checked={(selectedCheckboxes[5].isChecked)} />
                    <Flex gap="gap.smaller" vAlign="center">
                        <Text align="start" content={selectedCheckboxes[5].title} size="small" weight="semibold" />
                        <Text align="start" content={t("milCheckboxLabel")} size="small" />
                    </Flex>
                </Flex>
            </Flex>
            <Flex className="margin-top">
                <Text content={t("domainHelpTitle")} weight="semibold" />
            </Flex>
            <Flex>
                <Text content={t("domainHelpContent")} />
            </Flex>
        </div>
    );
}

export default PopUpDomainContent