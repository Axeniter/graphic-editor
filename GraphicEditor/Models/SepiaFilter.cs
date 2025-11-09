using Avalonia.Media.Imaging;
using System;

namespace GraphicEditor.Models
{
    public class SepiaFilter : IFilter
    {
        public string Name => "Sepia";
        public float MinIntensity => 1f;
        public float MaxIntensity => 1f;
        public float Intensity { get; set; } = 1f;

        public WriteableBitmap ProcessImage(WriteableBitmap bitmap)
        {
            using var result = bitmap.ToSKBitmap();

            unsafe
            {
                byte* ptr = (byte*)result.GetPixels().ToPointer();

                int totalPixels = result.Width * result.Height * 4;

                for (int i = 0; i < totalPixels; i += 4)
                {
                    byte b = ptr[i];
                    byte g = ptr[i + 1];
                    byte r = ptr[i + 2];

                    float tb = 0.272f * r + 0.534f * g + 0.131f * b;
                    float tg = 0.349f * r + 0.686f * g + 0.168f * b;
                    float tr = 0.393f * r + 0.769f * g + 0.189f * b;

                    ptr[i] = (byte)(b + (Math.Clamp(tb, 0, 255) - b) * Intensity);
                    ptr[i + 1] = (byte)(g + (Math.Clamp(tg, 0, 255) - g) * Intensity);
                    ptr[i + 2] = (byte)(r + (Math.Clamp(tr, 0, 255) - r) * Intensity);
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
