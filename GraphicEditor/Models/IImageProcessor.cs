using SkiaSharp;

namespace GraphicEditor.Models
{
    public interface IImageProcessor
    {
        SKBitmap Process(SKBitmap bitmap);
    }
}
