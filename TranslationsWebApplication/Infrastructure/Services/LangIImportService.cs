using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IImportServiceLang<TLang>
        where TLang : Language
    {
                Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
            }
}