using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKSoftware.Localization.MultiLanguages.UWP.Tests
{
    [TestClass]
    public class ExternalFileKeysProviderTests : TestBase
    {
       
        public ExternalFileKeysProviderTests()
        {
            //*************************************************************************************************
            // For UWP apps the resources can be Embedded or placed in externals files - either in the app's 
            // installation directory or the in a subfolder of the app's LocalFolder.
            //
            // Unremark the code below to test the scenarios'.  Note that when you use the LocalFolder option
            // you will need to manually create a folder under the app's LocalFolder location and copy all of 
            // language resource files, prior to running the app.  In this case the app will be installed
            // in this location:
            //     C:\Users\<your user>\AppData\Local\Packages\aaab2d02-8d02-4b44-b948-1d2c8fa9138a_3xecenf62363c\LocalState.  
            //  
            //*************************************************************************************************
            //var keysProvider = new ExternalFileKeysProvider(Assembly.GetExecutingAssembly(), "Resources", LocalizationFolderType.InstallationFolder);
            var keysProvider = new ExternalFileKeysProvider(Assembly.GetExecutingAssembly(), "Localization", LocalizationFolderType.LocalFolder);
            _service = new LanguageContainer(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);
        }
    }
}
