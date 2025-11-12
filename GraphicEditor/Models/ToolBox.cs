namespace GraphicEditor.Models
{
    public class ToolBox
    {
        public IImageOperation RotateLeft { get; } = new RotateOperation(-90);
        public IImageOperation RotateRight { get; } = new RotateOperation(90);
        public CropOperation Crop { get; } = new CropOperation();

        public IFilter Blur { get; } = new BlurFilter();
        public IFilter Brightness { get; } = new BrightnessFilter();
        public IFilter Invert { get; } = new InvertFilter();
        public IFilter Saturation { get; } = new SaturationFilter();
        public IFilter Contrast { get; } = new ContrastFilter();
        public IFilter Grayscale { get; } = new GrayscaleFilter();
        public IFilter Sepia { get; } = new SepiaFilter();

        public BrushTool Brush { get; } = new BrushTool();
        //public CropTool Crop { get; } = new CropTool();
        //public SelectionTool Selection { get; } = new SelectionTool();
    }
}
