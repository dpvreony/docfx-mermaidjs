// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Dhgms.DocFx.MermaidJs.Plugin.Javascript;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.DocFx.MermaidJs.UnitTests.Plugin.Javascript
{
    /// <summary>
    /// Unit Tests for <see cref="JavascriptHelper"/>.
    /// </summary>
    public static class JavascriptHelperTests
    {
        /// <summary>
        /// Unit Tests for <see cref="JavascriptHelper"/> constructor.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase, NetTestRegimentation.ITestConstructorMethodWithNullableParameters<ILogger<JavascriptHelper>>
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output Helper.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = new JavascriptHelper(Log.CreateLogger<JavascriptHelper>());
                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                ILogger<JavascriptHelper> arg,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new JavascriptHelper(arg));
                Assert.Equal(exception.ParamName, expectedParameterNameForException);
            }
        }

        /// <summary>
        /// Unit Tests for getting mermaid data.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class GetMermaidJsResultMethod : Foundatio.Xunit.TestWithLoggingBase
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMermaidJsResultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output Helper.</param>
            public GetMermaidJsResultMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to check we can get data out.
            /// </summary>
            [Fact]
            public void GetsData()
            {
                var instance = new JavascriptHelper(Log.CreateLogger<JavascriptHelper>());
                instance.GetMermaidJsResult();
            }
        }

        /// <summary>
        /// Test Source for Argument Null Exception.
        /// </summary>
#pragma warning disable CA1034 // Nested types should not be visible
        public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<ILogger<JavascriptHelper>?, string>
#pragma warning restore CA1034 // Nested types should not be visible
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
            /// </summary>
            public ThrowsArgumentNullExceptionTestSource()
            {
                Add(null, "logger");
            }
        }
    }
}
