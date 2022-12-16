using System.Collections;

var tunnels = File.ReadAllLines("input.txt")
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

const int time = 30;
Stack<(string, HashSet<string>, int, int)> toGo = new Stack<(string, HashSet<string>, int, int)>();
HashSet<(string, HashSet<string>, int, int)> visited = new HashSet<(string, HashSet<string>, int, int)>();

HashSet<string> startSet = new HashSet<string>();
startSet.Add("AA");

toGo.Push(("AA", new HashSet<string>(), time, 0));
Dictionary<string, int> times = new Dictionary<string, int>();

while (toGo.Any())
{
  var current = toGo.Pop();
  if (current.Item3 <= 2)
    continue;

  string currentId = current.Item1.Substring(current.Item1.Length - 2);

  var openedValves = current.Item2;
  int currentTime = current.Item3;
  int currentPressure = current.Item4;

  var nextValves = tunnels[currentId].NextValves.Select(v => tunnels[v]);

  foreach (var nextValve in nextValves)
  {
    string nextWay = current.Item1 + nextValve.Id;
    if (nextValve.Rate > 0 && !openedValves.Contains(nextValve.Id))
    {
      var nextOpenedValves = new HashSet<string>(openedValves);
      nextOpenedValves.Add(nextValve.Id);
      int openedPresure = currentPressure + (currentTime - 2) * nextValve.Rate;

      toGo.Push((nextWay, nextOpenedValves, currentTime - 2, openedPresure));
      visited.Add((nextWay, nextOpenedValves, currentTime - 2, openedPresure));
    }

    toGo.Push((nextWay, new HashSet<string>(openedValves), currentTime - 1, currentPressure));
    visited.Add((nextWay, new HashSet<string>(openedValves), currentTime - 1, currentPressure));
  }
}

int max = visited.Max(v => v.Item4);

var found = visited.First(v => v.Item4 == max);

var adcb = visited.Where(v => v.Item1.StartsWith("AADDCCBB")).ToList();

Console.WriteLine(found.Item4);

Console.WriteLine();



class Valve
{
  public string Id { get; set; } = "";
  public int Rate { get; set; } = 0;
  public List<string> NextValves { get; set; } = new List<string>();
}