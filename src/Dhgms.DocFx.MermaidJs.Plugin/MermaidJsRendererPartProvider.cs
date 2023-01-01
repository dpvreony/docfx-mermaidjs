// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Composition;
using Microsoft.DocAsCode.Dfm;

namespace Dhgms.DocFx.MermaidJs.Plugin
{
    [Export(typeof(IDfmCustomizedRendererPartProvider))]
    public sealed class MermaidJsRendererPartProvider : IDfmCustomizedRendererPartProvider
    {
        public IEnumerable<IDfmCustomizedRendererPart> CreateParts(IReadOnlyDictionary<string, object> parameters)
        {
            var settings = new MermaidJsRendererSettings(parameters);
            yield return new MermaidJsRendererPart(settings);
        }
    }
}
