// <copyright file="FakeHttpContext.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Fakes
{
    using System;
    using System.Security.Claims;
    using System.Security.Principal;

    using Microsoft.AspNetCore.Http;
    using Moq;

    /// <summary>
    /// Class to fake HTTP Context.
    /// </summary>
    public static class FakeHttpContext
    {
        /// <summary>
        /// Make fake HTTP context for unit testing.
        /// </summary>
        /// <returns>Fake HTTP context</returns>
        public static HttpContext GetMockHttpContextWithUserClaims()
        {
            var userAadObjectId = Guid.NewGuid();
            var context = new Mock<HttpContext>();
            var request = new Mock<HttpContext>();
            var response = new Mock<HttpContext>();
            var user = new Mock<ClaimsPrincipal>();
            var identity = new Mock<IIdentity>();
            var claim = new Claim[]
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userAadObjectId.ToString()),
            };

            context.Setup(ctx => ctx.User).Returns(user.Object);
            user.Setup(ctx => ctx.Identity).Returns(identity.Object);
            user.Setup(ctx => ctx.Claims).Returns(claim);
            identity.Setup(id => id.IsAuthenticated).Returns(true);
            identity.Setup(id => id.Name).Returns("test");
            return context.Object;
        }
    }
}