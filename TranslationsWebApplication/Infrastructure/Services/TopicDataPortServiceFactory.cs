using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class TopicDataPortServiceFactory
  : IDataPortServiceFactory<Topic>
    {
        private readonly DbtranslationAgencyContext Context;
        public TopicDataPortServiceFactory(DbtranslationAgencyContext Context)
        {
            this.Context = Context;
        }
        public IExportService<Topic> GetExportService(string contentType)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentType, StringComparison.InvariantCultureIgnoreCase))
            {
                return new TopicExportService(Context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type { contentType}");
        }
        public IImportService<Topic> GetImportService(string contentType)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentType, StringComparison.InvariantCultureIgnoreCase))
            {
                return new TopicImportService(Context);
            }
            throw new NotImplementedException($"No import service implemented for movies with content type {contentType}");
        }
    }
}