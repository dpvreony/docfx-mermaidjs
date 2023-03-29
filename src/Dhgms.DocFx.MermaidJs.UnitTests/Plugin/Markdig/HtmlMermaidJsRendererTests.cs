// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Dhgms.DocFx.MermaidJs.Plugin.Markdig;
using Dhgms.DocFx.MermaidJs.Plugin.Playwright;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
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
                throw new System.NotImplementedException();
            }

            public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<MarkdownContext, PlaywrightRenderer>
            {

            }
        }
    }
}
