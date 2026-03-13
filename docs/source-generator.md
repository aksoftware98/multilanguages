# Source Generator

The source generator creates a strongly-typed `IKeysAccessor` interface from your YAML files — no magic strings needed.

## Installation

```
dotnet add package AKSoftware.Localization.MultiLanguages.SourceGenerator
```

> The source generator automatically sets YAML files as embedded resources. No need to do it manually.

## Register the Service

```csharp
// Program.cs

// Register the language container
builder.Services.AddLanguageContainerForBlazorServer(typeof(Program).Assembly);

// Register the generated IKeysAccessor
builder.Services.AddKeysAccessorAsScoped();    // for Blazor Server
// or
builder.Services.AddKeysAccessorAsSingleton(); // for Blazor WASM
```

## Usage

```razor
@inject IKeysAccessor keys

<h1>@keys.HomePage.Title</h1>
<p>@keys.HomePage.HelloWorld</p>
<button>@keys.Counter.ClickMe</button>
```

## What Gets Generated

Given this `en-US.yml`:

```yaml
HomePage:
  Title: Home
  Welcome: Welcome {username}
Counter:
  Title: Counter
```

The generator creates:

```csharp
public interface IKeysAccessor
{
    IHomePageKeysAccessor HomePage { get; }
    ICounterKeysAccessor Counter { get; }
}

public interface IHomePageKeysAccessor
{
    string Title { get; }
    string Welcome(string username);
}

public interface ICounterKeysAccessor
{
    string Title { get; }
}
```

- Simple keys become **properties**
- Keys with `{placeholder}` become **methods** with parameters

## Multi-Project Solutions

The source generator must be installed in the project that contains the `en-US.yml` file. If your solution has multiple projects, create a shared localization project with the YAML files and the source generator, then reference it from other projects.
