using Markdig;
using Microsoft.DocAsCode;

namespace Dhgms.DocFx.MermaidJs.Sample.Cmd
{
    public static class Program
    {
        public static async Task<int> Main()
        {
            try
            {
                var options = new BuildOptions
                {
                    // Enable MermaidJS markdown extension
                    ConfigureMarkdig = pipeline => pipeline.UseMermaidJs(),
                };

                await Docset.Build("docfx.json", options);
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                return 1;
            }

            return 0;
        }
    }
}
