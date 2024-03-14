using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IDataPortServiceFactoryType<TType>
  where TType : TranslationsWebApplication.Models.Type
    {
        IImportServiceType<TType> GetImportServiceType(string contentType);
        IExportServiceType<TType> GetExportServiceType(string contentType);
    }
}
