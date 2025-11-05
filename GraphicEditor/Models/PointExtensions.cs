using SkiaSharp;
using Avalonia;

namespace GraphicEditor.Models
{
    public static class PointExtensions
    {
        public static SKPoint ToSKPoint(this Point point)
        {
            return new SKPoint((float)point.X, (float)point.Y);
        }
    }
}
