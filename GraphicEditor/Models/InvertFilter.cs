using Avalonia.Media.Imaging;

namespace GraphicEditor.Models
{
    public class InvertFilter : IFilter
    {
        public string Name => "Invert";
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
                    for (int channel = 0; channel < 3; channel++)
                    {
                        byte original = ptr[i + channel];
                        byte inverted = (byte)(255 - original);

                        byte final = (byte)(original + (inverted - original) * Intensity);
                        ptr[i + channel] = final;
                    }
                }
            }

            return result.ToWriteableBitmap();
        }
    }
}
