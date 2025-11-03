using Avalonia.Controls;
using Avalonia.Interactivity;

namespace GraphicEditor.Views
{
    public partial class NewFileDialog : Window
    {
        public bool IsCreated { get; private set; }
        public int FileWidth { get; private set; }
        public int FileHeight { get; private set; }

        public NewFileDialog()
        {
            InitializeComponent();
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (WidthBox?.Value is decimal width && HeightBox?.Value is decimal height)
            {
                FileWidth = (int)width;
                FileHeight = (int)height;
                IsCreated = true;
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsCreated = false;
            Close();
        }
    }
}