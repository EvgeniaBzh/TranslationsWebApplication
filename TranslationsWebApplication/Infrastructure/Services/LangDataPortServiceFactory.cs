using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class LangDataPortServiceFactory
  : IDataPortServiceFactoryLang<Language>
    {
        private readonly DbtranslationAgencyContext Context;
        public LangDataPortServiceFactory(DbtranslationAgencyContext Context)
        {
            this.Context = Context;
        }
        public IExportServiceLang<Language> GetExportServiceLang(string contentType)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentType, StringComparison.InvariantCultureIgnoreCase))
            {
                return new LangExportService(Context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type { contentType}");
        }
        public IImportServiceLang<Language> GetImportServiceLang(string contentLang)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentLang, StringComparison.InvariantCultureIgnoreCase))
            {
                return new LangImportService(Context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentLang}");
        }
    }
}