using ReservationApp.Application.Common.Interfaces;
using ReservationApp.Application.Common.utility;
using ReservationApp.Domain.Entities;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace ReservationApp.Infrastructure.Exporting;

public class Exporter: IExporter
{
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
    
                customerTable.Rows[0].Cells[0].Paragraphs[0].Append("Customer Name:");
                customerTable.Rows[0].Cells[1].Paragraphs[0].Append(booking.Name);
    
                customerTable.Rows[1].Cells[0].Paragraphs[0].Append("Phone:");
                customerTable.Rows[1].Cells[1].Paragraphs[0].Append(booking.Phone);
    
                customerTable.Rows[2].Cells[0].Paragraphs[0].Append("Email:");
                customerTable.Rows[2].Cells[1].Paragraphs[0].Append(booking.Email);
    
                customerTable.Rows[3].Cells[0].Paragraphs[0].Append("Invoice Date:");
                customerTable.Rows[3].Cells[1].Paragraphs[0].Append(DateTime.Now.ToString("dd/MM/yyyy"));
    
                customerTable.Rows[4].Cells[0].Paragraphs[0].Append("Booking ID:");
                customerTable.Rows[4].Cells[1].Paragraphs[0].Append($"#{booking.Id}");
    
                doc.InsertTable(customerTable);
                doc.InsertParagraph().SpacingAfter(20);
    
                // Booking details
                var detailsTable = doc.AddTable(6, 2);
                detailsTable.Alignment = Alignment.left;
                detailsTable.Design = TableDesign.LightShadingAccent1;
    
                detailsTable.Rows[0].Cells[0].Paragraphs[0].Append("Villa Name");
                detailsTable.Rows[0].Cells[1].Paragraphs[0].Append(booking.Villa.Name);
    
                detailsTable.Rows[1].Cells[0].Paragraphs[0].Append("Check-In Date");
                detailsTable.Rows[1].Cells[1].Paragraphs[0].Append(booking.CheckInDate.ToString("dd/MM/yyyy"));
    
                detailsTable.Rows[2].Cells[0].Paragraphs[0].Append("Check-Out Date");
                detailsTable.Rows[2].Cells[1].Paragraphs[0].Append(booking.CheckOutDate.ToString("dd/MM/yyyy"));
    
                detailsTable.Rows[3].Cells[0].Paragraphs[0].Append("Nights");
                detailsTable.Rows[3].Cells[1].Paragraphs[0].Append(booking.Nights.ToString());
    
                detailsTable.Rows[4].Cells[0].Paragraphs[0].Append("Status");
                detailsTable.Rows[4].Cells[1].Paragraphs[0].Append(booking.Status);
    
                detailsTable.Rows[5].Cells[0].Paragraphs[0].Append("Total Cost");
                detailsTable.Rows[5].Cells[1].Paragraphs[0].Append(booking.TotalCost.ToString("C"));
    
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