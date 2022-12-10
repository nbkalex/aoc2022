var instructions = File.ReadAllLines("input.txt").Select(l => l.Split(" ")).ToArray();
Dictionary<string, int> instructionCicles = new Dictionary<string, int>()
{
  {"noop", 1 },
  {"addx", 2 },
};

int x = 1;
Dictionary<string, Action<string[]>> instructionExe = new Dictionary<string, Action<string[]>>()
{
  {"noop", i => { return; } },
  {"addx", i => x += int.Parse(i[1]) }
};

int currentInstructionCycle = 0;
int ip = 0;

int total = 0;

for (int i = 0; i < 220; i++)
{
  // execution
  if(currentInstructionCycle == instructionCicles[instructions[ip][0]])
  {
    instructionExe[instructions[ip][0]](instructions[ip]);
    currentInstructionCycle = 0;
    ip++;

    if (ip == instructions.Length)
      break;
  }

  if ((i + 1) == 20 || (i+1 - 20) %40 == 0)
    total += (i + 1) * x;

  int pos = i % 40;
  Console.Write(pos > x-2 && pos < x+2 ? '#' : ' ' );
  if (pos == 39)
    Console.WriteLine();

  currentInstructionCycle++;
}

Console.WriteLine(x);
Console.WriteLine(total);