using SkiaSharp;

namespace AiArtDesctop.ArtTools;

public abstract class Brush:IBrush
{
    public SKColor Color { get; set; }
    public int StrokeWidth { get; set; }
    public BrushType Type { get; set; }
    public SKPaint Paint { get; set; }
    public SKCanvas Canvas { get; set; }

    public Brush(SKCanvas canvas,BrushType type, SKPaint paint)
    {
        this.Canvas = canvas;
        this.Type = type;
        this.Paint = paint;
    }
    
    public abstract void DrawTouch(float x, float y);
    public abstract void DrawDrag(float x, float y);
}