using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    /// <summary>
    /// Provides the ability to generate a class that wraps all the access to keys including nested keys in a strong typed way.
    /// It also provides the generation of the dependency injection extension method to register the keys accessor.
    /// </summary>
    public static class KeysAccessorGenerator
    {


        /// <summary>
        /// Generate the DependencyInjectionExtensions class that contains the extension methods to register the KeysAccessor class
        /// The class contains two methods, one to register the KeysAccessor as scoped and the other to register it as singleton
        /// </summary>
        /// <returns></returns>
        public static string GenerateDependencyInjectionExtensionClass()
        {
            return @"
using AKSoftware.Localization.MultiLanguages;

namespace AKSoftware.Localization.MultiLanguages
{
    public static class DependencyInjectionExtensions
    {
        /// <summary>
        /// Register the KeysAccessor auto-generated class and services as scoped (Preferred for most application types)
        /// </summary>
        public static IServiceCollection AddKeysAccessorAsScoped(this IServiceCollection services)
            => services.AddScoped<IKeysAccessor, KeysAccessor>();

        /// <summary>
        /// Register the KeysAccessor auto-generated class and services as singleton (Preferred for Blazor Web App and Blazor Server Apps)
        /// </summary>
        public static IServiceCollection AddKeysAccessorAsSingleton(this IServiceCollection services)
            => services.AddSingleton<IKeysAccessor, KeysAccessor>();
    }
        
}
            ";
        }

        /// <summary>
        /// Generate the KeysAccessor class with its nested classes and interfaces that contain all the keys and values from the en-US yaml file
        /// The IKeysAccessor is used to access all the keys in a strongly typed way
        /// </summary>
        /// <param name="fileContent"></param>
        /// <returns></returns>
        public static string GenerateKeysAccessorClassesAndInterfaces(string fileContent)
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

        private static void BuildClass(Dictionary<object, object> keyValues, string prefix, List<string> classes)
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

                        foreach (var group in regex)
                        {
                            if (group is Group g)
                            {
                                var rawName = g.Value.Replace("{", "").Replace("}", "");
                                var parameterName = rawName;
                                // Improve the name of the parameter
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

            // Generate the container class of type KeysAccessor and its interface
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
