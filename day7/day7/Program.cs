var instructions = File.ReadAllLines("input.txt");

string path = string.Empty;
Dir root = new Dir(null, "/");
Dir currentDir = root;
List<Dir> allDirs = new List<Dir>();
allDirs.Add(root);

foreach (string line in instructions)
{
  var tokens = line.Split(" ");

  if (tokens[1] == "cd")
  {
    if (tokens[2] != "..")
    {
      if(tokens[2] == root.name)
        continue;

      currentDir = currentDir.dirs[tokens[2]];
      path = Path.Combine(path, tokens[2]);
      continue;
    }
    else
    {
      currentDir = currentDir.Parent;
    }
  }

  int size = 0;
  if (int.TryParse(tokens[0], out size))
    currentDir.files.Add(tokens[1], size);
  else if (tokens[0] == "dir")
  { 
    var newDir = new Dir(currentDir, tokens[1]);
    currentDir.dirs.Add(tokens[1], newDir);
    allDirs.Add(currentDir.dirs[tokens[1]]);
  }
}

// Part 1
Console.WriteLine(allDirs.Select(d => d.Size).Where(size => size <= 100000).Sum());

// Part 2
const int TOTAL = 70000000;
const int NEEDED = 30000000;

long myNeeded = NEEDED - TOTAL + root.Size;
Console.WriteLine(allDirs.Select(d => d.Size).Where(s => s >= myNeeded).Min());


class Dir
{
  public Dir Parent { get; set; }
  public Dictionary<string, Dir> dirs = new Dictionary<string, Dir>();
  public string name = "";
  public Dictionary<string, int> files = new Dictionary<string, int>();
  public Dir(Dir aParent, string aname) { name = aname; Parent = aParent; }
  public int Size { get { return dirs.Values.Aggregate(0, (s, d) => s + d.Size) + files.Values.Sum(); } }
}