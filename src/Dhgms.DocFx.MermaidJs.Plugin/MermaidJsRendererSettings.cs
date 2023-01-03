// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.DocAsCode.Common;

namespace Dhgms.DocFx.MermaidJs.Plugin
{
    /// <summary>
    /// Configuration Settings for the MermaidJS integration.
    /// </summary>
    public sealed class MermaidJsRendererSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsRendererSettings"/> class.
        /// </summary>
        /// <param name="parameters">Collection of parameters from the DocFX process.</param>
        public MermaidJsRendererSettings(IReadOnlyDictionary<string, object> parameters)
        {
            Logger.LogInfo("checking keys", typeof(MermaidJsRendererPart).ToString());
            foreach (KeyValuePair<string, object> keyValuePair in parameters)
            {
                Logger.LogInfo(keyValuePair.Key);
            }

            OutputFormat = GetValueOrDefault(parameters, "mermaidJs.outputFormat", "png");
            InlineDiagrams = GetValueOrDefault(parameters, "mermaidJs.inlineDiagrams", true);
        }

        /// <summary>
        /// Gets the output format to use.
        /// </summary>
        public string OutputFormat { get; }

        /// <summary>
        /// Gets a value indicating whether to inline the diagrams.
        /// </summary>
        public bool InlineDiagrams { get; }

        private static T GetValueOrDefault<T>(IReadOnlyDictionary<string, object> collection, string key, T defaultValue)
        {
            try
            {
                if (collection.TryGetValue(key, out object value))
                {
                    return (T)value;
                }
            }
            catch (Exception e)
            {
                Logger.LogWarning($"Failed to read setting: \"{key}\" reason: \"{e.Message}\"");
            }

            return defaultValue;
        }
    }
}
