﻿using AKSoftware.Localization.MultiLanguages.CodeGeneration;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests.CodeGeneratorTests;

/// <summary>
/// TODO Add the assertions to the tests to verify the generated code using the Rosyln syntax tree
/// </summary>
public class KeysAccessorGeneratorTests
{

    [Fact]
    public void ValidYaml_Should_GenerateDecoratorInterfaces()
    {
        var generatedContext = KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(VALID_FLAT_YAML);
    }

    [Fact]
    public void FlatYaml_Should_GenerateOneLevelKeysAccessor()
    {
        var generatedContext = KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(VALID_FLAT_YAML);

    }

    [Fact]
    public void NestedYaml_Should_GenerateNestedKeysAccessor()
    {
        var generatedContext = KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(VALID_NESTED_YAML);

    }

    [Fact]
    public void YamlWithoutInterpolation_Should_GenerateKeysAsReadOnlyProperties()
    {
        var generatedContext = KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(VALID_FLAT_YAML);

    }

    [Fact]
    public void YamlWithInterpolation_Should_GenerateKeysAsMethodsWithParameters()
    {
        var generatedContext = KeysAccessorGenerator.GenerateKeysAccessorClassesAndInterfaces(VALID_INTERPOLATED_YAML);

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

    private const string VALID_INTERPOLATED_YAML = """
        Contact: Email us via {email} and call us on {phone}
        """;

}