using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class TypeDataPortServiceFactory
  : IDataPortServiceFactoryType<TranslationsWebApplication.Models.Type>
    {
        private readonly DbtranslationAgencyContext Context;
        public TypeDataPortServiceFactory(DbtranslationAgencyContext Context)
        {
            this.Context = Context;
        }
        public IExportServiceType<TranslationsWebApplication.Models.Type> GetExportServiceType(string contentType)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentType, StringComparison.InvariantCultureIgnoreCase))
            {
                return new TypeExportService(Context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type { contentType}");
        }
        public IImportServiceType<TranslationsWebApplication.Models.Type> GetImportServiceType(string contentType)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentType, StringComparison.InvariantCultureIgnoreCase))
            {
                return new TypeImportService(Context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
    }
}