using System.Drawing;

Dictionary<string, Point> directions = new Dictionary<string, Point>()
{
  { "R", new Point(1, 0) },
  { "L", new Point(-1, 0) },
  { "U", new Point(0, 1) },
  { "D", new Point(0, -1) },
};

int offset = 20;

var input = File.ReadAllLines("input.txt").Select(l => l.Split(" ")).Select(l => (directions[l[0]], int.Parse(l[1])));

Point head = new Point();
Point tail = new Point();
HashSet<Point> visited = new HashSet<Point>();
visited.Add(tail);

foreach (var line in input)
{
  for (int i = 0; i < line.Item2; i++)
  {
    head = new Point(head.X + line.Item1.X, head.Y + line.Item1.Y);
    if (Math.Abs(head.X - tail.X) > 1)
      tail = new Point(tail.X + line.Item1.X, head.Y);
    else if (Math.Abs(head.Y - tail.Y) > 1)
      tail = new Point(head.X, tail.Y + line.Item1.Y);
    visited.Add(tail);
  }
}

HashSet<Point> visited2 = new HashSet<Point>();
List<Point> tails = Enumerable.Range(1, 10).Select(r => new Point()).ToList();
visited2.Add(tails.Last());
foreach (var line in input)
{
  for (int i = 0; i < line.Item2; i++)
  {
    Print();
    bool enablePrint = false;
    Point direction = line.Item1;
    tails[0] = new Point(tails[0].X + direction.X, tails[0].Y + direction.Y);
    var currentHead = tails.First();
    for (int it = 1; it < tails.Count; it++)
    {
      Print();

      var prevPos = tails[it];
      if (Math.Abs(currentHead.X - tails[it].X) > 1)
      {
        var yVal = currentHead.Y;
        if (currentHead.Y - tails[it].Y > 1)
          yVal--;
        if (currentHead.Y - tails[it].Y < -1)
          yVal++;

        tails[it] = new Point(tails[it].X + direction.X, yVal);
        direction.Y = tails[it].Y - prevPos.Y;
      }
      else
      if (Math.Abs(currentHead.Y - tails[it].Y) > 1)
      {
        var xVal = currentHead.X;
        if (currentHead.X - tails[it].X > 1)
          xVal--;
        if (currentHead.X - tails[it].X < -1)
          xVal++;

        tails[it] = new Point(xVal, tails[it].Y + direction.Y);
        direction.X = tails[it].X - prevPos.X;
      }
      else
      break;

      Print();

      currentHead = tails[it];

      if (tails[it] == tails.Last())
        visited2.Add(tails.Last());
    }
  }
}


Console.WriteLine(visited.Count);
Console.WriteLine(visited2.Count);
void Print()
{
  //Console.Clear();
  //int index = 0;
  //var tailsR = new List<Point>(tails);
  //tailsR.Reverse();
  //foreach (var t in tailsR)
  //{
  //  Console.SetCursorPosition(offset + t.X, offset + t.Y);
  //  Console.Write(t == tailsR.Last() ? "H" : (9 - index).ToString());
  //  index++;
  //}
}