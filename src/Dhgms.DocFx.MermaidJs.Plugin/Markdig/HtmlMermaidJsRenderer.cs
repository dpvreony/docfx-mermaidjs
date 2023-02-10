// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdig.Parsers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using Markdig.Syntax;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    public sealed class HtmlMermaidJsRenderer : HtmlObjectRenderer<CodeBlock>
    {
        private readonly MarkdownContext _markdownContext;
        private HashSet<string>? _blocksAsDiv;

        /// <summary>
        /// Initializes a new instance of the <see cref="HtmlMermaidJsRenderer"/> class.
        /// </summary>
        /// <param name="markdownContext"></param>
        public HtmlMermaidJsRenderer(MarkdownContext markdownContext)
        {
            _markdownContext = markdownContext;
        }

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

            // TODO: this is the bit we need to look at for "mermaid"
            if (_blocksAsDiv is not null && (obj as FencedCodeBlock)?.Info is string info &&
                _blocksAsDiv.Contains(info))
            {
                var infoPrefix = (obj.Parser as FencedCodeBlockParser)?.InfoPrefix ??
                                 FencedCodeBlockParser.DefaultInfoPrefix;

                // We are replacing the HTML attribute `language-mylang` by `mylang` only for a div block
                // NOTE that we are allocating a closure here
                if (renderer.EnableHtmlForBlock)
                {
                    _ = renderer.Write("<div")
                        .WriteAttributes(
                            obj.TryGetAttributes(),
                            cls => cls.StartsWith(infoPrefix, StringComparison.Ordinal)
                                ? cls.Substring(infoPrefix.Length)
                                : cls)
                        .Write('>');
                }

                _ = renderer.WriteLeafRawLines(obj, true, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    _ = renderer.WriteLine("</div>");
                }
            }
            else
            {
                if (renderer.EnableHtmlForBlock)
                {
                    _ = renderer.Write("<pre");

                    if (OutputAttributesOnPre)
                    {
                        _ = renderer.WriteAttributes(obj);
                    }

                    _ = renderer.Write("><code");

                    if (!OutputAttributesOnPre)
                    {
                        _ = renderer.WriteAttributes(obj);
                    }

                    _ = renderer.Write('>');
                }

                _ = renderer.WriteLeafRawLines(obj, true, true);

                if (renderer.EnableHtmlForBlock)
                {
                    _ = renderer.WriteLine("</code></pre>");
                }
            }

            _ = renderer.EnsureLine();
        }
    }
}
