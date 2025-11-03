using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Platform.Storage;
using System.Threading.Tasks;

namespace GraphicEditor.Models
{
    public interface IFileDialogService
    {
        Task<string?> ShowOpenFileDialogAsync();
        Task<string?> ShowSaveFileDialogAsync();
    }

    public class FileDialogService : IFileDialogService
    {
        public async Task<string?> ShowOpenFileDialogAsync()
        {
            var topLevel = GetTopLevel();
            if (topLevel?.StorageProvider == null)
                return null;

            var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
            {
                Title = "Open Image",
                AllowMultiple = false,
                FileTypeFilter = new[]
                {
                    new FilePickerFileType("Images")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg", "*.png", "*.bmp", "*.gif" }
                    },
                    new FilePickerFileType("All Files")
                    {
                        Patterns = new[] { "*.*" }
                    }
                }
            });

            return files?.Count > 0 ? files[0].Path.AbsolutePath : null;
        }

        public async Task<string?> ShowSaveFileDialogAsync()
        {
            var topLevel = GetTopLevel();
            if (topLevel?.StorageProvider == null)
                return null;

            var file = await topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Save Image",
                DefaultExtension = ".png",
                ShowOverwritePrompt = true,
                FileTypeChoices = new[]
                {
                    new FilePickerFileType("PNG Image")
                    {
                        Patterns = new[] { "*.png" }
                    },
                    new FilePickerFileType("JPEG Image")
                    {
                        Patterns = new[] { "*.jpg", "*.jpeg" }
                    },
                    new FilePickerFileType("BMP Image")
                    {
                        Patterns = new[] { "*.bmp" }
                    }
                }
            });

            return file?.Path.AbsolutePath;
        }

        private static TopLevel? GetTopLevel()
        {
            var app = Application.Current;
            if (app == null) return null;

            var lifetime = app.ApplicationLifetime;
            if (lifetime is not IClassicDesktopStyleApplicationLifetime desktop)
                return null;

            var mainWindow = desktop.MainWindow;
            if (mainWindow == null) return null;

            return TopLevel.GetTopLevel(mainWindow);
        }
    }
}


