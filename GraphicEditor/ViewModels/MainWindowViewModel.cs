using GraphicEditor.Models;
using ReactiveUI;
using System.Reactive;

namespace GraphicEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ImageEditor _editor;
        private int _newWidth = 150;
        private int _newHeight = 100;
        private string _path = "fafa";

        public MainWindowViewModel()
        {
            _editor = new ImageEditor();
            _editor.NewFile(800,600);

            UndoCommand = ReactiveCommand.Create(_editor.Undo);
            RedoCommand = ReactiveCommand.Create(_editor.Redo);
            NewFileCommand = ReactiveCommand.Create(() => _editor.NewFile(_newWidth, _newHeight));
            OpenFileCommand = ReactiveCommand.Create(() => _editor.OpenFile(_path));
        }

        public ImageEditor Editor => _editor;

        public ReactiveCommand<Unit, Unit> NewFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }
    }
}
