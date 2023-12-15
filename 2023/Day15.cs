using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AoC2023
{
  class Lense
  {
    public string Label { get; set; }
    public int Focus { get; set; }
  }

  class Box
  {
    List<Lense> lenses = new List<Lense>();
    public void AddLense (string label, int focusLength)
    {
      var lense = lenses.FirstOrDefault(l => l.Label == label);
      if (lense == null)
      {
        lenses.Add(new Lense() { Label = label, Focus = focusLength });
      }
      else
      {
        lense.Focus = focusLength;
      }
    }

    public void RemoveLense(string label)
    {
      var lense = lenses.FirstOrDefault(l => l.Label == label);
      if (lense != null)
      {
        lenses.Remove(lense);
      }
    }

    internal int CalcFocalPower()
    {
      var sum = 0;
      for (int i = 0; i< lenses.Count; i++)
      { 
        sum += (i + 1) * lenses[i].Focus;
      }
      return sum;
    }
  }

  internal class Day15 : ISolver
  {
    ASCIIEncoding ascii = new ASCIIEncoding();
    private Box[] boxes = new Box[256];

    public int CalcMyHash(string part)
    {
      int s = 0;
      foreach (var bytes in ascii.GetBytes(part))
      {
        s = (s + bytes) * 17 % 256;
      }
      return s;
    }

    public void Solve1(string[] allLines)
    {
      var totalSum = 0;
      foreach (var line in allLines)
      {
        var parts = line.Split(',');
        foreach (var part in parts)
        {
          totalSum += CalcMyHash(part);
        }
      }
      Console.WriteLine($"Day15.1: {totalSum}");
    }

    public void Solve2(string[] allLines)
    {
      var totalSum = 0;
      for (int i = 0; i < boxes.Length; i++)
      {
        boxes[i] = new Box(); ;
      }
      foreach (var line in allLines)
      {
        var parts = line.Split(',');
        foreach (var part in parts)
        {
          var items = part.Split('=', '-');
          var label = items[0];
          var box = CalcMyHash(label);

          if (part.Contains("="))
          {
            var focusLength = int.Parse(items[1]);
            boxes[box].AddLense(label, focusLength);
          }
          else if (part.Contains("-"))
          {
            boxes[box].RemoveLense(label);
          }
        }

        for (int i = 0; i < boxes.Length; i++)
        {
          totalSum += (i+1) * boxes[i].CalcFocalPower();
        }
      }
      Console.WriteLine($"Day15.2: {totalSum}");
    }
  }
}
