using System.Drawing;

var rows = File.ReadAllLines("input.txt").Select(l => l.Select(c => int.Parse(c.ToString())).ToArray()).ToArray();

Dictionary<Point, int> trees = new Dictionary<Point, int>();
for (int i = 0; i < rows.Length; i++)
  for (int j = 0; j < rows[0].Length; j++)
    trees.Add(new Point(i,j),  rows[i][j]);

// Part 1
Console.WriteLine(trees.Count(t => IsVisibile(trees, t.Key, t.Value)));

// Part 2
Console.WriteLine(trees.Select(t => Count(trees, t.Key, t.Value)).Max());

bool IsVisibile(Dictionary<Point, int> trees, Point p, int height)
{
  if (p.X == 0 || p.Y == 0 || p.X == rows[0].Length-1 ||  p.Y == rows.Length-1) 
     return true;

  bool foundBigger = false;
  for (int i = 0; i < p.X; i++)
    if (trees[new Point(i, p.Y)] >= height)
    {
      foundBigger = true;
      break;
    }

  if (!foundBigger)
    return true;

  foundBigger = false;
  for (int i = p.X+1;  i  < rows[0].Length; i++)
    if (trees[new Point(i, p.Y)] >= height)
    {
      foundBigger = true;
      break;
    }

  if (!foundBigger)
    return true;

  foundBigger = false;
  for (int i = 0; i < p.Y; i++)
    if (trees[new Point(p.X, i)] >= height)
    {
      foundBigger = true;
      break;
    }

  if (!foundBigger)
    return true;

  foundBigger = false;
  for (int i = p.Y+1;  i  < rows.Length; i++)
    if (trees[new Point(p.X, i)] >= height)
    {
      foundBigger = true;
      break;
    }

  if (!foundBigger)
    return true;


  return false;
}

int Count(Dictionary<Point, int> trees, Point p, int height)
{
  if (p.X == 0 || p.Y == 0 || p.X == rows[0].Length - 1 || p.Y == rows.Length - 1)
    return 0;

  int count1 = 0;
  for (int i = p.X -1 ; i >= 0; i--)
  {
    count1++;
    if (trees[new Point(i, p.Y)] >= height)
      break;
  }

  int count2 = 0;
  for (int i = p.X + 1; i < rows[0].Length; i++)
  {
    count2++;
    if (trees[new Point(i, p.Y)] >= height)
      break;
  }

  int count3 = 0;
  for (int i = p.Y-1; i >=0 ; i--)
  {
    count3++;
    if (trees[new Point(p.X, i)] >= height)
      break;
  }

  int count4 = 0;
  for (int i = p.Y + 1; i < rows.Length; i++)
  {
    count4++;
    if (trees[new Point(p.X, i)] >= height)
      break;
  }

  return count1 * count2 * count3 * count4;
}