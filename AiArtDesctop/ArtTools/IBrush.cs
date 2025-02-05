using SkiaSharp;

namespace AiArtDesctop.ArtTools;

public interface IBrush
{
    public SKColor Color { get; set; }
    public int StrokeWidth { get; set; }
    public BrushType Type { get; set; }
    public SKPaint Paint { get; set; }

    public void DrawTouch(float x, float y);
    public void DrawDrag(float x, float y);

    public void ChangeStrokeWidth(int strokeWidth);

}