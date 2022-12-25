var codedNumbers = File.ReadAllLines("input.txt");

var cMap = new Dictionary<char, long>()
{
  { '2', 2 },
  { '1', 1 },
  { '0', 0 },
  { '-', -1 },
  { '=', -2 }
};

long sum = codedNumbers.Select(no => no.Reverse().Select((c, i) => cMap[c] * (long)Math.Pow(5, i)).Sum()).Sum();
Console.WriteLine(sum);

List<long> digits = new List<long>();

int index = 0;
while (sum > 0)
{
  long mod = sum % 5;
  digits.Insert(0, mod);
  sum /= 5;

  index++;
}

Update(digits, digits.Count - 1);

var cMapRev = new Dictionary<long, char>()
{
  { 2 , '2' },
  { 1 , '1' },
  { 0 , '0' },
  { -1, '-' },
  { -2, '=' }
};

foreach (long d in digits)
  Console.Write(cMapRev[d]);

void Update(List<long> aDigits, int aIndex)
{
  if(aIndex < 0)
    return;

  if (aDigits[aIndex] > 2)
  {
    aDigits[aIndex] = aDigits[aIndex] - 5;

    if (aIndex == 0)
      aDigits.Insert(0,1);
    else
      aDigits[aIndex-1]++;
  }

  Update(aDigits, --aIndex);
}