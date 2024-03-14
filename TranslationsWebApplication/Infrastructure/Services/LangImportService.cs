using ClosedXML.Excel;
using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class LangImportService : IImportServiceLang<Language>
    {
        private readonly DbtranslationAgencyContext context;

        public LangImportService(DbtranslationAgencyContext context)
        {
            this.context = context;
        }
        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Stream is not eadable", nameof(stream));
            }
            using var workBook = new XLWorkbook(stream);
            var worksheet = workBook.Worksheets.FirstOrDefault();
            if (worksheet is null)
            {
                return;
            }
            foreach (var rows in worksheet.RowsUsed().Skip(1))
            {
                await AddTypeAsync(rows, cancellationToken);
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddTypeAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var langName = GetLangName(row);
            var lang = await context.Languages.FirstOrDefaultAsync(l => l.LanguageName == langName, cancellationToken);
            if (lang == null)
            {
                lang = new Language { LanguageName = langName };
                context.Languages.Add(lang);
            }
        }

        private static string GetLangName(IXLRow row)
        {
            return row.Cell(1).GetValue<string>();
        }

    }
}
