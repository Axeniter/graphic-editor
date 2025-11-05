using SkiaSharp;

namespace GraphicEditor.Models
{
    public interface IImageOperation
    {
        string Name { get; }
        SKBitmap ProcessImage(SKBitmap bitmap);
    }

    public interface ITool : IImageOperation
    {
        void BeginInteraction(SKPoint position, SKBitmap image);
        void UpdateInteraction(SKPoint position);
        void EndInteraction(SKPoint position);
    }
}
