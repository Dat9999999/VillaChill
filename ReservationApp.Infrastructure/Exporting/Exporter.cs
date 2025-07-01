using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;
using Xceed.Document.NET;
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
}