
using System.Diagnostics.CodeAnalysis;

var input = File.ReadAllLines("input.txt").Select(l =>
{
  var tokens = l.Split(",");
  return new Point(int.Parse(tokens[0]), int.Parse(tokens[1]), int.Parse(tokens[2]));
}).ToHashSet();
int countCommon = 0;

// Part1
foreach (var p1 in input)
{
  int cubCount = 0;
  foreach (var p2 in input)
  {
    if (p1 == p2)
      continue;

    var zip = p1.Coords.Zip(p2.Coords);

    if (zip.Count(p => p.First == p.Second) == 2)
    {
      var diff = zip.First(p => p.First != p.Second);
      if (Math.Abs(diff.First - diff.Second) == 1)
        cubCount++;
    }
  }

  countCommon += cubCount;
}

int minx = input.Min(p => p.x) - 1;
int miny = input.Min(p => p.y) - 1;
int minz = input.Min(p => p.z) - 1;

int maxx = input.Max(p => p.x) + 1;
int maxy = input.Max(p => p.y) + 1;
int maxz = input.Max(p => p.z) + 1;

Console.WriteLine(input.Count * 6 - countCommon);

HashSet<Point> visited = new HashSet<Point>();

// lee din min sa nu depaseasca max
Queue<Point> queue = new Queue<Point>();
queue.Enqueue(new Point(minx,miny,minz));
var directions = new List<Point>()
{
  new Point(0,0,1),
  new Point(0,1,0),
  new Point(1,0,0),
  new Point(0,0,-1),
  new Point(0,-1,0),
  new Point(-1,0,0),
};

int total = 0;
while(queue.Any())
{
  var current = queue.Dequeue();

  foreach(var d in directions)
  {
    var newPoint = new Point(current.x + d.x, current.y + d.y, current.z + d.z);

    if(!input.Contains(newPoint) && !visited.Contains(newPoint)
      && newPoint.x <= maxx && newPoint.y <= maxy && newPoint.z <= maxz
      && newPoint.x >= minx && newPoint.y >= miny && newPoint.z >= minz)
    { 
      queue.Enqueue(newPoint);
      visited.Add(newPoint);

      foreach (var p2 in input)
      {
        var zip = newPoint.Coords.Zip(p2.Coords);

        if (zip.Count(p => p.First == p.Second) == 2)
        {
          var diff = zip.First(p => p.First != p.Second);
          if (Math.Abs(diff.First - diff.Second) == 1)
            total++;
        }
      }
    }
  }
}

Console.WriteLine(total);


struct Point
{
  public Point(int ax, int ay, int az) { x = ax; y = ay; z = az; Coords = new List<int>() { x, y, z }; }
  public int x, y, z;
  public List<int> Coords = new List<int>();

  public static bool operator ==(Point b, Point c)
  {
    return b.x == c.x && b.y == c.y && b.z == c.z;
  }

  public static bool operator !=(Point b, Point c)
  {
    return b.x != c.x || b.y != c.y || b.z != c.z;
  }

  public override bool Equals([NotNullWhen(true)] object? obj)
  {
    return this == (Point)obj;
  }
}



// 3222 too high
// 2887 too high
// 2311 too high
// 1831
// 1761