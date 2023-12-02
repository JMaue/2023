using System;
using System.Collections.Generic;
using System.IO;

namespace AoC2023
{
  interface ISolver
  {
    void Solve1();
    void Solve2();
  }

  internal class Program
  {
    static void Main(string[] args)
    {
      ISolver solver = new Day2();
      solver.Solve1();  
      solver.Solve2();
      Console.ReadKey();
    }   
  }
}
