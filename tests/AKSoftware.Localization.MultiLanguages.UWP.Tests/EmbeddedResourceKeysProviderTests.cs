using System.Globalization;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKSoftware.Localization.MultiLanguages.UWP.Tests
{
    [TestClass]
    public class EmbeddedResourceKeysProviderTests : TestBase
    {
       
        public EmbeddedResourceKeysProviderTests()
        {
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly(), "Resources");
            _service = new LanguageContainer(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);
        }
    }
}
