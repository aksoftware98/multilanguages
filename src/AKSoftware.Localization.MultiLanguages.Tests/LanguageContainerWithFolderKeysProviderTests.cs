﻿using AKSoftware.Localization.MultiLanguages.Providers;
using FluentAssertions.Common;
using System.Globalization;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
	public class LanguageContainerWithFolderKeysProviderTests
    {
        private IKeysProvider _keysProvider;
        private ILanguageContainerService _languageContainer; 
		
		public LanguageContainerWithFolderKeysProviderTests()
        {
            var path = "Resources";
            _keysProvider = new FolderResourceKeysProvider(path);
            _languageContainer = new LanguageContainer(_keysProvider);
        }

        [Fact]
		public void GetKeyFromFolderResource_ShouldReturnItCorrectly()
        {
            // Arrange 
            var key = "HomePage:HelloWorld";

            // Act 
            var value = _languageContainer[key];

            // Assert
            Assert.Equal("Hello World", value);
        }

        [Fact]
        public void GetNotExistedKeyFromFolderResource_ShouldReturnTheKeyItself()
        {
			// Arrange 
			var key = "NotFoundKey";

			// Act 
			var value = _languageContainer[key];

			// Assert
			Assert.Equal("NotFoundKey", value);
		}


		[Fact]
		public void GetNotExistedNestedKeyFromFolderResource_ShouldReturnTheKeyItself()
		{
			// Arrange 
			var key = "NotFoundKey:Hello";

			// Act 
			var value = _languageContainer[key];

			// Assert
			Assert.Equal("NotFoundKey:Hello", value);
		}

		[Fact]
		public void ChangeLanguage_ShouldChangeItSuccessfully()
        {
            // Arrange 
            var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), _keysProvider);
            var key = "HomePage:HelloWorld"; 
			
            var firstValue = languageContainer[key];
            Assert.Equal("Hello World", firstValue);

            // Change the language 
            languageContainer.SetLanguage(CultureInfo.GetCultureInfo("ca-ES"));
            var secondValue = languageContainer[key];
            Assert.Equal("Hola món", secondValue);
        }

        [Fact]
        public void Should_Be_Able_To_Get_Registered_Languages()
        {
            //Arrange
            var expectedLanguage = "ca-ES";

            //Act
            var languages = _languageContainer.RegisteredLanguages;

            //Assert
            Assert.Contains(CultureInfo.GetCultureInfo(expectedLanguage), languages);
        }
    }
}
