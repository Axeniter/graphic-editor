using Avalonia;
using Avalonia.Media.Imaging;
using GraphicEditor.Models;
using SkiaSharp;
using System;

public class CropOperation : IImageOperation
{
    public string Name => "Crop";

    public Rect CropRect { get; set; }

    public WriteableBitmap ProcessImage(WriteableBitmap source)
    {
        using var bitmap = source.ToSKBitmap();

        var cropX = Math.Max(0, Math.Min(CropRect.X, bitmap.Width - 1));
        var cropY = Math.Max(0, Math.Min(CropRect.Y, bitmap.Height - 1));
        var cropWidth = Math.Max(1, Math.Min(CropRect.Width, bitmap.Width - cropX));
        var cropHeight = Math.Max(1, Math.Min(CropRect.Height, bitmap.Height - cropY));

        var cropped = new SKBitmap((int)cropWidth, (int)cropHeight);

        using (var canvas = new SKCanvas(cropped))
        {
            var sourceRect = new SKRect(
                (float)cropX,
                (float)cropY,
                (float)(cropX + cropWidth),
                (float)(cropY + cropHeight));

            canvas.DrawBitmap(bitmap, sourceRect, new SKRect(0, 0, cropped.Width, cropped.Height));
        }

        return cropped.ToWriteableBitmap();
    }
}