using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace AKSoftware.Localization.MultiLanguages.CodeGeneration
{
    public class CreateCodeLogic
    {
        private ParseCodeLogic _parseLogic = new ParseCodeLogic();

        public void CreateOrUpdateResourceFileFromLocalizableStrings(ParseParms parms)
        {
            if (String.IsNullOrEmpty(parms.ResourceFilePath))
            {
                throw new ArgumentException("ResourceFilePath is required");
            }

            List<ParseResult> parseResults = _parseLogic.GetLocalizableStrings(parms);
            ModifyResourceFile(parms, parseResults);
        }

        public void ReplaceLocalizableStringsWithVariables(ParseParms parms)
        {
            if (String.IsNullOrEmpty(parms.ResourceFilePath))
            {
                throw new ArgumentException("ResourceFilePath is required");
            }

            List<ParseResult> parseResults = _parseLogic.GetLocalizableStrings(parms);
            ModifyResourceFile(parms, parseResults);
            ModifyRazorFiles(parseResults);
            ModifyRazorCSharpFiles(parms, parseResults);
        }


        private void ModifyResourceFile(ParseParms parms, List<ParseResult> parseResults)
        {
            foreach (var parseResult in parseResults)
            {
                AddResourceKeyValue(parms.ResourceFilePath, parseResult.Key, parseResult.LocalizableString);
            }
        }

        private void ModifyRazorCSharpFiles(ParseParms parms, List<ParseResult> localizableStrings)
        {
            var filesToModify = localizableStrings
                .Where(o => Path.GetFileName(o.FilePath).EndsWith(".razor"))
                .Select(o => o.FilePath)
                .Distinct().ToList();

            foreach (var file in filesToModify)
            {
                string cSharpFile = file + ".cs";
                if (!File.Exists(cSharpFile))
                {
                    continue;
                }

                string original;
                using (StreamReader reader = new StreamReader(cSharpFile))
                {
                    original = reader.ReadToEnd();
                }

                string modified = InjectLanguageContainer(original, parms.InjectLanguageContainerCode);
                modified = InitLanguageComponent(modified, parms.InitLanguageContainerCode);

                using (StreamWriter writer = new StreamWriter(cSharpFile))
                {
                    writer.Write(modified);
                }
            }
        }

        private void ModifyRazorFiles(List<ParseResult> parseResults)
        {
            // Order the parseResults by FilePath
            var orderedResults = parseResults
                .Where(o => Path.GetFileName(o.FilePath).EndsWith(".razor"))
                .OrderBy(pr => pr.FilePath).ToList();

            string currentFilePath = null;
            string content = null;

            foreach (var parseResult in orderedResults)
            {
                // If the file path has changed, read the new file
                if (currentFilePath != parseResult.FilePath)
                {
                    // Write the modified content of the previous file if it exists
                    if (currentFilePath != null && content != null)
                    {
                        File.WriteAllText(currentFilePath, content);
                    }

                    // Read the new file
                    currentFilePath = parseResult.FilePath;
                    content = File.ReadAllText(currentFilePath);
                }

                string replacement;
                if (parseResult.MatchValue.Contains("button")
                    || parseResult.MatchValue.Contains("SfButton"))
                {
                    replacement = parseResult.MatchValue.Replace($">{parseResult.LocalizableString}<"
                        , $">@_lc.Keys[\"{parseResult.Key}\"]<");
                }
                else
                {
                    replacement = parseResult.MatchValue.Replace(parseResult.LocalizableString, $"@_lc.Keys[\"{parseResult.Key}\"]");
                }

                content = content.Replace(parseResult.MatchValue, replacement);
            }

            // Write the last file if it exists
            if (currentFilePath != null && content != null)
            {
                File.WriteAllText(currentFilePath, content);
            }
        }

        private string InjectLanguageContainer(string input, string languageContainer)
        {
            // Check if the languageContainer already exists in the input
            if (input.Contains(languageContainer))
            {
                return input;
            }

            var lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            var injectLines = lines.Where(l => l.TrimStart().StartsWith("[Inject]")).ToList();

            if (injectLines.Any())
            {
                // Find the index of the last [Inject] line
                int lastInjectIndex = Array.LastIndexOf(lines, injectLines.Last());

                // Insert the new line after the last [Inject] line
                return string.Join(Environment.NewLine,
                    lines.Take(lastInjectIndex + 1)
                        .Concat(new[] { "        " + languageContainer })
                        .Concat(lines.Skip(lastInjectIndex + 1)));
            }

            // Find the index of the line with the opening curly brace of the class
            int classOpeningBraceIndex = Array.FindIndex(lines, l => l.TrimStart().StartsWith("public") && l.TrimEnd().EndsWith("{"));

            if (classOpeningBraceIndex != -1)
            {
                // Insert the new line after the class opening brace
                return string.Join(Environment.NewLine,
                    lines.Take(classOpeningBraceIndex + 1)
                        .Concat(new[] { "        " + languageContainer })
                        .Concat(lines.Skip(classOpeningBraceIndex + 1)));
            }

            // If no suitable location is found, return the original input
            return input;
        }

        private string InitLanguageComponent(string input, string initLanguage)
        {
            // Check if the initLanguage already exists in the input
            if (input.Contains(initLanguage))
            {
                return input;
            }

            string[] lines = input.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);

            // Look for "protected override void OnInitialized()"
            string insertOnInitializedResult = InsertOnInitialized(lines, initLanguage);
            if (insertOnInitializedResult != null)
            {
                return insertOnInitializedResult;
            }

            // Look for "protected override async Task OnInitializedAsync()"
            string insertOnInitializedAsyncResult = InsertOnInitializedAsync(lines, initLanguage);

            if (insertOnInitializedAsyncResult != null)
            {
                return insertOnInitializedAsyncResult;
            }

            // If neither method is found, create a new OnInitialized method
            string newOnInitializedResult = InsertNewOnInitialized(lines, initLanguage);

            if (newOnInitializedResult != null)
            {
                return newOnInitializedResult;
            }

            // If no suitable location is found, return the original input
            return input;
        }

        private string InsertNewOnInitialized(string[] lines, string initLanguage)
        {
            string newMethod = $@"
    protected override void OnInitialized()
    {{
        {initLanguage}
    }}";

            // Find the last closing brace (namespace)
            int lastClosingBraceIndex = Array.FindLastIndex(lines, l => l.Trim() == "}");

            if (lastClosingBraceIndex != -1)
            {
                // Find the next-to-last closing brace by searching up to the last closing brace index (class)
                int nextToLastClosingBraceIndex = Array.FindLastIndex(lines, lastClosingBraceIndex - 1, l => l.Trim() == "}");

                if (nextToLastClosingBraceIndex != -1)
                {
                    // Insert the new method before the class closing brace
                    return string.Join(Environment.NewLine,
                        lines.Take(nextToLastClosingBraceIndex)
                            .Concat(new[] { newMethod })
                            .Concat(lines.Skip(nextToLastClosingBraceIndex)));
                }
            }

            return null;
        }


        private string InsertOnInitializedAsync(string[] lines, string initLanguage)
        {
            int onInitializedAsyncIndex = Array.FindIndex(lines, l => l.Trim().StartsWith("protected override async Task OnInitializedAsync()"));
            if (onInitializedAsyncIndex != -1)
            {
                // Find the opening brace
                int openingBraceIndex = Array.FindIndex(lines, onInitializedAsyncIndex, l => l.Contains("{"));
                if (openingBraceIndex != -1)
                {
                    // Insert initLanguage after the opening brace
                    return string.Join(Environment.NewLine,
                        lines.Take(openingBraceIndex + 1)
                            .Concat(new[] { "            " + initLanguage })
                            .Concat(lines.Skip(openingBraceIndex + 1)));
                }
            }

            return null;
        }

        private string InsertOnInitialized(string[] lines, string initLanguage)
        {
            int onInitializedIndex = Array.FindIndex(lines, l => l.Trim().StartsWith("protected override void OnInitialized()"));
            if (onInitializedIndex != -1)
            {
                // Find the opening brace
                int openingBraceIndex = Array.FindIndex(lines, onInitializedIndex, l => l.Contains("{"));
                if (openingBraceIndex != -1)
                {
                    // Insert initLanguage after the opening brace
                    return string.Join(Environment.NewLine,
                        lines.Take(openingBraceIndex + 1)
                            .Concat(new[] { "            " + initLanguage })
                            .Concat(lines.Skip(openingBraceIndex + 1)));
                }
            }

            return null;
        }

        private void AddResourceKeyValue(string filePath, string key, string value)
        {
            string directory = Path.GetDirectoryName(filePath);
            // Create the directory if it does not exist
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new YAML file with the key-value pair if it doesn't exist
            if (!File.Exists(filePath))
            {
                CreateNewYamlFile(filePath, key, value);
                return;
            }

            UpdateYamlFile(filePath, key, value);
        }

        private void UpdateYamlFile(string filePath, string key, string value)
        {
            // Read the YAML file
            string yamlContent;
            using (StreamReader reader = new StreamReader(filePath))
            {
                yamlContent = reader.ReadToEnd();
            }

            // Deserialize YAML to dictionary
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            Dictionary<string, string> yamlObject = deserializer.Deserialize<Dictionary<string, string>>(yamlContent);

            // Add or update the key-value pair
            yamlObject[key] = value;

            // Sort the dictionary by keys
            var sortedYamlObject = yamlObject.OrderBy(pair => pair.Key)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            // Serialize the sorted dictionary back to YAML
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string updatedYamlContent = serializer.Serialize(sortedYamlObject);

            // Save the updated YAML content back to the file
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(updatedYamlContent);
            }
        }

        private void CreateNewYamlFile(string filePath, string key, string value)
        {
            var newSerializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            string newContent = newSerializer.Serialize(new Dictionary<string, string> { { key, value } });
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.Write(newContent);
            }
        }
    }
}
