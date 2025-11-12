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
        private ToolBox _toolBox;
        private IFileDialogService _fileDialogService;
        private ITool _currentTool;

        public MainWindowViewModel()
        {
            _editor = new ImageEditor();
            _toolBox = new ToolBox();
            _fileDialogService = new FileDialogService();

            _editor.NewFile(800,600);

            UndoCommand = ReactiveCommand.Create(_editor.Undo);
            RedoCommand = ReactiveCommand.Create(_editor.Redo);
            OpenFileCommand = ReactiveCommand.CreateFromTask(OpenFileAsync);
            SaveFileCommand = ReactiveCommand.CreateFromTask(SaveFileAsync);
            NewFileCommand = ReactiveCommand.CreateFromTask(ShowNewFileDialogAsync);
            CropCommand = ReactiveCommand.CreateFromTask(ShowCropDialogAsync);
            RotateLeftCommand = ReactiveCommand.Create(() => Editor.ApplyOperation(_toolBox.RotateLeft));
            RotateRightCommand = ReactiveCommand.Create(() => Editor.ApplyOperation(_toolBox.RotateRight));
            BlurCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Blur));
            BrightnessCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Brightness));
            InvertCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Invert));
            SaturationCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Saturation));
            ContrastCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Contrast));
            GrayscaleCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Grayscale));
            SepiaCommand = ReactiveCommand.CreateFromTask(() => ShowFilterDialogAsync(_toolBox.Sepia));
            SetToolCommand = ReactiveCommand.Create<ITool>(SetTool);
        }
        private Color _drawColor = Colors.Red;

        public ToolBox ToolBox => _toolBox;
        public Color DrawColor
        {
            get => _drawColor;
            set => this.RaiseAndSetIfChanged(ref _drawColor, value);
        }
        public ImageEditor Editor => _editor;
        public ITool CurrentTool
        {
            get => _currentTool;
            private set => this.RaiseAndSetIfChanged(ref _currentTool, value);
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

        private async Task ShowCropDialogAsync()
        {
            var dialog = new CropDialog(Editor.CurrentImage);

            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                if (desktop?.MainWindow != null)
                {
                    var result = await dialog.ShowDialog<Rect?>(desktop.MainWindow);
                    if (result.HasValue)
                    {
                        ToolBox.Crop.CropRect = result.Value;
                        Editor.ApplyOperation(ToolBox.Crop);
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

        private void SetTool(ITool tool)
        {
            CurrentTool = tool;
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
        public ReactiveCommand<ITool, Unit> SetToolCommand { get; }
        public ReactiveCommand<Unit, Unit> CropCommand { get; }
    }
}
