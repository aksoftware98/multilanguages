# Code Generation

Automatically create YAML resource files from your source code and replace hard-coded strings with localization keys.

## Create Resource File from Source Code

Scan your `.razor` or `.cs` files for localizable strings and generate a YAML resource file:

```csharp
ParseParms parms = new ParseParms
{
    SourceDirectories = new List<string> { "path/to/Pages", "path/to/Shared" },
    WildcardPatterns = new List<string> { "*.razor", "*.cs" },
    ExcludeDirectories = new List<string>(),
    ExcludeFiles = new List<string>(),
    ResourceFilePath = "path/to/Resources/en-US.yml"
};

CreateCodeLogic logic = new CreateCodeLogic();
logic.CreateOrUpdateResourceFileFromLocalizableStrings(parms);
```

This scans HTML tags (`<h1>`, `<p>`, `<button>`, `<label>`, `<span>`, etc.) and attributes (`Placeholder=""`, `Label=""`) for text content, generates keys automatically (PascalCase from first 4 words), and writes them to the YAML file.

## Replace Strings with Variables

After generating the resource file, replace hard-coded strings in your source files with localization variables:

```csharp
CreateCodeLogic logic = new CreateCodeLogic();
logic.ReplaceLocalizableStringsWithVariables(parms);
```

This modifies your Razor files in-place, replacing text like `<h1>Hello World</h1>` with `<h1>@Language["HelloWorld"]</h1>`, and injects the `ILanguageContainerService` into the component if not already present.

## ParseParms Configuration

| Property | Description |
|----------|-------------|
| `SourceDirectories` | Folders to scan |
| `WildcardPatterns` | File patterns (e.g., `*.razor`, `*.cs`) |
| `ExcludeDirectories` | Folders to skip |
| `ExcludeFiles` | Files to skip |
| `ResourceFilePath` | Path to the YAML output file |
| `KeyReference` | Variable name used in code (default: `"Language"`) |
| `Prefixes` | Key prefixes (default: `["Dynamic"]`) |
