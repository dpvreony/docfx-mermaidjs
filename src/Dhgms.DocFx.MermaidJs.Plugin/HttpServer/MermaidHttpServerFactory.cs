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
    public static class MermaidHttpServerFactory
    {
        public static TestServer GetTestServer(ILoggerFactory loggerFactory)
        {
            var builder = GetWebHostBuilder(loggerFactory);
            var testServer = new TestServer(builder);
            return testServer;
        }

        private static IWebHostBuilder GetWebHostBuilder(ILoggerFactory loggerFactory)
        {
            var builder = new WebHostBuilder()
                .ConfigureLogging(loggingBuilder => ConfigureLogging(
                    loggingBuilder,
                    loggerFactory))
                .ConfigureServices((webHostBuilderContext, serviceCollection) => ConfigureServices(
                    webHostBuilderContext,
                    serviceCollection))
                .Configure(ConfigureApp);
            return builder;
        }

        private static void ConfigureApp(WebHostBuilderContext arg1, IApplicationBuilder arg2)
        {
            // TODO: add integration test
            // TODO: npm install
            // TODO: pre-build task to embed mermaidjs files
            // TODO: serve embedded resources
            // TODO: routes for mermaid resources
            // TODO: index page
        }

        private static void ConfigureServices(WebHostBuilderContext webHostBuilderContext, IServiceCollection serviceCollection)
        {
            var embeddedProvider = new EmbeddedFileProvider(
                typeof(MermaidHttpServerFactory).Assembly,
                typeof(MermaidHttpServerFactory).Namespace + ".wwwroot");
            serviceCollection.AddSingleton<IFileProvider>(embeddedProvider);
        }

        private static void ConfigureLogging(ILoggingBuilder loggingBuilder, ILoggerFactory loggerFactory)
        {
            loggingBuilder.Services.AddSingleton(loggerFactory);
        }
    }
}
