using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  class Seeds
  {
    private List<long> RangeStart { get; set; } = new List<long>();
    private List<long> Length { get; set; } = new List<long>();

    public void AddRanges(List<long> numbers)
    {
      for (int idx = 0; idx < numbers.Count / 2; idx++)
      {
        RangeStart.Add(numbers[2*idx]);
        Length.Add(numbers[2*idx + 1]);
      }
    }

    public bool ContainsSeed(long seed)
    {
      for (int idx = 0; idx < RangeStart.Count; idx++)
      {
        var startIdx = RangeStart[idx];
        if (startIdx <= seed && seed < startIdx + Length[idx])
          return true;
      }

      return false;
    }
  }

  class Map
  {
    private List<long> DestRangeStart { get; set; } = new List<long>();
    private List<long> SourceRangeStart { get; set; } =  new List<long>();
    private List<long> Length { get; set; } = new List<long>();

    internal void AddRanges(List<long> numbers)
    {
      DestRangeStart.Add (numbers[0]);
      SourceRangeStart.Add (numbers[1]);
      Length.Add (numbers[2]);
    }

    public long Src2Dest(long src)
    {
      for (int idx=0; idx<SourceRangeStart.Count; idx++)
      {
        var source = SourceRangeStart[idx];
        if (src >= source && src < source + Length[idx])
        {
          return src-source+DestRangeStart[idx];
        }
      }
      return src;
    }

    public IEnumerable<long> GetLowestDestinations()
    {
      var destDict = new Dictionary<long, int>();
      for (int idx =0; idx<DestRangeStart.Count(); idx++)
      {
        destDict.Add(DestRangeStart[idx], idx);
      }

      var tmpRange = new List<long>();
      tmpRange.AddRange(DestRangeStart);
      tmpRange.Sort();
      for (int idx = 0; idx < tmpRange.Count; idx++)
      {
        var startIdx = tmpRange[idx];
        for (var idx2 = startIdx; idx2 < startIdx + Length[destDict[startIdx]]; idx2++)
        {
          yield return idx2;
        }
      }
    }

    internal long Dest2Src(long dest)
    {
      for (int idx = 0; idx < DestRangeStart.Count; idx++)
      {
        var destination = DestRangeStart[idx];
        if (dest >= destination && dest < destination + Length[idx])
        {
          return dest - destination + SourceRangeStart[idx];
        }
      }

      return dest;
    }
  }
  
  internal class Day5 : ISolver
  {
    readonly Map seed2Soil = new Map();
    readonly Map soil2Fertilizer = new Map();
    readonly Map fertilizer2Water = new Map();
    readonly Map water2Light = new Map();
    readonly Map light2Temperature = new Map();
    readonly Map temperature2Humidity = new Map();
    readonly Map humidity2Location = new Map();
    
    public void ReadInput(string[] allLines)
    {
      int row = 2;
      while (row < allLines.Length)
      {
        var caption = allLines[row];
        var map = FindMapFromCaption(caption);
        row++;
        while (row < allLines.Length && allLines[row] != "")
        {
          var numbers = ReadNumbers(allLines[row]);
          map.AddRanges(numbers);
          row++;
        }
        row++;
      }
      return;
    }

    private Map FindMapFromCaption(string caption)
    {
      switch (caption)
      {
        case "seed-to-soil map:": return seed2Soil; 
        case "soil-to-fertilizer map:": return soil2Fertilizer; 
        case "fertilizer-to-water map:": return fertilizer2Water;
        case "water-to-light map:": return water2Light;
        case "light-to-temperature map:": return light2Temperature;
        case "temperature-to-humidity map:": return temperature2Humidity;
        default: return humidity2Location; 
      }
    }

    private List<long> ReadNumbers(string line)
    {
      return line.Split(' ').Select(x => long.Parse(x)).ToList();
    }

    public void Solve1(string[] allLines)
    {
      var seeds = ReadNumbers(allLines[0].Split(':')[1].Trim());
      ReadInput(allLines);
      var minLocation = FindMinLocation(seeds);

      Console.WriteLine($"Task1: {minLocation}");
    }

    private long FindMinLocation(IEnumerable<long> seeds)
    {
      long minLocation = long.MaxValue;
      foreach (var seed in seeds)
      {
        var loc = humidity2Location.Src2Dest(
          temperature2Humidity.Src2Dest(
            light2Temperature.Src2Dest(
              water2Light.Src2Dest(
                fertilizer2Water.Src2Dest(
                  soil2Fertilizer.Src2Dest(
                    seed2Soil.Src2Dest(seed)))))));

        minLocation = Math.Min(minLocation, loc);
      }
      return minLocation;
    }

    private long FindMinLocationReverse(Seeds seeds)
    {
      long minLocation = long.MaxValue;
      int noOfTries = 1;
      foreach (var soil in humidity2Location.GetLowestDestinations())
      {
        var seed = seed2Soil.Dest2Src(
          soil2Fertilizer.Dest2Src(
            fertilizer2Water.Dest2Src(
              water2Light.Dest2Src(
                light2Temperature.Dest2Src(
                  temperature2Humidity.Dest2Src(
                    humidity2Location.Dest2Src(soil)))))));
        if (seed > 0 && seeds.ContainsSeed(seed))
        {
          minLocation = Math.Min(minLocation, soil);
          Console.WriteLine($"{seed} - {soil} after {noOfTries} tries.");
          break;
        }

        noOfTries++;
      }

      return minLocation;
    }

    public void Solve2(string[] allLines)
    {
      var seedInput = ReadNumbers(allLines[0].Split(':')[1].Trim());
      var seeds = new Seeds();
      seeds.AddRanges(seedInput);

      long minLocation = FindMinLocationReverse(seeds);

      Console.WriteLine($"Task2: {minLocation}");
    }
  }
}
