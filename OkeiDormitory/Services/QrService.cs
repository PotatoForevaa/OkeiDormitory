using Microsoft.EntityFrameworkCore;
using OkeiDormitory.Data;
using QRCoder;


namespace OkeiDormitory.Services
{
    public class QrService
    {
        private readonly DormitoryDbContext _context;
        public QrService(DormitoryDbContext context)
        {
            _context = context;
        }
        public async Task CreateAllQrCodesAsync(string? outputDir = null)
        {
            var qrGenerator = new QRCodeGenerator();
            foreach (var room in await _context.Rooms.ToListAsync())
            {
                string outputPath;
                if (outputDir == null)
                {
                    outputPath = Path.Combine("wwwroot", "QrCodes", $"Room{room.Number}Qr.pdf");
                }
                else
                {
                    outputPath = Path.Combine(outputDir, $"Room{room.Number}Qr.pdf");
                }

                var qrData = qrGenerator.CreateQrCode(@"http://localhost:5025/home/inspection?roomNumber=" + $"{room.Number}", QRCodeGenerator.ECCLevel.Q);
                PdfByteQRCode qrCode = new PdfByteQRCode(qrData);
                var qrPdfBytes = qrCode.GetGraphic(20);
                File.WriteAllBytes(outputPath, qrPdfBytes);
            }
        }
    }
}
