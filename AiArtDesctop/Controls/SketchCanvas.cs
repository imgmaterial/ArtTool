using System;
using System.Runtime.InteropServices;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using SkiaSharp;

public class SketchCanvas : Control
{
    
    private SKBitmap _skBitmap;
    private SKCanvas _skCanvas;
    private WriteableBitmap _avaloniaBitmap;

    public SketchCanvas()
    {
        this.AttachedToVisualTree += (s, e) => InitializeSkiaBitmap();
        PointerPressed += OnPointerPressed;
    }
    /// <summary>
    /// Initializes the bitmap and canvas and clears them to a default state.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    private void InitializeSkiaBitmap(int width = 500, int height = 500)
    {
        _skBitmap?.Dispose();
        _skBitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
        _skCanvas = new SKCanvas(_skBitmap);
        _skCanvas.Clear(SKColors.White);
        
        _avaloniaBitmap = new WriteableBitmap(
            new PixelSize(width, height),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
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
        float x = (float)position.X;
        float y = (float)position.Y;

        _skCanvas.DrawCircle(x, y, 2.5f, new SKPaint { Color = SKColors.Black, StrokeWidth = 5 });
        Console.WriteLine($"{x},{y}");
        InvalidateVisual();
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
        int width = 500;
        int height = 500;
        
        using (var buffer = _avaloniaBitmap.Lock())
        {
            byte[] pixelBytes = _skBitmap.Bytes;
            Marshal.Copy(pixelBytes, 0, buffer.Address, pixelBytes.Length);
        }
        context.DrawImage(_avaloniaBitmap, new Rect(0, 0, width, height));
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
}

