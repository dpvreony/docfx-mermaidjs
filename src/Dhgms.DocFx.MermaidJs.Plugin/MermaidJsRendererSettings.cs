// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.DocAsCode.Common;

namespace Dhgms.DocFx.MermaidJs.Plugin
{
    public sealed class MermaidJsRendererSettings
    {
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

        public string OutputFormat { get; }

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
