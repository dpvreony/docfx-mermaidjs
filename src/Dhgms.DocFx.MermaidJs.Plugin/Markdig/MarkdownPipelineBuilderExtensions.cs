// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Markdig;
using Microsoft.DocAsCode.MarkdigEngine.Extensions;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Extension methods for <see cref="MarkdownPipelineBuilder"/>.
    /// </summary>
    public static class MarkdownPipelineBuilderExtensions
    {
        /// <summary>
        /// Adds the MermaidJs plugin to the pipeline.
        /// </summary>
        /// <param name="pipeline">Markdown Pipeline Builder to modify.</param>
        /// <param name="context">DocFX Markdown Context.</param>
        /// <returns>Modified Pipeline Builder.</returns>
        public static MarkdownPipelineBuilder UseMermaidJsExtension(
            this MarkdownPipelineBuilder pipeline,
            MarkdownContext context)
        {
            ArgumentNullException.ThrowIfNull(pipeline);
            pipeline.Extensions.AddIfNotAlready(new MermaidJsExtension(context));
            return pipeline;
        }
    }
}
