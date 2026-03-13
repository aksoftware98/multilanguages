# Verification Tests

Write unit tests to catch localization issues: missing translations, unused keys, broken references, and duplicates.

All examples use `ParseCodeLogic` with `ParseParms` to scan your source files against your YAML resource files.

## Common Setup

```csharp
ParseParms parms = new ParseParms
{
    SourceDirectories = new List<string> { "path/to/Pages", "path/to/Shared" },
    WildcardPatterns = new List<string> { "*.razor" },
    ExcludeDirectories = new List<string>(),
    ExcludeFiles = new List<string>(),
    ResourceFilePath = "path/to/Resources/en-US.yml",
    KeyReference = "Language"
};
```

## Verify All Source Files Are Localized

Detect hard-coded strings that haven't been localized yet:

```csharp
[Fact]
public void VerifyAllSourceCodeFilesAreLocalized()
{
    ParseCodeLogic logic = new ParseCodeLogic();
    List<ParseResult> results = logic.GetLocalizableStrings(parms);

    Assert.Empty(results); // Fails if unlocalzied strings exist
}
```

## Verify All Keys Can Be Found

Detect typos in key references — keys used in code that don't exist in the YAML file:

```csharp
[Fact]
public void VerifyAllKeysCanBeFound()
{
    ParseCodeLogic logic = new ParseCodeLogic();
    var missingKeys = logic.GetExistingLocalizedStrings(parms)
        .Where(o => string.IsNullOrEmpty(o.LocalizableString));

    Assert.Empty(missingKeys); // Fails if a key reference has no matching YAML entry
}
```

## Verify No Unused Keys

Detect keys in the YAML file that aren't referenced anywhere in code:

```csharp
[Fact]
public void VerifyNoUnusedKeys()
{
    ParseCodeLogic logic = new ParseCodeLogic();
    List<string> unusedKeys = logic.GetUnusedKeys(parms);

    Assert.Empty(unusedKeys);
}
```

## Verify No Duplicate Keys

Detect strings that would produce duplicate keys if auto-generated:

```csharp
[Fact]
public void VerifyNoDuplicateKeys()
{
    ParseCodeLogic logic = new ParseCodeLogic();
    Dictionary<string, List<string>> duplicates = logic.GetDuplicateKeys(parms);

    Assert.Empty(duplicates);
}
```

When duplicates are found, you'll need to manually create unique keys for those strings.
