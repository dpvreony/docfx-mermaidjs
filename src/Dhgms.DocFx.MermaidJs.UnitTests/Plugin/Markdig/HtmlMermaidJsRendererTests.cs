﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Dhgms.DocFx.MermaidJs.Plugin.Settings;
using Docfx.MarkdigEngine.Extensions;
using Markdig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetTestRegimentation;
using Nito.AsyncEx.Synchronous;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.DocFx.MermaidJs.UnitTests.Plugin.Markdig
{
    /// <summary>
    /// Unit Tests for <see cref="HtmlMermaidJsRendererTests"/>.
    /// </summary>
    public static class HtmlMermaidJsRendererTests
    {
        /// <summary>
        /// Unit tests for the <see cref="HtmlMermaidJsRenderer.CreateAsync"/> method.
        /// </summary>
        public sealed class CreateAsyncMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<MarkdownContext, PlaywrightRenderer, MarkdownJsExtensionSettings>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CreateAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CreateAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure that the <see cref="HtmlMermaidJsRenderer"/> is created successfully.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsInstance()
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(Log);
                var logMessageActions = new PlaywrightRendererLogMessageActions();
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    logMessageActions,
                    Log.CreateLogger<PlaywrightRenderer>());
                var playwrightRenderer = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);

                var settings = new MarkdownJsExtensionSettings(OutputMode.Png);

                var instance = await HtmlMermaidJsRenderer.CreateAsync(new MarkdownContext(), playwrightRenderer, settings);

                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(MarkdownContext arg1, PlaywrightRenderer arg2, MarkdownJsExtensionSettings arg3, string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => HtmlMermaidJsRenderer.CreateAsync(arg1, arg2, arg3));
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<MarkdownContext?, PlaywrightRenderer?, MarkdownJsExtensionSettings?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(new NullLoggerFactory());
                    var logMessageActions = new PlaywrightRendererLogMessageActions();
                    var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                        logMessageActions,
                        new NullLogger<PlaywrightRenderer>());

                    Add(null, new PlaywrightRenderer(mermaidHttpServer, logMessageActionsWrapper), new MarkdownJsExtensionSettings(OutputMode.Png), "markdownContext");
                    Add(new MarkdownContext(), null, new MarkdownJsExtensionSettings(OutputMode.Png), "playwrightRenderer");
#pragma warning restore CA2000 // Dispose objects before losing scope
                    Add(new MarkdownContext(), new PlaywrightRenderer(mermaidHttpServer, logMessageActionsWrapper), null, "settings");
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="HtmlMermaidJsRenderer.Write"/>.
        /// </summary>
        public sealed class WriteMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WriteMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public WriteMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure markdown is formatted to mermaid.
            /// </summary>
            [Fact]
            public void WritesMarkdown()
            {
                var markdown = "```mermaid" + Environment.NewLine +
                               "graph TD;" + Environment.NewLine +
                               "    A-->B;" + Environment.NewLine +
                               "    A-->C;" + Environment.NewLine +
                               "    B-->D;" + Environment.NewLine +
                               "    C-->D;" + Environment.NewLine +
                               "```" + Environment.NewLine +
                               Environment.NewLine +
                               "```csharp" + Environment.NewLine +
                               "   int i = 1;" + Environment.NewLine +
                               "```";

                var context = new MarkdownContext();
                var pipelineBuilder = new MarkdownPipelineBuilder()
                    .UseMermaidJsExtension(context);

                var pipeline = pipelineBuilder.Build();
                var actualHtml = Markdown.ToHtml(markdown, pipeline);

                _logger.LogInformation(actualHtml);
            }
        }
    }
}
