using SkiaSharp;

namespace GraphicEditor.Models
{
    public class InvertFilter : IFilter
    {
        public string Name => "Invert";
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
                    for (int channel = 0; channel < 3; channel++)
                    {
                        byte original = srcPtr[i + channel];
                        byte inverted = (byte)(255 - original);

                        byte final = (byte)(original + (inverted - original) * Intensity);
                        dstPtr[i + channel] = final;
                    }

                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
