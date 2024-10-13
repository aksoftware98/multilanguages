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
            // Generate a simple HelloWorld class
            const string source = @"
            public static class HelloWorldGenerator
            {
                public static void SayHello()
                {
                    System.Console.WriteLine(""Hello from the source generator!"");
                }
            }";

            context.AddSource("HelloWorldGenerator.g.cs", SourceText.From(source, Encoding.UTF8));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }
    }
}