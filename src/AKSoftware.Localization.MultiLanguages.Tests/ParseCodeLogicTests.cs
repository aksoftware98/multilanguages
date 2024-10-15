using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests
{
    public class ParseCodeLogicTests
    {
        private ParseCodeLogic _logic = new ParseCodeLogic();

        [Fact]
        public void ParseSpan()
        {
            //Arrange
            string html = "<p><span>No images were found</span></p>";
            string expected = "No images were found";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseAnchor()
        {
            //Arrange
            string html = "<a href=\"/national/donations\">donate today</a>";
            string expected = "donate today";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseAnchorInsideAListItem()
        {
            //Arrange
            string html = "<li><a href=\"/national/donations\">donate today</a></li>";
            string expected = "donate today";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseListItem()
        {
            //Arrange
            string html = "<li>New Pillows</li>";
            string expected = "New Pillows";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseButton()
        {
            //Arrange
            string html = "<button class=\"navbar-toggler navbar-toggler-right\" type=\"button\" data-bs-toggle=\"collapse\" data-bs-target=\"#navbarResponsive\" aria-controls=\"navbarResponsive\" aria-expanded=\"false\" aria-label=\"Toggle navigation\">Menu <i class=\"fas fa-bars ms-1\"></i></button>";
            string expected = "Menu";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseHeading()
        {
            //Arrange
            string html = "<h1>Our Mission</h1>";
            string expected = "Our Mission";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void BuriedImage()
        {
            //Arrange
            string html =
                @"<p style=""text-align: center;""><br></p><p style=""text-align: center;""><a href=""https://www.stjohnsgc.org/""><img src=""/media/grove-city/pages/Partners/StJohnsChurch.png""></a></p>";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(0, result.Count);
        }

        [Fact]
        public void ParseParagraph()
        {
            //Arrange
            string html = "<p>If you like to volunteer with Bed Brigade, please select our Location and complete the Volunteer registration form</p>";
            string expected = "If you like to volunteer with Bed Brigade, please select our Location and complete the Volunteer registration form";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void BuriedSpan()
        {
            string html = @"<div class=""col-12 mb-4 d-none d-md-block wow slideInUp""
	 data-wow-delay=""0.4s"">
	<div><i class=""fas fa-long-arrow-alt-up fa-3x""></i></div>
	<i class=""fas fa-layer-group fa-2x""></i> <span>Bedding</span>
</div>";
            string expected = "Bedding";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void HandleBr()
        {
            string html = @"<p>
2024 – Bed Brigade. All rights reserved.<br />
</p>";
            string expected = "2024 – Bed Brigade. All rights reserved.";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ButtonWithOnClick()
        {
            //Arrange
            string html = @"<FooterTemplate>
	<button class=""btn btn-primary"" @onclick=@(() => Save(context as Donation)) IsPrimary=""true"">Save Donation</button>
</FooterTemplate>";
            string expected = "Save Donation";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }

        [Fact]
        public void ParseCounter_Should_Only_Return_Counter()
        {
            string html = @"@page ""/counter""
@inject ILanguageContainerService Language
<PageTitle>Counter</PageTitle>

<h1>@Language.Keys[""Counter:Title""]</h1>

<p>@Language.Keys[""Counter:Title""]: @currentCount</p>


<button class=""btn btn-primary"" @onclick=""IncrementCount"">@Language.Keys[""Counter:ClickMe""]</button>

@code {
    private int currentCount = 0;

    private void IncrementCount()
    {
        currentCount++;
    }

    protected override void OnInitialized()
    {
        Language.InitLocalizedComponent(this);
        base.OnInitialized();
    }
}";

            string expected = "Counter";

            //Act
            var result = _logic.GetLocalizableStringsInText(html);

            //Assert
            Assert.Equal(1, result.Count);
            Assert.Equal(expected, result[0].LocalizableString);
        }







    }
}
