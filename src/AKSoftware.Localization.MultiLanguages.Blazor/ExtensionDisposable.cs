using System;

namespace AKSoftware.Localization.MultiLanguages.Blazor
{
    internal sealed class ExtensionDisposable : IDisposable
    {
        private readonly ILanguageContainerService _language;
        private readonly IExtension _extension;
        private bool _disposed;

        public ExtensionDisposable(ILanguageContainerService language, IExtension extension)
        {
            _language = language;
            _extension = extension;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _language.RemoveExtension(_extension);
            }
        }
    }
}
