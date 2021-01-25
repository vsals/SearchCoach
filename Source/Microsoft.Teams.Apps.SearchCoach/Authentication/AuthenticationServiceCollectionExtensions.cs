// <copyright file="AuthenticationServiceCollectionExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authentication.AzureAD.UI;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Identity.Web;
    using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;

    /// <summary>
    /// Extension class for registering authentication services in Dependency Injection container.
    /// </summary>
    public static class AuthenticationServiceCollectionExtensions
    {
        private const string ClientIdConfigurationSettingsKey = "MicrosoftAppId";
        private const string TenantIdConfigurationSettingsKey = "App:TenantId";
        private const string ApplicationIdURIConfigurationSettingsKey = "AzureAd:ApplicationIdURI";
        private const string ValidIssuersConfigurationSettingsKey = "AzureAd:ValidIssuers";

        /// <summary>
        /// Extension method to register the authentication services.
        /// </summary>
        /// <param name="services">IServiceCollection instance for configuring authentication services.</param>
        /// <param name="configuration">IConfiguration instance for using the configuration properties.</param>
        public static void AddSearchCoachAuthentication(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            // This works specifically for single tenant application.
            ValidateAuthenticationConfigurationSettings(configuration);

            services.AddProtectedWebApi(configuration)
               .AddProtectedWebApiCallsProtectedWebApi(configuration)
               .AddInMemoryTokenCaches();

            services.Configure<JwtBearerOptions>(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                var azureADOptions = new AzureADOptions();
                var botOptions = new BotSettings();

                configuration.Bind("AzureAd", azureADOptions);
                configuration.Bind("App", botOptions);

                options.Authority = $"{azureADOptions.Instance}{botOptions.TenantId}/v2.0";
                options.SaveToken = true;
                options.TokenValidationParameters.ValidAudiences = GetValidAudiences(configuration);
                options.TokenValidationParameters.AudienceValidator = AudienceValidator;
                options.TokenValidationParameters.ValidIssuers = GetValidIssuers(configuration);
            });
        }

        /// <summary>
        /// Validate whether the configuration settings are missing or not.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        private static void ValidateAuthenticationConfigurationSettings(IConfiguration configuration)
        {
            var clientId = configuration[AuthenticationServiceCollectionExtensions.ClientIdConfigurationSettingsKey];
            if (string.IsNullOrWhiteSpace(clientId))
            {
                throw new ApplicationException("AzureAD ClientId is missing in the configuration file.");
            }

            var tenantId = configuration[AuthenticationServiceCollectionExtensions.TenantIdConfigurationSettingsKey];
            if (string.IsNullOrWhiteSpace(tenantId))
            {
                throw new ApplicationException("AzureAD TenantId is missing in the configuration file.");
            }

            var applicationIdURI = configuration[AuthenticationServiceCollectionExtensions.ApplicationIdURIConfigurationSettingsKey];
            if (string.IsNullOrWhiteSpace(applicationIdURI))
            {
                throw new ApplicationException("AzureAD ApplicationIdURI is missing in the configuration file.");
            }

            var validIssuers = configuration[AuthenticationServiceCollectionExtensions.ValidIssuersConfigurationSettingsKey];
            if (string.IsNullOrWhiteSpace(validIssuers))
            {
                throw new ApplicationException("AzureAD ValidIssuers is missing in the configuration file.");
            }
        }

        /// <summary>
        /// Gets the collection of configuration settings.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <param name="configurationSettingsKey">A valid issuer key.</param>
        /// <returns>A collection of configuration setting.</returns>
        private static IEnumerable<string> GetSettings(IConfiguration configuration, string configurationSettingsKey)
        {
            var configurationSettingsValue = configuration[configurationSettingsKey];
            var settings = configurationSettingsValue
                ?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                ?.Select(configurationSettingValue => configurationSettingValue.Trim());

            settings = settings ?? throw new ApplicationException($"{configurationSettingsKey} does not contain a valid value in the configuration file.");

            return settings;
        }

        /// <summary>
        /// Gets a collection of valid audience.
        /// </summary>
        /// <returns>A collection of valid audience.</returns>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        private static IEnumerable<string> GetValidAudiences(IConfiguration configuration)
        {
            var clientId = configuration[AuthenticationServiceCollectionExtensions.ClientIdConfigurationSettingsKey];

            var applicationIdURI = configuration[AuthenticationServiceCollectionExtensions.ApplicationIdURIConfigurationSettingsKey];

            var validAudiences = new List<string> { clientId, applicationIdURI.ToUpperInvariant() };

            return validAudiences;
        }

        /// <summary>
        /// Gets a collection of valid issuers.
        /// </summary>
        /// <param name="configuration">Represents a set of key/value application configuration properties.</param>
        /// <returns>A collection of valid issuers.</returns>
        private static IEnumerable<string> GetValidIssuers(IConfiguration configuration)
        {
            var tenantId = configuration[AuthenticationServiceCollectionExtensions.TenantIdConfigurationSettingsKey];

            var validIssuers =
                AuthenticationServiceCollectionExtensions.GetSettings(
                    configuration,
                    AuthenticationServiceCollectionExtensions.ValidIssuersConfigurationSettingsKey);

            validIssuers = validIssuers.Select(validIssuer => validIssuer.Replace("TENANT_ID", tenantId, StringComparison.OrdinalIgnoreCase));

            return validIssuers;
        }

        /// <summary>
        /// Checks whether an audience is valid or not.
        /// </summary>
        /// <param name="tokenAudiences">A collection of audience token.</param>
        /// <param name="securityToken">A security token.</param>
        /// <param name="validationParameters">Contains a set of parameters that are used by a Microsoft.IdentityModel.Tokens.SecurityTokenHandler
        /// when validating a Microsoft.IdentityModel.Tokens.SecurityToken.</param>
        /// <returns>A boolean value represents validity of audience.</returns>
        private static bool AudienceValidator(
            IEnumerable<string> tokenAudiences,
            SecurityToken securityToken,
            TokenValidationParameters validationParameters)
        {
            if (tokenAudiences == null || !tokenAudiences.Any())
            {
                throw new ApplicationException("No audience defined in token!");
            }

            var validAudiences = validationParameters.ValidAudiences;
            if (validAudiences == null || !validAudiences.Any())
            {
                throw new ApplicationException("No valid audiences defined in validationParameters!");
            }

            foreach (var tokenAudience in tokenAudiences)
            {
                if (validAudiences.Any(validAudience => validAudience.Equals(tokenAudience, StringComparison.OrdinalIgnoreCase)))
                {
                    return true;
                }
            }

            return false;
        }
    }
}