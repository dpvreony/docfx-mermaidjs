﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;
using Nito.AsyncEx.Synchronous;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// HTML renderer for MermaidJS Code Blocks.
    /// </summary>
    public sealed class HtmlMermaidJsRenderer : HtmlObjectRenderer<MermaidCodeBlock>
    {
        private readonly MarkdownContext _markdownContext;
        private readonly PlaywrightRenderer _playwrightRenderer;

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

        /// <inheritdoc/>
        protected override void Write(HtmlRenderer renderer, MermaidCodeBlock obj)
        {
            ArgumentNullException.ThrowIfNull(renderer);
            ArgumentNullException.ThrowIfNull(obj);
            _ = renderer.EnsureLine();

            /*
            var diagram = "graph TD;" + Environment.NewLine +
                          "    A-->B;" + Environment.NewLine +
                          "    A-->C;" + Environment.NewLine +
                          "    B-->D;" + Environment.NewLine +
                          "    C-->D;";
            */

            var mermaidMarkup = obj.Lines.ToSlice().Text;
            var responseModel = _playwrightRenderer.GetDiagram(
                mermaidMarkup,
                PlaywrightBrowserTypeAndChannel.ChromiumDefault()).WaitAndUnwrapException();

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
            return;
        }
    }
}
