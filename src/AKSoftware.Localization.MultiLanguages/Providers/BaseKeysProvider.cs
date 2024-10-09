using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace AKSoftware.Localization.MultiLanguages.Providers
{
    public abstract class BaseKeysProvider : IKeysProvider
    {
        protected Regex YamlFilePattern =
            new Regex(
                @"^([A-Za-z]{2}\-[A-Za-z]+\-[A-Za-z]{2}\.yml)|([A-Za-z]{2}\-[A-Za-z]{2}\.yml)|([A-Za-z]{2}\.yml)$",
                RegexOptions.Compiled);

        /// <summary>
        /// Retrieve an instance of <see cref="Keys"/> from a <see cref="CultureInfo"/> object
        /// </summary>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public abstract Keys GetKeys(CultureInfo cultureInfo);

        /// <summary>
        /// Retrieves an instance of <see cref="Keys"/> from the name of the culture, the name of the culture should be in the format of "en-US"
        /// </summary>
        /// <param name="cultureName"></param>
        /// <returns></returns>
        public abstract Keys GetKeys(string cultureName);

        /// <summary>
        /// Get a list of the registered languages
        /// </summary>
        public abstract IEnumerable<CultureInfo> RegisteredLanguages { get; }
    }
}
