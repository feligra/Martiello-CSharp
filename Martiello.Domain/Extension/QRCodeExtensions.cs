using QRCoder;

namespace Martiello.Domain.Extension
{
    public static class QRCodeExtensions
    {        
        public static byte[] ToQRCode(this string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("O conteúdo do QR Code não pode ser nulo ou vazio.", nameof(content));

            using var qrGenerator = new QRCodeGenerator();
            using var qrCodeData = qrGenerator.CreateQrCode(content, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrCodeData);

            return qrCode.GetGraphic(20);
        }
    }
}
