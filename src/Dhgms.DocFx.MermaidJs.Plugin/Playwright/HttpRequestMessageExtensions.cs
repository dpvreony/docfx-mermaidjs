// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Dhgms.DocFx.MermaidJs.Plugin.Playwright
{
    /// <summary>
    /// Extension methods for <see cref="HttpRequestMessage"/>.
    /// </summary>
    public static class HttpRequestMessageExtensions
    {
        /// <summary>
        /// Populates the HTTP request headers from a dictionary.
        /// </summary>
        /// <param name="httpRequestMessage">HTTP request message to populate.</param>
        /// <param name="requestHeaders">Request headers to use.</param>
        public static void PopulateHeaders(this HttpRequestMessage httpRequestMessage, Dictionary<string, string> requestHeaders)
        {
            ArgumentNullException.ThrowIfNull(httpRequestMessage);
            ArgumentNullException.ThrowIfNull(requestHeaders);

            var targetHeaders = httpRequestMessage.Headers;

            foreach (var requestHeader in requestHeaders)
            {
                targetHeaders.Add(requestHeader.Key, requestHeader.Value);
            }
        }
    }
}
