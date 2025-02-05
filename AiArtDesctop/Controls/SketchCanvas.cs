using System;
using System.IO;
using System.Runtime.InteropServices;
using AiArtDesctop.ArtTools;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Platform.Storage;
using SkiaSharp;
using Brush = AiArtDesctop.ArtTools.Brush;

namespace AiArtDesctop.Controls
{


    public class SketchCanvas : Control
    {

        private SKBitmap _skBitmap;
        private SKCanvas _skCanvas;
        private WriteableBitmap _avaloniaBitmap;
        private SKPaint _skPaint;
        private bool _isDrawing;
        public Brush Brush { get; set; }

        public SketchCanvas()
        {
            this.AttachedToVisualTree += (s, e) => InitializeSkiaBitmap();
            PointerPressed += OnPointerPressed;
            PointerMoved += OnPointerMoved;
            PointerReleased += OnPointerReleased;
        }

        /// <summary>
        /// Initializes the bitmap and canvas and clears them to a default state.
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        private void InitializeSkiaBitmap(int width = 512, int height = 512)
        {

            _skBitmap?.Dispose();
            _skBitmap = new SKBitmap(width, height, SKColorType.Rgba8888, SKAlphaType.Premul);
            _skCanvas = new SKCanvas(_skBitmap);
            _skPaint = new SKPaint
                { Color = SKColors.Black, StrokeWidth = 10, IsAntialias = true, Style = SKPaintStyle.StrokeAndFill };
            Brush = new LineBrush(this._skCanvas, _skPaint);
            ((LineBrush)Brush).Bitmap = _skBitmap;
            _skCanvas.DrawBitmap(_skBitmap, 0, 0);
            _skCanvas.Clear(SKColors.White);
            _avaloniaBitmap = new WriteableBitmap(
                new PixelSize(width, height),
                new Vector(96, 96),
                PixelFormat.Rgba8888,
                AlphaFormat.Premul
            );
        }

        /// <summary>
        /// On mouse click inside of the control draws a dot.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPointerPressed(object sender, PointerPressedEventArgs e)
        {
            var position = e.GetPosition(this);
            Brush.DrawTouch((float)position.X, (float)position.Y);
            _isDrawing = true;
            InvalidateVisual();
        }

        /// <summary>
        /// When the user is holding the button down draw a circle in the position
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPointerMoved(object sender, PointerEventArgs e)
        {
            if (!_isDrawing) return;
            var position = e.GetPosition(this);
            Brush.DrawDrag((float)position.X, (float)position.Y);
            InvalidateVisual();
        }

        /// <summary>
        /// When mouse button released reset _isDrawing to false.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnPointerReleased(object sender, PointerReleasedEventArgs e)
        {
            _isDrawing = false;
            ((LineBrush)this.Brush).OnPointerReleased();
        }

        /// <summary>
        /// Queues a re-render when first attached to a visual tree
        /// </summary>
        /// <param name="e"></param>
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            InvalidateVisual();
        }

        /// <summary>
        /// Copies the image from the skia canvas to the avalonia bitmap which is then drawn to the control.
        /// </summary>
        /// <param name="context"></param>
        public override void Render(DrawingContext context)
        {
            base.Render(context);
            int width = 512;
            int height = 512;

            using (var buffer = _avaloniaBitmap.Lock())
            {
                byte[] pixelBytes = _skBitmap.Bytes;
                Marshal.Copy(pixelBytes, 0, buffer.Address, pixelBytes.Length);
            }

            context.DrawImage(_avaloniaBitmap, new Rect(0, 0, width, height));
        }

        public void ClearCanvas()
        {
            _skCanvas.Clear(SKColors.White);
            InvalidateVisual();
        }

        private SKBitmap ToSkiaBitmap(Bitmap bitmap)
        {
            using (var stream = new MemoryStream())
            {
                bitmap.Save(stream);

                stream.Seek(0, SeekOrigin.Begin);

                return SKBitmap.Decode(stream);
            }
        }

        public void SetImage(Bitmap? image)
        {
            if (image == null) return;
            this._skCanvas.Clear(SKColors.White);
            SKBitmap newBitmap = ToSkiaBitmap(image);
            _skCanvas.DrawBitmap(newBitmap,0,0);
            InvalidateVisual();
        }

        /// <summary>
        /// Frees up memory when detached from the visual tree.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
        {
            _skBitmap?.Dispose();
            _skCanvas?.Dispose();
            _avaloniaBitmap?.Dispose();
            base.OnDetachedFromVisualTree(e);
        }

        public string GetCurrentImageAsHex()
        {
            byte[] bytes = _skBitmap.Bytes;
            string hexString = Convert.ToHexString(bytes);
            return hexString;
        }

    }
    
}

