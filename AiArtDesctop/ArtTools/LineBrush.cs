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
    private float _strokeBrushRadius;
    private float _strokeCellSize;
    private HashSet<(int, int)> _currentStrokeCells = new HashSet<(int, int)>();
    private SKPoint _lastPosition;
    public float BrushRadius { get; set; } = 5f;
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
        
        _strokeBrushRadius = BrushRadius;
        _strokeCellSize = 2 * _strokeBrushRadius;
        _currentStrokeCells = new HashSet<(int, int)>();
        
        DrawBrushCircle(_lastPosition);
    }
    
    private void DrawBrushCircle(SKPoint position)
    {
        var cell = (
            (int)Math.Floor(position.X / _strokeCellSize),
            (int)Math.Floor(position.Y / _strokeCellSize)
        );
        
        if (_currentStrokeCells.Contains(cell))
        {
            return;
        }
        Canvas.DrawCircle(position, _strokeBrushRadius, Paint);
        _currentStrokeCells.Add(cell);
    }
    
    public void OnPointerReleased()
    {
        _currentStrokeCells = new HashSet<(int, int)>();
    }

    public void Test()
    {
        SKBitmap bitmap = new SKBitmap(128, 128);
        SKBitmap bitmap2 = new SKBitmap(128, 128);
    }
}