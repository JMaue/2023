using System;
using System.Collections.Generic;

namespace AoC2023
{
  public enum TravelDir
  {
    North,
    East,
    South,
    West
  };

  public class Tile
  {
    private char _tile;
    public bool _isEnergized = false;
    private List<TravelDir> currentEncounters = new List<TravelDir>();

    public Tile(char tile)
    {
      _tile = tile;
    }
    public (TravelDir?, TravelDir?) EncounterBeam(TravelDir travelDir)
    {
      if (currentEncounters.Contains(travelDir))
      {
        return (null, null);
      }
      (TravelDir?, TravelDir?) newDir = (null, null);
      currentEncounters.Add(travelDir);
      _isEnergized = true;
      switch (travelDir)
      {
        case TravelDir.North:
          switch (_tile)
          {
            case '/': newDir = (TravelDir.East, null); break;
            case '\\': newDir = (TravelDir.West, null); break;
            case '-': newDir = (TravelDir.East, TravelDir.West); break;
            default: newDir = (TravelDir.North, null); break;
          }
          break;
        case TravelDir.East:
          switch (_tile)
          {
            case '/': newDir = (TravelDir.North, null); break;
            case '\\': newDir = (TravelDir.South, null); break;
            case '|': newDir = (TravelDir.North, TravelDir.South); break;
            default: newDir = (TravelDir.East, null); break;
          }
          break;
        case TravelDir.South:
          switch (_tile)
          {
            case '/': newDir = (TravelDir.West, null); break;
            case '\\': newDir = (TravelDir.East, null); break;
            case '-': newDir = (TravelDir.West, TravelDir.East); break;
            default: newDir = (TravelDir.South, null); break;
          }
          break;
        case TravelDir.West:
          switch (_tile)
          {
            case '/': newDir = (TravelDir.South, null); break;
            case '\\': newDir = (TravelDir.North, null); break;
            case '|': newDir = (TravelDir.South, TravelDir.North); break;
            default: newDir = (TravelDir.West, null)  ; break;  
          }
          break;
      }
      return newDir;
    }
  }

  class Beam
  {
    public int X;
    public int Y;
    public TravelDir TravelDir;
    public Beam(int x, int y, TravelDir travelDir)
    {
      X = x;
      Y = y;
      TravelDir = travelDir;
    }
  }

  internal class Day16 : ISolver
  {
    public Tile[,] InitTiles(string[] allLines)
    {
      Tile[,] tiles = new Tile[allLines[0].Length, allLines.Length];
      for (int r = 0; r < allLines.Length; r++)
      {
        for (int c = 0; c < allLines[0].Length; c++)
        {
          tiles[c, r] = new Tile(allLines[r][c]);
        }
      }
      return tiles;
    }

    public int Travel(Beam startBeam, string[] allLines)
    {
      Tile[,] tiles = InitTiles(allLines);

      var beams = new List<Beam> { startBeam };
      int beamsProcessed = 0;
      while (beamsProcessed < beams.Count)
      {
        var beam = beams[beamsProcessed];
        (TravelDir?, TravelDir?) travelDir = (beam.TravelDir, null);
        int x = beam.X;
        int y = beam.Y;

        while (travelDir.Item1.HasValue)
        {
          if (x < 0 || x >= allLines[0].Length || y < 0 || y >= allLines.Length)
          {
            break;
          }
          travelDir = tiles[x, y].EncounterBeam(travelDir.Item1.Value);
          if (travelDir.Item2.HasValue)
          {
            var (x2, y2) = ChangeDirection(x, y, travelDir.Item2.Value);
            beams.Add(new Beam(x2, y2, travelDir.Item2.Value));
          }
          if (travelDir.Item1.HasValue)
          {
            (x, y) = ChangeDirection(x, y, travelDir.Item1.Value);
          }
        }
        beamsProcessed++;
      }

      return SumEnergizedTiles(tiles);
    }

    public (int, int) ChangeDirection(int x, int y, TravelDir direction) 
    {
      switch (direction)
      {
        case TravelDir.North: y--; break;
        case TravelDir.East: x++; break;
        case TravelDir.South: y++; break;
        case TravelDir.West: x--; break;
      }
      return (x, y);
    }

    private int SumEnergizedTiles(Tile[,] tiles)
    {
      var sum = 0;
      for (int r = 0; r < tiles.GetLength(0); r++)
      {
        for (int c = 0; c < tiles.GetLength(1); c++)
        {
          if (tiles[c, r]._isEnergized)
          {
            sum++;
          }
        }
      }
      return sum;
    }

    public void Solve1(string[] allLines)
    {
      var sum = Travel(new Beam(0, 0, TravelDir.East), allLines);
      Console.WriteLine($"Day16.1: {sum}");
    }

    public void Solve2(string[] allLines)
    {
      var maxSum = 0;
      foreach (var sd in Enum.GetValues(typeof(TravelDir)))
      {
        TravelDir startDir = (TravelDir)sd;
        if (startDir == TravelDir.North || startDir == TravelDir.South)
        {
          for (int cx = 0; cx < allLines[0].Length; cx++)
          {
            var yStart = startDir == TravelDir.North ? allLines.Length - 1 : 0;
            var sum = Travel(new Beam(cx, yStart, startDir), allLines);
            maxSum = Math.Max(maxSum, sum);
          }
        }
        else if (startDir == TravelDir.East || startDir == TravelDir.West)
        {
          for (int cy = 0; cy < allLines.Length; cy++)
          {
            var xStart = startDir == TravelDir.West ? allLines[0].Length - 1 : 0;
            var sum = Travel(new Beam(cy, 0, startDir), allLines);
            maxSum = Math.Max(maxSum, sum);
          }
        }
      }

      Console.WriteLine($"Day16.2: {maxSum}");
    }
  }
}
