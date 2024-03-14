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
    public class TypeExportService : IExportServiceType<TranslationsWebApplication.Models.Type>
    {
        private const string RootWorksheetName = "Types";
        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "ID",
            "Type"
        };

        private readonly DbtranslationAgencyContext context;

        public TypeExportService(DbtranslationAgencyContext context)
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

        private static void WriteType(IXLWorksheet worksheet, TranslationsWebApplication.Models.Type type, int rowIndex)
        {
            worksheet.Cell(rowIndex, 1).Value = type.TypeId;
            worksheet.Cell(rowIndex, 2).Value = type.TypeName;
        }

        private static void WriteTypes(IXLWorksheet worksheet, ICollection<TranslationsWebApplication.Models.Type> types)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var type in types)
            {
                WriteType(worksheet, type, rowIndex);
                rowIndex += 1;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var types = await context.Types.ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(RootWorksheetName);

            WriteTypes(worksheet, types);

            workbook.SaveAs(stream);
        }
    }
}
