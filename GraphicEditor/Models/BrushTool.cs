using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using System;

namespace GraphicEditor.Models
{
    public class BrushTool : ReactiveObject, ITool
    {
        private WriteableBitmap _bitmap;
        private Point _previousPoint;
        private int _brushSize;
        private Color _brushColor;

        public BrushTool()
        {
            _brushColor = Colors.Black;
            _brushSize = 1;
            _bitmap = null;
        }

        public int BrushSize
        {
            get => _brushSize;
            set => this.RaiseAndSetIfChanged(ref _brushSize, Math.Max(0, value));
        }

        public Color BrushColor
        {
            get => _brushColor;
            set => this.RaiseAndSetIfChanged(ref _brushColor, value);
        }

        public void BeginInteraction(Point position, WriteableBitmap image)
        {
            _bitmap = image ?? throw new ArgumentNullException(nameof(image));
            _previousPoint = position;

            DrawPoint(position);
        }

        public void UpdateInteraction(Point position)
        {
            DrawLine(_previousPoint, position);
            _previousPoint = position;
        }

        public void EndInteraction(Point position)
        {
            DrawPoint(position);
            _bitmap = null;
        }

        private void DrawPoint(Point point)
        {
            using (var bitmapBuffer = _bitmap.Lock())
            {
                int x = (int)point.X;
                int y = (int)point.Y;

                DrawCircle(bitmapBuffer, x, y, _brushSize, BrushColor.ToUInt32());
            }
        }

        private void DrawLine(Point from, Point to)
        {
            using (var bitmapBuffer = _bitmap.Lock())
            {
                int x0 = (int)from.X;
                int y0 = (int)from.Y;
                int x1 = (int)to.X;
                int y1 = (int)to.Y;

                DrawBresenhamLine(bitmapBuffer, x0, y0, x1, y1, BrushColor.ToUInt32());
            }
        }

        private void DrawBresenhamLine(ILockedFramebuffer bitmapBuffer, int x0, int y0, int x1, int y1, uint color)
        {
            int dx = Math.Abs(x1 - x0);
            int dy = Math.Abs(y1 - y0);
            int sx = x0 < x1 ? 1 : -1;
            int sy = y0 < y1 ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                DrawCircle(bitmapBuffer, x0, y0, _brushSize, color);

                if (x0 == x1 && y0 == y1) break;

                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x0 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y0 += sy;
                }
            }
        }

        private void DrawCircle(ILockedFramebuffer bitmapBuffer, int centerX, int centerY, int radius, uint color)
        {
            int stride = bitmapBuffer.RowBytes;
            var size = _bitmap.Size;

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    if (x * x + y * y <= radius * radius)
                    {
                        int px = centerX + x;
                        int py = centerY + y;

                        if (px >= 0 && px < size.Width && py >= 0 && py < size.Height)
                        {
                            SetPixel(bitmapBuffer, px, py, color, stride);
                        }
                    }
                }
            }
        }

        private unsafe void SetPixel(ILockedFramebuffer bitmapBuffer, int x, int y, uint color, int stride)
        {
            var address = bitmapBuffer.Address;
            var bytesPerPixel = bitmapBuffer.Format.BitsPerPixel / 8;

            byte* ptr = (byte*)address + (y * stride) + (x * bytesPerPixel);

            *(uint*)ptr = color;
        }
    }
}