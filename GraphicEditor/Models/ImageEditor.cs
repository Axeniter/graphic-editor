using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            _currentImage = new SKBitmap(800, 600);
        }

        public SKBitmap CurrentImage
        {
            get => _currentImage;
            private set => this.RaiseAndSetIfChanged(ref _currentImage, value);
        }

        public bool CanUndo => _undoImages.Count > 0;
        public bool CanRedo => _redoImages.Count > 0;

        public void NewImage(int width, int height)
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
