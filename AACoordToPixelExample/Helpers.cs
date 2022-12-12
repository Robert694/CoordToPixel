using System;
using System.Drawing;
using System.Numerics;

namespace AACoordToPixelExample;

static class Helpers
{
    public static HSV RGBToHSV(RGB rgb)
    {
        double delta, min;
        double h = 0, s, v;

        min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
        v = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
        delta = v - min;

        if (v == 0.0)
            s = 0;
        else
            s = delta / v;

        if (s == 0)
            h = 0.0;

        else
        {
            if (rgb.R == v)
                h = (rgb.G - rgb.B) / delta;
            else if (rgb.G == v)
                h = 2 + (rgb.B - rgb.R) / delta;
            else if (rgb.B == v)
                h = 4 + (rgb.R - rgb.G) / delta;

            h *= 60;

            if (h < 0.0)
                h = h + 360;
        }

        return new HSV(h, s, (v / 255));
    }
    public static RGB HSVToRGB(HSV hsv)
    {
        double r = 0, g = 0, b = 0;

        if (hsv.S == 0)
        {
            r = hsv.V;
            g = hsv.V;
            b = hsv.V;
        }
        else
        {
            int i;
            double f, p, q, t;

            if (hsv.H == 360)
                hsv.H = 0;
            else
                hsv.H = hsv.H / 60;

            i = (int)Math.Truncate(hsv.H);
            f = hsv.H - i;

            p = hsv.V * (1.0 - hsv.S);
            q = hsv.V * (1.0 - (hsv.S * f));
            t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

            switch (i)
            {
                case 0:
                    r = hsv.V;
                    g = t;
                    b = p;
                    break;

                case 1:
                    r = q;
                    g = hsv.V;
                    b = p;
                    break;

                case 2:
                    r = p;
                    g = hsv.V;
                    b = t;
                    break;

                case 3:
                    r = p;
                    g = q;
                    b = hsv.V;
                    break;

                case 4:
                    r = t;
                    g = p;
                    b = hsv.V;
                    break;

                default:
                    r = hsv.V;
                    g = p;
                    b = q;
                    break;
            }

        }

        return new RGB((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
    }
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