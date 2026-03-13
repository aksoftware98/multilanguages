# Folder Provider

Load YAML resource files from the file system instead of embedded resources. Useful when you want to update translations without recompiling.

## Setup with Dependency Injection

```csharp
using AKSoftware.Localization.MultiLanguages;

// Singleton (e.g., Console, WASM)
builder.Services.AddLanguageContainerFromFolder(
    "Resources",
    CultureInfo.GetCultureInfo("en-US"));

// Scoped (Blazor Server)
builder.Services.AddLanguageContainerFromFolderForBlazorServer(
    "Resources",
    CultureInfo.GetCultureInfo("en-US"));
```

The path is relative to the application's working directory.

## Without Dependency Injection

```csharp
using AKSoftware.Localization.MultiLanguages;
using AKSoftware.Localization.MultiLanguages.Providers;

ILanguageContainerService language = new LanguageContainer(
    new FolderResourceKeysProvider("Resources"));

string value = language["HelloWorld"];
```

## When to Use

- **Embedded resources** (`EmbeddedResourceKeysProvider`): translations are compiled into the DLL. Best for most apps.
- **Folder provider** (`FolderResourceKeysProvider`): translations are read from disk at runtime. Best when you need to update translations without redeploying.

## File Requirements

YAML files in the folder must follow the same naming convention: `{culture-code}.yml` (e.g., `en-US.yml`, `fr-FR.yml`). Make sure the files are copied to the output directory (set **Copy to Output Directory** to "Copy if newer" or "Copy always").
