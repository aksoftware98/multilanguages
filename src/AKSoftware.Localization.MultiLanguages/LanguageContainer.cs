using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AKSoftware.Localization.MultiLanguages.Providers;

namespace AKSoftware.Localization.MultiLanguages
{
    public class LanguageContainer : ILanguageContainerService
    {

        private readonly IKeysProvider _keysProvider;
        private readonly object _extensionsLock = new object();

        /// <summary>
        /// Create instance of the container initialized with the specific culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="keysProvider"></param>
        public LanguageContainer(CultureInfo culture, IKeysProvider keysProvider)
        {
            _keysProvider = keysProvider;
            _extensions = new List<IExtension>();
            SetLanguage(culture, true);
        }

        /// <summary>
        /// Create instance of the container initialized with the default culture
        /// </summary>
        public LanguageContainer(IKeysProvider keysProvider) : this(CultureInfo.CurrentCulture, keysProvider)
        {
        }

        /// <summary>
        /// Keys of the language values
        /// </summary>
        public Keys Keys { get; private set; }

        /// <summary>
        /// Current Culture related to the selected language
        /// </summary>
        public CultureInfo CurrentCulture { get; private set; }

        /// <summary>
        /// Get the translation of the string key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Localized value</returns>
        public string this[string key] => Keys[key];

        /// <summary>
        /// Get the translation of the enum key
        /// </summary>
        /// <param name="key"></param>
        /// <returns>Localized value</returns>
        public string this[Enum key] => Keys[key];

        /// <summary>
        /// Dictionary of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">Object that holds the name of the properties to be replaced with, the object can be of any type</param>
        /// <param name="setEmptyIfNull">Set the behavior of the null value either to replace it with empty or throw an exception</param>
        /// <returns>Localized value</returns>
        public string this[string key, object keyValues, bool setEmptyIfNull = false] => Keys[(string)key, keyValues, (bool)setEmptyIfNull];

        /// <summary>
        /// Dictionary of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">Object that holds the name of the properties to be replaced with, the object can be of any type</param>
        /// <param name="setEmptyIfNull">Set the behavior of the null value either to replace it with empty or throw an exception</param>
        /// <returns>Localized value</returns>
        public string this[Enum key, object keyValues, bool setEmptyIfNull] => Keys[key, keyValues, setEmptyIfNull];

        /// <summary>
        /// Dictionary of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">IDictionary that holds the name of the properties to be replaced with</param>
        /// <param name="setEmptyIfNull">Set the behavior of the null value either to replace it with empty or throw an exception</param>
        /// <returns>Localized value</returns>
        public string this[string key, IDictionary<string, object> keyValues, bool setEmptyIfNull] => Keys[(string)key, keyValues, (bool)setEmptyIfNull];

        /// <summary>
        /// Dictionary of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">IDictionary that holds the name of the properties to be replaced with</param>
        /// <param name="setEmptyIfNull">Set the behavior of the null value either to replace it with empty or throw an exception</param>
        /// <returns>Localized value</returns>
        public string this[Enum key, IDictionary<string, object> keyValues, bool setEmptyIfNull] =>
            Keys[key, keyValues, setEmptyIfNull];

        /// <summary>
        /// Dictionary of the keyword for a value that holds variables to be replaced for example (Welcome {{firstname}} to the system), you can replace the firstname placeholder with any value you pass to it
        /// </summary>
        /// <param name="key">Key of the value</param>
        /// <param name="keyValues">Object that holds the name of the properties to be replaced with, the object can be of any type</param>
        /// <returns>Localized value</returns>
        public string this[object key, object keyValues] => Keys[(string)key, keyValues];


        /// <summary>
        /// Set language manually based on a specific culture
        /// </summary>
        /// <param name="culture">The required culture</param>
        /// <param name="isDefault">To indicates if this is the initial function</param>
        private void SetLanguage(CultureInfo culture, bool isDefault)
        {
            CurrentCulture = culture;
            Keys = _keysProvider.GetKeys(culture);

            InvokeExtensions();
        }

        /// <summary>
        /// Set language manually based on a specific culture
        /// </summary>
        /// <param name="culture">The required culture</param>
        public void SetLanguage(CultureInfo culture)
        {
            CurrentCulture = culture;
            Keys = _keysProvider.GetKeys(culture.Name);
            InvokeExtensions();
        }

        private void InvokeExtensions()
        {
            IExtension[] snapshot;
            lock (_extensionsLock)
            {
                snapshot = _extensions.ToArray();
            }

            var toRemove = new List<IExtension>();
            foreach (var item in snapshot)
            {
                if (item.Component == null)
                {
                    toRemove.Add(item);
                    continue;
                }
                item.Action.Invoke(item.Component);
            }

            if (toRemove.Count > 0)
            {
                lock (_extensionsLock)
                {
                    foreach (var item in toRemove)
                        _extensions.Remove(item);
                }
            }
        }

        private readonly List<IExtension> _extensions = null;

        public void AddExtension(IExtension extension)
        {
            lock (_extensionsLock)
            {
                var value = _extensions.SingleOrDefault(r => r == extension);
                if (value == null)
                    _extensions.Add(extension);
            }
        }

        public void RemoveExtension(IExtension extension)
        {
            lock (_extensionsLock)
            {
                _extensions.Remove(extension);
            }
        }

        /// <summary>
        /// Get a list of the keys in the language file
        /// </summary>
        /// <returns></returns>
        public List<string> GetKeys()
        {
            var result = new List<string>();
            FlattenKeysRecursive(Keys.KeyValues, "", result);
            return result;
        }

        private void FlattenKeysRecursive(object obj, string prefix, List<string> result)
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
                        result.Add(newPrefix);
                    }
                }
            }
            else if (!string.IsNullOrEmpty(prefix))
            {
                result.Add(prefix);
            }
        }

        /// <summary>
        /// Get a list of the registered languages
        /// </summary>
        public IEnumerable<CultureInfo> RegisteredLanguages => _keysProvider.RegisteredLanguages;
    }
}
