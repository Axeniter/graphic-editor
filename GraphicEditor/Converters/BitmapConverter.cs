using Avalonia.Data.Converters;
using Avalonia.Media.Imaging;
using SkiaSharp;
using System;
using System.Globalization;
using System.IO;


namespace GraphicEditor.Converters
{
    public class BitmapConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is SKBitmap skBitmap && skBitmap != null)
            {
                try
                {
                    using var memoryStream = new MemoryStream();
                    using var skData = skBitmap.Encode(SKEncodedImageFormat.Png, 100);
                    skData.SaveTo(memoryStream);
                    memoryStream.Position = 0;
                    return new Bitmap(memoryStream);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error converting SKBitmap: {ex.Message}");
                    return null;
                }
            }
            return null;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

