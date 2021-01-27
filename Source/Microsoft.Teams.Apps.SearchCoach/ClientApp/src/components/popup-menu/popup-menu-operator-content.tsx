// <copyright file="popup-menu-operator-content.tsx" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import React from "react";
import { useTranslation } from "react-i18next";
import { Button, Text, Flex } from "@fluentui/react-northstar";
import { AddIcon } from "@fluentui/react-icons-northstar"
import resources from "../../constants/resources";

import "./popup-menu.css";

interface IPopUpOperatorContentProps {
    onOperatorAdded: (value: string) => void;
}

/** 
* This component contains operator filter popup content.
* @param props {IPopUpOperatorContentProps} The props for this component.
*/
const PopUpOperatorContent: React.FunctionComponent<IPopUpOperatorContentProps> = (props: IPopUpOperatorContentProps) => {
    const { t } = useTranslation();

    return (
        <div className="operator-popup">
            <Flex gap="gap.smaller">
                <Flex.Item align="start" size="size.quarter">
                    <Button size="small" content={t("operatorsAndButtonLabel")} className="and-or-not-operators" icon={<AddIcon size="smaller" />} onClick={() => props.onOperatorAdded(resources.andOperatorText)} />
                </Flex.Item>
                <Flex.Item align="start" size="size.quarter">
                    <Button size="small" content={t("operatorsOrdButtonLabel")} className="and-or-not-operators" icon={<AddIcon size="smaller" />} onClick={() => props.onOperatorAdded(resources.orOperatorText)} />
                </Flex.Item>
                <Flex.Item align="start" size="size.quarter">
                    <Button size="small" content={t("operatorsNotButtonLabel")} icon={<AddIcon size="smaller" />} className="and-or-not-operators" onClick={() => props.onOperatorAdded(resources.notOperatorText)} />
                </Flex.Item>
            </Flex>
            <Flex className="margin-top">
                <Text content={t("operatorsHelpTitle")} weight="semibold" />
            </Flex>
            <Flex>
                <Text content={t("operatorsHelpContent")} />
            </Flex>
        </div>
    );
}

export default PopUpOperatorContent