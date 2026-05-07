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
            var assembly = Assembly.GetExecutingAssembly();
            const string localOrInstalledFolderName = "Localization";
            const string externalFolderPath = @"C:\temp\WinUIAkLocalization\Localization";
            var localizationSource = LocalizationSource.ExternalFolder;

            // End users must copy the YAML files from this sample project's 'Localization' folder when the selected
            // source reads from a writable runtime location rather than from packaged app content. For
            // LocalizationSource.UwpStyleLocalFolder, copy them to LocalState\Localization. For
            // LocalizationSource.ExternalFolder, copy them to the configured fully qualified path. For
            // LocalizationSource.AppManagedDefault, copy them only when the app runs packaged because that mode maps
            // packaged apps to LocalState\Localization; unpackaged apps use the installation folder instead.
            IKeysProvider keysProvider;

            switch (localizationSource)
            {
                case LocalizationSource.EmbeddedResources:
                    keysProvider = new EmbeddedResourceKeysProvider(assembly, "Resources");
                    break;

                case LocalizationSource.UwpStyleLocalFolder:
                    keysProvider = await ExternalFileKeysProvider.CreateFromLocalFolderAsync(assembly, localOrInstalledFolderName);
                    break;

                case LocalizationSource.InstallationFolder:
                    keysProvider = await ExternalFileKeysProvider.CreateFromInstallationFolderAsync(assembly, localOrInstalledFolderName);
                    break;

                case LocalizationSource.ExternalFolder:
                    keysProvider = await ExternalFileKeysProvider.CreateFromExternalFolderAsync(externalFolderPath);
                    break;

                case LocalizationSource.AppManagedDefault:
                    var isPackaged = IsPackaged();
                    Debug.WriteLine($"[WinUIAkLocalization] IsPackaged={isPackaged}, BaseDirectory='{AppContext.BaseDirectory}'");
                    Trace.WriteLine($"[WinUIAkLocalization] IsPackaged={isPackaged}, BaseDirectory='{AppContext.BaseDirectory}'");

                    if (isPackaged)
                    {
                        keysProvider = await ExternalFileKeysProvider.CreateFromLocalFolderAsync(assembly, localOrInstalledFolderName);
                    }
                    else
                    {
                        keysProvider = await ExternalFileKeysProvider.CreateFromInstallationFolderAsync(assembly, localOrInstalledFolderName);
                    }

                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            serviceCollection.AddLanguageContainer(keysProvider);
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

        private enum LocalizationSource
        {
            EmbeddedResources,
            UwpStyleLocalFolder,
            InstallationFolder,
            ExternalFolder,
            AppManagedDefault
        }
    }
}
