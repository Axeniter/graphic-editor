using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
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
        private OperationService _operationService;
        private IFileDialogService _fileDialogService;
        private ITool _currentTool;
        private BrushTool _brushTool;

        public MainWindowViewModel()
        {
            _editor = new ImageEditor();
            _operationService = new OperationService();
            _fileDialogService = new FileDialogService();
            _brushTool = new BrushTool();
            CurrentTool = _brushTool;

            _editor.NewFile(800,600);

            UndoCommand = ReactiveCommand.Create(_editor.Undo);
            RedoCommand = ReactiveCommand.Create(_editor.Redo);
            OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.CreateFromTask(SaveFileAsync);
            NewFileCommand = ReactiveCommand.CreateFromTask(ShowNewFileDialogAsync);
            RotateLeftCommand = ReactiveCommand.Create(() => Editor.ApplyOperation(_operationService.RotateLeft));
            RotateRightCommand = ReactiveCommand.Create(() => Editor.ApplyOperation(_operationService.RotateRight));
            BlurCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Blur));
            BrightnessCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Brightness));
            InvertCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Invert));
            SaturationCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Saturation));
            ContrastCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Contrast));
            GrayscaleCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Grayscale));
            SepiaCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_operationService.Sepia));
        }
        private Color _drawColor = Colors.Red;

        public BrushTool BrushTool => _brushTool;
        public Color DrawColor
        {
            get => _drawColor;
            set => this.RaiseAndSetIfChanged(ref _drawColor, value);
        }
        public ImageEditor Editor => _editor;
        public ITool CurrentTool
        {
            get => _currentTool;
            set => this.RaiseAndSetIfChanged(ref _currentTool, value);
        }

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

        private async Task ShowFilterDialogAsync(IFilter filter)
        {
            var dialog = new FilterDialog(filter);
            

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop?.MainWindow != null)
                {
                    await dialog.ShowDialog(desktop.MainWindow);
                    if (dialog.IsAplied)
                    {
                        filter.Intensity = dialog.Intensity;
                        Editor.ApplyOperation(filter);
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
        public ReactiveCommand<Unit, Unit> RotateLeftCommand { get; }
        public ReactiveCommand<Unit, Unit> RotateRightCommand { get; }
        public ReactiveCommand<Unit, Unit> BlurCommand { get; }
        public ReactiveCommand<Unit, Unit> BrightnessCommand { get; }
        public ReactiveCommand<Unit, Unit> InvertCommand { get; }
        public ReactiveCommand<Unit, Unit> SaturationCommand { get; }
        public ReactiveCommand<Unit, Unit> ContrastCommand { get; }
        public ReactiveCommand<Unit, Unit> GrayscaleCommand { get; }
        public ReactiveCommand<Unit, Unit> SepiaCommand { get; }
    }
}
