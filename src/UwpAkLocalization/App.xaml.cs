using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Resources;
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.UWP;
using Microsoft.Extensions.DependencyInjection;

namespace UwpAkLocalization
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application, IServiceProviderHost
    {

        public IServiceProvider ServiceProvider { get; private set; }
        private IServiceCollection serviceCollection_;
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
            RegisterServices();

            CustomXamlResourceLoader.Current = ServiceProvider.GetService<CustomXamlResourceLoader>();
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // When the navigation stack isn't restored navigate to the first page,
                    // configuring the new page by passing required information as a navigation
                    // parameter
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);
                }
                // Ensure the current window is active
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }

        private void RegisterServices()
        {
            serviceCollection_ = new ServiceCollection();
            serviceCollection_.AddSingleton<CustomXamlResourceLoader, ApplicationResourceLoader>();


            //*************************************************************************************************
            // For UWP apps the resources can be Embedded or placed in externals files - either in the app's 
            // installation directory or the in a subfolder of the app's LocalFolder.
            //
            // Unremark the code below to test the scenarios'.  Note that when you use the LocalFolder option
            // you will need to manually create a folder under the app's LocalFolder location and copy all of 
            // language resource files, prior to running the app.  In this case the app will be installed
            // in this location:
            //     C:\Users\<your user>\AppData\Local\Packages\e8428150-51ff-4bd2-8842-7dbd0047d3da_3xecenf62363c\LocalState.  
            //  
            //*************************************************************************************************
            //serviceCollection_.AddLanguageContainer<EmbeddedResourceKeysProvider>(Assembly.GetExecutingAssembly(),  "Resources");
            //serviceCollection_.AddLanguageContainer<ExternalFileKeysProvider>(Assembly.GetExecutingAssembly(), LocalizationFolderType.InstallationFolder, "Resources");
            serviceCollection_.AddLanguageContainer<ExternalFileKeysProvider>(Assembly.GetExecutingAssembly(), LocalizationFolderType.LocalFolder, "Localization");
            ServiceProvider = serviceCollection_.BuildServiceProvider();
        }
    }
}
