# String Interpolation

Use `{placeholder}` in your YAML values to inject dynamic values at runtime.

## YAML File

```yaml
Welcome: Welcome {username} to our system {version}
Hello: Hello {FirstName} {LastName}
```

## Using an Anonymous Object

```csharp
string result = language["Welcome", new
{
    Username = "ahmad",
    Version = "v6.0"
}];
// "Welcome ahmad to our system v6.0"
```

Property names are matched **case-insensitively** against placeholders.

## Using a Dictionary

```csharp
var values = new Dictionary<string, object>
{
    ["FirstName"] = "Ahmad",
    ["LastName"] = "Mozaffar"
};

string result = language["Hello", values];
// "Hello Ahmad Mozaffar"
```

## Null Handling

By default, if a property is `null`, the placeholder stays as-is. Pass `setEmptyIfNull: true` to replace nulls with empty strings:

```csharp
string result = language["Hello", new { FirstName = "Ahmad", LastName = (string)null }, setEmptyIfNull: true];
// "Hello Ahmad "
```

## With Source Generator

When using the source generator, interpolated keys become method parameters:

```yaml
Welcome: Welcome {username} to version {version}
```

Generated:

```csharp
string Welcome(string username, string version);
```

Usage:

```csharp
@inject IKeysAccessor keys

<p>@keys.Welcome("ahmad", "v6.0")</p>
```
