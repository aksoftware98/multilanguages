using NUnit.Framework;
using System.Globalization;
using System.IO;
using System.Reflection;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class LanguagesContainerInAssemblyTests
    {
        [SetUp]
        public void Setup()
        {
            
        }
        
        [Test]
        public void GetGermanyKeyTest()
        {
            ILanguageContainerService _service = new LanguageContainerInAssembly(Assembly.GetExecutingAssembly() , CultureInfo.GetCultureInfo("ca-ES"), "Resources");

            string value = _service.Keys["HomePage:HelloWorld"];
            Assert.AreEqual(value, "Hola món");
        }

    }
}
