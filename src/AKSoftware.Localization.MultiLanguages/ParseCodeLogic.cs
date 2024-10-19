using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AKSoftware.Localization.MultiLanguages
{
    public class ParseCodeLogic
    {
        private static List<string> _ignoreStartsWith = new List<string>()
        {
            "http://", "https://", "class=", "style=", "src=", "alt=", "width=", "height=", "id=", "if (", "var ", "%",
            "display:", "}", "else", "@*", "["
        };

        private static List<string> _ignoreContains = new List<string>()
        {
            "@(", "@_", "[@_", "=\""
        };

        private const string ReplacementMarker = "~~~";
        private const string StringType = "string";
        private const string PropertyTypeGroup = "propertyType";
        private const string ContentGroup = "content";
        
        private Regex _keyReferenceRegex;

        private static Regex _propertyRegex =
            new Regex(@"public\s+(?<propertyType>[^\s\?]+\??)\s+(?<content>\w+)\s*{\s*get;\s*set;\s*}",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _requiredAttributeWithMessageRegex = new Regex(
            @"\[Required\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _requiredAttributeRegex = new Regex(
            @"\[Required\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _maxLengthAttributeWithMessageRegex = new Regex(
            @"\[MaxLength\((?<maxLength>\d+),\s*ErrorMessage\s*=\s*""(?<content>.*?)""\)\]",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _maxLengthAttributeRegex = new Regex(
            @"\[MaxLength\((?<maxLength>\d+)\]",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _styleTag = new Regex(@"<style[^>]*>[^<]*<\/style>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _scriptTag = new Regex(@"<script[^>]*>[^<]*<\/script>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _imgTag = new Regex(@"<img[^>]*>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _iconTag = new Regex(@"<i\s+class[^>]*><\/i>", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _codeTag = new Regex(@"@code[\s\S]+", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _brTag = new Regex(@"(<br>)|(<br\s*\/>)", RegexOptions.Compiled | RegexOptions.Multiline);
        private static Regex _htmlComment = new Regex(@"<!--[\s\S]*?-->", RegexOptions.Compiled | RegexOptions.Multiline);

        //Not wrapped inside an HTML Tag
        private static Regex _notWrapped =
            new Regex(@"(<\/[A-Za-z0-9]+>+\s*(?<content>[A-Za-z][^<]+))|(^\s*(?<content>[A-Za-z][^<]+))",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static List<Regex> _removePatterns = new List<Regex>()
        {
            // Begin and end tag:  <i class="fa fa-home"></i>
            new Regex(@"<[A-Za-z]+[^>]*>.*<\/[A-Za-z]+>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            // No end tag, no forward slashes: <ValidationMessage For="@(() => loginModel.Email)" />
            new Regex(@"<[A-Za-z]+[^\/]*\/>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            // No end tag with forward slashes: <img src="https://www.example.com/image.jpg" alt="Example Image" />
            new Regex(@"<[A-Za-z]+[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            //Non-breaking space
            new Regex(@"&nbsp;", RegexOptions.Compiled|RegexOptions.IgnoreCase),
            //Razor variable
            new Regex(@"@[A-Za-z0-9\.]+", RegexOptions.Compiled|RegexOptions.IgnoreCase),
            //Begin tag hanging out there
            new Regex(@"<[A-Za-z]+[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            //End tag hanging out there
            new Regex(@"<\/[A-Za-z]+[^>]*>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
        };

        private static List<Regex> _contentPatterns = new List<Regex>()
        {
            new Regex(@"<a\s[^>]*>(?<content>[\s\S]+?)<\/a>", RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Multiline),
            new Regex(@"\sPlaceholder=""(?<content>[^""]+)""\s", RegexOptions.Compiled | RegexOptions.Multiline),
            new Regex(@"\sLabel=""(?<content>[^""]+)""\s", RegexOptions.Compiled | RegexOptions.Multiline),
            new Regex(@"<button[^<]+>(?<content>[\s\S]+?)</button>", RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Multiline),
            new Regex(@"<SfButton[^\/<]+>(?<content>[\s\S]+?)</SfButton>", RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Multiline),
            new Regex(@"<label[^<]+>(?<content>\s*[A-Za-z].*?)</label>", RegexOptions.Compiled|RegexOptions.IgnoreCase|RegexOptions.Multiline),
            new Regex(@"<b>(?<content>[\s\S]+?)<\/b>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<i>(?<content>[\s\S]+?)<\/i>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<strong>(?<content>[\s\S]+?)<\/strong>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<em>(?<content>[\s\S]+?)<\/em>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<small>(?<content>[\s\S]+?)<\/small>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<mark>(?<content>[\s\S]+?)<\/mark>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<sub>(?<content>[\s\S]+?)<\/sub>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<sup>(?<content>[\s\S]+?)<\/sup>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<u>(?<content>[\s\S]+?)<\/u>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<s>(?<content>[\s\S]+?)<\/s>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<PageTitle>(?<content>[\s\S]+?)<\/PageTitle>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<span[^\/>]*>(?<content>[\s\S]+?)<\/span>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<th[^\/>]*>(?<content>[A-Za-z].*?)<\/th>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<td[^\/>]*>(?<content>[\s\S]+?)<\/td>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<h\d[^\/>]*>(?<content>[^<]+)<\/h\d>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<li[^\/>]*>(?<content>[\s\S]+?)<\/li>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<code[^\/>]*>(?<content>[\s\S]+?)<\/code>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<pre[^\/>]*>(?<content>[\s\S]+?)<\/pre>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<p[^\/>]*>(?<content>[\s\S]+?)<\/p>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<div[^\/>]*>(?<content>\s*[A-Za-z][\s\S]*?)<\/div>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<blockquote[^\/>]*>(?<content>[\s\S]+?)<\/blockquote>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<aside[^\/>]*>(?<content>[\s\S]+?)<\/aside>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<article[^\/>]*>(?<content>[\s\S]+?)<\/article>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<section[^\/>]*>(?<content>[\s\S]+?)<\/section>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<header[^\/>]*>(?<content>[\s\S]+?)<\/header>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<footer[^\/>]*>(?<content>[\s\S]+?)<\/footer>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<nav[^\/>]*>(?<content>[\s\S]+?)<\/nav>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
            new Regex(@"<main[^\/>]*>(?<content>[\s\S]+?)<\/main>", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline),
        };

        /// <summary>
        /// Get a list of localizable strings from the source directories
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<ParseResult> GetLocalizableStrings(ParseParms parms)
        {
            if (parms.RemoveLocalizedKeys && String.IsNullOrEmpty(parms.ResourceFilePath))
            {
                throw new ArgumentException($"{nameof(parms.ResourceFilePath)} is required");
            }

            ValidateParseParameters(parms);

            List<string> files = GetAllFilesForSourceDirectoriesAndWildcards(parms);

            List<string> filesToProcess = files
                .Where(o => parms.ExcludeFiles.All(a => a != Path.GetFileName(o)))
                .Where(o => parms.ExcludeDirectories.All(a =>
                    !o.Contains(Path.DirectorySeparatorChar + a + Path.DirectorySeparatorChar)))
                .ToList();

            List<ParseResult> result = new List<ParseResult>();

            foreach (var file in filesToProcess)
            {
                string content = File.ReadAllText(file);

                List<ParseResult> fileResults = new List<ParseResult>();

                if (Path.GetExtension(file) == ".cs")
                {
                    fileResults.AddRange(GetRequiredWithErrorMessageInText(content));
                    fileResults.AddRange(GetRequiredAttributeInText(content));
                    fileResults.AddRange(GetMaxLengthAttributesWithErrorMessageInText(content));
                    fileResults.AddRange(GetMaxLengthAttributesInText(content));
                }
                else
                {
                    fileResults.AddRange(GetLocalizableStringsInText(content));
                }

                foreach (var fileResult in fileResults)
                {
                    fileResult.FilePath = file;
                }
                result.AddRange(fileResults);
            }

            if (parms.RemoveLocalizedKeys)
            {
                var existingKeyValues = ReadYamlFile(parms.ResourceFilePath);
                result = RemoveLocalizedKeys(parms, result, existingKeyValues);
            }

            return result
                .OrderBy(o => o.Key)
                .ToList();
        }





        private List<ParseResult> RemoveLocalizedKeys(ParseParms parms, List<ParseResult> parseResults, Dictionary<string, string> existingKeyValues)
        {
            List<ParseResult> result = new List<ParseResult>();

            foreach (var parseResult in parseResults)
            {
                if (parms.Prefixes.Any(o => parseResult.Key.StartsWith(o))
                    && !existingKeyValues.ContainsKey(parseResult.Key))
                {
                    result.Add(parseResult);
                }
            }

            return result;
        }

        private List<ParseResult> GetMaxLengthAttributesWithErrorMessageInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _maxLengthAttributeWithMessageRegex.Matches(text);

            foreach (Match match in matches)
            {
                string errorMessage = match.Groups[ContentGroup].Value;
                string maxLength = match.Groups["maxLength"].Value;
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;
                string propertyType = propertyStringMatch.Groups[PropertyTypeGroup].Value;

                //TODO:  We currently only support string properties for required
                if (!propertyType.ToLower().StartsWith(StringType))
                    continue;

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _maxLengthAttributeWithMessageRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"MaxLength{propertyName}{maxLength}"
                });
            }

            return result;
        }

        private List<ParseResult> GetMaxLengthAttributesInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _maxLengthAttributeRegex.Matches(text);

            foreach (Match match in matches)
            {
                string maxLength = match.Groups["maxLength"].Value;
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;
                string propertyType = propertyStringMatch.Groups[PropertyTypeGroup].Value;

                //TODO:  We currently only support string properties for required
                if (!propertyType.ToLower().StartsWith(StringType))
                    continue;

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} has a maximum length of {maxLength} characters";
                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _maxLengthAttributeRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"MaxLength{propertyName}{maxLength}"
                });
            }

            return result;
        }

        private List<ParseResult> GetRequiredWithErrorMessageInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _requiredAttributeWithMessageRegex.Matches(text);

            foreach (Match match in matches)
            {
                string errorMessage = match.Groups[ContentGroup].Value;
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;
                string propertyType = propertyStringMatch.Groups[PropertyTypeGroup].Value;

                //TODO:  We currently only support string properties for required
                if (!propertyType.ToLower().StartsWith(StringType))
                    continue;

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _requiredAttributeWithMessageRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"Required{propertyName}"
                });
            }

            return result;
        }

        private List<ParseResult> GetRequiredAttributeInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _requiredAttributeRegex.Matches(text);

            foreach (Match match in matches)
            {
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;
                string propertyType = propertyStringMatch.Groups[PropertyTypeGroup].Value;

                //TODO:  We currently only support string properties for required
                if (!propertyType.ToLower().StartsWith(StringType))
                    continue;

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} is required";

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _requiredAttributeWithMessageRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"Required{propertyName}"
                });
            }

            return result;
        }


        /// <summary>
        /// Get a list of localizable strings in the text.  The text can be a razor, HTML, or a C# model class
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public List<ParseResult> GetLocalizableStringsInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            if (string.IsNullOrEmpty(text))
            {
                return result;
            }

            text = PreProcess(text);

            foreach (Regex pattern in _contentPatterns)
            {
                MatchCollection matches = pattern.Matches(text);
                foreach (Match match in matches)
                {
                    string content = match.Groups[ContentGroup].Value;
                    content = RemovePatterns(content);
                    AddParseResult(pattern, match, result, content);
                }

                //We use this to split in the AddParseResult method
                //Example:  Some text <strong>bold text</strong> more text
                if (matches.Count > 0)
                {
                    text = pattern.Replace(text, ReplacementMarker);
                }
            }

            //If there is no code, then check for text that is not wrapped in an HTML tag
            if (!text.Contains("{"))
            {
                MatchCollection matches = _notWrapped.Matches(text);
                foreach (Match match in matches)
                {
                    string content = match.Groups[ContentGroup].Value;
                    content = RemovePatterns(content);
                    AddParseResult(_notWrapped, match, result, content);
                }
            }

            return result;
        }

        
        private List<ParseResult> GetExistingLocalizedStringsInText(string html,
            Dictionary<string, string> existingKeyValues)
        {
            List<ParseResult> result = new List<ParseResult>();
            html = PreProcess(html);

            MatchCollection matches = _keyReferenceRegex.Matches(html);
            foreach (Match match in matches)
            {
                string existingKey = match.Groups[ContentGroup].Value;
                string existingValue =
                    existingKeyValues.ContainsKey(existingKey) ? existingKeyValues[existingKey] : null;

                result.Add(new ParseResult
                {
                    LocalizableString = existingValue,
                    MatchingExpression = _keyReferenceRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = existingKey
                });
            }

            return result;
        }

        private string PreProcess(string input)
        {
            input = RemoveByTag(input, _scriptTag);
            input = RemoveByTag(input, _styleTag);
            input = RemoveByTag(input, _imgTag);
            input = RemoveByTag(input, _codeTag);
            input = RemoveByTag(input, _iconTag);
            input = RemoveByTag(input, _brTag);
            input = RemoveByTag(input, _htmlComment);
            return input;
        }

        private string RemoveByTag(string input, Regex tag)
        {
            return tag.Replace(input, ReplacementMarker);
        }

        /// <summary>
        /// Get a list of duplicate keys
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public Dictionary<string, List<string>> GetDuplicateKeys(ParseParms parms)
        {
            List<ParseResult> parseResults = GetLocalizableStrings(parms);
            Dictionary<string, List<string>> conflictingKeys = GetDuplicateKeys(parseResults);
            return conflictingKeys;
        }

        private Dictionary<string, List<string>> GetDuplicateKeys(List<ParseResult> parseResults)
        {
            var conflictingKeys = new Dictionary<string, List<string>>();
            var keyDict = new Dictionary<string, string>();

            foreach (var result in parseResults)
            {
                if (string.IsNullOrEmpty(result.Key))
                {
                    continue;
                }

                if (!keyDict.ContainsKey(result.Key))
                {
                    keyDict[result.Key] = result.LocalizableString;
                }
                else if (keyDict[result.Key] != result.LocalizableString)
                {
                    if (!conflictingKeys.ContainsKey(result.Key))
                    {
                        conflictingKeys.Add(result.Key, new List<string>() { keyDict[result.Key] });
                    }
                    conflictingKeys[result.Key].Add(result.LocalizableString);
                }
            }

            return conflictingKeys;
        }

        /// <summary>
        /// Get a list of keys that are in the YAML file but are not in use
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public List<string> GetUnusedKeys(ParseParms parms)
        {
            if (string.IsNullOrEmpty(parms.ResourceFilePath))
            {
                throw new ArgumentException("ResourceFilePath is required");
            }

            // Read existing keys from the YAML file
            var existingKeys = ReadYamlFile(parms.ResourceFilePath).Keys.ToList();

            //Exclude Required, MaxLength, Dynamic because those are used dynamically in ValidationLocalization
            existingKeys = existingKeys.Where(k => !parms.Prefixes.Any(p => k.StartsWith(p))).ToList();

            // Get localizable strings to find keys in use
            var parseResults = GetExistingLocalizedStrings(parms);
            var keysInUse = parseResults.Select(r => r.Key)
                .Distinct()
                .ToList();

            // Find keys that exist in the YAML file but are not in use
            var unusedKeys = existingKeys.Except(keysInUse).ToList();

            return unusedKeys;
        }

        /// <summary>
        /// Get a list of localizable strings that are already in the YAML file and in use
        /// </summary>
        /// <param name="parms"></param>
        /// <returns></returns>
        public List<ParseResult> GetExistingLocalizedStrings(ParseParms parms)
        {
            ValidateParseParameters(parms);
            string escapedKeyReference = Regex.Escape(parms.KeyReference);
            string pattern = $@"({escapedKeyReference}\.Keys\[""?(?<content>[^,""\]]+)?)|({escapedKeyReference}\[""?(?<content>[^,""\]]+)?)";

            _keyReferenceRegex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.Multiline);
            var existingKeyValues = ReadYamlFile(parms.ResourceFilePath);
            List<string> files =  GetAllFilesForSourceDirectoriesAndWildcards(parms);

            List<string> filesToProcess = files
                .Where(o => parms.ExcludeFiles.All(a => a != Path.GetFileName(o)))
                .Where(o => parms.ExcludeDirectories.All(a =>
                    !o.Contains(Path.DirectorySeparatorChar + a + Path.DirectorySeparatorChar)))
                .ToList();

            List<ParseResult> result = new List<ParseResult>();

            foreach (var file in filesToProcess)
            {
                string content = File.ReadAllText(file);
                var fileResults = GetExistingLocalizedStringsInText(content, existingKeyValues);

                foreach (var fileResult in fileResults)
                {
                    fileResult.FilePath = file;
                }
                result.AddRange(fileResults);
            }

            return result;
        }

        private List<string> GetAllFilesForSourceDirectoriesAndWildcards(ParseParms parms)
        {
            List<string> files = new List<string>();

            foreach (var sourceDirectory in parms.SourceDirectories)
            {
                foreach (var wildcardPattern in parms.WildcardPatterns)
                {
                    string[] sourceFiles = Directory.GetFiles(sourceDirectory, wildcardPattern, SearchOption.AllDirectories);
                    files.AddRange(sourceFiles);
                }
            }

            return files.Distinct().OrderBy(o => o) .ToList();
        }

        internal Dictionary<string, string> ReadYamlFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, string>();
            }

            string yamlContent;
            using (StreamReader reader = new StreamReader(filePath))
            {
                yamlContent = reader.ReadToEnd();
            }

            var objectDictionary = new Deserializer().Deserialize<Dictionary<object, object>>(yamlContent);
            var result = new Dictionary<string, string>();
            FlattenKeysRecursive(objectDictionary, "", result);
            return result;

        }

        private void FlattenKeysRecursive(object obj, string prefix, Dictionary<string, string> result)
        {
            if (obj is IReadOnlyDictionary<object, object> dict)
            {
                foreach (var kvp in dict)
                {
                    string newPrefix = string.IsNullOrEmpty(prefix) ? kvp.Key.ToString() : $"{prefix}:{kvp.Key}";
                    if (kvp.Value is IReadOnlyDictionary<object, object>)
                    {
                        FlattenKeysRecursive(kvp.Value, newPrefix, result);
                    }
                    else
                    {
                        result[newPrefix] = kvp.Value?.ToString() ?? string.Empty;
                    }
                }
            }
            else if (!string.IsNullOrEmpty(prefix))
            {
                result[prefix] = obj?.ToString() ?? string.Empty;
            }
        }

        private void AddParseResult(Regex pattern, Match match, List<ParseResult> result, string content)
        {
            const int minStringLength = 2;
            var contentList = content.Split(new[] { ReplacementMarker }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var contentItem in contentList.ToList())
            {
                var trimmed = contentItem.Trim();

                if (!String.IsNullOrEmpty(trimmed)
                    && trimmed.Length >= minStringLength
                    && !_ignoreStartsWith.Any(sw => trimmed.StartsWith(sw))
                    && !_ignoreContains.Any(c => trimmed.Contains(c))
                    && AtLeastOneAlphabeticCharacter(trimmed)
                    && !result.Any(o => String.IsNullOrEmpty(o.FilePath) && o.LocalizableString == trimmed))
                {
                    result.Add(new ParseResult
                    {
                        LocalizableString = trimmed,
                        MatchingExpression = pattern,
                        FilePath = string.Empty,
                        MatchValue = match.Value,
                        Key = BuildKey(trimmed)
                    });
                }
            }
        }



        private string BuildKey(string content)
        {
            const int maxWords = 4;
            string[] words = content.Split(new[] { ' ', '\t', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sb = new StringBuilder(content.Length);

            for (int i = 0; i < words.Length && i < maxWords; i++)
            {
                sb.Append(StringUtil.ProperCase(StringUtil.FilterAlphaNumeric(words[i])));
            }

            if (content.EndsWith(":"))
            {
                sb.Append("Colon");
            }

            return sb.ToString();
        }

        private bool AtLeastOneAlphabeticCharacter(string input)
        {
            return input.Any(char.IsLetter);
        }

        private string RemovePatterns(string html)
        {
            foreach (var pattern in _removePatterns)
            {
                html = pattern.Replace(html, ReplacementMarker);
            }

            return html;
        }

        private void ValidateParseParameters(ParseParms parms)
        {
            if (String.IsNullOrWhiteSpace(parms.KeyReference))
            {
                throw new ArgumentException("KeyReference is required");
            }

            if (parms.SourceDirectories == null || parms.SourceDirectories.Count == 0)
            {
                throw new ArgumentException("SourceDirectories is required and must have at least one directory specified");
            }

            if (parms.WildcardPatterns == null || parms.WildcardPatterns.Count == 0)
            {
                throw new ArgumentException("WildcardPattern is required and must have at least one specified.  Example:  *.razor");
            }

            foreach (var parmsSourceDirectory in parms.SourceDirectories)
            {
                if (!Directory.Exists(parmsSourceDirectory))
                {
                    throw new DirectoryNotFoundException($"The specified Source Directory does not exist: {parmsSourceDirectory}");
                }

            }
        }
    }
}
