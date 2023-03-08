using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Javascript
{
    public sealed class PlaywrightRenderer
    {
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

        private void Handler(IRoute obj)
        {
            // TODO: kestrel hook.
            obj.FulfillAsync(new RouteFulfillOptions { Body = "<html></html>", Status = 200 }).Wait();
        }
    }
}
