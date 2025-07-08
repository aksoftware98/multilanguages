using AKSoftware.Localization.MultiLanguages.Blazor;
using System.Globalization;

namespace BlazorWebApp.Sample.Components.Layout
{
    public partial class NavMenu
    {

        protected override void OnInitialized()
        {
            Language.InitLocalizedComponent(this);
        }

        private string _currentLanguage = "en-US";
        private string _otherLanguage => _currentLanguage == "en-US" ? "Spanish" : "English";

        private void SwitchLanguage()
        {
            _currentLanguage = _currentLanguage == "en-US" ? "es-ES" : "en-US";
            CultureInfo cultureInfo = new CultureInfo(_currentLanguage);
            Language.SetLanguage(cultureInfo);
            StateHasChanged();
        }
    }
}