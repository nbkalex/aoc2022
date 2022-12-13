var pairs = File.ReadAllText("input.txt").Split("\r\n\r\n").Select(group => group.Split("\r\n").Select(l => new ValuesList(l)).ToArray()).ToList();

foreach (var pair in pairs)
{
  Console.WriteLine();
  foreach (var list in pair)
    Console.WriteLine(list);
}

foreach (var pair in pairs)
  if (!(pair[0] > pair[1]))
    Console.Write(pairs.IndexOf(pair) + 1 + ",");

Console.WriteLine();

// Part 1
Console.WriteLine(pairs.Where(p => !(p[0] > p[1])).Select(p => pairs.IndexOf(p) + 1).Sum());

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
      if(this == (ValuesList)obj)
        return 0;
      else if(this > (ValuesList)obj)
        return 1;
      else return -1;
    }

    public static bool operator >(ValuesList a, ValuesList b)
  {
    for (int i = 0; i < a.Count; i++)
    {
      if (i == b.Count)
        return true;

      var v1List = a[i] as ValuesList;
      var v2List = b[i] as ValuesList;

      if (v1List == null && v2List == null)
      {
        int first = (int)a[i];
        int second = (int)b[i];

        if (first > second)
          return true;

        if (first < second)
          return false;
      }
      else
      {
        if (v1List == null)
          v1List = new ValuesList() { (int)a[i] };
        if (v2List == null)
          v2List = new ValuesList() { (int)b[i] };

        if (v1List == v2List)
          continue;

        return v1List > v2List;
      }
    }

    return false;
  }

  public static bool operator ==(ValuesList a, ValuesList b)
  {
    if (object.ReferenceEquals(a, null) && object.ReferenceEquals(b, null))
      return true;

    if (object.ReferenceEquals(a, null))
      return false;

    if (object.ReferenceEquals(b, null))
      return false;

    if (a.Count != b.Count) return false;

    for (int i = 0; i < a.Count; i++)
    {
      var v1List = a[i] as ValuesList;
      var v2List = b[i] as ValuesList;

      if (v1List == null && v2List == null)
      {
        int first = (int)a[i];
        int second = (int)b[i];
        if(first != second)
          return false;
      }
      else
      {
        if (v1List == null)
          v1List = new ValuesList() { (int)a[i] };
        if (v2List == null)
          v2List = new ValuesList() { (int)b[i] };

        if(v1List != v2List)
          return false;
      }
    }

    return true;
  }

  public static bool operator !=(ValuesList a, ValuesList b)
  {
    return !(a == b);
  }

  public static bool operator <(ValuesList a, ValuesList b)
  {
    return false;
  }
}
