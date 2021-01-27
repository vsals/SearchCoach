// <copyright file="ServicesExtension.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Localization;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Azure;
    using Microsoft.Bot.Builder.BotFramework;
    using Microsoft.Bot.Builder.Integration.AspNet.Core;
    using Microsoft.Bot.Connector;
    using Microsoft.Bot.Connector.Authentication;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Identity.Client;
    using Microsoft.Teams.Apps.SearchCoach.Bot;
    using Microsoft.Teams.Apps.SearchCoach.Common.BackgroundService;
    using Microsoft.Teams.Apps.SearchCoach.Helpers;
    using Microsoft.Teams.Apps.SearchCoach.ModelMappers;
    using Microsoft.Teams.Apps.SearchCoach.Models.Configuration;
    using Microsoft.Teams.Apps.SearchCoach.Providers;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Authentication;
    using Microsoft.Teams.Apps.SearchCoach.Services.MicrosoftGraph.Users;

    /// <summary>
    /// Class which helps to extend ServiceCollection.
    /// </summary>
    public static class ServicesExtension
    {
        /// <summary>
        /// Adds application configuration settings to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddConfigurationSettings(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<BotSettings>(options =>
            {
                options.AppBasePath = configuration.GetValue<string>("App:AppBaseUri");
                options.TenantId = configuration.GetValue<string>("App:TenantId");
                options.ManifestId = configuration.GetValue<string>("App:ManifestId");
                options.MicrosoftAppId = configuration.GetValue<string>("MicrosoftAppId");
                options.MicrosoftAppPassword = configuration.GetValue<string>("MicrosoftAppPassword");
                options.CardCacheDurationInHour = configuration.GetValue<double>("App:CardCacheDurationInHour");
            });

            services.Configure<TelemetrySettings>(options =>
            {
                options.InstrumentationKey = configuration.GetValue<string>("ApplicationInsights:InstrumentationKey");
            });

            services.Configure<StorageSettings>(options =>
            {
                options.ConnectionString = configuration.GetValue<string>("Storage:ConnectionString");
            });

            services.Configure<BingSearchSettings>(options =>
            {
                options.ApiUrl = configuration.GetValue<string>("BingSearch:ApiUrl");
                options.ApiKey = configuration.GetValue<string>("BingSearch:ApiKey");
                options.SafeSearch = configuration.GetValue<string>("BingSearch:SafeSearch");
                options.DefaultCountryCode = configuration.GetValue<string>("BingSearch:DefaultCountryCode");
            });

            services.Configure<AzureActiveDirectorySettings>(options =>
            {
                options.GraphScope = configuration.GetValue<string>("AzureAd:GraphScope");
            });
        }

        /// <summary>
        /// Add confidential credential provider to access API.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddConfidentialCredentialProvider(this IServiceCollection services, IConfiguration configuration)
        {
            configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            IConfidentialClientApplication confidentialClientApp = ConfidentialClientApplicationBuilder.Create(configuration["MicrosoftAppId"])
                .WithClientSecret(configuration["MicrosoftAppPassword"])
                .Build();
            services.AddSingleton<IConfidentialClientApplication>(confidentialClientApp);
        }

        /// <summary>
        /// Adds credential providers for authentication.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddCredentialProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<ICredentialProvider, ConfigurationCredentialProvider>();
            services.AddSingleton(new MicrosoftAppCredentials(configuration.GetValue<string>("MicrosoftAppId"), configuration.GetValue<string>("MicrosoftAppPassword")));

#pragma warning disable CA2000 // This is singleton which has lifetime same as the app.
            services.AddSingleton(new OAuthClient(new MicrosoftAppCredentials(configuration.GetValue<string>("MicrosoftAppId"), configuration.GetValue<string>("MicrosoftAppPassword"))));
