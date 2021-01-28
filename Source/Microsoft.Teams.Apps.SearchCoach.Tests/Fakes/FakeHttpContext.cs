// <copyright file="FakeHttpContext.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Fakes
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Teams.Apps.SearchCoach.Authentication.AuthenticationPolicy;

    /// <summary>
    /// Class to fake HTTP Context.
    /// </summary>
    public static class FakeHttpContext
    {
        /// <summary>
        /// Make fake HTTP context for unit testing.
        /// </summary>
        /// <returns>Fake HTTP context.</returns>
#pragma warning disable CA1024 // Use properties where appropriate
        public static HttpContext GetMockHttpContextWithUserClaims()
#pragma warning restore CA1024 // Use properties where appropriate
        {
            var userAadObjectId = "00000000-0000-0000-0000-000000000001";
            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(
                                "http://schemas.microsoft.com/identity/claims/objectidentifier",
                                userAadObjectId),
                        })),
            };

            context.Request.Headers["Authorization"] = "fake_token";
            return context;
        }

        /// <summary>
        /// Get authorization handler context.
        /// </summary>
        /// <returns>Authorization handler context.</returns>
        public static AuthorizationHandlerContext GetAuthorizationHandlerContextForTeamMember()
        {
            var userAadObjectId = "1a1cce71-2833-4345-86e2-e9047f73e6af";
            var requirement = new[] { new MustBeTeamMemberUserPolicyRequirement() };

            var context = new DefaultHttpContext()
            {
                User = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new Claim[]
                        {
                            new Claim(
                                "http://schemas.microsoft.com/identity/claims/objectidentifier",
                                userAadObjectId.ToString()),
                        })),
            };

            context.Request.Headers["Authorization"] = "fake_token";
            context.Request.QueryString = new QueryString("?groupId=1a1cce71-2833-4345-86e2-e9047f73e6af");

            var filters = new List<IFilterMetadata>();

            var resource = new AuthorizationFilterContext(
                new ActionContext(
                    context,
                    new AspNetCore.Routing.RouteData(),
                    new AspNetCore.Mvc.Abstractions.ActionDescriptor()), filters);

            return new AuthorizationHandlerContext(requirement, context.User, resource);
        }
    }
}