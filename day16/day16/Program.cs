using System.Collections;

var valves = File.ReadAllLines("input.txt")
  .Select(l =>
  {
    var tokensSpace = l.Split(" ");
    return new Valve()
    {
      Id = tokensSpace[1],
      Rate = int.Parse(tokensSpace[4].Split("=")[1].Split(";")[0]),
      NextValves = tokensSpace.Skip(9).Select(t => t.Split(",")[0]).ToList()
    };
  })
  .ToDictionary(t => t.Id, t => t);

Dictionary<string, Dictionary<string, int>> distances = new Dictionary<string, Dictionary<string, int>>();
distances.Add("AA", Distances("AA")
  .Where(v => valves[v.Key].Rate != 0)
  .ToDictionary(v => v.Key, v => v.Value));

foreach (var v in valves)
{
  if (v.Value.Rate == 0)
    continue;

  distances.Add(v.Key, Distances(v.Key)
    .Where(v => valves[v.Key].Rate != 0)
    .ToDictionary(v => v.Key, v => v.Value));
}

//Console.WriteLine(distances["AA"]["HH"]);

Stack<(string, HashSet<string>, int, int)> toVisit = new Stack<(string, HashSet<string>, int, int)>();
toVisit.Push(("AA", new HashSet<string>(), 30, 0));
List<(string, HashSet<string>, int, int)> visited = new List<(string, HashSet<string>, int, int)>();

while (toVisit.Any())
{
  var current = toVisit.Pop();
  var currentId = current.Item1;
  var opened = current.Item2;
  var currentTime = current.Item3;
  var total = current.Item4;

  if (currentTime < 2)
    continue;

  var closedValves = valves.Where(v => v.Value.Rate != 0 && !opened.Contains(v.Key)).ToList();

  if (!closedValves.Any())
    continue;

  foreach (var cv in closedValves)
  {
    int nextTime = currentTime - distances[currentId][cv.Key] - 1;
    int presure = cv.Value.Rate * nextTime;
    var openedNext = new HashSet<string>(opened);
    openedNext.Add(cv.Key);
    int nextTotal = total + presure;
    toVisit.Push((cv.Key, openedNext, nextTime, nextTotal));
    visited.Add((cv.Key, openedNext, nextTime, nextTotal));
  }
}

Console.WriteLine(visited.Max(v => v.Item4));

Dictionary<string, int> Distances(string start)
{
  Dictionary<string, int> distances = new Dictionary<string, int>();
  distances.Add(start, 0);
  Stack<(string, int)> to = new Stack<(string, int)>();
  to.Push((start, 0));
  while (to.Any())
  {
    var current = to.Pop();
    string currentId = current.Item1;
    int currentCost = current.Item2;

    int nextCost = currentCost + 1;
    foreach (var v in valves[current.Item1].NextValves)
    {
      if (distances.ContainsKey(v))
      {
        if (distances[v] < nextCost)
          continue;
        else
          distances[v] = nextCost;
      }
      else
        distances.Add(v, nextCost);

      to.Push((v, nextCost));
    }
  }

  return distances;
}

class Valve
{
  public string Id { get; set; } = "";
  public int Rate { get; set; } = 0;
  public List<string> NextValves { get; set; } = new List<string>();
}