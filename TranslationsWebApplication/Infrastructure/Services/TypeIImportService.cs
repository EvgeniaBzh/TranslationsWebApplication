using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IImportServiceType<TType>
        where TType : TranslationsWebApplication.Models.Type
    {
                Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
            }
}