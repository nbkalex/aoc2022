var blueprinsts = File.ReadAllLines("input.txt")
  .Select(l =>
  {
    var tokens = l.Split(" ");
    return new List<int>() {
      int.Parse(tokens[6]),
      int.Parse(tokens[12]),
      int.Parse(tokens[18]),
      int.Parse(tokens[21]),
      int.Parse(tokens[27]),
      int.Parse(tokens[30])
    };
  }).ToList();


const int kMinutes = 24;
const int kOreIndex = 0;
const int kClayIndex = 1;
const int kObsidianOreIndex = 2;
const int kObsidianClayIndex = 3;
const int kGeodeOreIndex = 4;
const int kGeodeObsidianIndex = 5;

foreach (var blueprint in blueprinsts)
{
  Stack<(int, List<int>, List<int>)> options = new Stack<(int, List<int>, List<int>)>();
  options.Push((kMinutes, new List<int>() { 0, 0, 0, 0 }, new List<int>() { 1, 0, 0, 0 }));

  List<int> geordesCounts = new List<int>();
  List<(int, List<int>, List<int>)> allOptions = new List<(int, List<int>, List<int>)>();
  while (options.Any())
  {
    var option = options.Pop();
    var init_res = new List<int>(option.Item2);
    var init_robots = new List<int>(option.Item3);

    int time = option.Item1;
    time--;

    var res = new List<int>(option.Item2);
    var robots = new List<int>(option.Item3);
    for (int i = 0; i < 4; i++)
      res[i] += robots[i];

    if (time == 0)
    {
      if (res[3] > 0)
        geordesCounts.Add(res[3]);
      continue;
    }

    options.Push((time, res, robots));

    if (init_res[0] >= blueprint[kOreIndex])
    {
      if (time == 1)
      {
        if (robots[3] > 0)
          geordesCounts.Add(res[3] + robots[3]);
      }
      else
      {
        var newrobots = new List<int>(option.Item3);
        newrobots[0]++;
        var newRes = new List<int>(res);
        newRes[0] -= blueprint[kOreIndex];
        options.Push((time, newRes, newrobots));
      }
    }

    if (init_res[0] >= blueprint[kClayIndex])
    {
      if (time == 1)
      {
        if (robots[3] > 0)
          geordesCounts.Add(res[3] + robots[3]);
      }
      else
      {
        var newrobots = new List<int>(option.Item3);
        var newRes = new List<int>(res);
        newRes[0] -= blueprint[kClayIndex];
        newrobots[1]++;
        options.Push((time, newRes, newrobots));
      }
    }

    if (init_res[0] > blueprint[kObsidianOreIndex] && init_res[1] >= blueprint[kObsidianClayIndex])
    {
      if (time == 1)
      {
        if (robots[3] > 0)
          geordesCounts.Add(res[3] + robots[3]);
      }
      else
      {
        var newrobots = new List<int>(option.Item3);
        newrobots[2]++;
        var newRes = new List<int>(res);
        newRes[0] -= blueprint[kObsidianOreIndex];
        newRes[1] -= blueprint[kObsidianClayIndex];
        options.Push((time, newRes, newrobots));
      }
    }

    if (init_res[0] > blueprint[kGeodeOreIndex] && init_res[2] >= blueprint[kGeodeObsidianIndex])
    {
      if (time == 1)
        geordesCounts.Add(res[3] + robots[3] + 1);
      else
      {
        var newrobots = new List<int>(option.Item3);
        newrobots[3]++;
        var newRes = new List<int>(res);
        newRes[0] -= blueprint[kGeodeOreIndex];
        newRes[2] -= blueprint[kGeodeObsidianIndex];
        options.Push((time, newRes, newrobots));
      }
    }
  }

  Console.WriteLine(geordesCounts.Max());
}