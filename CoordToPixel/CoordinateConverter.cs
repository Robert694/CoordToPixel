using System;
using System.Drawing;
using System.Numerics;

namespace CoordToPixel;

/// <summary>
/// Coordinates to image pixels converter
/// </summary>
public class CoordinateConverter : ICoordinateConverter
{
    private Vector2 ZeroZero { get; } //Zero Zero point relative to current image coordinates
    private Vector2 Multiplier { get; } //Amount of pixels in one degree
    private Vector2 Accuracy { get; }

    public CoordinateConverter(ICoordinates coords1, Point point1, ICoordinates coords2, Point point2)
    {
        var g1 = coords1.GetVector(); // In game location 1
        var i1 = point1.ToVector2(); // Image location 1

        var g2 = coords2.GetVector(); // In game location 2
        var i2 = point2.ToVector2(); // Image location 2

        //Implies coordinates system is perfectly square. It should be but if it wasn't you'd need to calculate the X and Y distances separately.
        var gameDistance = g1.ManhattanDistance(g2); // Manhattan distance between in game coords
        var imageDistance = i1.ManhattanDistance(i2); // Manhattan distance between image coords

        var pixelMultiplier = imageDistance / gameDistance; // Pixel multiplier for degree
        Multiplier = new Vector2(pixelMultiplier);

        //Negate flips the direction of the vector then we multiply by the pixelMultiplier and then add the image coords | Removed * Vector2.Negate(g1.Direction())
        var zeroZero1 = Vector2.Negate(g1) * pixelMultiplier + i1;// Calculate zero zero for coord 1
        var zeroZero2 = Vector2.Negate(g2) * pixelMultiplier + i2;// Calculate zero zero for coord 2
        var zeroZero = (zeroZero1 + zeroZero2) / 2; // Averages Zero Points - This might make it more inaccurate.

        ZeroZero = zeroZero; //ZeroZero = zeroZero.Round().ToPoint(); // Used before switching ZeroZero to Vector2 from Point

        //Calculate output inaccuracy in pixels using original inputs
        var calcPoint1 = Convert(coords1);
        var calcPoint2 = Convert(coords2);
        var rPoint1Acc1 = Vector2.Abs(point1.ToVector2() - calcPoint1.ToVector2());
        var rPoint1Acc2 = Vector2.Abs(point2.ToVector2() - calcPoint2.ToVector2());
        Accuracy = (rPoint1Acc1 + rPoint1Acc2) / 2;
    }

    public Point Convert(ICoordinates coordinates) => CoordsToPixels(ZeroZero, Multiplier, coordinates.GetVector());
    public Vector2 GetZeroPoint() => ZeroZero;
    public Vector2 GetMultiplier() => Multiplier;
    public Vector2 GetAccuracy() => Accuracy;

    public static Point CoordsToPixels(Vector2 zeroPoint, Vector2 pixelMultiplier, Vector2 coordinates)
    {
        var result = zeroPoint + coordinates * pixelMultiplier;
        return new Point((int)Math.Round(result.X), (int)Math.Round(result.Y));//Rounded since point is integer
    }

    //public static Point CoordsToPixels(Vector2 zeroPoint, Vector2 pixelMultiplier, Vector2 coordinates) => new Point(
    //    (int)Math.Round(zeroPoint.X + coordinates.X * pixelMultiplier.X),
    //    (int)Math.Round(zeroPoint.Y + coordinates.Y * pixelMultiplier.Y));


    //public static Point CoordsToPixels(Point zeroPoint, Vector2 pixelMultiplier, AACoords coords) => new Point(
    //    zeroPoint.X + (int) Math.Round(coords.X * pixelMultiplier.X),
    //    zeroPoint.Y + (int) Math.Round(coords.Y * pixelMultiplier.Y));
}