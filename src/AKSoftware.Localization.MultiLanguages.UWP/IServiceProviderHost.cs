using System;

namespace AKSoftware.Localization.MultiLanguages.UWP
{
    public interface IServiceProviderHost
    {
        // marker interface
        IServiceProvider ServiceProvider { get; }
    }
}
