// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Dhgms.DocFx.MermaidJs.Plugin.Playwright;
using Markdig;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Syntax;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetTestRegimentation;
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
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<MarkdownContext, PlaywrightRenderer>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new HtmlMermaidJsRenderer(new MarkdownContext(), new PlaywrightRenderer(Log));
                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(MarkdownContext arg1, PlaywrightRenderer arg2, string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new HtmlMermaidJsRenderer(arg1, arg2));

                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<MarkdownContext?, PlaywrightRenderer?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(null, new PlaywrightRenderer(new NullLoggerFactory()), "markdownContext");
#pragma warning restore CA2000 // Dispose objects before losing scope
                    Add(new MarkdownContext(), null, "playwrightRenderer");
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
