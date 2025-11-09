using Avalonia.Media.Imaging;

namespace GraphicEditor.Models
{
    public class GrayscaleFilter : IFilter
    {
        public string Name => "Grayscale";
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

                    byte gray = (byte)(0.299f * r + 0.587f * g + 0.114f * b);

                    ptr[i] = (byte)(b + (gray - b) * Intensity);
                    ptr[i + 1] = (byte)(g + (gray - g) * Intensity);
                    ptr[i + 2] = (byte)(r + (gray - r) * Intensity);
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
