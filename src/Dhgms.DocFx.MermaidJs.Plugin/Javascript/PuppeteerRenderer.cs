// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Net;
using System.Threading.Tasks;
using PuppeteerSharp;
using ReactiveMarbles.ObservableEvents;

namespace Dhgms.DocFx.MermaidJs.Plugin.Javascript
{
    /// <summary>
    /// Test renderer using Puppeteer Sharp.
    /// </summary>
    public sealed class PuppeteerRenderer
    {
        public async Task Render()
        {
            using (var browserFetcher = new BrowserFetcher())
            {
                var downloadResult = await browserFetcher.DownloadAsync();
            }

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true }))
            {
                await using (var page = await browser.NewPageAsync())
                {
                    // TODO: add the event source generation.
                    _ = page.SetRequestInterceptionAsync(true);
                    page.Events().Request.Subscribe(x => OnRequest(x));

                    _ = page.SetContentAsync("<html></html>");
                }
            }
        }

        private void OnRequest(RequestEventArgs e)
        {
            var request = e.Request;

            // TODO: hook into kestrel test server.
            var response = new ResponseData { Body = "<html></html>", Status = HttpStatusCode.OK };
            request.RespondAsync(response).Wait();
        }
    }
}
