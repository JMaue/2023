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
    private int FindHorizontalSymmetry(List<string> group, Func<string, string, bool> areIdentical)
    {
      for (int i = 0; i < group.Count; i++)
      {
        var line = group[i];
        if (i < group.Count - 1)
        {
          if (areIdentical(line, group[i + 1]))
          {
            // walk upwards to see if all match
            var distance = Math.Min(i, group.Count - i - 2);
            bool isSymmetric = true;
            while (isSymmetric && distance >= 0)
            {
              isSymmetric = areIdentical(group[i - distance], group[i + distance + 1]);
              distance--;
            }
            if (isSymmetric)
              return i+1;
          }
        }
      }
      return 0;
    }

    public int ProcessGroup1 (List<string> group)
    {
      bool areIdentical(string s1, string s2) => s1 == s2;

      var rc = 100 * FindHorizontalSymmetry(group, areIdentical);
      rc += FindVerticalSymmetry(group, areIdentical);
      Console.WriteLine(rc);
      return rc;
    }

    public int ProcessGroup2(List<string> group)
    {
      bool areIdentical(string s1, string s2)
      {
        if (s1 == s2)
          return true;

        int cnt = 0;
        for (int i = 0; i < s1.Length; i++)
        {
          if (s1[i] != s2[i])
            cnt++;
        }

        return cnt == 1;
      }

      var rc = 100 * FindHorizontalSymmetry(group, areIdentical);
      rc += FindVerticalSymmetry(group, areIdentical);
      Console.WriteLine(rc);
      return rc;
    }

    private int FindVerticalSymmetry(List<string> group, Func<string, string, bool> areIdentical)
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
      return FindHorizontalSymmetry(rotatedGroup, areIdentical);
    }

    public void Solve1(string[] allLines)
    {
      var group = new List<string>();
      var sum = 0;
      foreach (var line in allLines)
      {
        if (line.Length == 0 || line == "END")
        {
          sum += ProcessGroup1(group);
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
      var group = new List<string>();
      var sum = 0;
      foreach (var line in allLines)
      {
        if (line.Length == 0 || line == "END")
        {
          sum += ProcessGroup2(group);
          group.Clear();
        }
        else
        {
          group.Add(line);
        }
      }
      Console.WriteLine($"Task2: {sum}");
    }
  }
}
