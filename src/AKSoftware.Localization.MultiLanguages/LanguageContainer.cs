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

        public string this[string key] => Keys[key];

        public string this[string key, object keyValues, bool setEmptyIfNull = false] => Keys[(string)key, keyValues, (bool)setEmptyIfNull];

        public string this[string key, IDictionary<string, object> keyValues, bool setEmptyIfNull] => Keys[(string)key, keyValues, (bool)setEmptyIfNull];

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
            //NB:  Not sure if this should be called here...
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

        // TODO: Remove the destroyed component from the extensions list 
        private void InvokeExtensions()
        {
            if (_extensions.Any())
            {
                foreach (var item in _extensions.ToArray())
                {
                    if (item.Component == null)
                    {
                        _extensions.Remove(item);
                        continue;
                    }
                    item.Action.Invoke(item.Component);
                }
            }
        }

        private readonly List<IExtension> _extensions = null;
        public void AddExtension(IExtension extension)
        {
            // Add the extension if it is not exists 
            var value = _extensions.SingleOrDefault(r => r == extension);
            if (value == null)
                _extensions.Add(extension);
        }

    }
}