using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IDataPortServiceFactoryOrder<TOrder>
  where TOrder : Order
    {
        IExportServiceOrder<TOrder> GetExportServiceOrder(string contentOrder);
    }
}
