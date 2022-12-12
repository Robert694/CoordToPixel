using CoordToPixel;
using System.Collections.Generic;
using System;
using System.Numerics;
using System.Text.RegularExpressions;

namespace AACoordToPixelExample;

public class AACoords : ICoordinates
{
    public AACoords(string raw, double x, double y)
    {
        Raw = raw;
        X = Degree.FromDouble(x);
        Y = Degree.FromDouble(y);
        Value = new((float)X.Value, (float)Y.Value);
    }

    public string Raw { get; init; }
    private Vector2 Value { get; init; }
    public Degree X { get; }
    public Degree Y { get; }

    public Vector2 GetVector() => Value;

    public override string ToString()
    {
        return $"{(X.Value < 0 ? "W" : "E")} {X.Degrees:00} {X.Minutes:00} {X.Seconds:00} {(Y.Value < 0 ? "N" : "S")} {Y.Degrees:00} {Y.Minutes:00} {Y.Seconds:00}";
    }

    public static implicit operator Vector2(AACoords c) => new((float)c.X.Value, (float)c.Y.Value);

    public static AACoords ParseCoords(string cords)
    {
        cords = cords.ToUpper();
        cords = Regex.Replace(cords, "[^A-Za-z0-9]", " ");
        cords = Regex.Replace(cords, "([A-Z])", " $1 ");
        var split = cords.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (split.Length < 8) throw new Exception("Not enough values");
        if (split.Length > 8) throw new Exception("Too many values");

        var used = new HashSet<DirectionParser.DirectionType>();
        double x = 0, y = 0;
        for (var i = 0; i < split.Length; i += 4)
        {
            var dp = new DirectionParser(split[i], split[i + 1], split[i + 2], split[i + 3]);
            if (used.Contains(dp.Direction))
                throw new Exception($"Duplicate or Conflicting Direction: {dp.Direction}");
            switch (dp.Direction)
            {
                case DirectionParser.DirectionType.N:
                case DirectionParser.DirectionType.S:
                    used.Add(DirectionParser.DirectionType.N);
                    used.Add(DirectionParser.DirectionType.S);
                    break;
                case DirectionParser.DirectionType.E:
                case DirectionParser.DirectionType.W:
                    used.Add(DirectionParser.DirectionType.E);
                    used.Add(DirectionParser.DirectionType.W);
                    break;
            }

            switch (dp.Direction)
            {
                case DirectionParser.DirectionType.N:
                case DirectionParser.DirectionType.S:
                    y = dp.Value;
                    break;
                case DirectionParser.DirectionType.E:
                case DirectionParser.DirectionType.W:
                    x = dp.Value;
                    break;
            }
        }

        for (var i = 0; i < split.Length; i++)
        {
            if (int.TryParse(split[i], out var n)) split[i] = n.ToString("00");
        }

        var raw = string.Join(" ", split);
        return new AACoords(raw, x, y);
    }
}