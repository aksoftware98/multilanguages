using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages
{
    public static class StringUtil
    {
        public static string ProperCase(string input)
        {
            if (string.IsNullOrEmpty(input) || IsMixedCase(input))
            {
                // Return the input as is if it's already in mixed case
                return input;
            }

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            return textInfo.ToTitleCase(input.ToLower());
        }

        private static bool IsMixedCase(string input)
        {
            bool hasUpper = false;
            bool hasLower = false;

            foreach (char c in input)
            {
                if (char.IsUpper(c))
                    hasUpper = true;
                else if (char.IsLower(c))
                    hasLower = true;

                // If both upper and lower case characters are found, it's mixed case
                if (hasUpper && hasLower)
                    return true;
            }

            return false;
        }

        public static string FilterAlphaNumeric(string input)
        {
            var sb = new StringBuilder(input.Length);
            foreach (char c in input)
            {
                if (char.IsLetterOrDigit(c))
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// Insert spaces into a string 
        /// </summary>
        /// <example>
        /// OrderDetails = Order Details
        /// 10Net30 = 10 Net 30
        /// FTPHost = FTP Host
        /// </example> 
        /// <param name="input"></param>
        /// <returns></returns>
        public static string InsertSpaces(string input)
        {
            const string space = " ";
            bool isSpace = false;
            bool isUpperOrNumber = false;
            bool isLower = false;
            bool isLastUpper = true;
            bool isNextCharLower = false;

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            StringBuilder sb = new StringBuilder(input.Length + input.Length / 2);

            //Replace underline with spaces
            input = input.Replace("_", space);
            input = input.Replace("-", space);
            input = input.Replace("  ", space);

            //Trim any spaces
            input = input.Trim();

            char[] chars = input.ToCharArray();

            sb.Append(chars[0]);

            for (int i = 1; i < chars.Length; i++)
            {
                isUpperOrNumber = chars[i] >= 'A' && chars[i] <= 'Z' || chars[i] >= '0' && chars[i] <= '9';
                isNextCharLower = i < chars.Length - 1 && chars[i + 1] >= 'a' && chars[i + 1] <= 'z';
                isSpace = chars[i] == ' ';
                isLower = chars[i] >= 'a' && chars[i] <= 'z';

                //There was a space already added
                if (isSpace)
                {
                }
                //Look for upper case characters that have lower case characters before
                //Or upper case characters where the next character is lower
                else if (isUpperOrNumber && isLastUpper == false
                    || isUpperOrNumber && isNextCharLower && isLastUpper == true)
                {
                    sb.Append(space);
                    isLastUpper = true;
                }
                else if (isLower)
                {
                    isLastUpper = false;
                }

                sb.Append(chars[i]);

            }

            //Replace double spaces
            sb.Replace("  ", space);

            return sb.ToString();
        }
    }
}