#pragma warning restore CA2000 // This is singleton which has lifetime same as the app.
        }

        /// <summary>
        /// Adds providers to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IQuestionAnswersStorageProvider, QuestionAnswersStorageProvider>();
            services.AddSingleton<ITeamStorageProvider, TeamStorageProvider>();
            services.AddSingleton<IUserDetailProvider, UserDetailProvider>();

            services.AddSingleton<IUserResponseStorageProvider>(new UserResponseStorageProvider(
                configuration.GetValue<string>("Storage:ConnectionString"),
#pragma warning disable CA2000 // Dispose objects before losing scope is not required.
                new LoggerFactory().CreateLogger<BaseStorageProvider>()));
#pragma warning restore CA2000 // Dispose objects before losing scope is not required.

            services.AddSingleton<IQuestionAnswersStorageProvider>(new QuestionAnswersStorageProvider(
                configuration.GetValue<string>("Storage:ConnectionString"),
#pragma warning disable CA2000 // Dispose objects before losing scope is not required.
                new LoggerFactory().CreateLogger<BaseStorageProvider>()));
#pragma warning restore CA2000 // Dispose objects before losing scope is not required.

            services
               .AddSingleton<BackgroundTaskWrapper>();
        }

        /// <summary>
        /// Adds helpers to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddHelpers(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration.GetValue<string>("ApplicationInsights:InstrumentationKey"));
            services
                .AddSingleton<NotificationHelper>();

            services
                .AddSingleton<CardHelper>();
            services
                .AddSingleton<IQuestionAnswersMapper, QuestionAnswersMapper>();
            services
                .AddSingleton<QuestionAnswersHelper>();
            services
               .AddSingleton<IUserResponseMapper, UserResponseMapper>();

            services.AddSingleton<ITokenHelper, AccessTokenHelper>();
            services.AddSingleton<TokenAcquisitionHelper>();
        }

        /// <summary>
        /// Adds services to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddTransient<IUsersService, UsersService>();
        }

        /// <summary>
        /// Adds user state and conversation state to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddBotStates(this IServiceCollection services, IConfiguration configuration)
        {
            // Create the User state. (Used in this bot's Dialog implementation.)
            services.AddSingleton<UserState>();

            // Create the Conversation state. (Used by the Dialog system itself.)
            services.AddSingleton<ConversationState>();

            // For conversation state.
            services.AddSingleton<IStorage>(new AzureBlobStorage(configuration.GetValue<string>("Storage:ConnectionString"), "bot-state"));
        }

        /// <summary>
        /// Adds bot framework adapter to specified IServiceCollection.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        public static void AddBotFrameworkAdapter(this IServiceCollection services)
        {
            // Create the Bot Framework Adapter with error handling enabled.
            services.AddSingleton<IBotFrameworkHttpAdapter, AdapterWithErrorHandler>();

            // Create the middleware that will be added to the middleware pipeline in the AdapterWithErrorHandler.
            services.AddSingleton<ActivityMiddleware>();
            services.AddTransient(serviceProvider => (BotFrameworkAdapter)serviceProvider.GetRequiredService<IBotFrameworkHttpAdapter>());
            services.AddTransient<IBot, Bot.ActivityHandler>();
        }

        /// <summary>
        /// Add localization.
        /// </summary>
        /// <param name="services">Collection of services.</param>
        /// <param name="configuration">Application configuration properties.</param>
        public static void AddLocalization(this IServiceCollection services, IConfiguration configuration)
        {
            // Add i18n.
            services.AddLocalization(options => options.ResourcesPath = "Resources");

            services.Configure<RequestLocalizationOptions>(options =>
            {
                var defaultCulture = CultureInfo.GetCultureInfo(configuration.GetValue<string>("i18n:DefaultCulture"));
                var supportedCultures = configuration.GetValue<string>("i18n:SupportedCultures").Split(',')
                    .Select(culture => CultureInfo.GetCultureInfo(culture))
                    .ToList();

                options.DefaultRequestCulture = new RequestCulture(defaultCulture);
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders = new List<IRequestCultureProvider>
                {
                    new BotLocalizationCultureProvider(),
                };
            });
        }
    }
}