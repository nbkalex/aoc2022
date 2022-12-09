void Rezolve(string input, int length)
{
  for (int i = 0; i < input.Length - length; i++)
  {
    if (input.Skip(i).Take(length).Distinct().Count() == length)
    {
      Console.WriteLine(i + length);
      break;
    }
  }
}

string input = File.ReadAllText("input.txt");

Rezolve(input, 4);
Rezolve(input, 14);