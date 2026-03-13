# Language Switching

## Change Language at Runtime

```csharp
@inject ILanguageContainerService language

<button @onclick="SetFrench">French</button>
<button @onclick="SetEnglish">English</button>

@code {
    void SetFrench() => language.SetLanguage(CultureInfo.GetCultureInfo("fr-FR"));
    void SetEnglish() => language.SetLanguage(CultureInfo.GetCultureInfo("en-US"));
}
```

## Language Picker Dropdown

Build a dynamic dropdown from registered languages:

```razor
@page "/"
@using System.Globalization

<select @bind="SelectedCulture" @bind:event="onchange">
    @foreach (var culture in Cultures)
    {
        <option value="@culture.Name">@culture.EnglishName</option>
    }
</select>

@code {
    [Inject] private ILanguageContainerService _language { get; set; }

    public IEnumerable<CultureInfo> Cultures { get; set; } = new List<CultureInfo>();

    private string _selectedCulture;
    public string SelectedCulture
    {
        get => _selectedCulture;
        set
        {
            if (_selectedCulture != value)
            {
                _selectedCulture = value;
                _language.SetLanguage(CultureInfo.GetCultureInfo(value));
            }
        }
    }

    protected override void OnInitialized()
    {
        _language.InitLocalizedComponent(this);
        Cultures = _language.RegisteredLanguages;
        _selectedCulture = _language.CurrentCulture.Name;
    }
}
```

## Get Current Culture

```csharp
CultureInfo current = language.CurrentCulture;
```

## List Registered Languages

```csharp
IEnumerable<CultureInfo> languages = language.RegisteredLanguages;
```

## Culture Fallback Order

When a language is set, the library resolves it in this order:

1. Exact culture match (e.g. `fr-FR`)
2. Same language match (e.g. any `fr-*`)
3. English (`en-US`)
4. First available file
