// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Dhgms.DocFx.MermaidJs.Sample.Cmd;
using Xunit;

namespace Dhgms.DocFx.MermaidJs.IntegrationTests.Sample.Cmd
{
    /// <summary>
    /// Integration tests for the <see cref="Program"/> class in the sample command line application.
    /// </summary>
    public static class ProgramTests
    {
        /// <summary>
        /// Integration tests for the Main method of the <see cref="Program"/> class.
        /// </summary>
        public sealed class MainMethod
        {
            /// <summary>
            /// Tests that the Main method runs without throwing an exception.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task RunsWithoutExceptionButReturnsNonZeroAsync()
            {
                // Arrange & Act
                var exitCode = await Program.Main();

                // Assert
                Assert.Equal(1, exitCode);
            }
        }
    }
}
