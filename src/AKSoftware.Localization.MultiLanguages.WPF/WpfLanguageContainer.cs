using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKSoftware.Localization.MultiLanguages.WPF
{
    public class WpfLanguageContainer: LanguageContainer, INotifyPropertyChanged
    {
        public WpfLanguageContainer(IKeysProvider keysProvider) : base(keysProvider) { }

        public WpfLanguageContainer(CultureInfo culture, IKeysProvider keysProvider) : base(culture, keysProvider)
        { }

        public override void SetLanguage(CultureInfo culture)
        {
            base.SetLanguage(culture);
            OnPropertyChanged("");
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}