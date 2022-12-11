using System.Numerics;


var monkeys = File.ReadAllText("input.txt")
  .Split("\r\n\r\n")
  .Select(m => m.Split("\r\n"))
  .Select(m =>
  {
    return new Monkey()
    {
      Items = m[1].Replace("Starting items: ", "").Split(", ").Select(i => BigInteger.Parse(i)).ToList(),
      Operation = m[2].Split(" ").TakeLast(3).ToArray(),
      TestValue = long.Parse(m[3].Split(" ").Last()),
      TrueMonkeyIndex = int.Parse(m[4].Split(" ").Last()),
      FalseMonkeyIndex = int.Parse(m[5].Split(" ").Last())
    };
  }).ToList();
Dictionary<string, long> set = new Dictionary<string, long>();

long modNr = monkeys.Aggregate((long)1, (s, tn) => s *= tn.TestValue);

for (int i = 0; i < 10000; i++)
{
  foreach (var monkey in monkeys)
  {
    foreach (var item in monkey.Test())
    {
      var itemVal = item.Item2 % modNr;
      monkeys[item.Item1].Items.Add(itemVal);
      monkey.CountInspections++;
      monkey.CurrentCountInspections++;
    }

    monkey.Items.Clear();
  }

  foreach (var m in monkeys)
    m.CurrentCountInspections = 0;
}

var count = monkeys.Select(m => m.CountInspections).OrderByDescending(c => c).ToArray();
BigInteger result = count[0];
result *= count[1];
Console.WriteLine(result.ToString());

class Monkey
{

  static Dictionary<string, Func<BigInteger, BigInteger, BigInteger>> InspectTypes
  = new Dictionary<string, Func<BigInteger, BigInteger, BigInteger>>()
  {
    { "+", (f,s) => f + s },
    { "*", (f,s) => f * s },
  };

  public List<BigInteger> Items = new List<BigInteger>();
  public List<BigInteger> newItems = new List<BigInteger>();
  public string InspectType = "";
  public int TrueMonkeyIndex = 0;
  public int FalseMonkeyIndex = 0;
  public string[] Operation = new string[0];
  public long TestValue = 0;

  public long CountInspections = 0;

  public BigInteger CurrentCountInspections = 0;

  public BigInteger Inspect(BigInteger aValue)
  {
    BigInteger op1 = Operation[0] == "old" ? aValue : BigInteger.Parse(Operation[0]);
    BigInteger op2 = Operation[2] == "old" ? aValue : BigInteger.Parse(Operation[2]);
    return InspectTypes[Operation[1]](op1, op2);
  }
  public List<(int, BigInteger)> Test()
  {
    return Items.Select(i => Inspect(i)).Select(i =>
     (i % TestValue == 0 ? TrueMonkeyIndex : FalseMonkeyIndex, i)).ToList();
  }
}