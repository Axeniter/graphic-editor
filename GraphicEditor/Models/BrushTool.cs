using SkiaSharp;

namespace GraphicEditor.Models
{
    internal class BrushTool : ITool
    {
        private SKBitmap _currentBitmap;
        private SKCanvas _currentCanvas;
        private SKPaint _paint;
        private bool _isDrawing = false;
        private SKPoint _lastPoint;
        public string Name => "Brush";
        public SKPaint Paint => _paint;

        public BrushTool()
        {
            _paint = new SKPaint
            {
                Color = SKColors.Black,
                StrokeWidth = 5,
                IsAntialias = true,
                Style = SKPaintStyle.Stroke,
                StrokeCap = SKStrokeCap.Round,
                StrokeJoin = SKStrokeJoin.Round
            };
        }

        public SKBitmap ProcessImage(SKBitmap bitmap)
        {
            return _currentBitmap;
        }

        public void BeginInteraction(SKPoint position, SKBitmap image)
        {
            _currentBitmap = image;
            _currentCanvas = new SKCanvas(image);
            _isDrawing = true;
            _currentCanvas.DrawPoint(position, _paint);
            _lastPoint = position;
        }

        public void UpdateInteraction(SKPoint position)
        {
            _currentCanvas.DrawLine(_lastPoint, position, _paint);
            _lastPoint = position;
        }

        public void EndInteraction(SKPoint position)
        {
            _currentCanvas.DrawLine(_lastPoint, position, _paint);
            _lastPoint = position;
            _isDrawing = false;
        }
    }
}