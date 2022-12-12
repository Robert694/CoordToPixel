namespace AACoordToPixelExample;

public struct HSV
{
    private double _h;
    private double _s;
    private double _v;

    public HSV(double h, double s, double v)
    {
        this._h = h;
        this._s = s;
        this._v = v;
    }

    public double H
    {
        get { return this._h; }
        set { this._h = value; }
    }

    public double S
    {
        get { return this._s; }
        set { this._s = value; }
    }

    public double V
    {
        get { return this._v; }
        set { this._v = value; }
    }

    public bool Equals(HSV hsv)
    {
        return (this.H == hsv.H) && (this.S == hsv.S) && (this.V == hsv.V);
    }
}
