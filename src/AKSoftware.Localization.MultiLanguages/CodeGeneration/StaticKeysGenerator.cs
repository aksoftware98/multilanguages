using System;
using System.Collections.Generic;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    /// <summary>
    /// Provides the tool to generate a class that contains all the keys as static constants. Nested keys will be subclasses of the main class.
    /// </summary>
    public class StaticKeysGenerator
    {

        /// <summary>
        /// Generate a class that contains all the keys as static constants. Nested keys will be subclasses of the main class.
        /// </summary>
        /// <param name="enUSFileContent"></param>
        /// <returns></returns>
        public static string GenerateStaticKeyClass(string enUSFileContent)
        {
            var keyValues = new YamlDotNet.Serialization.Deserializer().Deserialize<Dictionary<object, object>>(enUSFileContent);

            var sourceCode = $@"
using System;
using AKSoftware.Localization.MultiLanguages;

namespace AKSoftware.Localization.MultiLanguages
{{
{BuildClass(keyValues, "LanguageKeys", string.Empty)}
}}";
            return sourceCode;
        }


        private static string BuildClass(Dictionary<object, object> keyValues, string className, string prefix)
        {
            var stringBuilder = new StringBuilder();
            prefix = string.IsNullOrWhiteSpace(prefix) ? string.Empty : prefix + ":";
            stringBuilder.AppendLine($"\tpublic static class {className}");
            stringBuilder.AppendLine("\t{");
            foreach (var key in keyValues)
            {

                if (key.Value is Dictionary<object, object> nestedKeyValues)
                {
                    stringBuilder.AppendLine(BuildClass(nestedKeyValues, key.Key.ToString(), $"{prefix}{key.Key}"));
                }
                else
                {
                    stringBuilder.AppendLine($"\t\tpublic const string {key.Key} = \"{prefix}{key.Key}\";");
                }

            }
            stringBuilder.AppendLine("\t}");

            return stringBuilder.ToString();
        }
    }
}
