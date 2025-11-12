using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;

namespace GraphicEditor.Views
{
    public partial class CropDialog : Window
    {
        public Rect CropRect { get; private set; }

        public CropDialog(WriteableBitmap bitmap)
        {
            InitializeComponent();
            SetImageSize(bitmap.PixelSize.Width, bitmap.PixelSize.Height);
        }

        public void SetImageSize(int width, int height)
        {
            StartXBox.Maximum = width - 1;
            StartYBox.Maximum = height - 1;
            EndXBox.Maximum = width;
            EndYBox.Maximum = height;

            StartXBox.Value = 0;
            StartYBox.Value = 0;
            EndXBox.Value = width;
            EndYBox.Value = height;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close(null);
        }

        private void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            if (StartXBox.Value.HasValue && StartYBox.Value.HasValue &&
                EndXBox.Value.HasValue && EndYBox.Value.HasValue)
            {
                var startX = (double)StartXBox.Value.Value;
                var startY = (double)StartYBox.Value.Value;
                var endX = (double)EndXBox.Value.Value;
                var endY = (double)EndYBox.Value.Value;

                var x = System.Math.Min(startX, endX);
                var y = System.Math.Min(startY, endY);
                var width = System.Math.Abs(endX - startX);
                var height = System.Math.Abs(endY - startY);

                CropRect = new Rect(x, y, width, height);
                Close(CropRect);
            }
        }
    }
}