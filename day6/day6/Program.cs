string input = File.ReadAllText("input.txt");
Console.WriteLine(Enumerable.Range(0, input.Length - 4).First(i => input.Skip(i).Take(4).Distinct().Count() == 4) + 4);
Console.WriteLine(Enumerable.Range(0, input.Length - 14).First(i => input.Skip(i).Take(14).Distinct().Count() == 14) + 14);