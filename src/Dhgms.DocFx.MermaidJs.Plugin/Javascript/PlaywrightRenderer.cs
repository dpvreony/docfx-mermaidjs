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
    public sealed class PlaywrightRenderer
    {
        private readonly TestServer _mermaidHttpServerFactory;

        public PlaywrightRenderer(MermaidHttpClientWrapper mermaidHttpClientWrapper, ILoggerFactory loggerFactory)
        {
            _mermaidHttpServerFactory = MermaidHttpServerFactory.GetTestServer(loggerFactory);
        }

        public async Task Render(string diagram)
        {
            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new() { Headless = false });
            var page = await browser.NewPageAsync();
            await page.RouteAsync("*", Handler);
            var apiRequest = page.APIRequest;
            var data = new Dictionary<string, object> { { "diagram", diagram } };

            var response = await apiRequest.PostAsync("/mermaid", new APIRequestContextOptions { DataObject = data });
        }

        private void Handler(IRoute route)
        {
            using (var client = _mermaidHttpServerFactory.CreateClient())
            {
                var request = GetRequestFromRoute(route);
                var response = client.Send(request);

                var routeFulfillOptions = new RouteFulfillOptions
                {
                    Status = (int)response.StatusCode,
                    // TODO: async handling
                    Body = response.Content.ReadAsStringAsync().Result,
                };

                if (response.Content.Headers.ContentType != null)
                {
                    routeFulfillOptions.ContentType = response.Content.Headers.ContentType.ToString();
                }

                route.FulfillAsync().Wait();
            }
        }

        private HttpRequestMessage GetRequestFromRoute(IRoute route)
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
    }
}
