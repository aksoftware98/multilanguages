using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AKSoftware.Localization.MultiLanguages
{
    public interface ILanguageContainerService
    {
        /// <summary>
        /// Current culture assocaited with the selected language
        /// </summary>
        CultureInfo CurrentCulture { get; }

        /// <summary>
        /// Dictionary of the language keywords
        /// </summary>
        Keys Keys { get; }

        /// <summary>
        /// Set a new language explicitly
        /// </summary>
        /// <param name="culture">Culture associated with the langauge to be set</param>
        void SetLanguage(CultureInfo culture);
    }
}
