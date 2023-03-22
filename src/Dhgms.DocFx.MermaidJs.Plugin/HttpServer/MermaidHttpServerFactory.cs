// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Dhgms.DocFx.MermaidJs.Plugin.HttpServer
{
    /// <summary>
    /// Factory for the Mermaid in memory HTTP server.
    /// </summary>
    public static class MermaidHttpServerFactory
    {
        /// <summary>
        /// Gets the In Memory Test Server.
        /// </summary>
        /// <param name="loggerFactory">Logging Factory.</param>
        /// <returns>In memory HTTP server instance.</returns>
        public static TestServer GetTestServer(ILoggerFactory loggerFactory)
        {
            var builder = GetWebHostBuilder(loggerFactory);
            var testServer = new TestServer(builder);
            return testServer;
        }

        private static IWebHostBuilder GetWebHostBuilder(ILoggerFactory loggerFactory)
        {
            var embeddedProvider = new EmbeddedFileProvider(
                typeof(MermaidHttpServerFactory).Assembly,
                typeof(MermaidHttpServerFactory).Namespace + ".wwwroot");

            var builder = new WebHostBuilder()
                .ConfigureLogging(loggingBuilder => ConfigureLogging(
                    loggingBuilder,
                    loggerFactory))
                .ConfigureServices((webHostBuilderContext, serviceCollection) => ConfigureServices(
                    webHostBuilderContext,
                    serviceCollection,
                    embeddedProvider))
                .Configure((webHostBuilderContext, applicationBuilder) => ConfigureApp(
                    webHostBuilderContext,
                    applicationBuilder,
                    embeddedProvider));

            return builder;
        }

        private static void ConfigureApp(WebHostBuilderContext webHostBuilderContext, IApplicationBuilder applicationBuilder, EmbeddedFileProvider embeddedFileProvider)
        {
            // TODO: add integration test
            // TODO: npm install
            // TODO: pre-build task to embed mermaidjs files
            // TODO: serve embedded resources
            // TODO: routes for mermaid resources
            // TODO: index page
            _ = applicationBuilder.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = embeddedFileProvider
            });
        }

        private static void ConfigureServices(WebHostBuilderContext webHostBuilderContext, IServiceCollection serviceCollection, EmbeddedFileProvider embeddedFileProvider)
        {
            _ = serviceCollection.AddSingleton<IFileProvider>(embeddedFileProvider);
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder, ILoggerFactory loggerFactory)
        {
            _ = loggingBuilder.Services.AddSingleton(loggerFactory);
        }
    }
}
