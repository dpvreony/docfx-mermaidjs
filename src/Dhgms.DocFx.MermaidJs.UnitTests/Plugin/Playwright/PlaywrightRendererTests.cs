// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.Playwright;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.DocFx.MermaidJs.UnitTests.Plugin.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="PlaywrightRenderer"/>.
    /// </summary>
    public static class PlaywrightRendererTests
    {
        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<ILoggerFactory>
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
                var instance = new PlaywrightRenderer(Log);
                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                ILoggerFactory arg,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new PlaywrightRenderer(arg));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<ILoggerFactory?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
                    Add(null, "loggerFactory");
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRenderer.GetSvg"/>.
        /// </summary>
        public sealed class GetSvgMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetSvgMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetSvgMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(string arg, string expectedParameterNameForException)
            {
                var instance = new PlaywrightRenderer(Log);
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => instance.GetSvg(arg))
                    .ConfigureAwait(false);

                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure the SVG generator returns specific results.
            /// </summary>
            /// <param name="diagram">Mermaid diagram to parse.</param>
            /// <param name="expectedStart">The expected result.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [ClassData(typeof(ReturnsResultTestSource))]
            public async Task ReturnsResult(string diagram, string expectedStart)
            {
                var instance = new PlaywrightRenderer(Log);
                var svg = await instance.GetSvg(diagram).ConfigureAwait(false);

                _logger.LogInformation(svg);

                Assert.NotNull(svg);
                Assert.StartsWith(expectedStart, svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRenderer.GetSvg"/>.
            /// </summary>
            public sealed class ReturnsResultTestSource : TheoryData<string, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ReturnsResultTestSource"/> class.
                /// </summary>
                public ReturnsResultTestSource()
                {
                    var graph = "graph TD;" + Environment.NewLine +
                        "    A-->B;" + Environment.NewLine +
                        "    A-->C;" + Environment.NewLine +
                        "    B-->D;" + Environment.NewLine +
                        "    C-->D;";

                    Add(graph, "<svg aria-roledescription=\"flowchart-v2\" role=\"graphics-document document\"");
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : TheoryData<string?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                {
                    Add(null, "diagram");
                }
            }
        }
    }
}
