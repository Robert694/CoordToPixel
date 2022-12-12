using System.Numerics;

namespace CoordToPixel;

public class Coordinates : ICoordinates
{
    public Coordinates(Vector2 value)
    {
        Value = value;
    }
    public Vector2 Value { get; }
    public Vector2 GetVector() => Value;
}
