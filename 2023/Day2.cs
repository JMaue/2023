using System;
using System.Linq;

namespace AoC2023
{
  internal class Cube
  {
    public const int MaxRed = 12;
    public const int MaxGreen = 13;
    public const int MaxBlue = 14;

    string Color { get; set; }
    int Count { get; set; }
    public Cube(string line)
    {
      ParseProperties(line);
    } 

    private void ParseProperties(string line)
    {
      //  4 green
      string[] props = line.Trim().Split(' ');
      if (props.Length == 2)
      {
        Count = Convert.ToInt32(props[0]);
        Color = props[1];
      }
    }
 
    public bool IsValid()
    {      
      if (Color == "red" && Count > MaxRed)
        return false;
      if (Color == "green" && Count > MaxGreen) 
        return false;
      if (Color == "blue" && Count > MaxBlue)
        return false;
      return true;
     }
  
    public static long GetPower(string set)
    {
      //  17 red, 10 green; 3 blue, 17 red, 7 green; 10 green, 1 blue, 10 red; 7 green, 15 red, 1 blue; 7 green, 8 blue, 16 red; 18 red, 5 green, 3 blue
      var cubes = set.Split(new char[] {';', ','}).Select(c => new Cube(c.Trim()));
      var minGreen = cubes.Where(c=>c.Color=="green").Select(c => c.Count).Max();
      var minRed = cubes.Where(c => c.Color == "red").Select(c => c.Count).Max();
      var minBlue = cubes.Where(c => c.Color == "blue").Select(c => c.Count).Max();
    
      return minGreen*minBlue*minRed;
    }
  }

  internal class Day2 : ISolver
  {
    public void Solve1(string[] allLines)
    {
      long sum = 0;
      foreach (var line in allLines)
      {
        sum += ProcessGame1(line);
      }
        
      Console.WriteLine($"Task1: {sum}");
      return;
    }

    private int ProcessGame1(string line)
    {
      // Game 1: 1 blue; 4 green, 5 blue; 11 red, 3 blue, 11 green; 1 red, 10 green, 4 blue; 17 red, 12 green, 7 blue; 3 blue, 19 green, 15 red
      string id = line.Substring(0, line.IndexOf(':'));
      id = id.Substring(id.IndexOf(' ') + 1);
      string game = line.Substring(line.IndexOf(':') + 1);
      string[] sets = game.Split(';');
      bool allValid = true;
      foreach (var set in sets)
      {
        string[] cubes = set.Split(',');
        allValid &= cubes.Select(cube=>new Cube(cube)).All(cube=>cube.IsValid());
      }
      if (allValid)
        return Convert.ToInt32(id);

      return 0;
    }

    public void Solve2(string[] allLines)
    {
      long sum = 0;
      foreach (var line in allLines)
      {
        sum += ProcessGame2(line);
      }
      Console.WriteLine($"Task2: {sum}");
      return;
    }

    private long ProcessGame2(string line)
    {
      // Game 1: 1 blue; 4 green, 5 blue; 11 red, 3 blue, 11 green; 1 red, 10 green, 4 blue; 17 red, 12 green, 7 blue; 3 blue, 19 green, 15 red
      string game = line.Substring(line.IndexOf(':') + 1);
      return Cube.GetPower(game);
    }
  }
}
