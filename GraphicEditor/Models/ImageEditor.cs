using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace GraphicEditor.Models
{
    public class ImageEditor : ReactiveObject, IDisposable
    {
        private WriteableBitmap _currentImage;
        private Stack<WriteableBitmap> _undoImages;
        private Stack<WriteableBitmap> _redoImages;

        public ImageEditor()
        {
            _undoImages = new Stack<WriteableBitmap>();
            _redoImages = new Stack<WriteableBitmap>();
            _currentImage = new WriteableBitmap(new PixelSize(1, 1), new Vector(96, 96),
                PixelFormat.Bgra8888, AlphaFormat.Opaque);
        }

        public WriteableBitmap CurrentImage
        {
            get => _currentImage;
            private set => this.RaiseAndSetIfChanged(ref _currentImage, value);
        }

        public bool CanUndo => _undoImages.Count > 0;
        public bool CanRedo => _redoImages.Count > 0;

        public void NewFile(int width, int height)
        {
            var oldImage = CurrentImage;
            CurrentImage = CreateWhiteBitmap(width, height);
            oldImage?.Dispose();
            ClearRedoStack();
            ClearUndoStack();
        }

        private WriteableBitmap CreateWhiteBitmap(int width, int height)
        {
            var bitmap = new WriteableBitmap(new PixelSize(width, height), new Vector(96, 96),
                PixelFormat.Bgra8888, AlphaFormat.Opaque);

            using (var buffer = bitmap.Lock())
            {
                unsafe
                {
                    var ptr = (uint*)buffer.Address;
                    var totalPixels = width * height;

                    for (int i = 0; i < totalPixels; i++)
                    {
                        ptr[i] = 0xFFFFFFFF;
                    }
                }
            }
            return bitmap;
        }

        public void OpenFile(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                var image = WriteableBitmap.Decode(stream);
                var oldImage = CurrentImage;
                CurrentImage = image;
                oldImage?.Dispose();
                ClearRedoStack();
                ClearUndoStack();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error opening file: {ex.Message}");
            }
        }

        public void SaveFile(string path)
        {
            if (CurrentImage == null) return;
            try
            {
                using var stream = File.Create(path);
                var extension = Path.GetExtension(path).ToLower();

                switch (extension)
                {
                    case ".png":
                        SaveWithSkiaSharp(stream, SKEncodedImageFormat.Png, 100);
                        break;
                    case ".jpg":
                    case ".jpeg":
                        SaveWithSkiaSharp(stream, SKEncodedImageFormat.Jpeg, 100);
                        break;
                    case ".bmp":
                        SaveWithSkiaSharp(stream, SKEncodedImageFormat.Bmp, 100);
                        break;
                    default:
                        CurrentImage.Save(stream);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
            }
        }

        private void SaveWithSkiaSharp(Stream stream, SKEncodedImageFormat format, int quality)
        {
            if (CurrentImage == null) return;

            try
            {
                using var skBitmap = CurrentImage.ToSKBitmap();
                if (skBitmap == null) return;

                using var image = SKImage.FromBitmap(skBitmap);
                using var data = image.Encode(format, quality);
                data.SaveTo(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving with SkiaSharp: {ex.Message}");
                throw;
            }
        }

        public void ApplyOperation(IImageOperation operation)
        {
            SaveState();
            CurrentImage = operation.ProcessImage(CurrentImage);
        }

        public void SaveState()
        {
            if (CurrentImage != null)
            {
                _undoImages.Push(CurrentImage.Copy());
                this.RaisePropertyChanged(nameof(CanUndo));
                ClearRedoStack();
            }
        }

        public void Undo()
        {
            if (CanUndo)
            {
                _redoImages.Push(CurrentImage);
                CurrentImage = _undoImages.Pop();
                this.RaisePropertyChanged(nameof(CanUndo));
                this.RaisePropertyChanged(nameof(CanRedo));
            }
        }

        public void Redo()
        {
            if (CanRedo)
            {
                _undoImages.Push(CurrentImage);
                CurrentImage = _redoImages.Pop();
                this.RaisePropertyChanged(nameof(CanUndo));
                this.RaisePropertyChanged(nameof(CanRedo));
            }
        }

        private void ClearUndoStack()
        {
            foreach (var image in _undoImages)
                image?.Dispose();
            _undoImages.Clear();
            this.RaisePropertyChanged(nameof(CanUndo));
        }

        private void ClearRedoStack()
        {
            foreach (var  image in _redoImages)
                image?.Dispose();
            _redoImages.Clear();
            this.RaisePropertyChanged(nameof(CanRedo));
        }

        public void Dispose()
        {
            _currentImage?.Dispose();
            foreach (var image in _undoImages)
                image?.Dispose();
            foreach (var image in _redoImages)
                image?.Dispose();
        }
    }
}
