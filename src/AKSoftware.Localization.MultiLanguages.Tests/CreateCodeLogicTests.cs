using AKSoftware.Localization.MultiLanguages.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class CreateCodeLogicTests
    {
        ILanguageContainerService _languageContainer;
        ICreateCodeLogic _createCodeLogic;
        public CreateCodeLogicTests()
        {
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
            _languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("ca-ES"), keysProvider);
            _createCodeLogic = new CreateCodeLogic(_languageContainer);
        }

        [Fact]
        public void CreateStaticConstantsKeyFile_ShouldCreateFileCorrectly()
        {
            // Arrange
            var namespaceName = "AKSoftware.Localization.MultiLanguages.Tests";
            var className = "TestKeys";
            var filePath = "TestKeys.cs";
            string expectedNamespace = $"namespace {namespaceName}";
            string expectedClassName = $"public static class {className}";
            string expectedConstant = "public const HomePageTitle = \"HomePage:Title\"";

            // Act
            _createCodeLogic.CreateStaticConstantsKeysFile(namespaceName, className, filePath);

            // Assert
            Assert.True(System.IO.File.Exists(filePath));
            var text = System.IO.File.ReadAllText(filePath);
            Assert.Contains(expectedNamespace, text);
            Assert.Contains(expectedClassName, text);
            Assert.Contains(expectedConstant, text);
        }

        [Fact]
        public void CreateEnumKeysFile_ShouldCreateFileCorrectly()
        {
            // Arrange
            var namespaceName = "AKSoftware.Localization.MultiLanguages.Tests";
            var enumName = "TestEnum";
            var filePath = "TestEnum.cs";
            string expectedNamespace = $"namespace {namespaceName}";
            string expectedEnumName = $"public enum {enumName}";
            string expectedValue = "HomePageTitle";
            string expectedDescription = "[Description(\"HomePage:Title\")]";

            // Act
            _createCodeLogic.CreateEnumKeysFile(namespaceName, enumName, filePath);

            // Assert
            Assert.True(System.IO.File.Exists(filePath));
            var text = System.IO.File.ReadAllText(filePath);
            Assert.Contains(expectedNamespace, text);
            Assert.Contains(expectedEnumName, text);
            Assert.Contains(expectedValue, text);
            Assert.Contains(expectedDescription, text);
        }


    }
}
