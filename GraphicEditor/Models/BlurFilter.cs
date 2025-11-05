using SkiaSharp;

namespace GraphicEditor.Models
{
    internal class BlurFilter : IFilter
    {
        public float MinIntensity => 0f;
        public float MaxIntensity => 20f;
        public float Intensity { get; set; } = 1f;
        public string Name => "Blur";

        public SKBitmap ProcessImage(SKBitmap bitmap)
        {
            if (bitmap == null) return null;

            var result = new SKBitmap(bitmap.Width, bitmap.Height, bitmap.ColorType, bitmap.AlphaType);

            using var canvas = new SKCanvas(result);
            using var paint = new SKPaint();

            paint.ImageFilter = SKImageFilter.CreateBlur(Intensity, Intensity);
            canvas.DrawBitmap(bitmap, 0, 0, paint);

            return result;
        }
    }
}
