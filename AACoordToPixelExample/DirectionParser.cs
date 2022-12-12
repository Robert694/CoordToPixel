using System;
using System.Collections.Generic;

namespace AACoordToPixelExample;

public class DirectionParser
{
    public enum DirectionType
    {
        N,
        S,
        E,
        W
    }

    public DirectionParser(string direction, string degrees, string minutes, string seconds)
    {
        if (!Enum.TryParse(typeof(DirectionType), direction, true, out var d))
            throw new Exception($"Invalid Direction '{direction}'");
        Direction = (DirectionType)d;

        var values = ParseDms(degrees, minutes, seconds);

        Value = ConvertDegreeAngleToDouble(values.degrees, values.minutes, values.seconds);

        TransformValue();
    }

    public double ConvertDegreeAngleToDouble(double degrees, double minutes, double seconds)
    {
        //Decimal degrees = 
        //   whole number of degrees, 
        //   plus minutes divided by 60, 
        //   plus seconds divided by 3600
        return degrees +
               minutes / 60 +
               seconds / 3600;
    }

    public DirectionType Direction { get; }

    public double Value { get; private set; }

    private void TransformValue()
    {
        switch (Direction)
        {
            case DirectionType.E:
            case DirectionType.S:
                Value = Math.Abs(Value);
                break;
            case DirectionType.W:
            case DirectionType.N:
                Value *= -1;
                break;
        }
    }

    private (int degrees, int minutes, int seconds) ParseDms(string d, string m, string s)
    {
        var errors = new List<string>();
        if (!int.TryParse(d, out var degrees)) errors.Add("Degrees");
        if (!int.TryParse(m, out var minutes)) errors.Add("Minutes");
        if (!int.TryParse(s, out var seconds)) errors.Add("Seconds");
        if (errors.Count > 0)
            throw new Exception($"Invalid ({string.Join(",", errors)}) for Direction: {Direction}");
        return (degrees, minutes, seconds);
    }
}