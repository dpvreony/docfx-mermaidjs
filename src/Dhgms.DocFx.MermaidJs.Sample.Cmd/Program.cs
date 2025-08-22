// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Microsoft.DocAsCode;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.Markdig.Settings;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Dhgms.DocFx.MermaidJs.Sample.Cmd
{
    /// <summary>
    /// Holds the program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <returns>Zero for success, non-zero for failure.</returns>
        public static async Task<int> Main()
        {
            try
            {
                using (var loggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory())
                {
                    var playwrightRenderer = PlaywrightRenderer.Default(loggerFactory);
                    var browserSession = await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome())
                        .ConfigureAwait(false);

                    var markdownJsExtensionSettings = new MarkdownJsExtensionSettings(
                        browserSession,
                        OutputMode.Svg);

                    var options = new BuildOptions
                    {
                        // Enable MermaidJS markdown extension
                        ConfigureMarkdig = pipeline => pipeline.UseMermaidJsExtension(
                                markdownJsExtensionSettings,
                                loggerFactory)
                    };
                    await Docset.Build("docfx.json", options);
                }
            }
#pragma warning disable CA1031
            catch (System.Exception ex)
            {
#pragma warning restore CA1031
                // Handle exceptions here
                System.Console.WriteLine($"An error occurred: {ex.Message}");
                return 1;
            }

            return 0;
        }
    }
}
