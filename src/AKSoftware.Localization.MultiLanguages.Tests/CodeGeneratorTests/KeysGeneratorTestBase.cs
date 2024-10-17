using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKSoftware.Localization.MultiLanguages.Tests.CodeGeneratorTests;

public class KeysGeneratorTestBase
{

    protected const string VALID_FLAT_YAML = """
        HelloWorld: Hello World
        Title: Title
        Home: Home
        About: About us
        """;

    protected const string VALID_NESTED_YAML = """
        Contact: Contact us
        Home:
            Title: Home Title
            Subtitle: Home Subtitle
        About:
            Title: About Title
            Subtitle: About Subtitle
        """;

    protected const string VALID_INTERPOLATED_YAML = """
        Contact: Email us via {email} and call us on {phone}
        """;


}
