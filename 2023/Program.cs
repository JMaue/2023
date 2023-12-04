﻿using System;
using System.Collections.Generic;
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
      ISolver solver = new Day3();
      var allLines = File.ReadAllLines(@"..\..\..\Input_3.txt");
      solver.Solve1(allLines);  
      solver.Solve2(allLines);
      Console.ReadKey();
    }   
  }
}
