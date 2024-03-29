﻿using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TranslationsWebApplication.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TranslationsWebApplication.Infrastructure.Services
{
    public class OrderExportService : IExportServiceOrder<Order>
    {
        private const string RootWorksheetName = "Orders";
        private static readonly IReadOnlyList<string> HeaderNames = new string[]
        {
            "ID",
            "Name",
            "Original Language",
            "Translation Language",
            "Type ID",
            "Topic ID",
            "Scope",
            "Price",
            "Submission Date",
            "Status"
        };

        private readonly DbtranslationAgencyContext context;

        public OrderExportService(DbtranslationAgencyContext context)
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

        private static void WriteOrder(IXLWorksheet worksheet, Order order, int rowIndex)
        {
            worksheet.Cell(rowIndex, 1).Value = order.OrderId;
            worksheet.Cell(rowIndex, 2).Value = order.OrderName;
            worksheet.Cell(rowIndex, 3).Value = order.OriginalLanguage?.LanguageName ?? "N/A";
            worksheet.Cell(rowIndex, 4).Value = order.TranslationLanguage?.LanguageName ?? "N/A";
            worksheet.Cell(rowIndex, 5).Value = order.Type?.TypeName ?? "N/A";
            worksheet.Cell(rowIndex, 6).Value = order.Topic?.TopicName ?? "N/A";
            worksheet.Cell(rowIndex, 7).Value = order.OrderScope;
            worksheet.Cell(rowIndex, 8).Value = order.OrderPrice;
            worksheet.Cell(rowIndex, 9).Value = order.OrderSubmissionDate.ToString("yyyy-MM-dd");
            worksheet.Cell(rowIndex, 10).Value = order.OrderStatus.ToString();
        }


        private static void WriteOrders(IXLWorksheet worksheet, ICollection<Order> orders)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var order in orders)
            {
                WriteOrder(worksheet, order, rowIndex);
                rowIndex += 1;
            }
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var orders = await context.Orders.Include(o => o.OriginalLanguage)
                                             .Include(o => o.TranslationLanguage)
                                             .Include(o => o.Type)
                                             .Include(o => o.Topic)
                                             .ToListAsync(cancellationToken);

            using var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add(RootWorksheetName);

            WriteOrders(worksheet, orders);

            workbook.SaveAs(stream);
        }
    }
}