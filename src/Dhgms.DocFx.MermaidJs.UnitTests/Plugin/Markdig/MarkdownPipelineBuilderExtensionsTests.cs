// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Xunit;

namespace Dhgms.DocFx.MermaidJs.UnitTests.Plugin.Markdig
{
    /// <summary>
    /// Unit Tests for <see cref="MarkdownPipelineBuilderExtensions"/>.
    /// </summary>
    public static class MarkdownPipelineBuilderExtensionsTests
    {
        /// <summary>
        /// Unit Test for <see cref="MarkdownPipelineBuilderExtensions.UseMermaidJsExtension(global::Markdig.MarkdownPipelineBuilder, Whipstaff.Playwright.PlaywrightBrowserTypeAndChannel)"/>.
        /// </summary>
        public sealed class UseMermaidJsExtensionMethod
        {
            /// <summary>
            /// Tests that the method returns an instance of <see cref="global::Markdig.MarkdownPipelineBuilder"/>.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var markdownPipelineBuilder = new global::Markdig.MarkdownPipelineBuilder();
                var instance = markdownPipelineBuilder.UseMermaidJsExtension(Whipstaff.Playwright.PlaywrightBrowserTypeAndChannel.Chrome());

                Assert.NotNull(instance);
                Assert.Same(markdownPipelineBuilder, instance);
            }
        }
    }
}
