// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Dhgms.DocFx.MermaidJs.Plugin.HttpServer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.DocFx.MermaidJs.IntegrationTests.HttpServer
{
    /// <summary>
    /// Integration tests for <see cref="MermaidHttpServerFactory"/>.
    /// </summary>
    public sealed class MermaidHttpServerFactoryTests : Foundatio.Xunit.TestWithLoggingBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidHttpServerFactoryTests"/> class.
        /// </summary>
        /// <param name="output">XUnit logging output.</param>
        public MermaidHttpServerFactoryTests(ITestOutputHelper output)
            : base(output)
        {
            Log.MinimumLevel = LogLevel.Trace;
        }

        /// <summary>
        /// Test to load the default mermaid html page.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task Test()
        {
            var testServer = MermaidHttpServerFactory.GetTestServer(Log);
            var httpClient = testServer.CreateClient();
#pragma warning disable CA2234 // Pass system uri objects instead of strings
            var response = await httpClient.GetAsync("index.html")
                .ConfigureAwait(false);
#pragma warning restore CA2234 // Pass system uri objects instead of strings
            _ = response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Test to check what embedded files are registered.
        /// </summary>
        [Fact]
        public void ListEmbeddedResources()
        {
            var testServer = MermaidHttpServerFactory.GetTestServer(Log);
            var fileProvider = testServer.Services.GetService<IFileProvider>();

            Assert.NotNull(fileProvider);

            var directoryContents = fileProvider.GetDirectoryContents(string.Empty);
            foreach (var directoryContent in directoryContents)
            {
                _logger.LogInformation(directoryContent.Name);
            }
        }
    }
}
