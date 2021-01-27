// <copyright file="authentication-metadata-api.ts" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

import axiosDecorator from "../api/axios-decorator";
import { AxiosResponse } from "axios";

/**
* Get authentication meta-data from API.
* @param {String} windowLocationOriginDomain Application base URL.
* @param {String} login_hint Login hint for SSO.
*/
export const getAuthenticationConsentMetadata = async (windowLocationOriginDomain: string, login_hint: string): Promise<AxiosResponse> => {
    const url = `/api/authenticationMetadata/consentUrl?windowLocationOriginDomain=${windowLocationOriginDomain}&loginhint=${login_hint}`;
    return await axiosDecorator.get(url, undefined, false);
}