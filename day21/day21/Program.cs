var monkeys = File.ReadAllLines("input.txt").Select(l => l.Split(" ")).ToDictionary(l => l[0].Replace(":", ""), l => l.Skip(1).ToList());

Dictionary<string, Func<long, long, long>> operations = new Dictionary<string, Func<long, long, long>>()
{
  { "*", (a,b) => a * b },
  { "/", (a,b) => a / b },
  { "+", (a,b) => a + b },
  { "-", (a,b) => a - b },
  { "=", (a,b) => a < b ? -1 : a > b ? 1 : 0 }
};


long GetValue(string operand)
{
  var operation = monkeys[operand];
  long val = 0;
  if (long.TryParse(operation[0], out val))
    return val;

  long val1 = GetValue(operation[0]);
  long val2 = GetValue(operation[2]);

  if (operand == "root")
  {
    //if(val1 < val2)
    //{
      
    //}

    Console.WriteLine(GetValue("humn").ToString() + ": " + val1 + ", " + val2);
    //Console.ReadKey();
  }
  return operations[operation[1]](val1, val2);
}

// Part 1
Console.WriteLine(GetValue("root"));

//Part 2
monkeys["root"][1] = "=";
long myNr = 0;
long myNr2 = long.MaxValue;
while (true)
{
  monkeys["humn"] = new List<string>() { ((myNr + myNr2) / 2).ToString() };
  long value = GetValue("root");
  if (value == 1)
    myNr = (myNr + myNr2) / 2;
  else if (value == -1)
    myNr2 = (myNr + myNr2) / 2;
  else
  { 
    Console.WriteLine(((myNr + myNr2) / 2).ToString());
    break;
  }
}