using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

namespace GraphicEditor.Models
{
    public static class SKBitmapExtensions
    {
        public static WriteableBitmap ToWriteableBitmap(this SKBitmap source)
        {
            var size = new PixelSize(source.Width, source.Height);
            var result = new WriteableBitmap(size, new Vector(96, 96), PixelFormat.Bgra8888);

            using (var buffer = result.Lock())
            using (var skImage = SKImage.FromBitmap(source))
            {
                var imageInfo = new SKImageInfo(size.Width, size.Height, SKColorType.Bgra8888);
                skImage.ReadPixels(imageInfo, buffer.Address, buffer.RowBytes, 0, 0);
            }
            return result;
        }
    }
}