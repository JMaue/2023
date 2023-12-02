using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2023
{
  internal class Day1 : ISolver
  {
    private static byte _zero = Convert.ToByte('0');

    public void Solve1(string[] allLines)
    {
      long sum = 0;
      foreach (var line in allLines)
      {
        var first = GetDigit(line, true);
        var last = GetDigit(line, false);
        sum = sum + 10 * first + last;
      }
      Console.WriteLine($"Task1: {sum}");
    }

    public void Solve2(string[] allLines)
    {
      long sum = 0;
      foreach (var line in allLines)
      {
        var first = GetDigitOrWord(line, true);
        var last = GetDigitOrWord(line, false);
        sum = sum + 10 * first + last;
      }
      Console.WriteLine($"Task2: {sum}");
    }

    public static int GetDigit(string line, bool first)
    {
      var indList = GetIndexOfDigit(line, first);
      indList.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
      if (first)
        return indList[0].Item2;

      return indList[indList.Count - 1].Item2;
    }

    public static int GetDigitOrWord(string line, bool first)
    {
      var indList = GetIndexOfDigit(line, first);
      indList.AddRange(GetIndexOfSpelledNumber(line, first));
      indList.Sort((t1, t2) => t1.Item1.CompareTo(t2.Item1));
      if (first)
        return indList[0].Item2;

      return indList[indList.Count - 1].Item2;
    }

    public static List<Tuple<int, int>> GetIndexOfDigit(string line, bool first)
    {
      var indList = new List<Tuple<int, int>>();
      for (int i = 1; i <= 9; i++)
      {
        var c = Convert.ToChar(_zero + i);
        var idx = first ? line.IndexOf(c)
                        : line.LastIndexOf(c);
        if (idx >= 0)
          indList.Add(new Tuple<int, int>(idx, i));
      }
      return indList;
    }

    public static List<Tuple<int, int>> GetIndexOfSpelledNumber(string line, bool first)
    {
      var spelledNumbers = new List<string> { "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };
      var indList = new List<Tuple<int, int>>();
      for (int i = 1; i <= 9; i++)
      {
        var idx = first ? line.IndexOf(spelledNumbers[i - 1])
                        : line.LastIndexOf(spelledNumbers[i - 1]);
        if (idx >= 0)
          indList.Add(new Tuple<int, int>(idx, i));
      }
      return indList;
    }

  }
}
