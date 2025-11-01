using GraphicEditor.Models;
using ReactiveUI;

namespace GraphicEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ImageEditor _editor;

        public MainWindowViewModel()
        {
            _editor = new ImageEditor();
            _editor.NewImage(800,600);
        }

        public ImageEditor Editor => _editor;
    }
}
