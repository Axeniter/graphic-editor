using SkiaSharp;
using System;

namespace GraphicEditor.Models
{
    public class ContrastFilter : IFilter
    {
        public string Name => "Contrast";
        public float MinIntensity => 0.5f;
        public float MaxIntensity => 2f;
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
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float pixel = srcPtr[i + channel] / 255.0f;
                        float adjusted = ((pixel - 0.5f) * Intensity) + 0.5f;
                        dstPtr[i + channel] = (byte)Math.Clamp(adjusted * 255, 0, 255);
                    }

                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
