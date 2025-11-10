using Avalonia.Controls;
using Avalonia.Input;
using GraphicEditor.ViewModels;

namespace GraphicEditor.Views
{
    public partial class MainWindow : Window
    {
        bool _isDrawing = false;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            if (DataContext is MainWindowViewModel vm && vm.CurrentTool != null)
            {
                var position = e.GetPosition(Canvas);
                var bitmap = vm.Editor.CurrentImage;

                vm.Editor.SaveState();
                vm.CurrentTool.BeginInteraction(position, bitmap);
                _isDrawing = true;
                Canvas.InvalidateVisual();
            }
        }

        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            if (_isDrawing && DataContext is MainWindowViewModel vm && vm.CurrentTool != null)
            {
                var position = e.GetPosition(Canvas);
                vm.CurrentTool.UpdateInteraction(position);
                Canvas.InvalidateVisual();
            }
        }

        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            if (_isDrawing && DataContext is MainWindowViewModel vm && vm.CurrentTool != null)
            {
                var position = e.GetPosition(Canvas);
                vm.CurrentTool.EndInteraction(position);
                _isDrawing = false;
                Canvas.InvalidateVisual();
            }
        }
    }
}