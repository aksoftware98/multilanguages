namespace AKSoftware.Localization.MultiLanguages.SourceGenerator.Tests;

// IMPORTANT:
// Decided between the name for the following
// IKeysAccessor, ILangKeysAccessor, IKeysAccessorGenerator
public class KeysAccessorGeneratorTests
{

    private readonly KeysDecoratorSourceGenerator _generator;

    public KeysAccessorGeneratorTests()
    {
        _generator = new KeysDecoratorSourceGenerator();
    }

    [Fact]
    public void ValidYaml_Should_GenerateDecoratorInterfaces()
    {
        var generatedContext = _generator.BuildClass(VALID_FLAT_YAML);
    }

    [Fact]
    public void FlatYaml_Should_GenerateOneLevelKeysAccessor()
    {
        var generatedContext = _generator.BuildClass(VALID_FLAT_YAML);

    }

    [Fact]
    public void NestedYaml_Should_GenerateNestedKeysAccessor()
    {
        var generatedContext = _generator.BuildClass(VALID_NESTED_YAML);

    }

    [Fact]
    public void YamlWithoutInterpolation_Should_GenerateKeysAsReadOnlyProperties()
    {
        
    }

    [Fact]
    public void YamlWithInterpolation_Should_GenerateKeysAsMethodsWithParameters()
    {

    }


    private const string VALID_FLAT_YAML = """
        HelloWorld: Hello World
        Title: Title
        Home: Home
        About: About us
        """;

    private const string VALID_NESTED_YAML = """
        Contact: Contact us
        Home:
            Title: Home Title
            Subtitle: Home Subtitle
        About:
            Title: About Title
            Subtitle: About Subtitle
        """;
}