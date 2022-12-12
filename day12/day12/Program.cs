using System.Drawing;

var map = File.ReadAllLines("input.txt")
  .Select((l,i) => l.Select((c,j) => (new Point(j,i),c)))
  .Aggregate(new List<(Point,char)>(), (s, p) => { s.AddRange(p); return s; })
  .ToDictionary(p => p.Item1, p => p.Item2);

Dictionary<Point, int> visited = new Dictionary<Point, int>();
Queue<Point> toVisit = new Queue<Point>();

Point start = map.FirstOrDefault(p => p.Value == 'S').Key;
Point end = map.FirstOrDefault(p => p.Value == 'E').Key;
visited.Add(start, 0);
toVisit.Enqueue(start);
map[start] = 'a';
map[end] = 'z';

List<Point> directions = new List<Point>()
{
  new Point(0,1),
  new Point(0,-1),
  new Point(1,0),
  new Point(-1,0),
};

// Part 1
while(toVisit.Any())
{
  var current = toVisit.Dequeue();
  if(current == end)
  { 
    Console.WriteLine(visited[current]);
    Console.WriteLine();

    break;
  }

  int currentLength = visited[current];
  
  foreach (var direction in directions)
  {
    var next = new Point(current.X + direction.X, current.Y + direction.Y);
    if (map.ContainsKey(next) && map[next] <= map[current]+1)
    {
      if (visited.ContainsKey(next))
      { 
        if(visited[next] < currentLength+1)
          visited[current] = visited[next]+1;
      }
      else
      {
        toVisit.Enqueue(next);
        visited.Add(next, currentLength+1);
      }
    }
  }

}

toVisit.Clear();
visited.Clear();
toVisit.Enqueue(end);
visited.Add(end,0);
//Part 2
while (toVisit.Any())
{
  var current = toVisit.Dequeue();
  int currentLength = visited[current];

  foreach (var direction in directions)
  {
    var next = new Point(current.X + direction.X, current.Y + direction.Y);
    if (map.ContainsKey(next) && map[current] <= map[next] + 1)
    {
      if (!visited.ContainsKey(next))
      {
        toVisit.Enqueue(next);
        visited.Add(next, currentLength + 1);
      }
    }
  }
}
var min = map.Where(p => p.Value == 'a').Where(p => visited.ContainsKey(p.Key)).Select(p => visited[p.Key]).Min();
Console.WriteLine(min);
