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

        public SKBitmap ProcessImage(SKBitmap bitmap)
        {
            var newWidth = bitmap.Height;
            var newHeight = bitmap.Width;

            var rotatedBitmap = new SKBitmap(newWidth, newHeight, bitmap.ColorType, bitmap.AlphaType);

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

                canvas.DrawBitmap(bitmap, 0, 0, paint);
            }

            return rotatedBitmap;
        }
    }


}
