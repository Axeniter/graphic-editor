using Avalonia;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System;

namespace GraphicEditor.Models
{
    public static class WriteableBitmapExtensions
    {
        public static WriteableBitmap Copy(this WriteableBitmap source)
        {
            var copy = new WriteableBitmap(
                new PixelSize(source.PixelSize.Width, source.PixelSize.Height),
                new Vector(96, 96),
                source.Format,
                source.AlphaFormat);

            using (var sourceBuffer = source.Lock())
            using (var copyBuffer = copy.Lock())
            {
                unsafe
                {
                    var sourcePtr = (byte*)sourceBuffer.Address;
                    var copyPtr = (byte*)copyBuffer.Address;

                    var bytesToCopy = sourceBuffer.RowBytes * sourceBuffer.Size.Height;
                    Buffer.MemoryCopy(sourcePtr, copyPtr, bytesToCopy, bytesToCopy);
                }
            }
            return copy;
        }

        public static SKBitmap ToSKBitmap(this WriteableBitmap source)
        {
            if (source == null) return new SKBitmap();

            var width = source.PixelSize.Width;
            var height = source.PixelSize.Height;

            var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);

            using (var buffer = source.Lock())
            {
                unsafe
                {
                    var sourcePtr = (byte*)buffer.Address;
                    var targetPtr = (byte*)bitmap.GetPixels().ToPointer();

                    for (int y = 0; y < height; y++)
                    {
                        var sourceRow = sourcePtr + y * buffer.RowBytes;
                        var targetRow = targetPtr + y * bitmap.RowBytes;

                        Buffer.MemoryCopy(sourceRow, targetRow, bitmap.RowBytes, buffer.RowBytes);
                    }
                }
            }
            return bitmap;
        }
    }
}