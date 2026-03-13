# YAML Resource File Format

## File Naming

Files must be named with a culture code:

```
en-US.yml     (English - United States)
fr-FR.yml     (French - France)
ar-SA.yml     (Arabic - Saudi Arabia)
ja-JP.yml     (Japanese - Japan)
de-DE.yml     (German - Germany)
```

Supported patterns: `xx.yml`, `xx-XX.yml`, `xx-Xxxx-XX.yml` (script subtag).

Both `.yml` and `.yaml` extensions work.

## Simple Keys

```yaml
HelloWorld: Hello World
Welcome: Welcome
GoodMorning: Good Morning
```

## Hierarchical Keys

```yaml
HomePage:
  Title: Home
  HelloWorld: Hello World
  Welcome: Welcome to our app

Counter:
  Title: Counter
  ClickMe: Click Me

FetchData:
  Title: Fetch Data
  Loading: Loading...
```

Access with `:` separator: `language["HomePage:Title"]`

## Interpolation Placeholders

```yaml
Welcome: Welcome {username} to our system {version}
Hello: Hello {FirstName} {LastName}
```

Placeholders are replaced at runtime. See [String Interpolation](string-interpolation.md).

## Build Action

When using `EmbeddedResourceKeysProvider`, YAML files must have their build action set to **EmbeddedResource**.

In `.csproj`:

```xml
<ItemGroup>
  <EmbeddedResource Include="Resources\en-US.yml" />
  <EmbeddedResource Include="Resources\fr-FR.yml" />
</ItemGroup>
```

> If using the Source Generator package, this is handled automatically.

When using `FolderResourceKeysProvider`, set **Copy to Output Directory** instead. See [Folder Provider](folder-provider.md).

## Online Translation

Upload your `en-US.yml` at [akmultilanguages.azurewebsites.net](https://akmultilanguages.azurewebsites.net) to generate translated files for 69+ languages.
