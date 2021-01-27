// <copyright file="popup-menu-freshness-content.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import * as React from "react";
import { useTranslation } from "react-i18next";
import { Button, Menu, MenuButton, MenuItem } from "@fluentui/react-northstar";

import "./popup-menu.css";

interface IPopUpFreshnessContentProps {
    onFreshnessChange: (value: string) => void;
}

/** 
* This component contains freshness filter popup content.
* @param props {IPopUpFreshnessContentProps} The props for this component.
*/
const PopUpFreshnessContent: React.FunctionComponent<IPopUpFreshnessContentProps> = (props: IPopUpFreshnessContentProps) => {

    const [popup, onPopUpChange] = React.useState({ isOpen: false });
    const { t } = useTranslation();

    /**
    * The event handler handles popup open/close change.
    * @param isOpen {Boolean} Marks whether it is open/close.
    */
    const onPopupOpenChange = (isOpen: boolean) => {
        onPopUpChange({ isOpen: isOpen });
    }

    /**
    * The function checks the selected value of freshness and returns a key value.
    * It is used to omit the passing of UI facing strings to back-end.
    * @param selectedValue {String} selected freshness value.
    */
    const checkSelectedValue = (selectedValue: string) => {
        let freshnessKey = "";

        switch (selectedValue) {

            case t("anyTimeLabelText"):
                freshnessKey = "1";
                break;
            case t("past24hoursLabelText"):
                freshnessKey = "2";
                break;
            case t("pastWeekLabelText"):
                freshnessKey = "3";
                break;
            case t("pastMonthLabelText"):
                freshnessKey = "4";
                break;
            default:
                freshnessKey = "1";
                break;
        }

        return freshnessKey;
    }

    /**
    * The event handler click of freshness value.
    * @param event {any} Event object for click.
    */
    const handleClick = (event: any) => {
        const freshnessKeyValue = checkSelectedValue(event.currentTarget.innerText);
        props.onFreshnessChange(freshnessKeyValue);
        onPopupOpenChange(false);
    };

    return (
        <MenuButton
            trigger={<Button content={t("anyTimeButton")} text size="small" />}
            onOpenChange={({ open }: any) => onPopupOpenChange(open)}
            menu={
                <Menu id="freshness-menu" vertical pointing="start">
                    <MenuItem onClick={handleClick}>{t("anyTimeLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("past24hoursLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("pastWeekLabelText")}</MenuItem>
                    <MenuItem onClick={handleClick}>{t("pastMonthLabelText")}</MenuItem>
                </Menu>
            }
            open={popup.isOpen}
        />
    );
}

export default PopUpFreshnessContent