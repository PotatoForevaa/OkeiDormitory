using ZXing;

namespace OkeiDormitory.Services
{
    public class Qr
    {
        public static void CreateQr()
        {
            var writer = new BarcodeWriter<SkiaSharp.SKBitmap>();
            writer.Format = BarcodeFormat.QR_CODE;
            writer.Options = new ZXing.Common.EncodingOptions()
            {
                Height = 150,
                Width = 150,
                NoPadding = true,
                Margin = 0,
                PureBarcode = true,
            };
            var bitmap = writer.Write("asdaf");
        }
    }
}
