using NUnit.Framework;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class LanguagesContainerInAssemblyTests
    {
        ILanguageContainerService _service;

        [SetUp]
        public void Setup()
        {
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly(), "Resources");
            _service = new LanguageContainerInAssembly(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);

        }

        [Test]
        public void Interpolation_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            string value = _service.Keys["HomePage:Login", new
            {
                Username = "AK Academy"
            }];
            Assert.AreEqual(value, "Welcome AK Academy to the system"); 
        }

        [Test]
        public void Get_Value_By_Composite_Key()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("ca-ES"));
            string value = _service.Keys["HomePage:HelloWorld"];
            Assert.AreEqual(value, "Hola món");
        }

        [Test]
        public void Get_Value_By_Composite_Key_By_Index()
        {
            var value = _service["HomePage:HelloWorld"];
            Assert.AreEqual(value, "Hola món");
        }

        [Test]
        public void Get_Value_By_Key_By_Index()
        {
            var value = _service["MerryChristmas"];
            Assert.AreEqual(value, "Feliz Navidad!");
        }

    }
}
