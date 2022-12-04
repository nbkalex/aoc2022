// Part 1
var points = new Dictionary<string, int>()
{
  { "A X", 3 },
  { "A Y", 6 },
  { "A Z", 0 },
  { "B X", 0 },
  { "B Y", 3 },
  { "B Z", 6 },
  { "C X", 6 },
  { "C Y", 0 },
  { "C Z", 3 }
}; 
Console.WriteLine(File.ReadAllLines("input.txt").Sum(l => points[l] + l.Last() - 'X' + 1));

// Part 2
var points2 = new Dictionary<string, int>()
{
  { "A X", 3 },
  { "A Y", 1 },
  { "A Z", 2 },
  { "B X", 1 },
  { "B Y", 2 },
  { "B Z", 3 },
  { "C X", 2 },
  { "C Y", 3 },
  { "C Z", 1 }
};
Console.WriteLine(File.ReadAllLines("input.txt").Sum(l => points2[l] + ((l.Last() - 'X') *3)));
