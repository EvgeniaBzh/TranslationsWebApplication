using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class OrderDataPortServiceFactory
  : IDataPortServiceFactoryOrder<Order>
    {
        private readonly DbtranslationAgencyContext Context;
        public OrderDataPortServiceFactory(DbtranslationAgencyContext Context)
        {
            this.Context = Context;
        }
        public IExportServiceOrder<Order> GetExportServiceOrder(string contentOrder)
        {
            if ("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet".Equals(contentOrder, StringComparison.InvariantCultureIgnoreCase))
            {
                return new OrderExportService(Context);
            }
            throw new NotImplementedException($"No export service implemented for movies with content type {contentOrder}");
        }
        
    }
}