using SkiaSharp;

namespace AiArtDesctop.ArtTools;

public abstract class Brush:IBrush
{
    public SKColor Color { get; set; }
    public int StrokeWidth { get; set; }
    public BrushType Type { get; set; }
    public SKPaint Paint { get; set; }
    
    public abstract void DrawTouch(float x, float y);
    public abstract void DrawDrag(float x, float y);
}