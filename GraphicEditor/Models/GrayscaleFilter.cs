using SkiaSharp;

namespace GraphicEditor.Models
{
    public class GrayscaleFilter : IFilter
    {
        public string Name => $"Grayscale";
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

                    byte gray = (byte)(0.299f * r + 0.587f * g + 0.114f * b);

                    dstPtr[i] = (byte)(b + (gray - b) * Intensity);
                    dstPtr[i + 1] = (byte)(g + (gray - g) * Intensity);
                    dstPtr[i + 2] = (byte)(r + (gray - r) * Intensity);
                    dstPtr[i + 3] = srcPtr[i + 3];
                }
            }

            return result;
        }
    }
}
