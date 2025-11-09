using Avalonia.Media.Imaging;
using System;

namespace GraphicEditor.Models
{
    public class BrightnessFilter : IFilter
    {
        public string Name => "Brightness";
        public float MaxIntensity => 1f;
        public float MinIntensity => -1f;
        public float Intensity { get; set; } = 0;

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
                        float adjusted = ptr[i + channel] + (Intensity * 255);
                        ptr[i + channel] = (byte)Math.Clamp(adjusted, 0, 255);
                    }
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
