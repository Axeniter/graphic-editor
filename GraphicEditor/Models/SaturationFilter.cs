using Avalonia.Media.Imaging;
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

        public WriteableBitmap ProcessImage(WriteableBitmap bitmap)
        {
            using var result = bitmap.ToSKBitmap();

            unsafe
            {
                byte* ptr = (byte*)result.GetPixels().ToPointer();

                int totalPixels = result.Width * result.Height * 4;

                for (int i = 0; i < totalPixels; i += 4)
                {
                    float b = ptr[i] / 255.0f;
                    float g = ptr[i + 1] / 255.0f;
                    float r = ptr[i + 2] / 255.0f;

                    float luminance = 0.299f * r + 0.587f * g + 0.114f * b;

                    b = luminance + (b - luminance) * Intensity;
                    g = luminance + (g - luminance) * Intensity;
                    r = luminance + (r - luminance) * Intensity;

                    ptr[i] = (byte)Math.Clamp(b * 255, 0, 255);
                    ptr[i + 1] = (byte)Math.Clamp(g * 255, 0, 255);
                    ptr[i + 2] = (byte)Math.Clamp(r * 255, 0, 255);
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
