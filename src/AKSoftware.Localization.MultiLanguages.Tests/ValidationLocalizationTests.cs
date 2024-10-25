using AKSoftware.Localization.MultiLanguages.Providers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using Xunit;
using Xunit.Abstractions;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class ValidationLocalizationTests
    {
        private readonly ITestOutputHelper _output;

        public ValidationLocalizationTests(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public void ValidateModelTest()
        {
            //Arrange
            var customer = new TestClasses.Customer();
            var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
            ILanguageContainerService language = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
            EditContext editContext = new EditContext(customer);
            var validationMessageStore = new ValidationMessageStore(editContext);

            //Act
            ValidationLocalization.ValidateModel(customer, validationMessageStore, language);

            //Assert
            List<string> messages = validationMessageStore[editContext.Field("Name")].ToList();
            Assert.NotEmpty(messages);
            Assert.Equal("Name is required", messages[0]);

        }
    }
}
