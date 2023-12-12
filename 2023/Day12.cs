using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
  internal class Day12 : ISolver
  {
    public List<int> CountArrangements(string input)
    {
      // .#.###.#.######
      var springs = input.Split('.');
      var arrangements = new List<int>();
      springs.Where(s=>!string.IsNullOrEmpty(s)).ToList().ForEach(s=>arrangements.Add(s.Length));
      return arrangements;
    }

    public string FindOption(string input, int option)
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

    public void Solve2(string[] allLines)
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
        for (int i = 0; i < Math.Pow(2, noOfUnknowns); i++)
        {
          var springs = FindOption(springInput, i);
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
  }
}
