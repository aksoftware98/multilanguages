using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AKSoftware.Localization.MultiLanguages
{
    public class LanguageContainerFromExternalFile : ILanguageContainerService
    {
        private string _folderName;

        public CultureInfo CurrentCulture { get; set; }
        public Keys Keys { get; }

        public string this[string key] => Keys[key];

        public string this[string key, object keyValues, bool setEmptyIfNull = false] => Keys[key, keyValues, setEmptyIfNull];

        public LanguageContainerFromExternalFile(CultureInfo culture, string folderName)
        {
            _folderName = folderName.Replace("/", ".").Replace("\\", ".");
            _extensions = new List<WeakReference<IExtension>>();
            SetLanguage(culture, true);
        }

        /// <summary>
        /// Create instance of the container that languages exists in a specific folder, initialized with the default culture
        /// </summary>
        /// <param name="folderName">Folder that contains the language files</param>
        public LanguageContainerFromExternalFile(string folderName)
        {
            _folderName = folderName.Replace("/", ".").Replace("\\", ".");
            _extensions = new List<WeakReference<IExtension>>();
            SetLanguage(CultureInfo.CurrentCulture, true);
        }

        private void SetLanguage(CultureInfo culture, bool isDefault)
        {
            CurrentCulture = culture;
        }

        public void SetLanguage(CultureInfo culture)
        {
            CurrentCulture = culture;
        }

        private readonly List<WeakReference<IExtension>> _extensions = null;

        public void AddExtension(IExtension extension)
        {
            // Add the extension if it does not exists 
            var value = _extensions.SingleOrDefault(r => r.TryGetTarget(out var e) && e == extension);
            if (value == null)
            {
                _extensions.Add(new WeakReference<IExtension>(extension));
            }
               
        }
    }
}