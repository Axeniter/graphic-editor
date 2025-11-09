using Avalonia.Media.Imaging;
using Avalonia;

namespace GraphicEditor.Models
{
    public interface IImageOperation
    {
        string Name { get; }
        WriteableBitmap ProcessImage(WriteableBitmap bitmap);
    }

    public interface ITool
    {
        void BeginInteraction(Point position, WriteableBitmap image);
        void UpdateInteraction(Point position);
        void EndInteraction(Point position);
    }

    public interface IFilter : IImageOperation
    {
        float MinIntensity { get; }
        float MaxIntensity { get; }
        float Intensity { get; set; }
    }
}
