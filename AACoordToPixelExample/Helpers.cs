using System;
using System.Drawing;
using System.Numerics;

namespace AACoordToPixelExample;

static class Helpers
{
    public static double Distance((string raw, double x, double y) one, (string raw, double x, double y) two)
    {
        double xd = two.x - one.x;
        double yd = two.y - one.y;
        return Math.Sqrt(xd * xd + yd * yd);
    }

    public static double Distance(AACoords a, AACoords b)
    {
        double xd = b.X.Value - a.X.Value;
        double yd = b.Y.Value - a.Y.Value;
        return Math.Sqrt(xd * xd + yd * yd);
    }

    public static double Distance(double x1, double y1, double x2, double y2)
    {
        double xd = x2 - x1;
        double yd = y2 - y1;
        return Math.Sqrt(xd * xd + yd * yd);
    }

    public static double Distance(Point one, Point two)
    {
        double xd = two.X - one.X;
        double yd = two.Y - one.Y;
        return Math.Sqrt(xd * xd + yd * yd);
    }

    public static Bitmap Crop(Image source, Point center, int width, int height)
    {
        var b = new Bitmap(width, height);
        using (var gg = Graphics.FromImage(b))
        {
            gg.DrawImage(source, new Rectangle(0, 0, b.Width, b.Height),
                new Rectangle(center.X - b.Width / 2, center.Y - b.Height / 2, b.Width, b.Height),
                GraphicsUnit.Pixel);
        }

        return b;
    }

    public static void DrawCircle(Graphics g, Point center, int radius, Color color)
    {
        float xx = center.X - radius;
        float yy = center.Y - radius;
        float width = 2 * radius;
        float height = 2 * radius;
        g.DrawEllipse(new Pen(color, 1), xx, yy, width, height);
    }

    public static void FillCircle(Graphics g, Point center, int radius, Color color)
    {
        float xx = center.X - radius;
        float yy = center.Y - radius;
        float width = 2 * radius;
        float height = 2 * radius;
        g.FillEllipse(new SolidBrush(color), xx, yy, width, height);
    }

    public static void DrawPixel(Graphics g, Point center, Color color)
    {
        g.FillRectangle(new SolidBrush(color), new Rectangle(center, new Size(1, 1)));
    }

    public static Vector2 CoordsToVector2(AACoords coords)
    {
        return new Vector2((float) coords.X.Value, (float) coords.Y.Value);
    }
}