// See https://aka.ms/new-console-template for more information
using AKSoftware.Localization.MultiLanguages;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using System.Globalization;
using System.Reflection;

Console.WriteLine("Benchmark results of retrieving the data with JSON still being used");
var summary = BenchmarkRunner.Run<LanguageContainerServiceBenchmarks>();
Console.WriteLine(summary.Table.ToString());
Console.ReadKey();

public class LanguageContainerServiceBenchmarks
{
    ILanguageContainerService _service;

    public LanguageContainerServiceBenchmarks()
    {
        var keysProvider = new EmbeddedResourceKeysProvider(Assembly.GetExecutingAssembly());
        _service = new LanguageContainer(CultureInfo.GetCultureInfo("en-US"), keysProvider);
    }

    [Benchmark]
    public string GetFirstLevelKey()
    {
        return _service.Keys["FooterMessage "];
    }

    [Benchmark]
    public string GetLevelTwoKey()
    {
        return _service.Keys["HomePage:Login:Title"];
    }
}