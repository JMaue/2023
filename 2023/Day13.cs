using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC2023
{
  internal class Day13 : ISolver
  {
    private int FindHorizontalSymmetry(List<string> group)
    {
      for (int i = 0; i < group.Count; i++)
      {
        var line = group[i];
        if (i < group.Count - 1)
        {
          if (line == group[i + 1])
          {
            // walk upwards to see if all match
            var distance = Math.Min(i, group.Count - i - 2);
            bool isSymmetric = true;
            while (isSymmetric && distance >= 0)
            {
              isSymmetric = group[i - distance] == group[i + distance + 1];
              distance--;
            }
            if (isSymmetric)
              return i+1;
          }
        }
      }
      return 0;
    }

    public int ProcessGroup (List<string> group)
    {
      var rc = 100*FindHorizontalSymmetry(group);
      rc += FindVerticalSymmetry(group);
      return rc;
    }

    private int FindVerticalSymmetry(List<string> group)
    {
      var rotatedGroup = new List<string>();
      bool first = true;
      foreach (var line in group)
      {
        if (first)
        {
          first = false;
          foreach (var c in line)
          {
            rotatedGroup.Add(c.ToString());
          }
        }
        else
        {
          for (int c = 0; c < line.Length; c++)
          {
            rotatedGroup[c] += line[c].ToString();
          }
        }
      }
      return FindHorizontalSymmetry(rotatedGroup);
    }

    public void Solve1(string[] allLines)
    {
      var group = new List<string>();
      var sum = 0;
      foreach (var line in allLines)
      {
        if (line.Length == 0 || line == "END")
        {
          sum += ProcessGroup(group);
          group.Clear();
        }
        else
        {
          group.Add(line);  
        }
      }
      Console.WriteLine($"Task1: {sum}");
    }

    public void Solve2(string[] allLines)
    {
      Console.WriteLine($"Task2: {0+0}");
    }
  }
}
