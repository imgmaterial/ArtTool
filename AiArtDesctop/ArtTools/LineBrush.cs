using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using Avalonia.Rendering;
using SkiaSharp;

namespace AiArtDesctop.ArtTools;

public class LineBrush:Brush
{
    public SKBitmap Bitmap { get; set; }
    private SKPoint _lastBrushPosition;
    private SKPoint _lastPosition;
    public LineBrush(SKCanvas canvas, SKPaint paint) : base(canvas, BrushType.Line, paint)
    {
    }
    
    
    
    public override void DrawTouch(float x, float y)
    {
        _lastBrushPosition = new SKPoint(x, y);
        DrawBrushCircle(_lastBrushPosition);
    }

    public override void DrawDrag(float x, float y)
    {
        _lastPosition = new SKPoint(x, y);
        DrawBrushCircle(_lastPosition);
    }
    
    private void DrawBrushCircle(SKPoint position)
    {
        Canvas.DrawCircle(position, StrokeWidth, Paint);
    }
    
    
}