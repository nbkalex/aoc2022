var input = File.ReadAllLines("input.txt").Select(l => new Item() { Value = long.Parse(l) * 811589153 }).ToList();

for (int i = 1; i < input.Count - 1; i++)
{
  input[i].Next = input[i + 1];
  input[i].Previous = input[i - 1];
}

input.Last().Next = input.First();
input.Last().Previous = input[input.Count - 2];
input.First().Next = input[1];
input.First().Previous = input.Last();

for (int mixTimes = 0; mixTimes < 10; mixTimes++)
{
  foreach (var item in input)
  {

    item.Previous.Next = item.Next;
    item.Next.Previous = item.Previous;

    long move = item.Value;
    var current = item.Previous;



    for (int i = 0; i < Minimize(Math.Abs(move)); i++)
    {
      if (move > 0)
        current = current.Next;
      else
        current = current.Previous;
    }

    // swap
    current.Next.Previous = item;
    item.Next = current.Next;
    current.Next = item;
    item.Previous = current;
  }
}

List<long> toFind = new List<long>();
var zero = input.First(i => i.Value == 0);
for (int i = 1; i < 3001; i++)
{
  zero = zero.Next;
  if (i % 1000 == 0)
    toFind.Add(zero.Value);
}

Console.WriteLine(toFind.Sum());

long Minimize(long val)
{
  if(val < input.Count)
    return val;
  else
    return Minimize(val % input.Count + val / input.Count);

}


class Item
{
  public long Value { get; set; }
  public Item Next { get; set; }
  public Item Previous { get; set; }
  public override string ToString()
  {
    return Value.ToString();
  }
}
