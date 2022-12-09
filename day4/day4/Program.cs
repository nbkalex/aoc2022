var input = File.ReadAllLines("input.txt")
  .Select(l => l.Split(","))
  .Select(l => (l[0].Split("-").Select(l0 => int.Parse(l0)).ToArray(), l[1].Split("-").Select(l1 => int.Parse(l1)).ToArray()));

// Part 1
Console.WriteLine(input.Count(l => (l.Item1[0] <= l.Item2[0] && l.Item1[1] >= l.Item2[1])
              || (l.Item1[0] >= l.Item2[0] && l.Item1[1] <= l.Item2[1])));

// Part 2
Console.WriteLine(input.Count() - input.Count(l => l.Item1[1] < l.Item2[0] || l.Item1[0] > l.Item2[1]));