using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphicEditor.Models
{
    public class BrightnessFilter : IFilter
    {
        public string Name => "Brightness";

        public float MaxIntensity => 1f;
        public float MinIntensity => -1f;
        public float Intensity { get; set; } = 0;

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
                        float pixel = srcPtr[i + channel];
                        float adjusted = pixel + (Intensity * 255);
                        dstPtr[i + channel] = (byte)Math.Clamp(adjusted, 0, 255);
                    }

                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
