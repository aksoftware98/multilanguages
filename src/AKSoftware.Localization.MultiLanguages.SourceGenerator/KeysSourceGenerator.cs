using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.SourceGenerator
{

    /// <summary>
    /// Source generator to scan the en-US language file and generate an injectable class that can be used to get the keys.
    /// The generated class take interpolation into consideration.
    /// </summary>
    [Generator]
    public class KeysDecoratorSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Try to fetch the en-US yaml file
            var englishFile = context
                                .AdditionalFiles
                                .FirstOrDefault(f => f.Path.EndsWith("en-US.yml") || f.Path.EndsWith("en-US.yaml"));

            if (englishFile == null)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("AKSML001", "No en-US file found", "No en-US file found", "Localization", DiagnosticSeverity.Warning, true), Location.None));
                return;
            }

            // Read the content of the file
            var fileContent = englishFile.GetText()?.ToString();
            // Deserialize the yaml file into a dictionary of object, object
            var keyValues = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, object>>(fileContent);

            // Generate the source code
            var keys = new StringBuilder();
            string prefix = string.Empty;
            PopulateKeys(keys, keyValues, prefix);

            string sourceCode = $@"
using System;

namespace AKSoftware.Localization.MultiLanguages.Keys
{{
    public static class LanguageKeys
    {{
        {keys.ToString()}
    }}
}}
                ";

            context.AddSource($"LanguageKeys.g.cs", sourceCode);
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        private void PopulateKeys(StringBuilder keysBuilder, Dictionary<string, object> keyValues, string prefix)
        {
            foreach (var item in keyValues)
            {
                if (item.Value is string)
                    keysBuilder.AppendLine($"\t\tpublic const string {prefix}{item.Key} = {item.Value};");
                else if (item.Value is Dictionary<string, object> dictionary)
                {
                    PopulateKeys(keysBuilder, dictionary, $"{prefix}{item.Key}_");
                }
            }
        }

    }
}