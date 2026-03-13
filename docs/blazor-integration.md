# Blazor Integration

## Setup

### Blazor WebAssembly

```csharp
// Program.cs
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;

builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
```

### Blazor Server

Use the scoped registration so each user gets their own language state:

```csharp
// Program.cs
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;

builder.Services.AddLanguageContainerForBlazorServer(Assembly.GetExecutingAssembly());
```

Or with a default culture:

```csharp
builder.Services.AddLanguageContainerForBlazorServer(
    Assembly.GetExecutingAssembly(),
    CultureInfo.GetCultureInfo("en-US"));
```

### Blazor Web App (.NET 8+)

```csharp
builder.Services.AddLanguageContainerForBlazorServer(typeof(Program).Assembly);
```

## Imports

Add to `_Imports.razor`:

```csharp
@using AKSoftware.Localization.MultiLanguages
@using AKSoftware.Localization.MultiLanguages.Blazor
```

## Using in Components

Inject the service and call `InitLocalizedComponent` so the component refreshes when the language changes:

```razor
@inject ILanguageContainerService language

<h1>@language["HomePage:Title"]</h1>
<p>@language["HomePage:Welcome"]</p>

@code {
    protected override void OnInitialized()
    {
        language.InitLocalizedComponent(this);
    }
}
```

`InitLocalizedComponent` registers the component so that `StateHasChanged` is called automatically whenever `SetLanguage` is invoked. Without it, the component won't update when the user switches language.
