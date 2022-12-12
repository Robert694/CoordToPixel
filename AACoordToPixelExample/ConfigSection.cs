using System.Collections.Generic;
using System.Drawing;

namespace AACoordToPixelExample;

class ConfigSection
{
    public string Name { get; set; }
    public string ImagePath { get; set; }
    public AACoords GameCoord1 { get; set; }
    public Point ImageCoord1 { get; set; }
    public AACoords GameCoord2 { get; set; }
    public Point ImageCoord2 { get; set; }
    public List<AACoords> Points { get; set; }
    public float Multiplier { get; set; }
    public float GridLineDegreeX { get; set; }
    public float GridLineDegreeY { get; set; }
}