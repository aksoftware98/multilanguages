# Getting Started

## Installation

Install the core package:

```
dotnet add package AKSoftware.Localization.MultiLanguages
```

For **Blazor** apps, also install:

```
dotnet add package AKSoftware.Localization.MultiLanguages.Blazor
```

For the **Source Generator** (strongly-typed keys), also install:

```
dotnet add package AKSoftware.Localization.MultiLanguages.SourceGenerator
```

## Create Resource Files

1. Create a `Resources` folder in your project
2. Add a YAML file named with the culture code, e.g. `en-US.yml`:

```yaml
HelloWorld: Hello World
Welcome: Welcome
GoodMorning: Good Morning
```

3. Set the build action to **EmbeddedResource** (right-click the file in Solution Explorer → Properties → Build Action)

> If you're using the Source Generator package, embedded resource is set automatically.

## Translate Your Files

Visit [akmultilanguages.azurewebsites.net](https://akmultilanguages.azurewebsites.net) to translate your YAML file into 69+ languages with one click. Add the translated files to your `Resources` folder and set them as EmbeddedResource too.

## Register the Service

In `Program.cs`:

```csharp
using AKSoftware.Localization.MultiLanguages;
using System.Reflection;

builder.Services.AddLanguageContainer(Assembly.GetExecutingAssembly());
```

To set a default language:

```csharp
builder.Services.AddLanguageContainer(
    Assembly.GetExecutingAssembly(),
    CultureInfo.GetCultureInfo("fr-FR"));
```

If no default is specified, the library picks the best match: current user's culture → same language → English → first available file.

## Use in Code

Inject `ILanguageContainerService` and access keys:

```csharp
@inject ILanguageContainerService language

<h1>@language["HelloWorld"]</h1>
<p>@language["Welcome"]</p>
```

That's it. See the other guides for Blazor state management, interpolation, source generator, and more.
