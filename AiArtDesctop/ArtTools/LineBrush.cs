using System.Reflection.Metadata.Ecma335;
using Avalonia.Rendering;
using SkiaSharp;

namespace AiArtDesctop.ArtTools;

public class LineBrush:Brush
{
    public LineBrush(SKCanvas canvas, SKPaint paint) : base(canvas, BrushType.Line, paint)
    {
        
    }
    
    
    public override void DrawTouch(float x, float y)
    {
        Canvas.DrawCircle(x,y,1,this.Paint);
    }

    public override void DrawDrag(float x, float y)
    {
        Canvas.DrawCircle(x,y,1,this.Paint);
    }
}