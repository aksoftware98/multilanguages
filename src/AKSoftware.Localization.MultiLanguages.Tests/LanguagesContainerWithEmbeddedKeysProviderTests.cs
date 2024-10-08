﻿using System;
using AKSoftware.Localization.MultiLanguages.Providers;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Reflection;
using Xunit;
using FluentAssertions;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
	public class LanguagesContainerWithEmbeddedKeysProviderTests
    {
        ILanguageContainerService _service;

        public LanguagesContainerWithEmbeddedKeysProviderTests()
        {
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
            _service = new LanguageContainer(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);

        }
		
        [Fact]
        public void ChangeLanguage_should_change_the_language_correctly()
        {
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
            _service = new LanguageContainer(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);
            var targetKey = "HomePage:HelloWorld";

            Assert.Equal("Hola món", _service[targetKey]);

            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));

            Assert.Equal("Hello World", _service[targetKey]);
        }

        [Fact]
        public void Interpolation_With_Null_Value_Should_Replace_With_Empty_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            string value = _service.Keys["HomePage:Login", new
            {
                Username = (string)null
            }, true];
            Assert.Equal("Welcome  to the system", value); 
        }

        [Fact]
        public void Interpolation_Multi_Replacement_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            string value = _service.Keys["HomePage:Hello", new
            {
                FirstName = "AK",
                LastName = "Academy"
            }];
            Assert.Equal("Hello AK Academy", value);
        }

        [Fact]
        public void Interpolation_Multi_Replacement_With_Dictionary_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
            var replacements = new Dictionary<string, object> {["FirstName"] = "AK", ["LastName"] = "Academy"};
            string value = _service.Keys["HomePage:Hello", replacements];
            Assert.Equal("Hello AK Academy", value);
        }

        [Fact]
        public void Interpolation_Multi_Replacement_With_Expando_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));

            var expando = new ExpandoObject();
            var replacements = (IDictionary<string, object>)expando;
            replacements["FirstName"] = "AK";
            replacements["LastName"] = "Academy";

            string value = _service.Keys["HomePage:Hello", replacements];
            Assert.Equal("Hello AK Academy", value);
        }

        [Fact]
        public void Interpolation_With_ExpandoObject_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));

            var expando = new ExpandoObject();
            var dictionary = (IDictionary<string, object>) expando;
            dictionary["Username"] = "AK Academy";

            string value = _service.Keys["HomePage:Login", expando];
            Assert.Equal("Welcome AK Academy to the system", value);
        }

        [Fact]
        public void Interpolation_With_Dictionary_Test()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("en-US"));


            var dictionary = new Dictionary<string, object> {["Username"] = "AK Academy"};
            var value = _service.Keys["HomePage:Login", dictionary];
            Assert.Equal("Welcome AK Academy to the system", value);
        }

        [Fact]
        public void Get_Value_By_Composite_Key()
        {
            _service.SetLanguage(CultureInfo.GetCultureInfo("ca-ES"));
            string value = _service.Keys["HomePage:HelloWorld"];
            Assert.Equal("Hola món", value);
        }

        [Fact]
        public void Get_Value_By_Composite_Key_By_Index()
        {
            var value = _service["HomePage:HelloWorld"];
            Assert.Equal("Hola món", value);
        }

        [Fact]
        public void Get_Value_By_Key_By_Index()
        {
            var value = _service["MerryChristmas"];
            Assert.Equal("Feliz Navidad!", value);
        }

        [Fact]
		public void GetNestedValue_When_NestedKey_NotFound_Should_Return_NestedKeyOnly()
        {
			var value = _service["HomePage:NotFound"];
			Assert.Equal("NotFound", value);
		}

        [Fact]
        public void Should_Be_Able_To_Get_Keys()
        {
            //Arrange
            string expectedKey = "MerryChristmas";

            //Act
            var keys = _service.GetKeys();

            //Assert
            Assert.Contains(expectedKey, keys);
        }

        [Fact]
        public void Should_Be_Able_To_Enumerate()
        {
            //Arrange
            string expectedKey = "MerryChristmas";
            bool found = false;

            //Act
            foreach (KeyValuePair<object, object> keyValue in _service.Keys)
            {
                if (keyValue.Key.ToString() == expectedKey)
                {
                    found = true;
                    break;
                }
            }

            //Assert
            Assert.True(found);
        }

        [Fact]
        public void Enumerate_Should_Handle_Nested_Keys_Gracefully()
        {
            var keys = new List<string>();

            foreach (var item in _service.Keys)
            {
                keys.Add(item.Key.ToString());
            }

            keys.Should().HaveCount(20);
            keys[0].Should().Be("HomePage:Title");
            keys.Should().ContainEquivalentOf("MerryChristmas");
            keys.Should().ContainEquivalentOf("Contacts:Address:City");
        }
    }
}
