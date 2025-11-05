using SkiaSharp;
using System;

namespace GraphicEditor.Models
{
    public class SepiaFilter : IFilter
    {
        public string Name => "Sepia";
        public float MinIntensity => 1f;
        public float MaxIntensity => 1f;
        public float Intensity { get; set; } = 1f;

        public SKBitmap ProcessImage(SKBitmap bitmap)
        {
            if (bitmap == null) return null;

            var result = new SKBitmap(bitmap.Width, bitmap.Height, bitmap.ColorType, bitmap.AlphaType);

            unsafe
            {
                byte* srcPtr = (byte*)bitmap.GetPixels().ToPointer();
                byte* dstPtr = (byte*)result.GetPixels().ToPointer();

                int totalPixels = bitmap.Width * bitmap.Height * 4;

                for (int i = 0; i < totalPixels; i += 4)
                {
                    byte b = srcPtr[i];
                    byte g = srcPtr[i + 1];
                    byte r = srcPtr[i + 2];

                    float tr = 0.393f * r + 0.769f * g + 0.189f * b;
                    float tg = 0.349f * r + 0.686f * g + 0.168f * b;
                    float tb = 0.272f * r + 0.534f * g + 0.131f * b;

                    dstPtr[i] = (byte)(b + (Math.Clamp(tb, 0, 255) - b) * Intensity);
                    dstPtr[i + 1] = (byte)(g + (Math.Clamp(tg, 0, 255) - g) * Intensity);
                    dstPtr[i + 2] = (byte)(r + (Math.Clamp(tr, 0, 255) - r) * Intensity);
                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
