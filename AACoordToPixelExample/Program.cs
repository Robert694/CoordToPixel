using System.Drawing;
using System.Net;
using System.Numerics;
using System.Drawing.Imaging;
using IniParser;
using IniParser.Parser;
using IniParser.Model;
using CoordToPixel;

namespace AACoordToPixelExample;

class Program
{
    static WebClient wc = new WebClient();
    const string ConfigFile = "Configuration.ini";

    static async Task Main(string[] args)
    {
        if (!File.Exists(ConfigFile))
        {
            Console.WriteLine($"Config File Not Found: {ConfigFile}");
            return;
        }

        var cfgsections = GetConfigSections(ConfigFile);

        for (int i = 0; i < cfgsections.Count; i++)
        {
            Console.WriteLine($"{i + 1}) {cfgsections[i].Name}");
        }
        Console.WriteLine("Type 'all' for all actions.");
    input:
        Console.Write("Section: ");
        string input = Console.ReadLine()?.Trim();
        if (input?.ToLower() == "all")
        {
            Console.WriteLine(new string('=', 20));
            foreach (var s in cfgsections)
            {
                ProcessConfigSection(s);
                Console.WriteLine(new string('=', 20));
            }
        }
        else
        {
            if (int.TryParse(input, out int result) && result > 0 && result <= cfgsections.Count)
            {
                Console.WriteLine(new string('=', 20));
                ProcessConfigSection(cfgsections[result - 1]);
            }
            else
            {
                goto input;
            }
        }
    }

