using System.Globalization;
using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;
using ReservationApp.ViewModels;
using Xceed.Document.NET;
using Xceed.Drawing;
using Xceed.Words.NET;

namespace ReservationApp.Infrastructure.Exporting;

public class Exporter: IExporter
{
    void addRows(Table table, (string label, string value)[] rows)
    {
        for (int i = 0; i < rows.Length; i++)
        {
            table.Rows[i].Cells[0].Paragraphs[0].Append(rows[i].label);
            table.Rows[i].Cells[1].Paragraphs[0].Append(rows[i].value);
        }
    }
    public byte[] ExportBookingInvoice(Booking booking)
    {
        using (var stream = new MemoryStream())
        {
            using (var doc = DocX.Create(stream))
            {
                var logoPath = Path.Combine(Directory.GetCurrentDirectory(), SD.LogoPath);

                if (File.Exists(logoPath))
                {
                    var img = doc.AddImage(logoPath);
                    var picture = img.CreatePicture(60, 60);
                    var paragraphWithImg = doc.InsertParagraph();
                    paragraphWithImg.AppendPicture(picture);
                    paragraphWithImg.Alignment = Alignment.left;
                }
                // main title
                doc.InsertParagraph(SD.InvoiceTitle)
                    .Font("Arial")
                    .FontSize(22)
                    .Bold()
                    .Alignment = Alignment.center;
    
                doc.InsertParagraph().SpacingAfter(15);
                
                // Customer info
                var customerTable = doc.AddTable(5, 2);
                customerTable.Alignment = Alignment.left;
                customerTable.Design = TableDesign.ColorfulList;

                var rows = new (string label, string value)[]
                {
                    ("Customer Name", booking.Name),
                    ("Phone", booking.Phone),
                    ("Email", booking.Email),
                    ("Invoice Date", DateTime.Now.ToString("dd/MM/yyyy")),
                    ("Booking ID", $"#{booking.Id}")
                };
                addRows(customerTable, rows);
                doc.InsertTable(customerTable);
                doc.InsertParagraph().SpacingAfter(20);
    
                // Booking details
                var detailsTable = doc.AddTable(6, 2);
                detailsTable.Alignment = Alignment.left;
                detailsTable.Design = TableDesign.LightShadingAccent1;
    
                rows = new (string label, string value)[]
                {
                    ("Villa Name", booking.Villa.Name),
                    ("Check-In Date", booking.CheckInDate.ToString("dd/MM/yyyy")),
                    ("Check-Out Date", booking.CheckOutDate.ToString("dd/MM/yyyy")),
                    ("Nights", booking.Nights.ToString()),
                    ("Status", booking.Status),
                    ("Total Cost", booking.TotalCost.ToString("C"))
                };
                addRows(detailsTable, rows);;
                doc.InsertTable(detailsTable);
                
                //Thanks for choosing
                doc.InsertParagraph()
                    .AppendLine(SD.ThanksMessage)
                    .Italic()
                    .FontSize(12);
    
                doc.Save();
            }
    
            return stream.ToArray();
        }
    }

    public byte[] ExportRevenueReport(RevenueReportDto report)
{
    using (var stream = new MemoryStream())
    {
        using (var doc = DocX.Create(stream))
        {
            // Logo
            var logoPath = Path.Combine(Directory.GetCurrentDirectory(), SD.LogoPath);
            if (File.Exists(logoPath))
            {
                var img = doc.AddImage(logoPath);
                var picture = img.CreatePicture(60, 60);
                var paragraphWithImg = doc.InsertParagraph();
                paragraphWithImg.AppendPicture(picture);
                paragraphWithImg.Alignment = Alignment.left;
            }

            // Title
            doc.InsertParagraph("📊 Weekly Revenue Report")
                .Font("Arial")
                .FontSize(20)
                .Bold()
                .Color(Color.LightBlue)
                .Alignment = Alignment.center;

            doc.InsertParagraph().SpacingAfter(10);
            // Summary Table
            var summaryTable = doc.AddTable(2, 2);
            summaryTable.Design = TableDesign.LightShadingAccent1;
            summaryTable.Alignment = Alignment.left;

            var summaryRows = new (string label, string value)[]
            {
                ("Total Revenue", report.totalRevenue.ToString("C")),
                ("Number of Bookings", report.numberBookings.ToString())
            };

            addRows(summaryTable, summaryRows);
            doc.InsertTable(summaryTable);
            doc.InsertParagraph().SpacingAfter(20);

            // Daily Breakdown
            doc.InsertParagraph("📅 Daily Breakdown")
                .Bold()
                .FontSize(14)
                .SpacingAfter(8);

            var breakdownTable = doc.AddTable(report.revenueByWeek.Count() + 1, 2);
            breakdownTable.Alignment = Alignment.left;
            breakdownTable.Design = TableDesign.MediumGrid3Accent2;

            // Headers
            // Headers
            var headerRow = breakdownTable.Rows[0];
            headerRow.Cells[0].Paragraphs[0].Append("📅 Date").Bold().Color(Color.White).Alignment = Alignment.center;
            headerRow.Cells[1].Paragraphs[0].Append("💰 Revenue").Bold().Color(Color.White).Alignment = Alignment.center;

            // Rows
            var revenueList = report.revenueByWeek.ToList();
            for (int i = 0; i < revenueList.Count; i++)
            {
                var row = revenueList[i];
                var tableRow = breakdownTable.Rows[i + 1];

                tableRow.Cells[0].Paragraphs[0]
                    .Append(row.Date.ToString("dd/MM/yyyy"))
                    .Font("Arial")
                    .FontSize(11)
                    .Alignment = Alignment.center;

                tableRow.Cells[1].Paragraphs[0]
                    .Append(row.Revenue.ToString("C", new CultureInfo("vi-VN")))
                    .Font("Arial")
                    .FontSize(11)
                    .Alignment = Alignment.center;
            }

            doc.InsertTable(breakdownTable);
            doc.InsertParagraph().SpacingAfter(20);
            // Thank you
            doc.InsertParagraph()
                .AppendLine("Thank you for choosing our platform!")
                .Italic()
                .FontSize(12);

            // Footer
            doc.InsertParagraph()
                .AppendLine($"© {DateTime.Now.Year} VillaChill Platform. All rights reserved.")
                .FontSize(10)
                .Color(Color.Gray)
                .Alignment = Alignment.center;

            doc.Save();
        }

        return stream.ToArray();
    }
}

}