// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using Microsoft.ClearScript;
using Microsoft.ClearScript.JavaScript;
using Microsoft.ClearScript.V8;
using Microsoft.Extensions.Logging;
using static Microsoft.ClearScript.V8.V8CpuProfile;

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

                var documentSettings = v8.DocumentSettings;

                documentSettings.SearchPath = "C:\\GitHub\\dpvreony\\docfx-mermaidjs\\src\\Dhgms.DocFx.MermaidJs.Plugin\\node_modules\\mermaid\\dist\\";
                documentSettings.AccessFlags = DocumentAccessFlags.EnableFileLoading;

                // see https://mermaid.js.org/config/usage.html#api-usage
                var sb = new System.Text.StringBuilder(495);

                // next step is https://github.com/microsoft/ClearScript/issues/143#issuecomment-557729084 to preload the module.
                _ = sb.AppendLine(@"import mermaid from 'mermaid.esm.mjs';");
                _ = sb.AppendLine(@"mermaid.initialize({ startOnLoad: false });");
                _ = sb.AppendLine(@"const { svg } = await mermaid.render('graphDiv', 'A --> B');");

                v8.Execute(new DocumentInfo() { Category = ModuleCategory.Standard }, sb.ToString());
            }
        }
    }
}
