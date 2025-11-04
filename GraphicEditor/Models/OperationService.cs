namespace GraphicEditor.Models
{
    public class OperationService
    {
        public IImageOperation RotateLeft { get; } = new RotateOperation(-90);
        public IImageOperation RotateRight { get; } = new RotateOperation(90);
    }
}
