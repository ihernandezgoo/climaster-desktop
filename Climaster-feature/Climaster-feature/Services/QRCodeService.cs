using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace Climaster_feature.Services
{
    public class QRCodeService
    {
        public void GenerateQRCode(string data, string fileName)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrCodeData);
                using var qrCodeImage = qrCode.GetGraphic(20);
                
                qrCodeImage.Save(fileName, ImageFormat.Png);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar código QR: {ex.Message}", ex);
            }
        }

        public Bitmap GenerateQRCodeBitmap(string data)
        {
            try
            {
                using var qrGenerator = new QRCodeGenerator();
                using var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                using var qrCode = new QRCode(qrCodeData);
                
                // Return the bitmap - it will be disposed by BitmapHelper
                return qrCode.GetGraphic(20);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar bitmap del código QR: {ex.Message}", ex);
            }
        }
    }
}
