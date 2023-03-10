using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;

namespace Dhgms.DocFx.MermaidJs.Plugin.HttpServer
{
    public sealed class MermaidHttpClientWrapper : IDisposable
    {
        private readonly TestServer _testServer;
        private bool _disposedValue;

        private MermaidHttpClientWrapper(TestServer testServer)
        {
            _testServer = testServer;
        }

        public static MermaidHttpClientWrapper CreateInstance(ILoggerFactory loggerFactory)
        {
            var testServer = MermaidHttpServerFactory.GetTestServer(loggerFactory);
            return new MermaidHttpClientWrapper(testServer);
        }

        public async Task<string> GetSvg(string diagram, ILoggerFactory loggerFactory)
        {
            using (var client = _testServer.CreateClient())
            {
                var httpResponse = await client.GetAsync("https://localhost/mermaidsvg")
                    .ConfigureAwait(false);

                httpResponse.EnsureSuccessStatusCode();

                return await httpResponse.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
            }
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _testServer.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                _disposedValue = true;
            }
        }
    }
}
