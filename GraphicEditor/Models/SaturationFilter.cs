using SkiaSharp;
using System;

namespace GraphicEditor.Models
{
    public class SaturationFilter : IFilter
    {
        public string Name => "Saturation";

        public float MinIntensity => 0f;

        public float MaxIntensity => 2f;

        public float Intensity { get; set; } = 0.5f;

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
                    float b = srcPtr[i] / 255.0f;
                    float g = srcPtr[i + 1] / 255.0f;
                    float r = srcPtr[i + 2] / 255.0f;

                    float luminance = 0.299f * r + 0.587f * g + 0.114f * b;

                    r = luminance + (r - luminance) * Intensity;
                    g = luminance + (g - luminance) * Intensity;
                    b = luminance + (b - luminance) * Intensity;

                    dstPtr[i] = (byte)Math.Clamp(b * 255, 0, 255);
                    dstPtr[i + 1] = (byte)Math.Clamp(g * 255, 0, 255);
                    dstPtr[i + 2] = (byte)Math.Clamp(r * 255, 0, 255);
                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
