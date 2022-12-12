using System;
using System.Drawing;
using System.Numerics;

namespace CoordToPixel;

public static class VectorExtensions
{
    public static Point ToPoint(this Vector2 a) => new Point((int)a.X, (int)a.Y);
    public static Vector2 ToVector2(this Point a) => new Vector2(a.X, a.Y);

    public static Vector2 Round(this Vector2 a)
    {
        return new Vector2(MathF.Round(a.X), MathF.Round(a.Y));
    }

    public static Vector2 Direction(this Vector2 a)
    {
        return new Vector2(a.X < 0 ? -1 : 1, a.Y < 0 ? -1 : 1);
    }

    public static float ManhattanDistance(this Vector2 a, Vector2 b)
    {
        var dx = Math.Abs(b.X - a.X);
        var dy = Math.Abs(b.Y - a.Y);
        return dx + dy;
    }

    public static float ManhattanDistance(this Vector3 a, Vector3 b)
    {
        var dx = Math.Abs(b.X - a.X);
        var dy = Math.Abs(b.Y - a.Y);
        var dz = Math.Abs(b.Z - a.Z);
        return dx + dy + dz;
    }

    public static float ManhattanDistance(this Vector4 a, Vector4 b)
    {
        var dx = Math.Abs(b.X - a.X);
        var dy = Math.Abs(b.Y - a.Y);
        var dz = Math.Abs(b.Z - a.Z);
        var dw = Math.Abs(b.W - a.W);
        return dx + dy + dz + dw;
    }
}