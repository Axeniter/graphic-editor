using Avalonia.Media.Imaging;
using SkiaSharp;

namespace GraphicEditor.Models
{
    public class BlurFilter : IFilter
    {
        public float MinIntensity => 0f;
        public float MaxIntensity => 20f;
        public float Intensity { get; set; } = 1f;
        public string Name => "Blur";

        public WriteableBitmap ProcessImage(WriteableBitmap bitmap)
        {
            using var result = bitmap.ToSKBitmap();

            using var canvas = new SKCanvas(result);
            using var paint = new SKPaint();

            paint.ImageFilter = SKImageFilter.CreateBlur(Intensity, Intensity);
            canvas.DrawBitmap(result, 0, 0, paint);

            return result.ToWriteableBitmap();
        }
    }
}