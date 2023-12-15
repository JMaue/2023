using System;
using System.IO;

namespace AoC2023
{
  interface ISolver
  {
    void Solve1(string[] allLines);
    void Solve2(string[] allLines);
  }

  internal class Program
  {
    static void Main(string[] args)
    {
      ISolver solver = new Day15();

      var allLines = File.ReadAllLines(@"..\..\..\Input_15.txt");
      solver.Solve1(allLines);
      solver.Solve2(allLines);
      Console.ReadKey();
    }   
  }
}
