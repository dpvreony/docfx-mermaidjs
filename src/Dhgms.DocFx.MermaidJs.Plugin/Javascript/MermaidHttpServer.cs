using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Dhgms.DocFx.MermaidJs.Plugin.Javascript
{
    public class MermaidHttpServer
    {
        public async Task<string> Run()
        {
            using (var webApplicationFactory = new WebApplicationFactory<MermaidHttpStartup>())
            using (var client = webApplicationFactory.CreateClient())
            {
                var httpResponse = await client.GetAsync("https://localhost/mermaidsvg")
                    .ConfigureAwait(false);

                httpResponse.EnsureSuccessStatusCode();

                return await httpResponse.Content.ReadAsStringAsync()
                    .ConfigureAwait(false);
            }
        }
    }
}
