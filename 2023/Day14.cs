using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2023
{
  internal class Day14 : ISolver
  {
    public void Solve1(string[] allLines)
    {
      var totalSum = 0;
      for (int c = 0; c < allLines[0].Length; c++)
      {
        var sum = 0;
        int weight = allLines.Length;
        for (int i = 0; i < allLines.Length; i++)
        {
          if (allLines[i][c] == 'O')
          {
            sum += weight;
            weight--;
          }
          else if (allLines[i][c] == '#')
          {
            weight = allLines.Length - i - 1; ;
          }
        }
        totalSum += sum;
      }
      Console.WriteLine($"Day14.2: {totalSum}");
    }

    public void Solve2(string[] allLines)
    {
      Console.WriteLine("Day14.2: ");
    }
  }
}
