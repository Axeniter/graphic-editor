using SkiaSharp;

namespace GraphicEditor.Models
{
    public interface IImageOperation
    {
        string Name { get; }
        SKBitmap ApplyOperation(SKBitmap bitmap);
    }

    public interface ITool : IImageOperation
    {
        void BeginInteraction();
        void UpdateInteraction();
    }
}
