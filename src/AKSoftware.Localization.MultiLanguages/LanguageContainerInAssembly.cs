using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace AKSoftware.Localization.MultiLanguages
{
    public class LanguageContainerInAssembly : ILanguageContainerService
    {

        private readonly IKeysProvider _keysProvider;

        /// <summary>
        /// Create instance of the container initialized with the specific culture
        /// </summary>
        /// <param name="culture"></param>
        /// <param name="keyProvider"></param>
        public LanguageContainerInAssembly(CultureInfo culture, IKeysProvider keyProvider) : this(keyProvider)
        {
            SetLanguage(culture, true);
        }

        /// <summary>
        /// Create instance of the container initialized with the default culture
        /// </summary>
        public LanguageContainerInAssembly(IKeysProvider keysProvider)
        {
            _keysProvider = keysProvider;
            _extensions = new List<WeakReference<IExtension>>();
            SetLanguage(CultureInfo.CurrentCulture, true);
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
        /// <exception cref="FileNotFoundException">If there is no culture file exists in the resources folder</exception>
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
        /// <exception cref="FileNotFoundException">If the required culture language file is not exist</exception>
        public void SetLanguage(CultureInfo culture)
        {
            CurrentCulture = culture;
            Keys = _keysProvider.GetKeys(culture.Name);
            InvokeExtensions();
        }

        private void InvokeExtensions()
        {
            if (_extensions.Any())
            {
                foreach (var item in _extensions)
                {
                    var result = item.TryGetTarget(out var extension);
                    if (result)
                        extension.Action.Invoke(extension.Component);
                }
            }
        }

        #region Extensions 
        private readonly List<WeakReference<IExtension>> _extensions = null;

        public void AddExtension(IExtension extension)
        {
            // Add the extension if it is not exists 
            var value = _extensions.SingleOrDefault(r => r.TryGetTarget(out var e) && e == extension);
            if (value == null)
                _extensions.Add(new WeakReference<IExtension>(extension));
        }
        #endregion 
    }
}

