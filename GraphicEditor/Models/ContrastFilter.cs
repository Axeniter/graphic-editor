using Avalonia.Media.Imaging;
using System;

namespace GraphicEditor.Models
{
    public class ContrastFilter : IFilter
    {
        public string Name => "Contrast";
        public float MinIntensity => 0.5f;
        public float MaxIntensity => 2f;
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
                    for (int channel = 0; channel < 3; channel++)
                    {
                        float adjusted = ((ptr[i + channel] / 255.0f) - 0.5f) * Intensity + 0.5f;
                        ptr[i + channel] = (byte)Math.Clamp(adjusted * 255, 0, 255);
                    }
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
