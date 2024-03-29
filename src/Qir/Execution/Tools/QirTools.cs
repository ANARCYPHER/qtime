﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Quantum.Qir.Tools.Executable;
using Microsoft.Quantum.QsCompiler;
using Microsoft.Quantum.QsCompiler.QIR;

namespace Microsoft.Quantum.Qir.Tools
{
    /// <summary>
    /// Provides high-level utility methods to work with QIR.
    /// </summary>
    public static class QirTools
    {
        /// <summary>
        /// Creates QIR-based executables from a .NET DLL generated by the Q# compiler.
        /// </summary>
        /// <param name="qsharpDll">.NET DLL generated by the Q# compiler.</param>
        /// <param name="libraryDirectories">Directory where the libraries to link to are located.</param>
        /// <param name="includeDirectories">Directory where the headers needed for compilation are located.</param>
        /// <param name="executablesDirectory">Directory where the created executables are placed.</param>
        public static async Task BuildFromQSharpDll(
            FileInfo qsharpDll,
            IList<DirectoryInfo> libraryDirectories,
            IList<DirectoryInfo> includeDirectories,
            DirectoryInfo executablesDirectory)
        {
            using var qirContentStream = new MemoryStream();
            if (!AssemblyLoader.LoadQirBitcode(qsharpDll, qirContentStream))
            {
                throw new ArgumentException("The given DLL does not contain QIR bitcode.");
            }

            executablesDirectory.Create();

            // Concurrently build an executable for each entry point operation.
            var tasks = EntryPointLoader.LoadEntryPointOperations(qsharpDll).Select(entryPointOp =>
            {
                var exeFileInfo = new FileInfo(Path.Combine(executablesDirectory.FullName, $"{entryPointOp.Name}.exe"));
                var exe = new QirFullStateExecutable(exeFileInfo, qirContentStream.ToArray());
                return exe.BuildAsync(entryPointOp, libraryDirectories, includeDirectories);
            });
            
            await Task.WhenAll(tasks);

            // ToDo: Return list of created file names
        }
    }
}
