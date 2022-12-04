// Part 1
Console.WriteLine(File.ReadAllLines("input.txt")
  .Select(l => (l.Take(l.Length/2), l.TakeLast(l.Length/2)))
  .Sum(l => l.Item1.Intersect(l.Item2).Sum(i => i < 'a' ? i - 'A' + 27 : i - 'a' + 1)));

// Part 2

var groups = File.ReadAllLines("input.txt")
  .Select((l, i) => (l, i))
  .GroupBy(l => l.i / 3)
  .Select(g => g.Select(l => l.l)
  .ToArray());

int total = 0;
foreach(var g in groups)
{
  var found = g[0].Intersect(g[1]).Intersect(g[2]).First();
  total += found < 'a' ? found - 'A' + 27 : found - 'a' + 1;
}

Console.WriteLine(total);