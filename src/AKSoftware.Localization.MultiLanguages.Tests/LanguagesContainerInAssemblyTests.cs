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
            ILanguageContainerService _service = new LanguageContainerInAssembly(Assembly.GetExecutingAssembly() , CultureInfo.GetCultureInfo("de-DE"));

            string value = _service.Keys["firstname"].ToLower();
            Assert.AreEqual(value, "vorname");
        }

        [Test]
        public void GetArabicKeyTest()
        {
            ILanguageContainerService _service = new LanguageContainerInAssembly(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("ar-SA"));

            string value = _service.Keys["firstname"].ToLower();
            Assert.AreEqual(value, "الاسم الأول");
        }

        [Test]
        public void InitializeWithNotSpecifiedCultureTest()
        {
            ILanguageContainerService _service = new LanguageContainerInAssembly(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("ar-TG"));

            string value = _service.Keys["firstname"].ToLower();
            Assert.AreEqual(value, "الاسم الأول");
        }

        [Test]
        public void SetNotSpecifiedCultureTest()
        {
            ILanguageContainerService _service = new LanguageContainerInAssembly(Assembly.GetExecutingAssembly(), CultureInfo.GetCultureInfo("ar-SA"));
            _service.SetLanguage(CultureInfo.GetCultureInfo("fr-FR"));
            string value = _service.Keys["firstname"].ToLower();
            Assert.AreEqual(value, "الاسم الأول");
        }
    }
}
