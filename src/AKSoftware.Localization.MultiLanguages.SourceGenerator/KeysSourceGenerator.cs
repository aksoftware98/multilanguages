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
    public class KeysDecoratorSourceGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            // Try to fetch the en-US yaml file
            if (!context.TryGetEnUSFileContent(out var fileContent))
                return;

            // Deserialize the yaml file into a dictionary of object, object
            var keyValues = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<string, object>>(fileContent);

            context.AddSource($"LanguageKeys.g.cs", BuildClass(fileContent));
        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public string BuildClass(string fileContent)
        {
            var keyValues = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<object, object>>(fileContent);
            var classes = new List<string>();
            BuildClass(keyValues, string.Empty, classes);

            var allClasses = string.Join(Environment.NewLine, classes);

            var sourceCode = $@"
using System;
using AKSoftware.Localization.MultiLanguages;

namespace AKSoftware.Localization.MultiLanguages
{{
{allClasses}
}}";

            return sourceCode;
        }

        private void BuildClass(Dictionary<object, object> keyValues, string prefix, List<string> classes)
        {
            prefix = string.IsNullOrEmpty(prefix) ? string.Empty : $"{prefix}:";
            var className = prefix.Replace(":", "");

            var interfaceKeysBuilder = new StringBuilder();
            var keysBuilder = new StringBuilder();
            var constructorBuilder = new StringBuilder();

            foreach (var item in keyValues)
            {
                if (item.Value is Dictionary<object, object> nestedKeyValues)
                {

                    var nestedPropertyName = $"{className}{item.Key}";
                    var property = $"\t\tpublic I{nestedPropertyName}KeysAccessor {item.Key} {{ get; }}";
                    keysBuilder.AppendLine(property);
                    interfaceKeysBuilder.AppendLine(property);
                    constructorBuilder.AppendLine($"\t\t\t{nestedPropertyName} = new {nestedPropertyName}KeysAccessor(_languageContainer, \"{prefix}{item.Key}\");");
                    BuildClass(nestedKeyValues, nestedPropertyName, classes);
                }
                else // Value is string
                {
                    // Check if the value contains interpolations 
                    var value = item.Value;
                    var regex = Regex.Matches(value.ToString(), @"\{([^}]*)\}");
                    if (regex.Count > 0)
                    {
                        // Append a method that accepts the parameters
                        var parameters = new List<string>();
                        var parameterAssignments = new StringBuilder();
                        var parameterIndex = 0;
                        foreach (var group in regex)
                        {
                            if (group is Group g)
                            {
                                var rawName = g.Value.Replace("{", "").Replace("}", "");
                                var parameterName = rawName;
                                // Imrove the name of the parameter
                                if (parameterName.Length > 0 && char.IsDigit(parameterName[0]))
                                    parameterName = $"_{parameterName}";
                                if (parameterName.Contains(" "))
                                    parameterName = parameterName.Replace(" ", "_");
                                if (parameterName.Contains("-"))
                                    parameterName = parameterName.Replace("-", "_");
                                if (parameterName.Contains("#"))
                                    parameterName = parameterName.Replace("#", "Number");
                                if (parameterName.Contains("."))
                                    parameterName = parameterName.Replace(".", "Dot");
                                if (parameterName.Contains("$"))
                                    parameterName = parameterName.Replace("$", "_");
                                if (parameterName.Contains("%"))
                                    parameterName = parameterName.Replace("%", "_");
                                if (parameterName.Contains("&"))
                                    parameterName = parameterName.Replace("&", "_");
                                if (parameterName.Contains("*"))
                                    parameterName = parameterName.Replace("*", "_");
                                if (parameterName.Contains("/"))
                                    parameterName = parameterName.Replace("/", "_");
                                if (parameterName.Contains("\\"))
                                    parameterName = parameterName.Replace("\\", "_");
                                if (parameterName.Contains(":"))
                                    parameterName = parameterName.Replace(":", "_");

                                parameters.Add($"string {parameterName}");
                                parameterAssignments.AppendLine($"\t\t\t\t{rawName} = {parameterName},");
                            }

                        }
                        // Append the method for interpolation
                        var methodParameters = string.Join(",", parameters);
                        keysBuilder.AppendLine($"\t\tpublic string {item.Key}({methodParameters})");
                        keysBuilder.AppendLine($"\t\t\t=> _languageContainer[\"{prefix}{item.Key}\", new");
                        keysBuilder.AppendLine($"\t\t\t{{");
                        keysBuilder.AppendLine($"{parameterAssignments}");
                        keysBuilder.AppendLine($"\t\t\t}}");

                        interfaceKeysBuilder.AppendLine($"\t\tpublic string {item.Key}({methodParameters});");
                    }
                    else // Append normal string 
                    {
                        keysBuilder.AppendLine($"\t\tpublic string {item.Key} => _languageContainer[\"{prefix}{item.Key}\"];");
                        interfaceKeysBuilder.AppendLine($"\t\tpublic string {item.Key} {{ get; }}");
                    }

                }
            }

            var newClassName = $"{className}KeysAccessor";
            var interfaceName = $"I{className}KeysAccessor";
            var constructorParameters = string.IsNullOrWhiteSpace(prefix) ? "ILanguageContainerService languageContainer" : "ILanguageContainerService languageContainer, string prefix";
            var sourceCode = $@"
            
    public interface {interfaceName}
    {{
{interfaceKeysBuilder}
    }}
    
    public class {newClassName} : {interfaceName}
    {{
        private readonly ILanguageContainerService _languageContainer;
        public {newClassName}({constructorParameters})
        {{
            _languageContainer = languageContainer;
{constructorBuilder}
        }}

{keysBuilder}
    }}";


            classes.Add(sourceCode);
        }


    }
}