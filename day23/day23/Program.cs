using System.Drawing;

var lines = File.ReadAllLines("input.txt");

List<Point> elves = new List<Point>();
for (int i = 0; i < lines.Length; i++)
  for (int j = 0; j < lines[0].Length; j++)
    if (lines[i][j] == '#')
      elves.Add(new Point(j, i));

Queue<(Point, HashSet<Point>)> directions = new Queue<(Point, HashSet<Point>)>(
  new[]
{
  (new Point(0,-1), new HashSet<Point>(){ new Point(0, -1), new Point(1, -1), new Point(-1, -1) }),    // N
  (new Point(0,1), new HashSet<Point>(){ new Point(0, 1), new Point(1, 1), new Point(-1, 1) }),        // S
  (new Point(-1,0), new HashSet<Point>(){ new Point(-1, 0), new Point(-1, -1), new Point(-1, 1) }),    // V
  (new Point(1,0), new HashSet<Point>(){ new Point(1, 0), new Point(1, -1), new Point(1, 1) }),        // E
});

const int ROUNDS = 1000;
for (int i = 0; i < ROUNDS; i++)
{
  //Print();

  // proposal phase 
  List<(Point, Point)> proposals = new List<(Point, Point)>();
  foreach (var elf in elves)
  {
    Point? dirToUse = null;
    bool hasNeighbour = false;
    foreach (var dir in directions)
    {
      bool foundDir = true;

      var newPositions = dir.Item2.Select(d => new Point(d.X + elf.X, d.Y + elf.Y));
      foreach (var newPos in newPositions)
        if (elves.Contains(newPos))
        {
          foundDir = false;
          hasNeighbour = true;
          break;
        }

      if (foundDir && dirToUse == null)
        dirToUse = dir.Item1;
    }

    if(hasNeighbour && dirToUse != null)
      proposals.Add((elf, new Point(elf.X + dirToUse.Value.X, elf.Y + dirToUse.Value.Y)));
  }


  // validate proposal
  proposals = proposals.Where(p => proposals.Count(p2 => p2.Item2 == p.Item2) == 1).ToList();

  if(proposals.Count == 0)
  {
    Console.WriteLine(i+1);
    break;
  }

  // apply proposal
  foreach (var proposal in proposals)
    elves[elves.IndexOf(proposal.Item1)] = proposal.Item2;

  // rotate direction
  var firstDir = directions.Dequeue();
  directions.Enqueue(firstDir);

  //Print();
}

int width = elves.Max(e => e.X) - elves.Min(e => e.X) + 1;
int height = elves.Max(e => e.Y) - elves.Min(e => e.Y) + 1;

Console.WriteLine(width * height - elves.Count);

void Print()
{
  // print
  Console.Clear();
  foreach (var e in elves)
  {
    Console.SetCursorPosition(10 + e.X, 10 + e.Y);
    Console.Write('#');
  }
  Console.ReadKey();
}