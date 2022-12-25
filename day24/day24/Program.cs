using System.Diagnostics;
using System.Drawing;

var map = File.ReadAllLines("input.txt")
  .Select((l, i) => l.Select((c, j) => (new Point(j, i), c)))
  .Aggregate(new List<(Point, char)>(), (s, col) => { s.AddRange(col); return s; })
  .ToDictionary(p => p.Item1, p => p.Item2);

var directions = new Dictionary<char, Point>
{
  { '^', new Point(0, -1)},  // N
  { 'v', new Point(0, 1) },  // S
  { '>', new Point(1, 0) },  // E
  { '<', new Point(-1,0) },  // V
};


var walls = map.Keys.Where(mp => map[mp] == '#').ToHashSet();
var currentPoint = new Point(1,0);
var blizzards = map.Keys.Where(mp => directions.ContainsKey(map[mp])).ToList();

var width = walls.Max(w => w.X) - walls.Min(w => w.X) - 1;
var height = walls.Max(w => w.Y) - walls.Min(w => w.Y) - 1;

int maxY = walls.Max(w => w.Y);

Stack<(Point, int)> toGo = new Stack<(Point, int)>();
toGo.Push((currentPoint, 0));

int minTime = int.MaxValue;

Dictionary<string, int> visited = new Dictionary<string, int>();

//PrintMap(currentPoint, blizzards.Select(b => (b, map[b])).ToList());

while (toGo.Any())
{
  var current = toGo.Pop();
  Point currentPos = current.Item1;
  int currentTime = current.Item2;
  currentTime++;

  if (currentTime >= minTime)
    continue;

  string hash = currentPos.X + " " + currentPos.Y + " " + (currentTime % width).ToString() + " " + (currentTime % height).ToString();
  if (visited.ContainsKey(hash))
    if (visited[hash] <= currentTime)
      continue;
    else
      visited[hash] = currentTime;
  else
    visited.Add(hash, currentTime);

  if (currentPos.Y == maxY)
  {
    Console.WriteLine(currentTime);
    if (currentTime < minTime)
      minTime = currentTime;
  }

  var currentBlizzardsWithDir = new List<(Point, char)>();
  foreach (var b in blizzards)
  {
    var newPoint = new Point(b.X + (currentTime * directions[map[b]].X), b.Y + (currentTime * directions[map[b]].Y));
    currentBlizzardsWithDir.Add((ProjectPoint(newPoint), map[b]));
  }

  var currentBlizzards = currentBlizzardsWithDir.Select(b => b.Item1).ToHashSet();
  var newAvailablePositions = directions.Values
    .Select(d => new Point(currentPos.X + d.X, currentPos.Y + d.Y))
    .Where(p => map.ContainsKey(p) && map[p] != '#' && !currentBlizzards.Contains(p))
    .ToList();

  if(!currentBlizzards.Contains(currentPos))
    newAvailablePositions.Add(currentPos); // wait
  newAvailablePositions = newAvailablePositions.OrderByDescending(nap => width - nap.X + height - nap.Y).ToList();
  //PrintMap(currentPos, currentBlizzardsWithDir);
  foreach (var p in newAvailablePositions)
  {
    //PrintMap(p, currentBlizzardsWithDir);
    toGo.Push((p, currentTime));
  }
}

Console.WriteLine(minTime -1);

Point ProjectPoint(Point p)
{
  int x = p.X;
  int y = p.Y;

  if (x > 0)
    x = (x-1) % width + 1;
  else
    x = width - (Math.Abs(x) % width);

  if (y > 0)
    y = (y-1) % height + 1;
  else
    y = height - (Math.Abs(y) % height);

  return new Point(x, y);
}

void PrintMap(Point aCurrentPos, List<(Point, char)> aBlizzards)
{
  Console.Clear();
  for (int i = 0; i < height + 2; i++)
  {
    for (int j = 0; j < width + 2; j++)
      Console.Write(".");
    Console.WriteLine();
  }

  foreach (var wall in walls)
  {
    Console.SetCursorPosition(wall.X, wall.Y);
    Console.Write("#");
  }

  Console.SetCursorPosition(aCurrentPos.X, aCurrentPos.Y);
  Console.Write("E");

  foreach (var blizz in aBlizzards)
  {
    int count = aBlizzards.Count(b => b.Item1 == blizz.Item1);

    Console.SetCursorPosition(blizz.Item1.X, blizz.Item1.Y);
    if (count == 1)
      Console.Write(blizz.Item2);
    else
      Console.Write(count);
  }

  Console.ReadKey();
}

// 206 too low