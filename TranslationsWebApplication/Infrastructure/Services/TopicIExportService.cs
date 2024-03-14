using DocumentFormat.OpenXml.Vml.Office;
using TranslationsWebApplication.Models;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public interface IExportService<TTopic>
 where TTopic : Topic
    {
        Task WriteToAsync(Stream stream, CancellationToken cancellationToken);
    }
}
