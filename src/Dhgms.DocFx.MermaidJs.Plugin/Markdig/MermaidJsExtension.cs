// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Markdig;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Handles the registration of the MermaidJs plugin into Markdig.
    /// </summary>
    public sealed class MermaidJsExtension : IMarkdownExtension
    {
        private readonly MarkdownContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsExtension"/> class.
        /// </summary>
        /// <param name="context">Markdig context instance.</param>
        public MermaidJsExtension(MarkdownContext context)
        {
            _context = context;
        }

        /// <inheritdoc/>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            ArgumentNullException.ThrowIfNull(pipeline);

            // pipeline.BlockParsers.AddIfNotAlready<MermaidJsBlockParser>();
        }

        /// <inheritdoc/>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            ArgumentNullException.ThrowIfNull(renderer);

            if (renderer is HtmlRenderer htmlRenderer)
            {
                // there is a built in renderer for mermaidjs in markdig, but it uses JS to do rendering in the browser
                // we don't want that, we want the assets produced at build time
                // single point of processing and avoids issue with the DocFX pdf processor.
                var codeRenderer = htmlRenderer.ObjectRenderers.FindExact<CodeBlockRenderer>()!;
                _ = codeRenderer.BlocksAsDiv.Remove("mermaid");

                // Must be inserted before CodeBlockRenderer
                htmlRenderer.ObjectRenderers.Insert(0, new HtmlMermaidJsRenderer(_context));
            }
        }
    }
}
