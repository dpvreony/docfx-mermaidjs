// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using Microsoft.DocAsCode.Common;
using Microsoft.DocAsCode.Dfm;
using Microsoft.DocAsCode.MarkdownLite;

namespace Dhgms.DocFx.MermaidJs.Plugin
{
    /// <summary>
    /// Carries out the rendering of MermaidJS code blocks in the markdown files.
    /// This uses external nodejs and the mermaid-cli under the hood so has a dependency on npm package restore as it's an MVP approach.
    /// It can be improved in future to embed the cli and self host it inside puppeteer etc.
    /// Need to look at how we can support incremental builds. currently png files go missing if they are not rebuilt.
    /// </summary>
    public sealed class MermaidJsRendererPart : DfmCustomizedRendererPartBase<IMarkdownRenderer, MarkdownCodeBlockToken, MarkdownBlockContext>
    {
        private readonly MermaidJsRendererSettings _settings;

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsRendererPart"/> class.
        /// </summary>
        /// <param name="settings">Settings for the MermaidJs Rendering.</param>
        public MermaidJsRendererPart(MermaidJsRendererSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc/>
        public override string Name => "MermaidJsRendererPart";

        /// <inheritdoc/>
        public override bool Match(IMarkdownRenderer renderer, MarkdownCodeBlockToken token, MarkdownBlockContext context)
        {
            return token.Lang == "mermaid";
        }

        /// <inheritdoc/>
        public override StringBuffer Render(IMarkdownRenderer renderer, MarkdownCodeBlockToken token, MarkdownBlockContext context)
        {
            if (_settings.InlineDiagrams)
            {
                return RenderInlineDiagram(
                    renderer,
                    token,
                    context);
            }

            return RenderExternalFile(
                renderer,
                token,
                context);
        }

        private StringBuffer RenderExternalFile(IMarkdownRenderer renderer, MarkdownCodeBlockToken token, MarkdownBlockContext context)
        {
            var sourceInfo = token.SourceInfo;

            var inputFilename = Path.GetFileNameWithoutExtension(sourceInfo.File);

            var targetFileName = $"{inputFilename}-{sourceInfo.LineNumber}-mermaidjs.png";

            GenerateMermaidFile(
                renderer,
                token,
                targetFileName,
                context);

            // TODO: support ALT text
            return $"<img alt=\"Mermaid Diagram\" src=\"{targetFileName}\">";
        }

        private StringBuffer RenderInlineDiagram(IMarkdownRenderer renderer, MarkdownCodeBlockToken token, MarkdownBlockContext context)
        {
            var fileExtension = ".png";

            var tempFileName = Path.Combine(
                Path.GetTempPath(),
                $"{Path.GetRandomFileName()}{fileExtension}");

            GenerateMermaidFile(
                renderer,
                token,
                tempFileName,
                context);

            var fileBytes = File.ReadAllBytes(tempFileName);
            var imageBase64 = Convert.ToBase64String(fileBytes);

            File.Delete(tempFileName);

            // TODO: support ALT text
            var tag = $"<img alt=\"Mermaid Diagram\" src=\"data:image/png;base64,{imageBase64}\">";
            Logger.LogDiagnostic(tag);
            return tag;
        }

        private void GenerateMermaidFile(
            IMarkdownRenderer renderer,
            MarkdownCodeBlockToken markdownCodeBlockToken,
            string targetFileName,
            MarkdownBlockContext markdownBlockContext)
        {
            // TODO: already de-ref'd this in calling method.
            var sourceInfo = markdownCodeBlockToken.SourceInfo;

            var rootPath = markdownBlockContext.GetBaseFolder();

            var byteArray = Encoding.UTF8.GetBytes(markdownCodeBlockToken.Code);

            var command = Path.Combine(
                rootPath,
                "node_modules\\.bin\\mmdc.cmd");

            if (!File.Exists(command))
            {
                Logger.LogError("markdown requires NPM and mermaid-cli", file: sourceInfo.File, line: sourceInfo.LineNumber.ToString());
                return;
            }

            var outputPath = Path.GetDirectoryName(sourceInfo.File);

            // this is taking the input folder, the output folder
            var outputFilename = Path.Combine(
                rootPath,
                "_mermaidjs",
                outputPath,
                targetFileName);

            // and reversing the path separator on the output
            outputFilename = Path.GetFullPath(outputFilename);

            var targetDir = Path.GetDirectoryName(outputFilename);

            if (!Directory.Exists(targetDir))
            {
                _ = Directory.CreateDirectory(targetDir);
            }

            TemporaryFileHelpers.WithTempFile(
                byteArray,
                ".mmd",
                fileName => RunProcessWithTempFile(fileName, command, outputFilename, rootPath),
                true);

            if (_settings.InlineDiagrams)
            {
                // TODO: inline the diagram
                // TODO: delete the output file
            }
        }

        private void RunProcessWithTempFile(string inputFileName, string command, string outputFileName, string rootPath)
        {
            var args = $"-o \"{outputFileName}\" -i \"{inputFileName}\"";

            Logger.LogInfo($"Input Filename: {inputFileName}");
            Logger.LogInfo($"Output Filename: {outputFileName}");
            Logger.LogInfo($"Command: {command}");
            Logger.LogInfo($"Args: {args}");

            var startInfo = new ProcessStartInfo(command, args)
            {
                WorkingDirectory = rootPath,
                UseShellExecute = false,
            };

            var process = new Process
            {
                StartInfo = startInfo,
            };

            if (!process.Start())
            {
                Logger.LogError($"mermaid-cli failed to start");
                return;
            }

            process.WaitForExit();

            var exitCode = process.ExitCode;
            if (exitCode != 0)
            {
                Logger.LogError($"mermaid-cli process exit code is: {exitCode}");
            }
        }
    }
}
