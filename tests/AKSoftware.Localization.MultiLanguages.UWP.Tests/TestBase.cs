using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AKSoftware.Localization.MultiLanguages.UWP.Tests
{
    public abstract class TestBase
    {
        protected ILanguageContainerService _service;

        [TestMethod]
        public void Interpolation_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            string value = _service.Keys["HomePage:Login", new
            {
                Username = "AK Academy"
            }];
            Assert.AreEqual(value, "Welcome AK Academy to the system");
        }

        [TestMethod]
        public void Interpolation_Multi_Replacement_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            string value = _service.Keys["HomePage:Hello", new
            {
                FirstName = "AK",
                LastName = "Academy"
            }];
            Assert.AreEqual(value, "Hello AK Academy");
        }

        [TestMethod]
        public void Interpolation_Multi_Replacement_With_Dictionary_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            var replacements = new Dictionary<string, object> { ["FirstName"] = "AK", ["LastName"] = "Academy" };
            string value = _service.Keys["HomePage:Hello", replacements];
            Assert.AreEqual(value, "Hello AK Academy");
        }

        [TestMethod]
        public void Interpolation_Multi_Replacement_With_Expando_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));

            var expando = new ExpandoObject();
            var replacements = (IDictionary<string, object>)expando;
            replacements["FirstName"] = "AK";
            replacements["LastName"] = "Academy";

            string value = _service.Keys["HomePage:Hello", replacements];
            Assert.AreEqual(value, "Hello AK Academy");
        }

        [TestMethod]
        public void Interpolation_With_ExpandoObject_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));

            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>)expando;
            dictionary["Username"] = "AK Academy";

            string value = _service.Keys["HomePage:Login", expando];
            Assert.AreEqual(value, "Welcome AK Academy to the system");
        }

        [TestMethod]
        public void Interpolation_With_Dictionary_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));


            var dictionary = new Dictionary<string, object> { ["Username"] = "AK Academy" };
            var value = _service.Keys["HomePage:Login", dictionary];
            Assert.AreEqual(value, "Welcome AK Academy to the system");
        }

        [TestMethod]
        public void Get_Value_By_Composite_Key()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("ca-ES"));
            string value = _service.Keys["HomePage:HelloWorld"];
            Assert.AreEqual(value, "Hola món");
        }

        [TestMethod]
        public void Get_Value_By_Composite_Key_By_Index()
        {
            var value = _service["HomePage:HelloWorld"];
            Assert.AreEqual(value, "Hola món");
        }

        [TestMethod]
        public void Get_Value_By_Key_By_Index()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("ca-ES"));
            var value = _service["MerryChristmas"];
            Assert.AreEqual(value, "Feliz Navidad!");
        }
    }



}
