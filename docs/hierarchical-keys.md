# Hierarchical Keys

Organize translation keys with YAML nesting. Access nested keys with the `:` separator.

## YAML File

```yaml
HomePage:
  Title: Home
  HelloWorld: Hello World
  Welcome: Welcome to our app

Counter:
  Title: Counter
  ClickMe: Click Me

NavMenu:
  About: About
  Home: Home
```

## Accessing Keys

Use `:` to access nested values:

```razor
<h1>@language["HomePage:Title"]</h1>
<p>@language["HomePage:Welcome"]</p>
<button>@language["Counter:ClickMe"]</button>
```

## With Source Generator

Hierarchical keys generate nested interfaces:

```csharp
@inject IKeysAccessor keys

<h1>@keys.HomePage.Title</h1>
<button>@keys.Counter.ClickMe</button>
```

## Listing All Keys

`GetKeys()` returns flattened key paths:

```csharp
List<string> keys = language.GetKeys();
// ["HomePage:Title", "HomePage:HelloWorld", "HomePage:Welcome", "Counter:Title", ...]
```

## Iterating Key-Value Pairs

```csharp
foreach (KeyValuePair<object, object> kv in language.Keys)
{
    Console.WriteLine($"{kv.Key}: {kv.Value}");
}
```

Note: for nested keys, `kv.Value` will be a dictionary. Use `GetKeys()` for a flat list.
