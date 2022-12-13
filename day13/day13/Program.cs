var pairs = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(group => group.Split("\r\n").Select(l => new ValuesList(l)).ToArray()).ToList();

foreach (var pair in pairs)
{
  Console.WriteLine();
  foreach (var list in pair)
    Console.WriteLine(list);
}


// Part 1
Console.WriteLine(pairs.Where(p => ValuesList.compare(p[0], p[1]) == -1).Select(p => pairs.IndexOf(p) + 1).Sum());

// Part 2
var lists = File.ReadAllLines("input.txt")
  .Where(l => l.Any())
  .Select(l => new ValuesList(l))
  .ToList();
lists.Add(new ValuesList("[[2]]"));
lists.Add(new ValuesList("[[6]]"));

var ordered = lists.OrderBy(l => l).ToList();
Console.WriteLine((ordered.FindIndex(l => l.ToString() == "[[2]]") + 1) * (ordered.FindIndex(l => l.ToString() == "[[6]]") + 1));


class ValuesList : List<object>, IComparable
{
  public ValuesList(string aValue)
  {
    Stack<ValuesList> valuesLists = new Stack<ValuesList>() { };
    valuesLists.Push(this);
    for (int i = 1; i < aValue.Length - 1; i++)
    {
      char c = aValue[i];
      if (c == '[')
      {
        var newList = new ValuesList();
        valuesLists.First().Add(newList);
        valuesLists.Push(newList);
      }
      else if (c == ']')
      {
        valuesLists.Pop();
      }
      else if (c == ',')
        continue;
      else
      {
        string number = c.ToString();
        while (char.IsDigit(aValue[i + 1]))
          number += aValue[++i];
        valuesLists.First().Add(int.Parse(number));
      }
    }
  }

  public ValuesList() { }

  public override string ToString()
  {
    string val = "[";

    foreach (var obj in this)
      val += obj.ToString() + ",";

    if (val.Length > 1)
      val = val.Substring(0, val.Length - 1);

    val += "]";

    return val;
  }

  public int CompareTo(object? obj)
  {
    return compare(this, (ValuesList)obj);
  }

  public static int compare(ValuesList a, ValuesList b)
  {
    if(a == b)
      return 0;

    for (int i = 0; i < a.Count; i++)
    {
      if (i == b.Count)
        return 1;

      var v1List = a[i] as ValuesList;
      var v2List = b[i] as ValuesList;

      if (v1List == null && v2List == null)
      {
        int first = (int)a[i];
        int second = (int)b[i];
        if(first.CompareTo(second) != 0)
          return first.CompareTo(second);
      }
      else
      {
        if (v1List == null)
          v1List = new ValuesList() { (int)a[i] };
        if (v2List == null)
          v2List = new ValuesList() { (int)b[i] };

        if (compare(v1List, v2List) == 0)
          continue;

        return compare(v1List, v2List);
      }
    }

    return a.Count.CompareTo(b.Count);
  }

}
