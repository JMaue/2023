using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  class Race
  {
    public Race(long time, long recordToBeat)
    {
      Time = time;
      RecordToBeat = recordToBeat;
    }

    public long Time { get; set; }
    public long RecordToBeat { get; set; }
    public long GetNumberOfWins() => Enumerable.Range(1, (int)Time).Count (t => t * (Time - t) > RecordToBeat);
  }
 
  internal class Day6 : ISolver
  {
    List<long> _times = new List<long>();
    List<long> _distances = new List<long>();
    
    public void ReadInput(string[] allLines)
    {
      _times = ReadNumbers(allLines[0].Split(':')[1].Trim());
      _distances = ReadNumbers(allLines[1].Split(':')[1].Trim());
      return;
    }


    private List<long> ReadNumbers(string line)
    {
      return line.Split(' ').Where(x=>!string.IsNullOrEmpty(x)).Select(x => long.Parse(x)).ToList();
    }

    public string RemoveAllSpaces(string line)
    {
      return line.Replace(" ", "");
    }

    public void Solve1(string[] allLines)
    {
      ReadInput(allLines);
      long res = 1;
      Enumerable.Range(0, _times.Count)
        .Select(t=>new Race(_times[t], _distances[t]))
        .ToList()
        .ForEach(race => res *= race.GetNumberOfWins());

      Console.WriteLine($"Task1: {res}");
    }

    public void Solve2(string[] allLines)
    {
      _times = ReadNumbers(RemoveAllSpaces (allLines[0].Split(':')[1]));
      _distances = ReadNumbers(RemoveAllSpaces(allLines[1].Split(':')[1]));
      long res = new Race(_times[0], _distances[0]).GetNumberOfWins();
      Console.WriteLine($"Task2: {res}");
    }
  }
}
