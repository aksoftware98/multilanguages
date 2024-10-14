using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.SourceGenerator
{
    internal static class GeneratorExtensions
    {

        /// <summary>
        /// Try find the en-US file content in the referenced project
        /// </summary>
        /// <param name="context"></param>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        internal static bool TryGetEnUSFileContent(this GeneratorExecutionContext context, out string fileContent)
        {
            // Try to fetch the en-US yaml file
            var enUSFileContent = context
                                .AdditionalFiles
                                .FirstOrDefault(f => f.Path.EndsWith("en-US.yml") || f.Path.EndsWith("en-US.yaml"));

            if (enUSFileContent == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("AKSML001", "No en-US file found", "No en-US file found", "Localization", DiagnosticSeverity.Warning, true), Location.None));
                fileContent = null;
                return false;
            }

            // Read the content of the file
            fileContent = enUSFileContent.GetText()?.ToString();
            return true;
        }

    }
}
