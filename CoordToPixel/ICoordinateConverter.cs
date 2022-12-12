using System.Drawing;
using System.Numerics;

namespace CoordToPixel;

public interface ICoordinateConverter
{
    Vector2 GetZeroPoint();
    Vector2 GetMultiplier();
    Vector2 GetAccuracy();
    public Point Convert(ICoordinates coordinates);
}