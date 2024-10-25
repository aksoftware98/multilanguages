using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace AKSoftware.Localization.MultiLanguages
{
    internal class DataAttributeParsing
    {
        public const string RequiredPrefix = "Required";
        public const string MaxLengthPrefix = "MaxLength";
        public const string StringLengthPrefix = "StringLength";
        public const string RangePrefix = "Range";
        public const string RegularExpressionPrefix = "RegularExpression";
        public const string ComparePrefix = "Compare";
        public const string CreditCardPrefix = "CreditCard";
        public const string EmailAddressPrefix = "EmailAddress";
        public const string PhonePrefix = "Phone";
        public const string UrlPrefix = "Url";

        private static Regex _propertyRegex = new Regex(@"public\s+(?<propertyType>[^\s\?]+\??)\s+(?<content>\w+)\s*{\s*get;\s*set;\s*}",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _requiredAttributeWithMessageRegex = new Regex(
            @"\[Required\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _requiredAttributeRegex = new Regex(
            @"\[Required\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _emailAddressAttributeWithMessageRegex = new Regex(
            @"\[EmailAddress\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _emailAddressAttributeRegex = new Regex(
            @"\[EmailAddress\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _phoneAttributeWithMessageRegex = new Regex(
            @"\[Phone\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _phoneAttributeRegex = new Regex(
            @"\[Phone\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _creditCardAttributeWithMessageRegex = new Regex(
            @"\[CreditCard\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _creditCardAttributeRegex = new Regex(
            @"\[CreditCard\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _urlAttributeWithMessageRegex = new Regex(
            @"\[Url\(ErrorMessage\s*=\s*""(?<content>.*?)""\)\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _urlAttributeRegex = new Regex(
            @"\[Url\]", RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _maxLengthAttributeWithMessageRegex = new Regex(@"\[MaxLength\((?<maxLength>\d+),\s*ErrorMessage\s*=\s*""(?<content>.*?)""\)\]",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _maxLengthAttributeRegex = new Regex(@"\[MaxLength\((?<maxLength>\d+)\]",
            RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _rangeAttributeRegex =
            new Regex(@"\[Range\((?<min>\d+),\s*(?<max>\d+)\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _rangeAttributeWithMessageRegex =
            new Regex(@"\[Range\((?<min>\d+),\s*(?<max>\d+),\s*ErrorMessage\s*=\s*""(?<content>.*?)""\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _compareAttributeRegex =
            new Regex(@"\[Compare\(\s*""(?<compare>.*?)""\s*\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _compareAttributeWithMessageRegex =
            new Regex(@"\[Compare\(\s*""(?<compare>.*?)""\s*,\s*ErrorMessage\s*=\s*""(?<content>.*?)""\s*\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _regularExpressionAttributeRegex =
            new Regex(@"\[RegularExpression\(\s*@?""(?<pattern>.*?)""\s*\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private static Regex _regularExpressionWithMessageRegex =
            new Regex(@"\[RegularExpression\(\s*@?""(?<pattern>.*?)""\s*,\s*ErrorMessage\s*=\s*""(?<content>.*?)""\s*\)\]",
                RegexOptions.Compiled | RegexOptions.Multiline);

        private const string StringType = "string";
        private const string PropertyTypeGroup = "propertyType";
        private const string ContentGroup = "content";

        public List<string> GetDataAttributePrefixes()
        {
            return new List<string>()
            {
                RequiredPrefix,
                MaxLengthPrefix,
                StringLengthPrefix,
                RangePrefix,
                RegularExpressionPrefix,
                ComparePrefix,
                CreditCardPrefix,
                EmailAddressPrefix,
                PhonePrefix,
                UrlPrefix
            };
        }

        public List<ParseResult> ParseDataAttributes(List<ParseResult> parseResults, string text)
        {
            parseResults.AddRange(GetRequiredAttributeInText(text));
            parseResults.AddRange(GetRequiredWithErrorMessageInText(text));
            parseResults.AddRange(GetEmailAddressAttributeInText(text));
            parseResults.AddRange(GetEmailAddressWithErrorMessageInText(text));
            parseResults.AddRange(GetPhoneAttributeInText(text));
            parseResults.AddRange(GetPhoneWithErrorMessageInText(text));
            parseResults.AddRange(GetCreditCardAttributeInText(text));
            parseResults.AddRange(GetCreditCardWithErrorMessageInText(text));
            parseResults.AddRange(GetUrlAttributeInText(text));
            parseResults.AddRange(GetUrlWithErrorMessageInText(text));
            parseResults.AddRange(GetMaxLengthAttributesInText(text));
            parseResults.AddRange(GetMaxLengthAttributesWithErrorMessageInText(text));
            parseResults.AddRange(GetRangeAttributesInText(text));
            parseResults.AddRange(GetRangeAttributesWithMessageInText(text));
            parseResults.AddRange(GetCompareAttributesInText(text));
            parseResults.AddRange(GetCompareAttributesWithMessageInText(text));
            parseResults.AddRange(GetRegularExpressionAttributeInText(text));
            parseResults.AddRange(GetRegularExpressionWithErrorMessageInText(text));


            return parseResults;
        }

        private List<ParseResult> GetRequiredWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_requiredAttributeWithMessageRegex, text, RequiredPrefix);
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

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} is required";

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _requiredAttributeRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{RequiredPrefix}{propertyName}"
                });
            }

            return result;
        }

        private List<ParseResult> GetEmailAddressWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_emailAddressAttributeWithMessageRegex, text, EmailAddressPrefix);
        }

        private List<ParseResult> GetEmailAddressAttributeInText(string text)
        {
            return NoParmAttributeInText(_emailAddressAttributeRegex, text, EmailAddressPrefix);
        }

        private List<ParseResult> GetPhoneWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_phoneAttributeWithMessageRegex, text, PhonePrefix);
        }

        private List<ParseResult> GetPhoneAttributeInText(string text)
        {
            return NoParmAttributeInText(_phoneAttributeRegex, text, PhonePrefix);
        }
        private List<ParseResult> GetCreditCardWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_creditCardAttributeWithMessageRegex, text, CreditCardPrefix);
        }

        private List<ParseResult> GetCreditCardAttributeInText(string text)
        {
            return NoParmAttributeInText(_creditCardAttributeRegex, text, CreditCardPrefix);
        }

        private List<ParseResult> GetUrlWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_urlAttributeWithMessageRegex, text, UrlPrefix);
        }

        private List<ParseResult> GetUrlAttributeInText(string text)
        {
            return NoParmAttributeInText(_urlAttributeRegex, text, UrlPrefix);
        }

        private List<ParseResult> GetRegularExpressionWithErrorMessageInText(string text)
        {
            return NoParmAttributeWithErrorMessageInText(_regularExpressionWithMessageRegex, text, RegularExpressionPrefix);
        }

        private List<ParseResult> GetRegularExpressionAttributeInText(string text)
        {
            return NoParmAttributeInText(_urlAttributeRegex, text, RegularExpressionPrefix);
        }

        private List<ParseResult> NoParmAttributeWithErrorMessageInText(Regex regex, string text, string prefix)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                string errorMessage = match.Groups[ContentGroup].Value;
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = regex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{prefix}{propertyName}"
                });
            }

            return result;
        }


        private List<ParseResult> NoParmAttributeInText(Regex regex, string text, string prefix)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = regex.Matches(text);

            foreach (Match match in matches)
            {
                int index = text.IndexOf(match.Value);
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} is an invalid format";

                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = regex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{prefix}{propertyName}"
                });
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
                    Key = $"{MaxLengthPrefix}{propertyName}{maxLength}"
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

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} has a maximum length of {maxLength} characters";
                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _maxLengthAttributeRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{MaxLengthPrefix}{propertyName}{maxLength}"
                });
            }

            return result;
        }

        private List<ParseResult> GetRangeAttributesInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _rangeAttributeRegex.Matches(text);

            foreach (Match match in matches)
            {
                string min = match.Groups["min"].Value;
                string max = match.Groups["max"].Value;
                int index = text.IndexOf(match.Value);

                // Find the associated property name
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} must be between {min} and {max}";
                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _rangeAttributeRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{RangePrefix}{propertyName}{min}To{max}"
                });
            }

            return result;
        }

        private List<ParseResult> GetRangeAttributesWithMessageInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matchesWithMessage = _rangeAttributeWithMessageRegex.Matches(text);

            foreach (Match match in matchesWithMessage)
            {
                string min = match.Groups["min"].Value;
                string max = match.Groups["max"].Value;
                string content = match.Groups["content"].Value;
                int index = text.IndexOf(match.Value);

                // Find the associated property name
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                result.Add(new ParseResult
                {
                    LocalizableString = content,
                    MatchingExpression = _rangeAttributeWithMessageRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{RangePrefix}{propertyName}{min}{max}"
                });
            }

            return result;
        }

        private List<ParseResult> GetCompareAttributesInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matches = _compareAttributeRegex.Matches(text);

            foreach (Match match in matches)
            {
                string compare = match.Groups["compare"].Value;
                int index = text.IndexOf(match.Value);

                // Find the associated property name
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                string errorMessage = $"{StringUtil.InsertSpaces(propertyName)} must match {StringUtil.InsertSpaces(compare)}";
                result.Add(new ParseResult
                {
                    LocalizableString = errorMessage,
                    MatchingExpression = _compareAttributeRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{ComparePrefix}{propertyName}To{compare}"
                });
            }

            return result;
        }

        private List<ParseResult> GetCompareAttributesWithMessageInText(string text)
        {
            List<ParseResult> result = new List<ParseResult>();

            MatchCollection matchesWithMessage = _compareAttributeWithMessageRegex.Matches(text);

            foreach (Match match in matchesWithMessage)
            {
                string compare = match.Groups["compare"].Value;
                string content = match.Groups["content"].Value;
                int index = text.IndexOf(match.Value);

                // Find the associated property name
                Match propertyStringMatch = _propertyRegex.Match(text.Substring(index));
                string propertyName = propertyStringMatch.Groups[ContentGroup].Value;

                result.Add(new ParseResult
                {
                    LocalizableString = content,
                    MatchingExpression = _compareAttributeWithMessageRegex,
                    FilePath = string.Empty,
                    MatchValue = match.Value,
                    Key = $"{ComparePrefix}{propertyName}To{compare}"
                });
            }

            return result;
        }

    }
}
