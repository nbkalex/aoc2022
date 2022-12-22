using System.Drawing;

var lines = File.ReadAllLines("input.txt");

Dictionary<Point, char> map = new Dictionary<Point, char>();
for (int i = 0; i < lines.Length - 1; i++)
  for (int j = 0; j < lines[i].Length; j++)
    if (lines[i][j] != ' ')
      map.Add(new Point(j + 1, i + 1), lines[i][j]);

//foreach (var p in map.Keys)
//{
//  Console.SetCursorPosition(p.X, p.Y);
//  Console.Write(map[p]);
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

  Point direction = directions[currentDirectionIndex % directions.Count];
  //Console.SetCursorPosition(currentPos.X, currentPos.Y);
  //Console.WriteLine(directionsDisplay[currentDirectionIndex % directions.Count] );
  //Console.ReadKey();

  int stepsCount = int.Parse(stepts);
  for (int step = 0; step < stepsCount; step++)
  {
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
      nextPos = currentPos;
      while (map.ContainsKey(nextPos))
        nextPos = new Point(nextPos.X - direction.X, nextPos.Y - direction.Y);

      nextPos = new Point(nextPos.X + direction.X, nextPos.Y + direction.Y);

      if (map[nextPos] == '#')
        break;
      currentPos = nextPos;
    }

    //Console.SetCursorPosition(currentPos.X, currentPos.Y);
    //Console.WriteLine(directionsDisplay[Math.Abs(currentDirectionIndex)]);
    //Console.ReadKey();
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

Console.WriteLine(((1000 * currentPos.Y) + (4 * currentPos.X) + (Math.Abs(currentDirectionIndex) % 4)).ToString());