    public static List<ConfigSection> GetConfigSections(string configfile)
    {
        var parser = new FileIniDataParser(new ConcatenateDuplicatedKeysIniDataParser());
        IniData data = parser.ReadFile(configfile);
        List<ConfigSection> sections = new();
        foreach (var s in data.Sections)
        {
            Console.WriteLine($"Loading section: {s.SectionName}");
            var temp = data[s.SectionName];

            try
            {
                var path = temp["ImagePath"];
                var sec = new ConfigSection
                {
                    Name = s.SectionName,
                    ImagePath = path,
                    GameCoord1 = AACoords.ParseCoords(temp["GameCoord1"]),
                    GameCoord2 = AACoords.ParseCoords(temp["GameCoord2"])
                };
                var splitp1 = temp["ImageCoord1"].Split(',', StringSplitOptions.RemoveEmptyEntries);
                sec.ImageCoord1 = new Point(int.Parse(splitp1[0]), int.Parse(splitp1[1]));
                var splitp2 = temp["ImageCoord2"].Split(',', StringSplitOptions.RemoveEmptyEntries);
                sec.ImageCoord2 = new Point(int.Parse(splitp2[0]), int.Parse(splitp2[1]));
                sec.Points = temp["Points[]"].Split(';', StringSplitOptions.RemoveEmptyEntries).Distinct().Select(AACoords.ParseCoords).ToList();
                if (temp.ContainsKey("GridLineDegreeX") && float.TryParse(temp["GridLineDegreeX"], out var glx)) sec.GridLineDegreeX = glx;
                if (temp.ContainsKey("GridLineDegreeY") && float.TryParse(temp["GridLineDegreeY"], out var gly)) sec.GridLineDegreeY = gly;
                if (temp.ContainsKey("Multiplier"))
                {
                    if (float.TryParse(temp["Multiplier"], out var multi))
                    {
                        sec.Multiplier = multi;
                        sec.ImageCoord1 = (sec.ImageCoord1.ToVector2() * multi).Round().ToPoint();
                        sec.ImageCoord2 = (sec.ImageCoord2.ToVector2() * multi).Round().ToPoint();
                    }
                }
                sections.Add(sec);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        return sections;
    }

    private static bool PathIsUrl(string path)
    {
        if (File.Exists(path))
            return false;
        try
        {
            Uri uri = new Uri(path);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static void ProcessConfigSection(ConfigSection s)
    {
        Console.WriteLine($"Processing: {s.Name}");
        bool isTemp = false;
        if (PathIsUrl(s.ImagePath))
        {
            isTemp = true;
            var url = s.ImagePath;
            Console.WriteLine($"Downloading: {url}");
            s.ImagePath = Path.GetTempFileName();
            wc.DownloadFile(url, s.ImagePath);
            Console.WriteLine($"Saved as: {s.ImagePath}");
        }
        if (!File.Exists(s.ImagePath))
        {
            Console.WriteLine($"Image File Not Found: {s.ImagePath}");
            return;
        }

        ICoordinateConverter converter = new CoordinateConverter(s.GameCoord1, s.ImageCoord1, s.GameCoord2, s.ImageCoord2);

        Console.WriteLine($"ZeroZero: {converter.GetZeroPoint()}");
        Console.WriteLine($"Multiplier: {converter.GetMultiplier()}");
        Console.WriteLine($"Inaccuracy: {converter.GetAccuracy()}");

        //RemoveClose(s.Points); //Remove coords that are too close to each other

        using (var map = Image.FromFile(s.ImagePath))
        {
            Console.WriteLine($"Map: [{Path.GetFileName(s.ImagePath)}] ({map.Width}x{map.Height})");
            int successCount = 0;
            //var points = new List<Point>();
            var pathPoints = new List<Vector2>();
            using (var image = new Bitmap(map.Width, map.Height, PixelFormat.Format32bppArgb))
            {
                using (var mapg = Graphics.FromImage(map))
                using (var g = Graphics.FromImage(image))
                {
                    DrawGrid(converter, map, mapg, s); //Draws degree grid lines
                    foreach (var coord in s.Points)
                    {
                        var point = converter.Convert(coord);
                        pathPoints.Add(point.ToVector2());

                        if (point.X < 0 || point.X > map.Width || point.Y < 0 || point.Y > map.Height)
                        {
                            //Console.WriteLine($"Invalid: {point.ToString()}"); //Print Invalid coordinates
                            continue;
                        }

                        successCount++;
                        //points.Add(point);
                        //Helpers.FillCircle(g, point, 5, Color.FromArgb(255/2, Color.Red)); //Draw circle on point
                        Helpers.FillCircle(g, point, 5, Color.Red); //Draw circle on point
                        Helpers.DrawPixel(g, point, Color.Black); //Draw circle on point

                        Helpers.FillCircle(mapg, point, 5, Color.Red); //Draw circle on point
                        Helpers.DrawPixel(mapg, point, Color.Black); //Draw circle on point
                    }

                    //Console.WriteLine("Calculating Convex hull");
                    //var convexHull = ConvexHull.GetConvexHull(points);
                    //Console.WriteLine("Drawing Convex hull");
                    //g.DrawClosedCurve(new Pen(Color.FromArgb(180, Color.Blue), 3), convexHull.ToArray(), 0, FillMode.Winding);
                    //mapg.DrawClosedCurve(new Pen(Color.FromArgb(180, Color.Blue), 3), convexHull.ToArray(), 0, FillMode.Winding);

                    //Random r = new Random();
                    //List<Vector2> shortestPath = new List<Vector2>();
                    //for (int n = pathPoints.Count; n-- > 0;)
                    //{
                    //    Vector2 current = new Vector2();
                    //    if (n == 0)
                    //    {
                    //        current = pathPoints[0];
                    //        shortestPath.Add(current);
                    //    }
                    //    float minDist = float.MaxValue;
                    //    var index = -1;
                    //    for (int i = 0; i < pathPoints.Count; i++)
                    //    {
                    //        //var d = current.ManhattanDistance(pathPoints[i]);
                    //        var d = Vector2.Distance(current, pathPoints[i]);
                    //        if (d < minDist)
                    //        {
                    //            minDist = d;
                    //            current = pathPoints[i];
                    //            index = i;
                    //        }
                    //    }
                    //    shortestPath.Add(current); // Add next closest point to new order
                    //    pathPoints.RemoveAt(index);//Removes closest point from path points so its not used again
                    //}

                    //mapg.DrawClosedCurve(new Pen(Color.FromArgb(180, Color.Blue), 3), shortestPath.Select(x => x.ToPoint()).ToArray(), 0, FillMode.Winding);
                }

                Console.WriteLine($"{successCount}/{s.Points.Count} points drawn.");
                if (successCount > 0)
                {
                    var name = ValidFileName(s.Name);
                    Directory.CreateDirectory("Output");
                    var ver = DateTime.UtcNow.ToFileTimeUtc();
                    Console.WriteLine("Saving Alpha...");
                    image.Save($"Output\\{ver}_alpha_{name}.png", ImageFormat.Png);
                    Console.WriteLine("Saving Map...");
                    //var result = Overlap(map, image);
                    map.Save($"Output\\{ver}_map_{name}.png", ImageFormat.Png);
                }
            }
        }

        if (isTemp)
            File.Delete(s.ImagePath);
    }

    public static void DrawGrid(ICoordinateConverter converter, Image map, Graphics g, ConfigSection section)
    {
        //TODO: Should probably figure out what is the min and max for x and y so I dont have to go over -40 to +40
        if (section.GridLineDegreeX > 0)
        {
            for (float x = -40; x < 40; x += section.GridLineDegreeX)//goes over every degree marker
            {
                var point = converter.Convert(new Coordinates(new Vector2(x, 0)));//creates image position
                if (point.X < 0 || point.X > map.Width) continue;//checks to see if the point is on the image otherwise skip
                var distFromWhole = Math.Round(Math.Abs(x % 1), 2);
                var wholeDegree = distFromWhole <= (float.Epsilon * 100);
                Console.WriteLine($"Drawing Horizontal Grid: X={point.X} [Degree Marker: {wholeDegree}]");
                var color = wholeDegree ? Color.FromArgb(180, Color.Blue) : Color.FromArgb(180, Color.Yellow);
                g.DrawLine(new Pen(color, 2), new Point(point.X, 0), new Point(point.X, map.Height));//draw line on image
            }
        }
        if (section.GridLineDegreeY > 0)
        {
            for (float y = -40; y < 40; y += section.GridLineDegreeY)//goes over every degree marker
            {
                var point = converter.Convert(new Coordinates(new Vector2(0, y)));//creates image position
                if (point.Y < 0 || point.Y > map.Height) continue; //checks to see if the point is on the image otherwise skip
                var distFromWhole = Math.Round(Math.Abs(y % 1), 2);
                var wholeDegree = distFromWhole <= (float.Epsilon * 100);
                Console.WriteLine($"Drawing Vertical Grid: Y={point.Y} [Degree Marker: {wholeDegree}]");
                var color = wholeDegree ? Color.FromArgb(180, Color.Blue) : Color.FromArgb(180, Color.Yellow);
                g.DrawLine(new Pen(color, 2), new Point(0, point.Y), new Point(map.Width, point.Y)); //draw line on image
            }
        }
    }

    public static string ValidFileName(string fileName)
    {
        return Path.GetInvalidFileNameChars()
            .Aggregate(fileName, (current, c) => current.Replace(c, '_'));
    }

    public static float GetDist(List<Vector2> points, int[] order)
    {
        float dist = 0;
        int lastIndex = order[0];
        for (int i = 1; i < points.Count; i++)
        {
            //dist += points[lastIndex].ManhattanDistance(points[order[i]]);
            dist += Vector2.DistanceSquared(points[lastIndex], points[order[i]]);
        }

        return dist;
    }

    public static void RemoveClose(List<AACoords> coords, double minDist = 0.05)
    {
        for (int i = coords.Count; i-- > 0;)
        {
            for (int x = coords.Count; x-- > 0;)
            {
                if (i == x) continue;
                double dist = Helpers.Distance(coords[i], coords[x]);
                if (dist <= minDist)
                {
                    Console.WriteLine($"Removing: {dist}|{coords[i].Raw} == {coords[x].Raw}");
                    //coords[i] = (coords[i].raw, (coords[i].x + coords[x].x) / 2, (coords[i].y + coords[x].y) / 2);
                    coords.RemoveAt(x);
                }
            }
        }
    }

    ////TODO: vvv - use commented out function below
    //public static Image Overlap(Image source1, Image source2)
    //{
    //    var target = new Bitmap(source1.Width, source1.Height, PixelFormat.Format32bppArgb);
    //    using var graphics = Graphics.FromImage(target);
    //    graphics.CompositingMode = CompositingMode.SourceOver; // this is the default, but just to be clear

    //    graphics.DrawImage(source1, 0, 0);
    //    graphics.DrawImage(source2, 0, 0);

    //    return target;
    //}

    ////TODO: Should use this because it doesn't have scaling issues with dpi
    //public static Image Overlay(this Image input, Image overlay, Point point)
    //{
    //    var clone = new Bitmap(input.Width, input.Height);
    //    using Graphics g = Graphics.FromImage(clone);
    //    g.DrawImage(input, new Rectangle(new Point(0, 0), new Size(input.Width, input.Height)));
    //    g.DrawImage(overlay, new Rectangle(point, new Size(overlay.Width, overlay.Height)));
    //    return clone;
    //}

    //public static Image SetImageOpacity(Image image, float opacity)
    //{
    //    Bitmap bmp = new Bitmap(image.Width, image.Height);
    //    using (Graphics g = Graphics.FromImage(bmp))
    //    {
    //        ColorMatrix matrix = new ColorMatrix();
    //        matrix.Matrix33 = opacity;
    //        ImageAttributes attributes = new ImageAttributes();
    //        attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default,
    //            ColorAdjustType.Bitmap);
    //        g.DrawImage(image, new Rectangle(0, 0, bmp.Width, bmp.Height),
    //            0, 0, image.Width, image.Height,
    //            GraphicsUnit.Pixel, attributes);
    //    }
    //    return bmp;
    //}

}