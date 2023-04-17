// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.HttpServer;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Playwright
{
    /// <summary>
    /// Markdown renderer using Playwright.
    /// </summary>
    public sealed class PlaywrightRenderer
    {
        private readonly TestServer _mermaidHttpServerFactory;
        private readonly ILogger<PlaywrightRenderer> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="PlaywrightRenderer"/> class.
        /// </summary>
        /// <param name="loggerFactory">Logging framework instance.</param>
        public PlaywrightRenderer(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _logger = loggerFactory.CreateLogger<PlaywrightRenderer>();
            _mermaidHttpServerFactory = MermaidHttpServerFactory.GetTestServer(loggerFactory);
        }

        /// <summary>
        /// Gets the SVG for the Mermaid Diagram.
        /// </summary>
        /// <param name="markdown">Diagram markdown to convert.</param>
        /// <returns>SVG diagram.</returns>
        public async Task<GetDiagramResponseModel?> GetDiagram(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
            {
                throw new ArgumentNullException(nameof(markdown));
            }

            using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync()
                .ConfigureAwait(false))
            await using (var browser = await playwright.Chromium.LaunchAsync(new()
                         {
                             Headless = true,
                             Channel = "chrome"
                         }))
            {
                var page = await browser.NewPageAsync()
                    .ConfigureAwait(false);

                await page.RouteAsync(
                        "https://localhost/index.html",
                        route => MermaidPostHandler(route, markdown))
                    .ConfigureAwait(false);

                await page.RouteAsync(
                        "**/*.{mjs,js}",
                        route => DefaultHandler(route))
                    .ConfigureAwait(false);

                var pageResponse = await page.GotoAsync("https://localhost/index.html", new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle })
                    .ConfigureAwait(false);

                if (pageResponse == null)
                {
                    return null;
                }

                if (!pageResponse.Ok)
                {
                    return null;
                }

                _ = await pageResponse.FinishedAsync().ConfigureAwait(false);

                await page.WaitForLoadStateAsync(LoadState.NetworkIdle).ConfigureAwait(false);

                var mermaidElement = page.Locator("#mermaid-element");

                if (mermaidElement == null)
                {
                    return null;
                }

                var innerText = await mermaidElement.InnerHTMLAsync();
                var png = await mermaidElement.ScreenshotAsync(new LocatorScreenshotOptions { Type = ScreenshotType.Png })
                    .ConfigureAwait(false);

                return new(innerText, png);
            }
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route, string markdown)
        {
            var httpRequestMessage = new HttpRequestMessage();

            var request = route.Request;

            httpRequestMessage.RequestUri = new Uri(request.Url);
            httpRequestMessage.Method = HttpMethod.Post;
            httpRequestMessage.Content = new FormUrlEncodedContent(new KeyValuePair<string, string>[]
            {
                new("diagram", markdown)
            });

            return httpRequestMessage;
        }

        private static HttpRequestMessage GetRequestFromRoute(IRoute route)
        {
            var request = route.Request;

            var httpRequestMessage = new HttpRequestMessage
            {
                RequestUri = new Uri(request.Url)
            };

            httpRequestMessage.PopulateHeaders(request.Headers);

            var httpMethod = GetHttpMethod(request.Method);

            if (httpMethod == HttpMethod.Patch || httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put)
            {
                AddContent(httpRequestMessage, request);
            }

            return httpRequestMessage;
        }

        private static HttpMethod GetHttpMethod(string requestMethod)
        {
            return requestMethod switch
            {
                "DELETE" => HttpMethod.Delete,
                "GET" => HttpMethod.Get,
                "HEAD" => HttpMethod.Head,
                "OPTIONS" => HttpMethod.Options,
                "PATCH" => HttpMethod.Patch,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "TRACE" => HttpMethod.Trace,
                _ => throw new ArgumentException(
                                        "Failed to map request HTTP method",
                                        nameof(requestMethod)),
            };
        }

        private static void AddContent(HttpRequestMessage httpRequestMessage, IRequest request)
        {
            if (request.PostDataBuffer != null)
            {
                httpRequestMessage.Content = new StreamContent(new MemoryStream(request.PostDataBuffer));
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

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);
            }
        }

        private async Task DefaultHandler(IRoute route)
        {
            if (!route.Request.Url.StartsWith("https://localhost/", StringComparison.OrdinalIgnoreCase))
            {
                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = 404
                };

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);

                return;
            }

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

                await route.FulfillAsync(routeFulfillOptions)
                    .ConfigureAwait(false);
            }
        }
    }
}
