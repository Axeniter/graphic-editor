using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using GraphicEditor.Models;
using GraphicEditor.Views;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;

namespace GraphicEditor.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ImageEditor _editor;
        private IFileDialogService _fileDialogService;

        public MainWindowViewModel()
        {
            _editor = new ImageEditor();
            _fileDialogService = new FileDialogService();
            _editor.NewFile(800,600);

            UndoCommand = ReactiveCommand.Create(_editor.Undo);
            RedoCommand = ReactiveCommand.Create(_editor.Redo);
            OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.CreateFromTask(SaveFileAsync);
            NewFileCommand = ReactiveCommand.CreateFromTask(ShowNewFileDialogAsync);
        }

        public ImageEditor Editor => _editor;

        private async Task ShowNewFileDialogAsync()
        {
            var dialog = new NewFileDialog();

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop.MainWindow != null)
                {
                    await dialog.ShowDialog(desktop.MainWindow);
                    if (dialog.IsCreated)
                    {
                        Editor.NewFile(dialog.FileWidth, dialog.FileHeight);
                    }
                }
            }
        }

        private async Task OpenFileAsync()
        {
            var filePath = await _fileDialogService.ShowOpenFileDialogAsync();
            if (!string.IsNullOrEmpty(filePath))
            {
                Editor.OpenFile(filePath);
            }
        }

        private async Task SaveFileAsync()
        {
            var filePath = await _fileDialogService.ShowSaveFileDialogAsync();
            if (!string.IsNullOrEmpty(filePath))
            {
                Editor.SaveFile(filePath);
            }
        }


        public ReactiveCommand<Unit, Unit> NewFileCommand { get; }
        public ReactiveCommand<Unit, Unit> SaveFileCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenFileCommand { get; }
        public ReactiveCommand<Unit, Unit> UndoCommand { get; }
        public ReactiveCommand<Unit, Unit> RedoCommand { get; }
    }
}
