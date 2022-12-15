using System.Drawing;

var sensors = File.ReadAllLines("input.txt").Select(l =>
{
  var tokens = l.Split(" ");
  return (new Point(int.Parse(new string(tokens[2].Split("=")[1].SkipLast(1).ToArray())),
                    int.Parse(new string(tokens[3].Split("=")[1].SkipLast(1).ToArray()))),
         new Point(int.Parse(new string(tokens[8].Split("=")[1].SkipLast(1).ToArray())),
                    int.Parse(tokens[9].Split("=")[1])));
}).ToDictionary(l => l.Item1, l => l.Item2);

long Distance(Point p1, Point p2)
{
  return Math.Abs((long)p1.X - p2.X) + Math.Abs(p1.Y - p2.Y);
}

int maxCoord = 4000000;

var distances = sensors.ToDictionary(s => s.Key, s => Distance(s.Key, s.Value));

HashSet<(Point, Point)> done = new HashSet<(Point, Point)>();
int count = 0;
foreach (var s1 in sensors)
{
  foreach (var s2 in sensors)
  {
    if (s1.Key == s2.Key || done.Contains((s1.Key, s2.Key)))
      continue;

    done.Add((s1.Key,s2.Key));
    done.Add((s2.Key, s1.Key));

    long sensorsDist = Distance(s1.Key, s2.Key);
    long radiusSum = Distance(s1.Key, s1.Value) + Distance(s2.Key, s2.Value);
    Console.WriteLine(sensorsDist - radiusSum);

    long diff = sensorsDist - radiusSum;

    if (diff == 2)
    {
      // Check diagonal
      Point nextP = new Point(s1.Key.X + (int)distances[s1.Key] + 1, s1.Key.Y);
      while(nextP.X <= maxCoord && nextP.Y <= maxCoord)
      {
        CheckP(nextP);
        nextP.X++;
        nextP.Y++;
      }

      nextP = new Point(s1.Key.X + (int)distances[s1.Key] + 1, s1.Key.Y);
      while (nextP.X >= 0 && nextP.Y >=0)
      {
        CheckP(nextP);
        nextP.X--;
        nextP.Y--;
      }

    }
  }
}

bool CheckP(Point p)
{
  if (sensors.Keys.All(s => Distance(s, p) > distances[s]))
  {
    Console.WriteLine("Eureka: " + ((long)p.X * 4000000 + (long)p.Y));
    return true;
  }

  return false;
}


bool CheckPoint(Point p)
{
  for (int i = 0; i <= maxCoord; i++)
    CheckP(new Point(i, p.Y));
  // --------------------------------------------

  for (int i = 0; i <= maxCoord; i++)
    CheckP(new Point(p.X, i));

  return false;
}

if (CheckPoint(new Point(0, 0)) || CheckPoint(new Point(maxCoord, maxCoord)) || CheckPoint(new Point(1, 1)) || CheckPoint(new Point(maxCoord-1, maxCoord-1)))
{

}

foreach (var s in sensors)
{
  if (CheckPoint(s.Key))
  {
  }
}

Console.WriteLine();

// 15355747566756 too high