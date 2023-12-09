using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  internal class Day9 : ISolver
  {
    private List<List<int>> CalcDerivatives(string line)
    {
      var allDerivatives = new List<List<int>> { line.Split(' ').Select(p => int.Parse(p)).ToList() };
      var values = allDerivatives[0];
      while (!values.All(v => v == 0))
      {
        values = CalcDeltas(values);
        allDerivatives.Add(values);
      }
      return allDerivatives;
    }

    private List<int> CalcDeltas(List<int> values)
    {
      var rc = new List<int>();
      for (int i = 0; i < values.Count - 1; i++)
      {
        rc.Add(values[i + 1] - values[i]);
      }
      return rc;
    }

    public void Solve1(string[] allLines)
    {
      var sum = 0;
      foreach (var line in allLines)
      {
        var allDerivatives = CalcDerivatives(line);

        var delta = 0;
        for (int i = allDerivatives.Count-2; i>=0; i--)
        {
          delta = allDerivatives[i].Last()+delta;
        }
        sum += delta;
      } 
      Console.WriteLine(sum);
    }

    public void Solve2(string[] allLines)
    {
      int sum = 0;
      foreach (var line in allLines)
      {
        var allDerivatives = CalcDerivatives(line);

        var delta = 0;
        for (int i = allDerivatives.Count - 2; i >= 0; i--)
        {
          delta = allDerivatives[i].First() - delta;
        }
        sum += delta;
      }
      Console.WriteLine(sum);
    }
  }
}
