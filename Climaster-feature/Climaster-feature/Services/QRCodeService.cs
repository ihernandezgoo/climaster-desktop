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
                var qrGenerator = new QRCodeGenerator();
                var qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                var qrCode = new QRCode(qrCodeData);
                
                // Don't use 'using' here because we're returning the Bitmap
                // The caller is responsible for disposing it (or BitmapHelper will handle it)
                var bitmap = qrCode.GetGraphic(20);
                
                // Cleanup QR objects but NOT the bitmap
                qrCode.Dispose();
                qrCodeData.Dispose();
                qrGenerator.Dispose();
                
                return bitmap;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al generar bitmap del código QR: {ex.Message}", ex);
            }
        }
    }
}
