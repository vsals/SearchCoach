// <copyright file="AuthenticationOptions.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Authentication
{
    /// <summary>
    /// Options useful for authenticating the user successfully.
    /// </summary>
    public class AuthenticationOptions
    {
        /// <summary>
        /// Gets or sets the Azure active directory instance.
        /// </summary>
        public string AzureAadInstance { get; set; }

        /// <summary>
        /// Gets or sets the Azure active directory tenant id.
        /// </summary>
        public string AzureAadTenantId { get; set; }

        /// <summary>
        /// Gets or sets the Azure active directory client id.
        /// </summary>
        public string AzureAadClientId { get; set; }

        /// <summary>
        /// Gets or sets the Azure active directory application id URI.
        /// </summary>
        public string AzureAadApplicationIdUri { get; set; }

        /// <summary>
        /// Gets or sets the Azure active directory valid issuers.
        /// </summary>
        public string AzureAadValidIssuers { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the "must be a UPN in the authorized list
        /// in order to use the app and create notifications" check should be disabled.
        /// </summary>
        public bool DisableCreatorUpnCheck { get; set; }

        /// <summary>
        /// Gets or sets admin team id. Members of the team would only be allowed to access the app.
        /// </summary>
        public string AdminTeamId { get; set; }
    }
}
