# Enum & Static Keys

Access translation keys using enums or static constants instead of raw strings.

## Enum Keys

Define an enum where each value maps to a YAML key. Use `[Description]` to map to hierarchical keys:

```csharp
using System.ComponentModel;

public enum LanguageKeys
{
    [Description("HomePage:Title")]
    HomePageTitle,

    // Without Description, the enum name is used as the key
    FirstName
}
```

Use it:

```razor
<h1>@language[LanguageKeys.HomePageTitle]</h1>
<p>@language[LanguageKeys.FirstName]</p>
```

With interpolation:

```csharp
string result = language[LanguageKeys.HomePageTitle, new { Username = "Ahmad" }];
```

## Generate an Enum Keys File

Auto-generate the enum from your YAML file:

```csharp
var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
var generator = new GenerateStaticKeysService(languageContainer);

generator.CreateEnumKeysFile("MyApp.Localization", "LanguageKeys", @"path\LanguageKeys.cs");
```

## Generate a Static Constants File

Generate a class with `const string` fields:

```csharp
var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
var languageContainer = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
var createCodeLogic = new CreateCodeLogic(languageContainer);

createCodeLogic.CreateStaticConstantsKeysFile("MyApp.Localization", "LanguageKeys", @"path\LanguageKeys.cs");
```

Produces:

```csharp
namespace MyApp.Localization
{
    public static class LanguageKeys
    {
        public const string HomePageTitle = "HomePage:Title";
        public const string FirstName = "FirstName";
    }
}
```

Usage:

```razor
<h1>@language[LanguageKeys.HomePageTitle]</h1>
```

> For the best experience with compile-time safety, consider using the [Source Generator](source-generator.md) instead — it generates a full typed accessor without any manual steps.
