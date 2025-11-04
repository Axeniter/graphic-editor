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
        void BeginInteraction();
        void UpdateInteraction();
        void EndInteraction();
    }
}
