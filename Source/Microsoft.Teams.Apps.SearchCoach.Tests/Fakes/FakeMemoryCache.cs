// <copyright file="FakeMemoryCache.cs" company="Microsoft">
// Copyright (c) Microsoft. All rights reserved.
// </copyright>

namespace Microsoft.Teams.Apps.SearchCoach.Tests.Fakes
{
    using System;

    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Fake memory cache for test project.
    /// </summary>
#pragma warning disable CA1063 // Implement IDisposable Correctly
    public class FakeMemoryCache : IMemoryCache
#pragma warning restore CA1063 // Implement IDisposable Correctly
    {
        /// <inheritdoc/>
        public ICacheEntry CreateEntry(object key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
#pragma warning disable CA1063 // Implement IDisposable Correctly
#pragma warning disable CA1816 // Dispose methods should call SuppressFinalize
        public void Dispose()
#pragma warning restore CA1816 // Dispose methods should call SuppressFinalize
#pragma warning restore CA1063 // Implement IDisposable Correctly
        {
#pragma warning disable CA1065 // Do not raise exceptions in unexpected locations
            throw new NotImplementedException();
#pragma warning restore CA1065 // Do not raise exceptions in unexpected locations
        }

        /// <inheritdoc/>
        public void Remove(object key)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public bool TryGetValue(object key, out object value)
        {
            value = true;
            return true;
        }
    }
}