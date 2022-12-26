var blueprints = File.ReadAllLines("input.txt")
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


const int kMinutes = 32;
const int kOreIndex = 0;
const int kClayIndex = 1;
const int kObsidianOreIndex = 2;
const int kObsidianClayIndex = 3;
const int kGeodeOreIndex = 4;
const int kGeodeObsidianIndex = 5;

int totalScore = 0;

foreach (var blueprint in blueprints)
{
  Stack<(int, List<int>, List<int>)> options = new Stack<(int, List<int>, List<int>)>();
  options.Push((0, new List<int>() { 0, 0, 0, 0 }, new List<int>() { 1, 0, 0, 0 }));

  int maxGeodes = 0;
  Dictionary<int, int> minTimeGeodes = new Dictionary<int, int>();
  while (options.Any())
  {
    var option = options.Pop();

    int time = option.Item1;
    List<int> res = option.Item2;
    List<int> robots = option.Item3;

    if (time == kMinutes-1)
    {
      if (res[3] + robots[3] > maxGeodes)
        maxGeodes = res[3] + robots[3];
      continue;
    }

    // build ore
    int neededTime = 0;
    List<int> newRes = new List<int>(res);
    List<int> newRobots = new List<int>(robots);
    while (newRes[0] < blueprint[kOreIndex])
    {
      for (int i = 0; i < 4; i++)
        newRes[i] += robots[i];

      neededTime++;

      if (time + neededTime == kMinutes)
      {
        if (newRes[3] > maxGeodes)
          maxGeodes = newRes[3];

        break;
      }
    }

    if (time + neededTime + 1 < kMinutes)
    {
      for (int i = 0; i < 4; i++)
        newRes[i] += robots[i];

      newRes[0] -= blueprint[kOreIndex];
      newRobots[0]++;
      options.Push((time + neededTime + 1, newRes, newRobots));
    }

    // build clay
    neededTime = 0;
    newRes = new List<int>(res);
    newRobots = new List<int>(robots);
    while (newRes[0] < blueprint[kClayIndex])
    {
      for (int i = 0; i < 4; i++)
        newRes[i] += robots[i];

      neededTime++;

      if (time + neededTime == kMinutes)
      {
        if (newRes[3] > maxGeodes)
          maxGeodes = newRes[3];

        break;
      }
    }

    if (time + neededTime + 1 < kMinutes)
    {
      for (int i = 0; i < 4; i++)
        newRes[i] += robots[i];

      newRes[0] -= blueprint[kClayIndex];
      newRobots[1]++;
      options.Push((time + neededTime + 1, newRes, newRobots));
    }

    // build obsidian
    if (robots[1] > 0)
    {
      neededTime = 0;
      newRes = new List<int>(res);
      newRobots = new List<int>(robots);
      while (newRes[0] < blueprint[kObsidianOreIndex] || newRes[1] < blueprint[kObsidianClayIndex])
      {
        for (int i = 0; i < 4; i++)
          newRes[i] += robots[i];

        neededTime++;

        if (time + neededTime == kMinutes)
        {
          if (newRes[3] > maxGeodes)
            maxGeodes = newRes[3];

          break;
        }
      }

      if (time + neededTime + 1 < kMinutes)
      {
        for (int i = 0; i < 4; i++)
          newRes[i] += robots[i];

        newRes[0] -= blueprint[kObsidianOreIndex];
        newRes[1] -= blueprint[kObsidianClayIndex];
        newRobots[2]++;
        options.Push((time + neededTime + 1, newRes, newRobots));
      }
    }

    // build geode
    if (robots[2] > 0)
    {
      neededTime = 0;
      newRes = new List<int>(res);
      newRobots = new List<int>(robots);
      while (newRes[0] < blueprint[kGeodeOreIndex] || newRes[2] < blueprint[kGeodeObsidianIndex])
      {
        for (int i = 0; i < 4; i++)
          newRes[i] += robots[i];

        neededTime++;

        if (time + neededTime == kMinutes)
        {
          if (newRes[3] > maxGeodes)
            maxGeodes = newRes[3];

          break;
        }
      }

      if (time + neededTime + 1 < kMinutes)
      {
        for (int i = 0; i < 4; i++)
          newRes[i] += robots[i];

        newRes[0] -= blueprint[kGeodeOreIndex];
        newRes[2] -= blueprint[kGeodeObsidianIndex];
        newRobots[3]++;
        options.Push((time + neededTime + 1, newRes, newRobots));
      }
    }
  }

  Console.WriteLine(maxGeodes);
  totalScore += maxGeodes * (blueprints.IndexOf(blueprint) + 1);
}

Console.WriteLine(totalScore);