using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranslationsWebApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class LangExportService : IExportServiceLang<Language>
    {
        private const string RootWorksheetName = "Types";
        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "ID",
            "Language"
        };

        private readonly DbtranslationAgencyContext context;

        public LangExportService(DbtranslationAgencyContext context)
        {
            this.context = context;
        }

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private static void WriteType(IXLWorksheet worksheet, Language lang, int rowIndex)
        {
            worksheet.Cell(rowIndex, 1).Value = lang.LanguageId;
            worksheet.Cell(rowIndex, 2).Value = lang.LanguageName;
        }

        private static void WriteLangs(IXLWorksheet worksheet, ICollection<Language> langs)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var lang in langs)
            {
                WriteType(worksheet, lang, rowIndex);
                rowIndex += 1;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var langs = await context.Languages.ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(RootWorksheetName);

            WriteLangs(worksheet, langs);

            workbook.SaveAs(stream);
        }
    }
}
