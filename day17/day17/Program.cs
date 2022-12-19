using System.Drawing;

var input = File.ReadAllText("input.txt").Split("\r\n\r\n");

var shapes = input.Take(5)
                  .Select(s => s.Split("\r\n").Select((l, i) => l.Select((c,ic) => (new Point(ic, s.Split("\r\n").Length - i-1), c)))
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

//Console.ReadKey();
//int coutFall = 1;
while (shapesCount <= 100000)
{
  if(!falling)
  {
    //int minX = shapes[shapesCount % shapes.Count].Min(s => s.X);

    shapeFalling = shapes[shapesCount % shapes.Count].Select(p => new Point(p.X+ LEFT_PADDING, p.Y + currentHeight + 4)).ToList();
    shapesCount++;
    falling = true;
    //coutFall = 1;
  }
  else 
  {
    // move
    int direction = wind[windIndex % wind.Length] == '<' ? -1 : 1;
    if(shapeFalling.All(b => b.X + direction >= 0 && b.X + direction < WIDTH && !falledRocksBits.Contains(new Point(b.X + direction, b.Y))))
    {
      for (int i = 0; i < shapeFalling.Count; i++)
        shapeFalling[i] = new Point(shapeFalling[i].X + direction, shapeFalling[i].Y);
    }

    if (display)
    {
      const int HEIGHT_OFFSET = 40;
      
      Console.Clear();
      Console.SetCursorPosition(0, 40);
      Console.Write("-------");
      foreach (var b in falledRocksBits)
      {
        Console.SetCursorPosition(b.X, HEIGHT_OFFSET - b.Y);
        Console.Write('#');
      }

      foreach (var b in shapeFalling)
      {
        Console.SetCursorPosition(b.X, HEIGHT_OFFSET - b.Y);
        Console.Write('@');
      }

      Console.ReadKey();
    }

    windIndex++;
    if(windIndex % wind.Length == 0)
    {
      Console.WriteLine(shapesCount + ", height: " + currentHeight);
    }

    // fall
    if (/*coutFall > 3 || */shapeFalling.Any(b => falledRocksBits.Contains(new Point(b.X, b.Y-1)) || b.Y - 1 <= 0))
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
        shapeFalling[i] = new Point(shapeFalling[i].X, shapeFalling[i].Y-1);
    }

    if (display)
    {
      const int HEIGHT_OFFSET = 40;

      Console.Clear();
      Console.SetCursorPosition(0, 40);
      Console.Write("-------");
      foreach (var b in falledRocksBits)
      {
        Console.SetCursorPosition(b.X, HEIGHT_OFFSET - b.Y);
        Console.Write('#');
      }

      foreach (var b in shapeFalling)
      {
        Console.SetCursorPosition(b.X, HEIGHT_OFFSET - b.Y);
        Console.Write('@');
      }

      Console.ReadKey();
    }
  } 
}

// Part 1
Console.WriteLine(currentHeight);

// Part 2
// nr of stones / cicle = 1714, 3434, 5154, 6874
// next shape 1714 % 5 = 4

// (1000000000000 - 1714) / 1720
// 