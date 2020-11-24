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

        string this[string key] { get; }

        /// <summary>
        /// Dictionary of of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">Object that holds the name of the propreties to be replaced with, the object can be of any type</param>
        /// <param name="setEmptyIfNull">Set the behaviour of the null value either to replace it with empty or throw an exception</param>
        /// <returns>Localized value</returns>
        string this[string key, object keyValues, bool setEmptyIfNull = false] { get; }

        /// <summary>
        /// Set a new language explicitly
        /// </summary>
        /// <param name="culture">Culture associated with the langauge to be set</param>
        void SetLanguage(CultureInfo culture);
    }
}
