using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IDataPortServiceFactory<TTopic>
  where TTopic : Topic
    {
        IImportService<TTopic> GetImportService(string contentType);
        IExportService<TTopic> GetExportService(string contentType);
    }
}
