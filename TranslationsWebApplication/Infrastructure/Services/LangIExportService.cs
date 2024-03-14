using DocumentFormat.OpenXml.Vml.Office;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IExportServiceLang<TLang>
 where TLang : Language
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
