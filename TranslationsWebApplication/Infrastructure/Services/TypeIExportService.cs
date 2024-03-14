using DocumentFormat.OpenXml.Vml.Office;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IExportServiceType<TType>
 where TType : TranslationsWebApplication.Models.Type
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
