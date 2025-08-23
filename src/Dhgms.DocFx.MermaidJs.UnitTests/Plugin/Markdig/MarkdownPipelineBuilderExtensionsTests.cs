// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Microsoft.Extensions.Logging;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;
using Xunit;

namespace Dhgms.DocFx.MermaidJs.UnitTests.Plugin.Markdig
{
    /// <summary>
    /// Unit Tests for <see cref="MarkdownPipelineBuilderExtensions"/>.
    /// </summary>
    public static class MarkdownPipelineBuilderExtensionsTests
    {
        /// <summary>
        /// Unit Test for <see cref="MarkdownPipelineBuilderExtensions.UseMermaidJsExtension(global::Markdig.MarkdownPipelineBuilder, Whipstaff.Mermaid.Playwright.PlaywrightRendererBrowserInstance, ILoggerFactory)"/>.
        /// </summary>
        public sealed class UseMermaidJsExtensionMethod
        {
            /// <summary>
            /// Tests that the method returns an instance of <see cref="global::Markdig.MarkdownPipelineBuilder"/>.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsInstanceAsync()
            {
                using (var loggerFactory = new Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory())
                {
                    var markdownPipelineBuilder = new global::Markdig.MarkdownPipelineBuilder();
                    var playwrightRenderer = PlaywrightRenderer.Default(loggerFactory);
                    var instance = markdownPipelineBuilder.UseMermaidJsExtension(
                        await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome()),
                        loggerFactory);

                    Assert.NotNull(instance);
                    Assert.Same(markdownPipelineBuilder, instance);
                }
            }
        }
    }
}
