// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dhgms.DocFx.MermaidJs.Plugin.Playwright;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
using Nito.AsyncEx.Synchronous;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// HTML renderer for MermaidJS Code Blocks.
    /// </summary>
    public sealed class HtmlMermaidJsRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private readonly MarkdownContext _markdownContext;
        private readonly PlaywrightRenderer _playwrightRenderer;
        private HashSet<string>? _blocksAsDiv;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="markdownContext">DocFX Markdown context.</param>
        /// <param name="playwrightRenderer">Playwright Renderer used to generate mermaid.</param>
        public HtmlMermaidJsRenderer(MarkdownContext markdownContext, PlaywrightRenderer playwrightRenderer)
        {
            ArgumentNullException.ThrowIfNull(markdownContext);
            ArgumentNullException.ThrowIfNull(playwrightRenderer);

            _markdownContext = markdownContext;
            _playwrightRenderer = playwrightRenderer;
        }

        /// <summary>
        /// Gets or sets a value indicating whether to output attributes on a HTML PRE block.
        /// </summary>
        public bool OutputAttributesOnPre { get; set; }

        /// <summary>
        /// Gets a map of fenced code block infos that should be rendered as div blocks instead of pre/code blocks.
        /// </summary>
        public HashSet<string> BlocksAsDiv => _blocksAsDiv ??= new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        /// <inheritdoc/>
        protected override void Write(HtmlRenderer renderer, CodeBlock obj)
        {
            ArgumentNullException.ThrowIfNull(renderer);
            _ = renderer.EnsureLine();

            /*
            var diagram = "graph TD;" + Environment.NewLine +
                          "    A-->B;" + Environment.NewLine +
                          "    A-->C;" + Environment.NewLine +
                          "    B-->D;" + Environment.NewLine +
                          "    C-->D;";
            */

            if (obj?.Lines == null)
            {
                return;
            }

            var mermaidMarkup = obj.Lines.ToSlice().Text;
            var responseModel = _playwrightRenderer.GetDiagram(mermaidMarkup).WaitAndUnwrapException();

            if (responseModel == null)
            {
                return;
            }

            var imageBase64 = Convert.ToBase64String(responseModel.Png);

            var properties = new List<KeyValuePair<string, string?>>
            {
                new("alt", "Mermaid Diagram"),
                new("src", $"data:image/png;base64,{imageBase64}")
            };

            var attributes = new HtmlAttributes
            {
                Properties = properties
            };

            _ = renderer.Write("<img")
                .WriteAttributes(attributes)
                .Write('>');

            _ = renderer.EnsureLine();
        }
    }
}
