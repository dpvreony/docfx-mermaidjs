using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    public static class MarkdownPipelineBuilderExtensions
    {
        public static MarkdownPipelineBuilder UseDocfxExtensions(
            this MarkdownPipelineBuilder pipeline,
            MarkdownContext context)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            pipeline.Extensions.AddIfNotAlready(new MermaidJsExtension(context));
            return pipeline;
        }
    }
}
