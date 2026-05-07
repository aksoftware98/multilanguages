using System;
using System.Diagnostics;
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
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            // Initialize services on the UI thread before creating the main window
            await RegisterServicesAsync();

            _window = new MainWindow();
            _window.Activate();
        }

        private async System.Threading.Tasks.Task RegisterServicesAsync()
        {
            var serviceCollection = new ServiceCollection();

            bool useEmbeddedResources = false; // Set to true to use embedded resources instead of external files

            bool useExternalFiles = true; // Set to true to use external files for localization

            if (useEmbeddedResources)
            {
                // Register the embedded resource keys provider
                serviceCollection.AddLanguageContainer<EmbeddedResourceKeysProvider>(Assembly.GetExecutingAssembly(), "Resources");
            }
            else if (useExternalFiles)
            {
                var keysProvider = await ExternalFileKeysProvider.CreateAsync(
                 Assembly.GetExecutingAssembly(),
                 @"C:\temp\WinUIAkLocalization\Localization",
                 LocalizationFolderType.ExternalFolder);

                serviceCollection.AddLanguageContainer(keysProvider);
            }
            else
            {
                var isPackaged = IsPackaged();
                var localizationFolderType = isPackaged
                    ? LocalizationFolderType.LocalFolder
                    : LocalizationFolderType.InstallationFolder;

                Debug.WriteLine($"[WinUIAkLocalization] IsPackaged={isPackaged}, LocalizationFolderType={localizationFolderType}, BaseDirectory='{AppContext.BaseDirectory}'");
                Trace.WriteLine($"[WinUIAkLocalization] IsPackaged={isPackaged}, LocalizationFolderType={localizationFolderType}, BaseDirectory='{AppContext.BaseDirectory}'");

                // Initialize ExternalFileKeysProvider asynchronously on the UI thread
                // This ensures WinRT APIs like ApplicationData.Current are accessible
                var keysProvider = await ExternalFileKeysProvider.CreateAsync(
                    Assembly.GetExecutingAssembly(),
                    "Localization",
                    localizationFolderType);

                serviceCollection.AddLanguageContainer(keysProvider);
            }

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private static bool IsPackaged()
        {
            try
            {
                _ = Windows.ApplicationModel.Package.Current;
                Debug.WriteLine("[WinUIAkLocalization] Package.Current succeeded.");
                Trace.WriteLine("[WinUIAkLocalization] Package.Current succeeded.");
                return true;
            }
            catch (InvalidOperationException ex)
            {
                Debug.WriteLine($"[WinUIAkLocalization] Package.Current failed: {ex.Message}");
                Trace.WriteLine($"[WinUIAkLocalization] Package.Current failed: {ex.Message}");
                return false;
            }
        }
    }
}
