// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Extensions.Logging;

namespace Dhgms.DocFx.MermaidJs.Plugin.Javascript
{
    /// <summary>
    /// Helper for processing the MermaidJS in a javascript engine.
    /// </summary>
    public sealed class JavascriptHelper
    {
        private readonly ILogger<JavascriptHelper> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="JavascriptHelper"/> class.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public JavascriptHelper(ILogger<JavascriptHelper> logger)
        {
            ArgumentNullException.ThrowIfNull(logger);
            _logger = logger;
        }

        /// <summary>
        /// Gets the MermaidJS result.
        /// </summary>
        public void GetMermaidJsResult()
        {
            using (var v8 = new V8ScriptEngine())
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                _logger.LogDebug("Starting v8 process");
#pragma warning restore CA1848 // Use the LoggerMessage delegates

                v8.DocumentSettings.AccessFlags = DocumentAccessFlags.None;
                var code = File.ReadAllText(
                    "C:\\GitHub\\dpvreony\\docfx-mermaidjs\\src\\Dhgms.DocFx.MermaidJs.Plugin\\node_modules\\mermaid\\dist\\mermaid.js");

                v8.Execute(new DocumentInfo() { Category = ModuleCategory.CommonJS }, code);

                // see https://mermaid.js.org/config/usage.html#api-usage
                var sb = new System.Text.StringBuilder(495);
                _ = sb.AppendLine(@"<script type=""module"">");
                _ = sb.AppendLine(@"  import mermaid from './mermaid.mjs';");
                _ = sb.AppendLine(@"  mermaid.mermaidAPI.initialize({ startOnLoad: false });");
                _ = sb.AppendLine(@"  $(async function () {");
                _ = sb.AppendLine(@"    // Example of using the API var");
                _ = sb.AppendLine(@"    element = document.querySelector('#graphDiv');");
                _ = sb.AppendLine(@"    const insertSvg = function (svgCode, bindFunctions) {");
                _ = sb.AppendLine(@"      element.innerHTML = svgCode;");
                _ = sb.AppendLine(@"    };");
                _ = sb.AppendLine(@"    const graphDefinition = 'graph TB\na-->b';");
                _ = sb.AppendLine(@"    const graph = await mermaid.mermaidAPI.render('graphDiv', graphDefinition, insertSvg);");
                _ = sb.AppendLine(@"  });");
                _ = sb.AppendLine(@"</script>");

                var result = v8.ExecuteCommand(sb.ToString());
            }
        }
    }
}
