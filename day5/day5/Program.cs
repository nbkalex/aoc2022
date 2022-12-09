using System.Linq;

var lines = File.ReadAllLines("input.txt");
var processed = lines.Select(l => 
l.Replace("move ", "")
 .Replace("from ", "")
 .Replace("to ", "")
 ).ToArray();

Stack<char>[] stacks = Enumerable.Range(0, (lines[0].Length - (lines[0].Length/3)) / 3 + 1).Select(r => new Stack<char>()).ToArray();

int index = 0;
foreach(var line in processed.TakeWhile(l => !l.StartsWith(" 1")).Reverse())
{
  index++;

  for (int i = 0; i < line.Length; i+=4)
  {
    if (line[i+1] != ' ')
      stacks[i/4].Push(line[i + 1]);
  }
}

// Part 1
//for (int i = index + 2; i < processed.Length; i++)
//{
//  var operands = processed[i].Split(" ").Select(o => int.Parse(o)-1).ToArray();
//  for (int oi = 0; oi <= operands[0]; oi++)
//    stacks[operands[2]].Push(stacks[operands[1]].Pop());
//}

// Part 2
for (int i = index + 2; i < processed.Length; i++)
{
  var operands = processed[i].Split(" ").Select(o => int.Parse(o) - 1).ToArray();
  List<char> cratesToMove = new List<char>();
  for (int oi = 0; oi <= operands[0]; oi++)
    cratesToMove.Add(stacks[operands[1]].Pop());

  cratesToMove.Reverse();
  foreach (var crate in cratesToMove)
    stacks[operands[2]].Push(crate);
}

Console.WriteLine(new string(stacks.Select(s => s.Pop()).ToArray()));