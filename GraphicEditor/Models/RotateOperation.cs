using Avalonia.Media.Imaging;
using SkiaSharp;

namespace GraphicEditor.Models
{
    public class RotateOperation : IImageOperation
    {
        private readonly float _angle;

        public string Name
        {
            get
            {
                if (_angle >= 0) return "Rotate right";
                else return "Rotate left";
            }
        }

        public RotateOperation(float angle)
        {
            _angle = angle;
        }

        public WriteableBitmap ProcessImage(WriteableBitmap bitmap)
        {
            using var skbitmap = bitmap.ToSKBitmap();
            var newWidth = skbitmap.Height;
            var newHeight = skbitmap.Width;

            using var rotatedBitmap = new SKBitmap(newWidth, newHeight, skbitmap.ColorType, skbitmap.AlphaType);

            using (var canvas = new SKCanvas(rotatedBitmap))
            using (var paint = new SKPaint { IsAntialias = true })
            {
                if (_angle >= 0)
                {
                    canvas.Translate(newWidth, 0);
                    canvas.RotateDegrees(90);
                }
                else
                {
                    canvas.Translate(0, newHeight);
                    canvas.RotateDegrees(-90);
                }

                canvas.DrawBitmap(skbitmap, 0, 0, paint);
            }

            return rotatedBitmap.ToWriteableBitmap();
        }
    }
}
