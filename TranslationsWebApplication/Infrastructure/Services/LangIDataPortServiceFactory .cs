using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IDataPortServiceFactoryLang<TLang>
  where TLang : Language
    {
        IImportServiceLang<TLang> GetImportServiceLang(string contentType);
        IExportServiceLang<TLang> GetExportServiceLang(string contentType);
    }
}
