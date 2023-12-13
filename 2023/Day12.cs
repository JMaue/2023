using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AoC2023
{
  internal class Day12 : ISolver
  {
    public int CountBits(int value)
    {
      int cnt = 0;
      while (value > 0)
      {
        if (value % 2 == 1)
        {
          cnt++;
        }
        value >>= 1;
      }
      return cnt;
    }

    public List<int> CountArrangements(string input)
    {
      // .#.###.#.######
      var springs = input.Split('.');
      var arrangements = new List<int>();
      springs.Where(s => !string.IsNullOrEmpty(s)).ToList().ForEach(s => arrangements.Add(s.Length));
      return arrangements;
    }

    public List<int> TestArrangements(string input)
    {
      // .#.###.#.######
      var springs = input.Split('?');
      return CountArrangements(springs[0]);
    }

    public string FindOption(string input, long option)
    {
      var sb = new StringBuilder();
      // ?#?#?#?#?#?#?#?
      for (int i = 0; i < input.Length; i++)
      {
        if (input[i] == '?')
        {
          sb.Append((option & 1) == 1 ? '#' : '.');
          option >>= 1;
        }
        else
        {
          sb.Append(input[i]);
        }
      }
      return sb.ToString();
    }

    public void Solve1(string[] allLines)
    {
      var totalSum = 0;
      foreach (var line in allLines)
      {
        var parts = line.Split(' ');
        var sizes = parts[1].Split(',').Select(p => int.Parse(p)).ToList();
        var sum = Solve1(parts[0], sizes);
        Console.WriteLine($" | {sum}");
        totalSum += sum;
      }
      Console.WriteLine($"Day 12.2: {totalSum}");
    }

    private int Solve1(string springInput, List<int> sizes)
    {
      var currentSize = 0;
      var allSizes = new List<int>();
      var lastDotIdx = 0;

      for (int idx = 0; idx < springInput.Length; idx++)
      {
        if (springInput[idx] == '#')
        {
          currentSize++;
        }
        else if (springInput[idx] == '.')
        {
          if (currentSize > 0)
          {
            allSizes.Add(currentSize);
            var jmx = sizes.Take(allSizes.Count).ToList();
            if (!jmx.SequenceEqual(allSizes))
            {
              return 0;
            }
            currentSize = 0;
          }
          lastDotIdx = idx;
        }
        else if (springInput[idx] == '?')
        {
          var part1 = springInput.Substring(lastDotIdx, idx-lastDotIdx);
          var part2 = springInput.Length > idx + 1 ? springInput.Substring(idx + 1) : string.Empty;

          return Solve1($"{part1}#{part2}", sizes.Skip(allSizes.Count).ToList())
           + Solve1($"{part1}.{part2}", sizes.Skip(allSizes.Count).ToList());
        }
      }
      if (currentSize != 0)
      {
        allSizes.Add(currentSize);
      }
      return allSizes.SequenceEqual(sizes) ? 1 : 0;
    }

    public void Solve1a(string[] allLines)
    {
      var totalSum = 0;
      foreach (var line in allLines)
      {
        int sum = 0;
        // ?#?#?#?#?#?#?#? 1,3,1,6
        var parts = line.Split(' ');
        var sizes = parts[1].Split(',').Select(p => int.Parse(p)).ToList();
        var noOfUnknowns = parts[0].Count(c => c == '?');
        for (int i = 0; i < Math.Pow(2, noOfUnknowns); i++)
        {
          var springs = FindOption(parts[0], i);
          var arrangements = CountArrangements(springs);
          if (arrangements.SequenceEqual(sizes))
          {
            sum++;
          }
        }
        Console.WriteLine($"{parts[0]} {parts[1]} | {sum}");
        totalSum += sum;
      }
      Console.WriteLine($"Day 12.1: {totalSum}");
    }

    public string UnfoldString(string input, char separator)
    {
      return $"{input}{separator}{input}{separator}{input}{separator}{input}{separator}{input}";
    }
    public string UnfoldString2(string input, char separator)
    {
      return $"{input}{separator}{input}";
    }

    public bool IsConflict (string input, List<int> sizes)
    {
      var arrangements = TestArrangements(input);
      if (arrangements.Count > sizes.Count)
      {
        return true;
      }

      for (int i=0; i<arrangements.Count; i++)
      {
        if (arrangements[i] > sizes[i])
        {
          return true;
        }
      }
      return false;
    }

    public int CountSolutions(string input, List<int> sizes)
    {
      if (!input.Contains('?'))
      {
        if (CountArrangements(input).SequenceEqual(sizes))
        {
          return 1;
        }
        else
          return 0;
      }

      var sum = 0;
      var sb = new StringBuilder();
      int pos = 0;
      while (input[pos] != '?')
      {
        sb.Append(input[pos]);
        pos++;
      }
      foreach (char c in new char[]{ '#', '.'})
      {
        // a) replace '?' with both options
        var sb1 = new StringBuilder(sb.ToString());
        sb1.Append(c);
        sb1.Append(input.Substring(pos + 1));
        if (IsConflict(sb1.ToString(), sizes))
          continue;

        sum += CountSolutions(sb1.ToString(), sizes);
      }
      return sum;
    } 

    public void Solve2(string[] allLines)
    {
      long totalSum = 0;
      foreach (var line in allLines)
      {
        Console.Write($"{line} |");
        var parts = line.Split(' ');
        var springInput = UnfoldString(parts[0], '?');
        var sizes = UnfoldString(parts[1], ',').Split(',').Select(p => int.Parse(p)).ToList();
        var sum = Solve1(springInput, sizes);
        Console.WriteLine($" | {sum}");
        totalSum += sum;
      }
      Console.WriteLine($"Day 12.2: {totalSum}");
    }

    private int CalcGCD(int s1, int s2)
    {
      int remainder;
      while (s2 != 0)
      {
        remainder = s1 % s2;
        s1 = s2;
        s2 = remainder;
      }
      return s1;
    }

    public void Solve2c(string[] allLines)
    {
      var totalSum = 0;
      foreach (var line in allLines)
      {
        Console.Write($"{line} |");
        var parts = line.Split(' ');
        // var springInput = parts[0];
        //var sizes = parts[1].Split(',').Select(int.Parse).ToList();
        var springInput = UnfoldString(parts[0], '?');
        var sizes = UnfoldString(parts[1], ',').Split(',').Select(p => int.Parse(p)).ToList();
        var sum = CountSolutions(springInput, sizes);

        Console.WriteLine($" | {sum}");
        totalSum += sum;
      }
      Console.WriteLine($"Day 12.2: {totalSum}");
    }

    public void Solve2b(string[] allLines)
    {
      var totalSum = 0;
      foreach (var line in allLines)
      {
        int sum = 0;
        // ?#?#?#?#?#?#?#? 1,3,1,6
        var parts = line.Split(' ');
        var springInput = UnfoldString(parts[0], '?');
        var sizes = UnfoldString(parts[1], ',').Split(',').Select(p => int.Parse(p)).ToList();
        var noOfUnknowns = springInput.Count(c => c == '?');
        Console.Write($"{springInput} | {noOfUnknowns} |");
        var numberOfSprings = sizes.Sum();
        var missingSprings= numberOfSprings - springInput.Count(c => c == '#');
        var positions = springInput.Count(c => c == '?');
        Console.Write($"({positions}|{missingSprings}) ");
        foreach (var option in GetBinaryOptions(positions, missingSprings))
        {
          var springs = FindOption(springInput, option);
          var arrangements = CountArrangements(springs);
          if (arrangements.SequenceEqual(sizes))
          {
            sum++;
          }
        }
        Console.WriteLine($" | {sum}");
        totalSum += sum;
      }
      Console.WriteLine($"Day 12.2: {totalSum}");
    }

    public IEnumerable<long> GetBinaryOptions(int numberOfSprings, int missingSprings)
    {
      if (missingSprings == 0)
        yield return 0;
      else
        for (int i = missingSprings - 1; i < numberOfSprings; i++)
        {
          long v = (long)Math.Pow(2, i);
          if (CountBits((int)v) == missingSprings)
          {
            yield return (int)v;
          }
          else
          {
            foreach (var x in GetBinaryOptions(i, missingSprings - 1))
            {
              yield return x + v;
            }
          }
        }
    }
  }
}
