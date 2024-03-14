using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IImportService<TTopic>
        where TTopic : Topic
    {
                Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken);
            }
}