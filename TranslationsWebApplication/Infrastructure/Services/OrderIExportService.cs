using DocumentFormat.OpenXml.Vml.Office;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IExportServiceOrder<TOrder>
 where TOrder : Order
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
