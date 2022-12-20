using System.Drawing;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");

var shapes = input.Take(5)
                  .Select(s => s.Split("\r\n").Select((l, i) => l.Select((c, ic) => (new Point(ic, s.Split("\r\n").Length - i - 1), c)))
                  .Aggregate(new List<Point>(), (list, points) =>
                  {
                    list.AddRange(points.Where(p => p.c != '.').Select(p => p.Item1)); return list;
                  })).ToList();

string wind = input.Last();

bool falling = false;
List<Point> shapeFalling = null;
int shapesCount = 0;
int currentHeight = 0;

const int WIDTH = 7;
const int LEFT_PADDING = 2;

List<Point> falledRocksBits = new List<Point>();
int windIndex = 0;

bool display = false;
var lastLineHash = new Dictionary<string, (int, int)>();

int resetCount = 0;

long nr = 0;
long mod = 0;
long endOfCycleHeight = 0;
long lastHeight = 0;

while (shapesCount <= 10000)
{
  if (!falling)
  {
    resetCount++;

    string lastLine = "";
    for (int i = 0; i < WIDTH; i++)
    {
      var colBits = falledRocksBits.Where(b => b.X == i && currentHeight - b.Y < 100).OrderBy(b => b.Y);
      foreach (var b in colBits)
        lastLine += currentHeight - b.Y;
    }

    if (nr == 0)
    {
      if (shapesCount != 0)
      {
        string currentHash = ((windIndex - 1) % wind.Length).ToString() + " " + (shapesCount % shapes.Count).ToString() + " " + lastLine;

        if (!lastLineHash.ContainsKey(currentHash))
          lastLineHash.Add(currentHash, (shapesCount, currentHeight));
        else
        {
          var previous = lastLineHash[currentHash];
          var current = (shapesCount, currentHeight);

          int cicleSize = current.shapesCount - previous.Item1;
          endOfCycleHeight = current.currentHeight - previous.Item2;
          lastHeight = currentHeight;

          long size = 1000000000000 - previous.Item1;
          mod = size % cicleSize;
          long sizeDiv = size / cicleSize;
          nr = sizeDiv * endOfCycleHeight + lastLineHash[currentHash].Item2;
          resetCount = 0;
        }
      }
    }
    else
    {
      if(resetCount == mod)
      {
        nr += currentHeight - lastHeight;
        Console.WriteLine(nr);
        return;
      }
    }

    shapeFalling = shapes[shapesCount % shapes.Count].Select(p => new Point(p.X + LEFT_PADDING, p.Y + currentHeight + 4)).ToList();
    shapesCount++;
    falling = true;
  }
  else
  {
    // move
    int direction = wind[windIndex % wind.Length] == '<' ? -1 : 1;
    if (shapeFalling.All(b => b.X + direction >= 0 && b.X + direction < WIDTH && !falledRocksBits.Contains(new Point(b.X + direction, b.Y))))
    {
      for (int i = 0; i < shapeFalling.Count; i++)
        shapeFalling[i] = new Point(shapeFalling[i].X + direction, shapeFalling[i].Y);
    }

    windIndex++;
    //if (windIndex % wind.Length == 0)
    //{
    //  Console.WriteLine(shapesCount + ", height: " + currentHeight);
    //}

    // fall
    if (shapeFalling.Any(b => falledRocksBits.Contains(new Point(b.X, b.Y - 1)) || b.Y - 1 <= 0))
    {
      falling = false;
      falledRocksBits.AddRange(shapeFalling);
      int maxY = shapeFalling.Max(b => b.Y);
      if (currentHeight < maxY)
        currentHeight = maxY;

      shapeFalling.Clear();
    }
    else
    {
      //coutFall++;
      for (int i = 0; i < shapeFalling.Count; i++)
        shapeFalling[i] = new Point(shapeFalling[i].X, shapeFalling[i].Y - 1);
    }

    falledRocksBits.RemoveAll(b => b.Y < currentHeight - 100);

  }
}

// Part 1
Console.WriteLine(currentHeight);

// Part 2
// (3315, 5025)
// (3350, 5078)
//
//5113
