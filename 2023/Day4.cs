using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  internal class Day4 : ISolver
  {
    public void Solve1(string[] allLines)
    {
      long sum = 0;
      foreach (var line in allLines)
      {
        var numberSets = line.Split(new char[] {':', '|' });
        var winningNumbers = numberSets[1].Replace("  ", " 0").Trim().Split(' ').ToList();
        var myNumbers = numberSets[2].Replace("  ", " 0").Trim().Split(' ');
        var winningNumbersSum = CalculateWinningNumbers(line);
        if (winningNumbersSum > 0)
          sum += (long)Math.Pow(2, winningNumbersSum-1);
      }
      Console.WriteLine($"Task1: {sum}");
      return;
    }

    private int CalculateWinningNumbers(string line)
    {
      var numberSets = line.Split(new char[] { ':', '|' });
      var winningNumbers = numberSets[1].Replace("  ", " 0").Trim().Split(' ').ToList();
      var myNumbers = numberSets[2].Replace("  ", " 0").Trim().Split(' ');
      return winningNumbers.FindAll(wn => myNumbers.Contains(wn)).Count();
    }

    public void Solve2(string[] allLines)
    {
      var cardsDict = new Dictionary<int, int>();
      for (int i = 0; i < allLines.Length; i++)
      {
        cardsDict.Add(i, 1);
      }
      for (int i = 0; i < allLines.Length; i++)
      {
        var line = allLines[i];
        var winningNumbersSum = CalculateWinningNumbers(line);
        for (var j = 1; j <= winningNumbersSum; j++)
        {
          cardsDict[i + j] += cardsDict[i];
        }
      }
      Console.WriteLine($"Task2: {cardsDict.Values.Sum()}");
      return;
    }
  }
}
