using System.Diagnostics;
using System.Drawing;

var lines = File.ReadAllLines("input.txt");

Dictionary<Point, char> map = new Dictionary<Point, char>();
for (int i = 0; i < lines.Length - 1; i++)
  for (int j = 0; j < lines[i].Length; j++)
    if (lines[i][j] != ' ')
      map.Add(new Point(j + 1, i + 1), lines[i][j]);

//Console.Clear();
//Console.SetBufferSize(10000, 10000);

//foreach (var p in map)
//{
//  Console.SetCursorPosition(p.Key.X, p.Key.Y);
//  Console.Write(p.Value);
//}

var path = lines.Last();

var directions = new List<Point>()
{
  new Point(1,0),
  new Point(0,1),
  new Point(-1,0),
  new Point(0,-1)
};

var directionsDisplay = new List<char>() 
{
  '>',
  'v',
  '<',
  '^'
};

int cout = 0;

int currentDirectionIndex = 0;
int currentPathIndex = 0;
Point currentPos = map.Keys.Where(k => k.Y == 1).OrderBy(k => k.X).First();

while (currentPathIndex < path.Length)
{
  cout++;
  string stepts = "";
  while (currentPathIndex < path.Length && path[currentPathIndex] != 'R' && path[currentPathIndex] != 'L')
  {
    stepts += path[currentPathIndex];
    currentPathIndex++;
  }

  int stepsCount = int.Parse(stepts);
  Debug.WriteLine(stepsCount);
  for (int step = 0; step < stepsCount; step++)
  {
    //Thread.Sleep(20);
    ////Console.ReadKey();
    //Console.SetCursorPosition(currentPos.X, currentPos.Y);
    //Console.Write(directionsDisplay[currentDirectionIndex % 4]);

    Point direction = directions[currentDirectionIndex % directions.Count];
    Point nextPos = new Point(currentPos.X + direction.X, currentPos.Y + direction.Y);
    if (map.ContainsKey(nextPos))
    {
      if (map[nextPos] == '#')
        break;
      else
        currentPos = nextPos;
    }
    else
    {
      int dirIndex = -1;
      int x = nextPos.X, y = nextPos.Y;
      // 11111111
      if (y < 1 && currentPos.X <= 100)
      {
        nextPos.X = 1;
        nextPos.Y = x + 100;
        dirIndex = 0;
      }

      else if (x < 1 && currentPos.Y > 150)
      {
        nextPos.X = y - 100;
        nextPos.Y = 1;
        dirIndex = 1;
      }
      //--------------------------------------------------------
      //22222222222222222
      else if (currentPos.X <= 100 && currentPos.Y <= 50 && x == 50)
      {
        nextPos.X = 1;
        nextPos.Y = 151 - y;
        dirIndex = 0;
      }

      else if (x < 1 && currentPos.Y > 100 && currentPos.Y <= 150)
      {
        nextPos.X = 51;
        nextPos.Y = 151 - y;
        dirIndex = 0;
      }
      // ----------------------------------------------------------
      //33333333333333
      else if (currentPos.Y >50 && currentPos.Y <= 100 && x == 50)
      {
        nextPos.X = y - 50;
        nextPos.Y = 101;
        dirIndex = 1;
      }

      else if (currentPos.Y > 100 && currentPos.Y <= 150 && currentPos.X <=50 && y == 100)
      {
        nextPos.X = 51;
        nextPos.Y = x + 50;
        dirIndex = 0;
      }

      // ------------------------------------------------------------
      //4444444444444444
      else if (currentPos.X > 100 && y < 1)
      {
        nextPos.X = x - 100;
        nextPos.Y = 200;
        dirIndex = 3;
      }
      else if (y > 200)
      {
        nextPos.X = x + 100;
        nextPos.Y = 1;
        dirIndex = 1;
      }

      // ---------------------------------------------------------
      //55555555555555555555
      else if (x > 150)
      {
        nextPos.Y = 151 - y;
        nextPos.X = 100;
        dirIndex = 2;
      }
      else if (currentPos.Y > 100 && x > 100)
      {
        nextPos.Y = 151 - y;
        nextPos.X = 150;
        dirIndex = 2;
      }

      // ---------------------------------------------------------
      //6666666666666666666666666
      else if (currentPos.X > 100 && currentPos.Y <= 50 && y > 50)
      {
        nextPos.X = 100;
        nextPos.Y = x - 50;
        dirIndex = 2;
      }
      else if (currentPos.X > 50 && currentPos.Y > 50 && currentPos.Y <= 100 && x > 100)
      {
        nextPos.X = y + 50;
        nextPos.Y = 50;
        dirIndex = 3;
      }

      // ---------------------------------------------------------
      //777777777777777777777777777777777
      else if (currentPos.Y > 100 && currentPos.X > 50 && y > 150)
      {
        nextPos.Y = 100 + x;
        nextPos.X = 50;
        dirIndex = 2;
      }
      else if (currentPos.Y > 150 && x > 50)
      {
        nextPos.X = y - 100;
        nextPos.Y = 150;
        dirIndex = 3;
      }

      if (map[nextPos] == '#')
        break;

      currentDirectionIndex = dirIndex;
      currentPos = nextPos;
    }
  }

  if (currentPathIndex < path.Length)
  {
    if (path[currentPathIndex] == 'R')
      currentDirectionIndex++;
    else
    {
      currentDirectionIndex--;
      if (currentDirectionIndex < 0)
        currentDirectionIndex = 3;
    }
    currentPathIndex++;
  }
}

Console.WriteLine(1000 * currentPos.Y + 4 * currentPos.X + (Math.Abs(currentDirectionIndex) % 4));