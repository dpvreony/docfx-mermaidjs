using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace Dhgms.DocFx.MermaidJs.Plugin.HttpServer
{
    public class MermaidHttpServer
    {
        public async Task<string> Run(ILoggerFactory loggerFactory)
        {
            var testServer = GetTestServer(loggerFactory);
            using (var client = testServer.CreateClient())
            {
                var httpResponse = await client.GetAsync("https://localhost/mermaidsvg")
                    .ConfigureAwait(false);

                httpResponse.EnsureSuccessStatusCode();

                return await httpResponse.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
            }
        }

        private TestServer GetTestServer(ILoggerFactory loggerFactory)
        {
            var builder = new WebHostBuilder()
                .ConfigureLogging(loggingBuilder => ConfigureLogging(loggingBuilder, loggerFactory))
                .ConfigureServices((webHostBuilderContext, serviceCollection) => ConfigureServices(webHostBuilderContext, serviceCollection))
                .Configure(ConfigureApp);
            var testServer = new TestServer(builder);
            return testServer;
        }

        private void ConfigureApp(WebHostBuilderContext arg1, IApplicationBuilder arg2)
        {
            // TODO: add integration test
            // TODO: npm install
            // TODO: pre-build task to embed mermaidjs files
            // TODO: serve embedded resources
            // TODO: routes for mermaid resources
            // TODO: index page
        }

        private void ConfigureServices(WebHostBuilderContext webHostBuilderContext, IServiceCollection serviceCollection)
        {
            var embeddedProvider = new EmbeddedFileProvider(
                typeof(MermaidHttpServer).Assembly,
                typeof(MermaidHttpServer).Namespace + ".wwwroot");
            serviceCollection.AddSingleton<IFileProvider>(embeddedProvider);
        }

        private void ConfigureLogging(ILoggingBuilder loggingBuilder, ILoggerFactory loggerFactory)
        {
            loggingBuilder.Services.AddSingleton(loggerFactory);
        }
    }
}
