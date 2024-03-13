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
    public class TopicExportService : IExportService<Topic>
    {
        private const string RootWorksheetName = "Topics";
        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "ID",
            "Topic"
        };

        private readonly DbtranslationAgencyContext context;

        public TopicExportService(DbtranslationAgencyContext context)
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

        private static void WriteTopic(IXLWorksheet worksheet, Topic topic, int rowIndex)
        {
            worksheet.Cell(rowIndex, 1).Value = topic.TopicId;
            worksheet.Cell(rowIndex, 2).Value = topic.TopicName;
        }

        private static void WriteTopics(IXLWorksheet worksheet, ICollection<Topic> topics)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var topic in topics)
            {
                WriteTopic(worksheet, topic, rowIndex);
                rowIndex += 1;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var topics = await context.Topics.ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(RootWorksheetName);

            WriteTopics(worksheet, topics);

            workbook.SaveAs(stream);
        }
    }
}
