using AKSoftware.Localization.MultiLanguages.CodeGeneration;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.SourceGenerator
{

    /// <summary>
    /// Source generator to generate a static class with a list of constants for each key in the language file
    /// </summary>
    [Generator]
    public class StaticKeysSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Try to fetch the en-US yaml file
            if (!context.TryGetEnUSFileContent(out var fileContent))
                return;

            context.AddSource($"LanguageKeys.g.cs", StaticKeysGenerator.GenerateStaticKeyClass(fileContent));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization is needed
        }
    }
}