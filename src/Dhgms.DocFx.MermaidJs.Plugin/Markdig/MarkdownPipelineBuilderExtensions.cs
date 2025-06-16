// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Markdig;
using Microsoft.Extensions.Logging;
using Whipstaff.Markdig.Settings;
using Whipstaff.Playwright;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Extension methods for <see cref="MarkdownPipelineBuilder"/>.
    /// </summary>
    /// <remarks>
    /// These now redirect to <see cref="Whipstaff.Markdig.Mermaid.MarkdownPipelineBuilderExtensions"/>, this library has been stripped back due to using the MarkDig Mermaid logic elsewhere, and with DocFX plugs taking on extra packages such as PlantUML, and not offering a core plug in package.</remarks>
    public static class MarkdownPipelineBuilderExtensions
    {
        /// <summary>
        /// Adds the MermaidJs plugin to the pipeline.
        /// </summary>
        /// <param name="pipeline">Markdown Pipeline Builder to modify.</param>
        /// <param name="playwrightBrowserTypeAndChannel">Browser and channel type to use.</param>
        /// <returns>Modified Pipeline Builder.</returns>
        public static MarkdownPipelineBuilder UseMermaidJsExtension(
            this MarkdownPipelineBuilder pipeline,
            PlaywrightBrowserTypeAndChannel playwrightBrowserTypeAndChannel) =>
            Whipstaff.Markdig.Mermaid.MarkdownPipelineBuilderExtensions.UseMermaidJsExtension(
                pipeline,
                playwrightBrowserTypeAndChannel);

        /// <summary>
        /// Adds the MermaidJs plugin to the pipeline.
        /// </summary>
        /// <param name="pipeline">Markdown Pipeline Builder to modify.</param>
        /// <param name="settings">Settings to use for the extension.</param>
        /// <param name="loggerFactory">Logger Factory instance to use.</param>
        /// <returns>Modified Pipeline Builder.</returns>
        public static MarkdownPipelineBuilder UseMermaidJsExtension(
            this MarkdownPipelineBuilder pipeline,
            MarkdownJsExtensionSettings settings,
            ILoggerFactory loggerFactory) =>
            Whipstaff.Markdig.Mermaid.MarkdownPipelineBuilderExtensions.UseMermaidJsExtension(
                pipeline,
                settings,
                loggerFactory);
    }
}
