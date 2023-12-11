using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  internal class Day11 : ISolver
  {
    List<int> rowsToInsert;
    List<int> colsToInsert;
    List<Coord> galaxies;

    public class Coord
    {
      public long X { get; set; }
      public long Y { get; set; }

      public long DistanceFrom(Coord other) => Math.Abs(X - other.X) + Math.Abs(Y - other.Y);
    
      public override string ToString() => $"({X},{Y})";
    }

    private void PrepareLists(string[] allLines)
    {
      galaxies = new List<Coord>();
      rowsToInsert = new List<int>();
      colsToInsert = new List<int> (Enumerable.Range(0, allLines[0].Length) );
      var colsToRemove = new List<int>();
      for (int i = 0; i < allLines.Length; i++)
      {
        var line = allLines[i];
        if (line.All(c => c == '.'))
          rowsToInsert.Add(i);

        for (int j = 0; j < line.Length; j++)
        {
          if (line[j] == '#')
          {
            colsToRemove.Add(j);
            galaxies.Add(new Coord() { X = j, Y = i });
          }
        }
      }
      colsToInsert.RemoveAll(c => colsToRemove.Contains(c));
    }

    private void StretchGalaxies(int stretchFactor)
    {
      var toAdd = stretchFactor - 1;
      for (int c = colsToInsert.Count - 1; c >= 0; c--)
      {
        galaxies.Where(g => g.X >= colsToInsert[c]).ToList().ForEach(g => g.X += toAdd);
      }
      for (int r = rowsToInsert.Count - 1; r >= 0; r--)
      {
        galaxies.Where(g => g.Y >= rowsToInsert[r]).ToList().ForEach(g => g.Y += toAdd);
      }
    }

    private long CalculateSumDist()
    {
      long sumDist = 0;
      for (int g1 = 0; g1 < galaxies.Count; g1++)
      {
        var galaxy1 = galaxies[g1];
        for (int g2 = g1 + 1; g2 < galaxies.Count; g2++)
        {
          sumDist += galaxies[g2].DistanceFrom(galaxy1);
        }
      }
      return sumDist;
    }

    public void Solve1(string[] allLines)
    {
      PrepareLists(allLines);
      StretchGalaxies(2);
      Console.WriteLine($"Task1: {CalculateSumDist()}");
    }

    public void Solve2(string[] allLines)
    {
      int stretchFactor = 1000000;
      PrepareLists(allLines);
      StretchGalaxies(stretchFactor);
      Console.WriteLine($"Task2: {CalculateSumDist()}");
    }
  }
}
