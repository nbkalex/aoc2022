using System.Drawing;

var input = File.ReadAllLines("input.txt")
  .Select(l => l.Split(" -> ")
                .Select(t => new Point(int.Parse(t.Split(",")[0]), int.Parse(t.Split(",")[1])))
                .ToList())
  .ToList();

var map = new Dictionary<Point, char>();
foreach (var blocks in input)
{
  Point current = blocks.First();
  foreach (var p in blocks.Skip(1))
  {
    if (p.X != current.X)
      for (int i = Math.Min(current.X, p.X); i <= Math.Max(current.X, p.X); i++)
        if (!map.ContainsKey(new Point(i, p.Y)))
          map.Add(new Point(i, p.Y), '#');

    if (p.Y != current.Y)
      for (int i = Math.Min(current.Y, p.Y); i <= Math.Max(current.Y, p.Y); i++)
        if (!map.ContainsKey(new Point(p.X, i)))
          map.Add(new Point(p.X, i), '#');

    current = p;
  }
}

Point sandGen = new Point(500, 0);
Point sandGenFirst = new Point(500, 1);
//map.Add(sandGen, '+');

int minX = map.Keys.Min(k => k.X);
int maxX = map.Keys.Max(k => k.X);
int minY = map.Keys.Min(k => k.Y);
int maxY = map.Keys.Max(k => k.Y);

bool isDropping = false;
Point currentDrop = new Point();
bool displayMap = false;
while (true)
{
  if (!isDropping)
  {
    if (map.ContainsKey(sandGen))
      break;
    isDropping = true;
    currentDrop = sandGen;
    map.Add(sandGen, 'O');
  }
  else
  {
    Point down = new Point(currentDrop.X, currentDrop.Y + 1);
    Point downLeft = new Point(currentDrop.X - 1, currentDrop.Y + 1);
    Point downRight = new Point(currentDrop.X + 1, currentDrop.Y + 1);

    if (down.Y != 2 + maxY)
    {
      if (!map.ContainsKey(down))
      {
        map.Add(down, 'O');
        map.Remove(currentDrop);
        currentDrop = down;
      }
      else if (!map.ContainsKey(downLeft))
      {
        map.Add(downLeft, 'O');
        map.Remove(currentDrop);
        currentDrop = downLeft;
      }
      else if (!map.ContainsKey(downRight))
      {
        map.Add(downRight, 'O');
        map.Remove(currentDrop);
        currentDrop = downRight;
        //DisplayMap();
      }
      else
      {
        isDropping = false;
      }
    }
    else
    {
      isDropping = false;
    }

    //if(currentDrop.Y > maxY)
      //break;
  }


  if (displayMap)
    DisplayMap();
}

Console.WriteLine(map.Values.Count(c => c == 'O'));

// ----------------------------------------------------------------------------------------------------------

void DisplayMap()
{
  //Console.ReadKey();
  int minX2 = map.Keys.Min(k => k.X);

  Console.Clear();
  foreach (var p in map)
  {
    Console.SetCursorPosition(p.Key.X - minX2, p.Key.Y - minY + 3);
    Console.Write(p.Value);
  }
}