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
      ISolver solver = new Day13();

      //var width = 6;
      //var d12 = (Day12)solver;
      //var x1 = d12.GetBinaryOptions(width, 3);
      //foreach (var x in x1)
      //{
      //  Console.WriteLine(Convert.ToString(x, 2).PadLeft(width, '0'));
      //}
      //Console.ReadKey();

      var allLines = File.ReadAllLines(@"..\..\..\Input_13.txt");
      solver.Solve1(allLines);
      Console.WriteLine("");
      Console.WriteLine("");
      solver.Solve2(allLines);
      Console.ReadKey();
    }   
  }
}
