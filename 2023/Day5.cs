using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AoC2023
{
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
  }
  internal class Day5 : ISolver
  {
    List<long> seeds = new List<long>();
    Map seed2Soil = new Map();
    Map soil2Fertilizer = new Map();
    Map fertilizer2Water = new Map();
    Map water2Light = new Map();
    Map light2Temperature = new Map();
    Map temperature2Humidity = new Map();
    Map humidity2Location = new Map();
    
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
      seeds = ReadNumbers(allLines[0].Split(':')[1].Trim());
      ReadInput(allLines);
      List<long> locations = SeedToLocations();

      Console.WriteLine($"Task1: {locations.Min()}");
      return;
    }

    private List<long> SeedToLocations()
    {
      List<long> locations = new List<long>();
      foreach (var seed in seeds)
      {
        locations.Add(humidity2Location.Src2Dest(
          temperature2Humidity.Src2Dest(
            light2Temperature.Src2Dest(
              water2Light.Src2Dest(
                fertilizer2Water.Src2Dest(
                  soil2Fertilizer.Src2Dest(
                    seed2Soil.Src2Dest(seed))))))));
      }
      return locations;
    }

    public void Solve2(string[] allLines)
    {
      var seedInput = ReadNumbers(allLines[0].Split(':')[1].Trim());
      seeds = new List<long>();
      for (int idx=0; idx<seedInput.Count/2; idx++)
      {
        var startIdx = seedInput[2*idx];
        for (var idx2 = startIdx; idx2 < startIdx + seedInput[2 * idx + 1]; idx2++)
        {
          seeds.Add(idx2);
        }
      }
      ReadInput(allLines);
      List<long> locations = SeedToLocations();

      Console.WriteLine($"Task2: {locations.Min()}");
      return;
    }
  }
}
