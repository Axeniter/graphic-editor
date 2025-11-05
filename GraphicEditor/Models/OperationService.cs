namespace GraphicEditor.Models
{
    public class OperationService
    {
        public IImageOperation RotateLeft { get; } = new RotateOperation(-90);
        public IImageOperation RotateRight { get; } = new RotateOperation(90);
        public IFilter Blur { get; } = new BlurFilter();
        public IFilter Brightness { get; } = new BrightnessFilter();
        public IFilter Invert { get; } = new InvertFilter();
        public IFilter Saturation { get; } = new SaturationFilter();
        public IFilter Contrast { get; } = new ContrastFilter();
        public IFilter Grayscale { get; } = new GrayscaleFilter();
        public IFilter Sepia { get; } = new SepiaFilter();
    }
}
