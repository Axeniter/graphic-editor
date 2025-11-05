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
        private SKBitmap _currentImage;
        private Stack<SKBitmap> _undoImages;
        private Stack<SKBitmap> _redoImages;

        public ImageEditor()
        {
            _undoImages = new Stack<SKBitmap>();
            _redoImages = new Stack<SKBitmap>();
            _currentImage = new SKBitmap();
        }

        public SKBitmap CurrentImage
        {
            get => _currentImage;
            private set => this.RaiseAndSetIfChanged(ref _currentImage, value);
        }

        public bool CanUndo => _undoImages.Count > 0;
        public bool CanRedo => _redoImages.Count > 0;
        public void Refresh()
        {
            this.RaisePropertyChanged(nameof(CurrentImage));
        }

        private WriteableBitmap ConvertSKBitmapToWriteableBitmap(SKBitmap skBitmap)
        {
            var writeableBitmap = new WriteableBitmap(
                new PixelSize(skBitmap.Width, skBitmap.Height),
                new Vector(96, 96),
                PixelFormat.Bgra8888);

            using (var buffer = writeableBitmap.Lock())
            {
                unsafe
                {
                    var ptr = (byte*)buffer.Address;

                    var skPixels = skBitmap.GetPixels();
                    var sourcePtr = (byte*)skPixels.ToPointer();

                    for (int y = 0; y < skBitmap.Height; y++)
                    {
                        for (int x = 0; x < skBitmap.Width; x++)
                        {
                            var sourceIndex = (y * skBitmap.RowBytes) + (x * 4);
                            var targetIndex = (y * buffer.RowBytes) + (x * 4);

                            ptr[targetIndex + 0] = sourcePtr[sourceIndex + 2];
                            ptr[targetIndex + 1] = sourcePtr[sourceIndex + 1];
                            ptr[targetIndex + 2] = sourcePtr[sourceIndex + 0];
                            ptr[targetIndex + 3] = sourcePtr[sourceIndex + 3];
                        }
                    }
                }
            }

            return writeableBitmap;
        }

        private SKBitmap ConvertWriteableBitmapToSKBitmap(WriteableBitmap writeableBitmap)
        {
            var skBitmap = new SKBitmap(writeableBitmap.PixelSize.Width, writeableBitmap.PixelSize.Height);

            using (var buffer = writeableBitmap.Lock())
            {
                unsafe
                {
                    var ptr = (byte*)buffer.Address;
                    var skPixels = skBitmap.GetPixels();
                    var targetPtr = (byte*)skPixels.ToPointer();

                    for (int y = 0; y < writeableBitmap.PixelSize.Height; y++)
                    {
                        for (int x = 0; x < writeableBitmap.PixelSize.Width; x++)
                        {
                            var sourceIndex = (y * buffer.RowBytes) + (x * 4);
                            var targetIndex = (y * skBitmap.RowBytes) + (x * 4);

                            targetPtr[targetIndex + 0] = ptr[sourceIndex + 2];
                            targetPtr[targetIndex + 1] = ptr[sourceIndex + 1];
                            targetPtr[targetIndex + 2] = ptr[sourceIndex + 0];
                            targetPtr[targetIndex + 3] = ptr[sourceIndex + 3];
                        }
                    }
                }
            }

            return skBitmap;
        }

        public void NewFile(int width, int height)
        {
            var image = new SKBitmap(width, height);
            using (var canvas = new SKCanvas(image))
            {
                canvas.Clear(SKColors.White);
            }
            var oldImage = CurrentImage;
            CurrentImage = image;
            oldImage.Dispose();
            ClearRedoStack();
            ClearUndoStack();
        }

        public void OpenFile(string path)
        {
            try
            {
                using var stream = File.OpenRead(path);
                var image = SKBitmap.Decode(stream);
                var oldImage = CurrentImage;
                CurrentImage = image;
                oldImage.Dispose();
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
            try
            {
                using var stream = File.Create(path);

                var extension = Path.GetExtension(path).ToLower();
                var format = extension switch
                {
                    ".jpg" or ".jpeg" => SKEncodedImageFormat.Jpeg,
                    ".png" => SKEncodedImageFormat.Png,
                    ".bmp" => SKEncodedImageFormat.Bmp,
                    _ => SKEncodedImageFormat.Png
                };

                using var image = SKImage.FromBitmap(CurrentImage);
                using var data = image.Encode(format, 100);
                data.SaveTo(stream);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving file: {ex.Message}");
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
