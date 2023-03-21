// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.HttpServer;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Javascript
{
    /// <summary>
    /// Markdown renderer using Playwright.
    /// </summary>
    public sealed class PlaywrightRenderer
    {
        private readonly TestServer _mermaidHttpServerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRenderer"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logging framework instance.</param>
        public PlaywrightRenderer(ILoggerFactory loggerFactory)
        {
            _mermaidHttpServerFactory = MermaidHttpServerFactory.GetTestServer(loggerFactory);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram.
        /// </summary>
        /// <param name="diagram">Diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<string?> GetSvg(string diagram)
        {
            using (var playwright = await Playwright.CreateAsync()
                .ConfigureAwait(false))
            await using (var browser = await playwright.Chromium.LaunchAsync(new() { Headless = true }))
            {
                var page = await browser.NewPageAsync()
                    .ConfigureAwait(false);

                await page.RouteAsync(
                        "https://localhost/mermaid.html",
                        route => MermaidPostHandler(route, diagram))
                    .ConfigureAwait(false);

                await page.RouteAsync(
                        "*",
                        route => DefaultHandler(route))
                    .ConfigureAwait(false);

                var pageResponse = await page.GotoAsync("https://localhost/mermaid.html")
                    .ConfigureAwait(false);

                var mermaidElement = await page.QuerySelectorAsync("mermaid-element")
                    .ConfigureAwait(false);

                if (mermaidElement == null)
                {
                    return null;
                }

                var innerText = await mermaidElement.InnerTextAsync().ConfigureAwait(false);
                return innerText;
            }
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route, string diagram)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);

            switch (request.Method)
            {
                case "DELETE":
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case "GET":
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;
                case "HEAD":
                    httpRequestMessage.Method = HttpMethod.Head;
                    break;
                case "OPTIONS":
                    httpRequestMessage.Method = HttpMethod.Options;
                    break;
                case "PATCH":
                    httpRequestMessage.Method = HttpMethod.Patch;
                    break;
                case "POST":
                    httpRequestMessage.Method = HttpMethod.Post;
                    httpRequestMessage.Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
                    {
                        new("diagram", diagram)
                    });
                    break;
                case "PUT":
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                case "TRACE":
                    httpRequestMessage.Method = HttpMethod.Trace;
                    break;
                default:
                    throw new ArgumentException("Failed to map request HTTP method", nameof(route));
            }

            return httpRequestMessage;
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            PopulateHeaders(httpRequestMessage, request.Headers);

            switch (request.Method)
            {
                case "DELETE":
                    httpRequestMessage.Method = HttpMethod.Delete;
                    break;
                case "GET":
                    httpRequestMessage.Method = HttpMethod.Get;
                    break;
                case "HEAD":
                    httpRequestMessage.Method = HttpMethod.Head;
                    break;
                case "OPTIONS":
                    httpRequestMessage.Method = HttpMethod.Options;
                    break;
                case "PATCH":
                    httpRequestMessage.Method = HttpMethod.Patch;
                    break;
                case "POST":
                    httpRequestMessage.Method = HttpMethod.Post;

                    if (request.PostDataBuffer != null)
                    {
                        httpRequestMessage.Content = new StreamContent(new MemoryStream(request.PostDataBuffer));
                    }

                    break;
                case "PUT":
                    httpRequestMessage.Method = HttpMethod.Put;
                    break;
                case "TRACE":
                    httpRequestMessage.Method = HttpMethod.Trace;
                    break;
                default:
                    throw new ArgumentException("Failed to map request HTTP method", nameof(route));
            }

            return httpRequestMessage;
        }

        private void PopulateHeaders(HttpRequestMessage httpRequestMessage, Dictionary<string, string> requestHeaders)
        {
            var targetHeaders = httpRequestMessage.Headers;

            foreach (var requestHeader in requestHeaders)
            {
                targetHeaders.Add(requestHeader.Key, requestHeader.Value);
            }
        }
        
        private async Task MermaidPostHandler(IRoute route, string diagram)
        {
            using (var client = _mermaidHttpServerFactory.CreateClient())
            using (var request = GetRequestFromRoute(route, diagram))
            {
                var response = await client.SendAsync(request)
                    .ConfigureAwait(false);

                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = (int)response.StatusCode,
                    Body = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                };

                if (response.Content.Headers.ContentType != null)
                {
                    routeFulfillOptions.ContentType = response.Content.Headers.ContentType.ToString();
                }

                await route.FulfillAsync()
                    .ConfigureAwait(false);
            }
        }

        private async Task DefaultHandler(IRoute route)
        {
            using (var client = _mermaidHttpServerFactory.CreateClient())
            using (var request = GetRequestFromRoute(route))
            {
                var response = await client.SendAsync(request)
                    .ConfigureAwait(false);

                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = (int)response.StatusCode,
                    Body = await response.Content.ReadAsStringAsync().ConfigureAwait(false),
                };

                if (response.Content.Headers.ContentType != null)
                {
                    routeFulfillOptions.ContentType = response.Content.Headers.ContentType.ToString();
                }

                await route.FulfillAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
