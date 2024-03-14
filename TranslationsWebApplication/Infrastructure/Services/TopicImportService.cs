using ClosedXML.Excel;
using TranslationsWebApplication.Models;
using TranslationsWebApplication.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class TopicImportService : IImportService<Topic>
    {
        private readonly DbtranslationAgencyContext context;

        public TopicImportService(DbtranslationAgencyContext context)
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
                await AddTopicAsync(rows, cancellationToken);
            }
            await context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddTopicAsync(IXLRow row, CancellationToken cancellationToken)
        {
            var topicName = GetTopicName(row);
            var topic = await context.Topics.FirstOrDefaultAsync(t => t.TopicName == topicName, cancellationToken);
            if (topic == null)
            {
                topic = new Topic { TopicName = topicName };
                context.Topics.Add(topic);
            }
        }

        private static string GetTopicName(IXLRow row)
        {
            return row.Cell(1).GetValue<string>();
        }

    }
}
