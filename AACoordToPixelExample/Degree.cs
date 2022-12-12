namespace AACoordToPixelExample;
public struct Degree
{
    public Degree(double value, double degrees, double minutes, double seconds)
    {
        Value = value;
        Degrees = degrees;
        Minutes = minutes;
        Seconds = seconds;
    }

    public double Value { get; }
    public double Degrees { get; }
    public double Minutes { get; }
    public double Seconds { get; }

    private static int ExtractDegrees(double value)
    {
        return (int)value;
    }

    private static int ExtractMinutes(double value)
    {
        value = Math.Abs(value);
        return (int)((value - ExtractDegrees(value)) * 60);
    }

    private static int ExtractSeconds(double value)
    {
        value = Math.Abs(value);
        double minutes = (value - ExtractDegrees(value)) * 60;
        return (int)Math.Round((minutes - ExtractMinutes(value)) * 60);
    }

    public static Degree FromDouble(double value)
    {
        return new Degree(value, Math.Abs(ExtractDegrees(value)), Math.Abs(ExtractMinutes(value)), Math.Abs(ExtractSeconds(value)));
    }
}