using AKSoftware.Localization.MultiLanguages.CodeGeneration;
using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AKSoftware.Localization.MultiLanguages.SourceGenerator
{

    /// <summary>
    /// Source generator to scan the en-US language file and generate an injectable class that can be used to get the keys.
    /// The generated class take interpolation into consideration.
    /// </summary>
    [Generator]
    public class KeysAccessorSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Try to fetch the en-US yaml file
            if (!context.TryGetEnUSFileContent(out var fileContent))
                return;

            context.AddSource($"LanguageKeysAccessor.g.cs", KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(fileContent));
            context.AddSource($"DependencyInjectionExtensions.g.cs", KeysAccessorGenerator.GenerateDependencyInjectionExtensionClass());

        }

        public void Initialize(GeneratorInitializationContext context)
        {
            // No initialization is needed
        }



    }
}