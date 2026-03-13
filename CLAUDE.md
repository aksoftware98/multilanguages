# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

AKSoftware.Localization.MultiLanguages is a .NET localization library using YAML files (instead of .resx) for multi-language support across Blazor, ASP.NET Core, UWP, WPF, and Console apps.

## Build & Test Commands

```bash
# Build entire solution
dotnet build src/AKSoftware.Localization.MultiLanguages.sln --configuration Release

# Build core library only
dotnet build src/AKSoftware.Localization.MultiLanguages/AKSoftware.Localization.MultiLanguages.csproj

# Run all tests
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests/AKSoftware.Localization.MultiLanguages.Tests.csproj

# Run a single test
dotnet test src/AKSoftware.Localization.MultiLanguages.Tests --filter "FullyQualifiedName~TestMethodName"
```

## Architecture

### Core Library (`AKSoftware.Localization.MultiLanguages`)
Targets netstandard2.0, net8.0, net9.0. Central service is `ILanguageContainerService` implemented by `LanguageContainer`, which loads YAML resource files and provides key-value access with string interpolation (`{PropertyName}` placeholders).

**Provider Pattern**: `IKeysProvider` abstraction with two implementations:
- `EmbeddedResourceKeysProvider` — loads YAML from compiled assembly embedded resources
- `FolderResourceKeysProvider` — loads YAML from the filesystem

Both extend `BaseKeysProvider` which handles culture-matching regex patterns.

**Culture Fallback**: exact culture → language match → English → first available.

**Key Access**: `Keys` class wraps parsed YAML as a dynamic object. Supports hierarchical keys with `:` separator (e.g., `HomePage:Title`) and interpolation via regex replacement.

### Blazor Extension (`AKSoftware.Localization.MultiLanguages.Blazor`)
Targets net8.0, net9.0. Provides `AddLanguageContainer` DI extension and `IExtension`-based component tracking so Blazor components auto-refresh on language change via `StateHasChanged`.

### Source Generator (`AKSoftware.Localization.MultiLanguages.SourceGenerator`)
Targets netstandard2.0. Roslyn incremental source generator that reads YAML files and generates an `IKeysAccessor` interface for strongly-typed key access. Also auto-embeds YAML files as resources.

### Validation (`ValidationLocalization`)
Integrates with ASP.NET Core DataAnnotations (Required, MaxLength, StringLength, Range, RegularExpression, Compare) to produce localized validation messages.

### Code Generation (`CodeGeneration/`)
`ParseCodeLogic` scans source files for localizable strings. `CreateCodeLogic` generates YAML resource files, static key classes, and enums from parsed results.

## Key Dependencies

- **YamlDotNet** (v9.1.0) — YAML parsing
- **Microsoft.Extensions.DependencyInjection.Abstractions** — DI registration
- **xUnit** + **FluentAssertions** — testing

## YAML Resource File Convention

Files named `{culture}.yml` (e.g., `en-US.yml`, `fr-FR.yml`). Must be set as `EmbeddedResource` build action when using `EmbeddedResourceKeysProvider`. Simple key-value YAML format; hierarchical keys accessed with `:` separator.

## CI/CD

Azure Pipelines (`azure-pipelines.yml`) triggers on `master`, builds on `windows-latest`, runs NDepend analysis, and packs NuGet packages.
