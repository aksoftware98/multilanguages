using AKSoftware.Localization.MultiLanguages.Providers;
using AKSoftware.Localization.MultiLanguages.WPF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace WpfSampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App: Application
    {
        public static readonly WpfLanguageContainer LanguageContainer
             = new(new("en-us"), new FolderResourceKeysProvider("Resources"));
    }
}
