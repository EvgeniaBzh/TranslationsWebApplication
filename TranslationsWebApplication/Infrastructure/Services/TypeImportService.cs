using ClosedXML.Excel;
using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class TypeImportService : IImportServiceType<TranslationsWebApplication.Models.Type>
    {
        private readonly DbtranslationAgencyContext context;

        public TypeImportService(DbtranslationAgencyContext context)
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
            foreach (var rows in worksheet.RowsUsed().Skip(1)) //пропустити перший рядок, бо це заголовок
            {
                await AddTypeAsync(rows, cancellationToken);
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddTypeAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var typeName = GetTypesName(row);
            var type = await context.Types.FirstOrDefaultAsync(t => t.TypeName == typeName, cancellationToken);
            if (type == null)
            {
                type = new TranslationsWebApplication.Models.Type { TypeName = typeName };
                context.Types.Add(type);
            }
        }

        private static string GetTypesName(IXLRow row)
        {
            return row.Cell(1).GetValue<string>();
        }

    }
}
