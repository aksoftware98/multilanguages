using AKSoftware.Localization.MultiLanguages.CodeGeneration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AKSoftware.Localization.MultiLanguages.Tests.CodeGeneratorTests;

public class StaticKeysGeneratorTests : KeysGeneratorTestBase
{

    [Fact]
    public void ValidYaml_Should_GenerateStaticKeysClass()
    {
        var generatedContext = StaticKeysGenerator.GenerateStaticKeyClass(VALID_FLAT_YAML);
    }

    [Fact]
    public void ValidNestedYaml_Should_GenerateStaticKeysClass()
    {
        var generatedContext = StaticKeysGenerator.GenerateStaticKeyClass(VALID_NESTED_YAML);
    }
}
