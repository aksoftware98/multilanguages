using System;
using System.Reflection;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;
using AKSoftware.Localization.MultiLanguages.WinUI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;

namespace WinUIAkLocalization
{
    public partial class App : Application, IServiceProviderHost
    {
        public IServiceProvider ServiceProvider { get; private set; }

        private Window _window;

        public App()
        {
            InitializeComponent();
            RegisterServices();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            _window = new MainWindow();
            _window.Activate();
        }

        private void RegisterServices()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLanguageContainer<EmbeddedResourceKeysProvider>(Assembly.GetExecutingAssembly(), "Resources");
            ServiceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
