# AKSoftware.Localization.MultiLanguages

[![Build Status](https://aksoftware98.visualstudio.com/AkMultiLanguages/_apis/build/status/aksoftware98.multilanguages?branchName=master)](https://aksoftware98.visualstudio.com/AkMultiLanguages/_build/latest?definitionId=4&branchName=master)
![Nuget](https://img.shields.io/nuget/dt/AKSoftware.Localization.MultiLanguages?color=nuget&label=Nuget&style=plastic)

<img width="100" src="https://github.com/aksoftware98/multilanguages/blob/master/src/AKSoftware.Localization.MultiLanguages/AkMultiLanguages.png?raw=true" />

The most advanced .NET localization library — light, fast, and simple. Uses YAML files instead of heavy XML .resx files.

Works with **Blazor (Server & WASM)**, **ASP.NET Core**, **UWP**, **WPF**, and **Console** apps.

Built with love by [Ahmad Mozaffar](http://ahmadmozaffar.net)

## Why YAML over .resx?

- **Smaller files** — no XML boilerplate
- **Faster parsing** — YAML is lightweight
- **Nested keys** — organize with hierarchy instead of long concatenated names
- **Easy automation** — simple structure enables source generators and code gen

## Features at a Glance

| Feature | Description |
|---------|-------------|
| **YAML Resources** | Light, fast `.yml` files instead of `.resx` |
| **Hierarchical Keys** | Nested keys with `:` separator (`HomePage:Title`) |
| **String Interpolation** | `{placeholder}` values replaced at runtime |
| **Source Generator** | Strongly-typed `IKeysAccessor` — no magic strings |
| **Blazor State Management** | Components auto-refresh on language change |
| **Validation Localization** | Localized DataAnnotation validation messages |
| **Multiple Providers** | Load from embedded resources or file system |
| **Code Generation** | Auto-generate resource files, static classes, enums |
| **Verification Tests** | Detect missing keys, unused keys, duplicates |
| **Online Translator** | [Translate to 69 languages](https://akmultilanguages.azurewebsites.net) in one click |

## Quick Start

```
dotnet add package AKSoftware.Localization.MultiLanguages
```

1. Create a `Resources` folder in your project
2. Add a `en-US.yml` file:

```yaml
HelloWorld: Hello World
Welcome: Welcome {username}
```

3. Set the file's build action to **EmbeddedResource**
4. Use it:

```csharp
builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
```

```razor
@inject ILanguageContainerService language

<h1>@language["HelloWorld"]</h1>
```

## Packages

| Package | Purpose |
|---------|---------|
| `AKSoftware.Localization.MultiLanguages` | Core library |
| `AKSoftware.Localization.MultiLanguages.Blazor` | Blazor component state management |
| `AKSoftware.Localization.MultiLanguages.SourceGenerator` | Strongly-typed key accessor generation |

## Documentation

Full documentation for each feature:

| Guide | Description |
|-------|-------------|
| [Getting Started](docs/getting-started.md) | Installation, setup, and first steps |
| [Blazor Integration](docs/blazor-integration.md) | Blazor Server & WebAssembly setup |
| [Language Switching](docs/language-switching.md) | Change languages at runtime |
| [String Interpolation](docs/string-interpolation.md) | Dynamic values in translations |
| [Hierarchical Keys](docs/hierarchical-keys.md) | Organize keys with nesting |
| [Source Generator](docs/source-generator.md) | Strongly-typed `IKeysAccessor` |
| [Validation](docs/validation.md) | Localized DataAnnotation messages |
| [Folder Provider](docs/folder-provider.md) | Load YAML from the file system |
| [Enum & Static Keys](docs/enum-and-static-keys.md) | Use enums or constants as keys |
| [Code Generation](docs/code-generation.md) | Auto-generate resource files from code |
| [Verification Tests](docs/verification-tests.md) | Detect localization issues |
| [YAML Format](docs/yaml-format.md) | Resource file conventions |

## YouTube Session

https://youtu.be/Xz68c8GBYz4

## Contributors

<a href="https://github.com/aksoftware98/multilanguages/graphs/contributors">
<img src="https://contrib.rocks/image?repo=aksoftware98/multilanguages" />
</a>
