
// Part 1
Console.WriteLine(File.ReadAllText("input.txt")
  .Split("\r\n\r\n")
  .Select(g => g.Split("\r\n").Select(l => int.Parse(l)).Sum())
  .Max());

// Part 2
Console.WriteLine(File.ReadAllText("input.txt")
  .Split("\r\n\r\n")
  .Select(g => g.Split("\r\n").Select(l => int.Parse(l)).Sum())
  .OrderBy(s => s).TakeLast(3).Sum());