using System;
using System.Collections.Generic;
using System.Linq;

namespace AoC2023
{
  public class Number
  {
    public int Value { get; set; }
    public int Row { get; set; }
    public int StartCol { get; set; }
    public int EndCol { get; set; }

    public override string ToString() => $"{Value}: Row: {Row}, {StartCol}-{EndCol}";
  }

  public class Symbol
  {
    public int Row { get; set; }
    public int Col { get; set; }

    public override string ToString() => $"Row: {Row}, Col: {Col}";

    public bool SymbolTouchesNumber(Number number) => ColumnsOverlap(number) && RowsOverlap(number);
    private bool ColumnsOverlap(Number number) => number.StartCol <= Col && number.EndCol >= Col || number.EndCol == Col-1 || number.StartCol == Col+1;
    private bool RowsOverlap(Number number) => number.Row >= Row-1 && number.Row <= Row+1;
  }

  internal class Day3 : ISolver
  {
    public void Solve1(string[] allLines)
    {
      List<Symbol> symbols = ExtractAllSymbols(allLines, (char c) => c != '.');  
      List<Number> numbers = ExtractAllNumbers(allLines);
      var sum = 0;
      foreach (var symbol in symbols)
      {
        foreach (var number in numbers)
        {
          if (symbol.SymbolTouchesNumber(number))
          {
            sum += number.Value;
          }
        }
      }
      Console.WriteLine($"Task1: {sum}");
      return;
    }

    private List<Number> ExtractAllNumbers(string[] allLines)
    {
      var numbers = new List<Number>();
      for (int row = 0; row<allLines.Length; row++)
      {
        var line = allLines[row];
        numbers.AddRange(ExtractNumbers(row, line));
      }
      return numbers;
    }

    private List<Number> ExtractNumbers(int row, string line)
    {
      List<Number> numbers = new List<Number>();
      var seperators = ExtractSeperators(line);
      var nArray = line.Split(seperators.ToArray());
      int col = 0;
      foreach (var n in nArray)
      {
        if (!string.IsNullOrEmpty(n))
        {
          numbers.Add(new Number { Row = row, StartCol = col, EndCol = col + n.Length - 1, Value = int.Parse(n) });
          col+=n.Length;
        }
        col++;
      }
      return numbers;
    }

    private List<Char> ExtractSeperators(string line)
    {
      var seperators = new List<Char>();
      foreach (var c in line)
      {
        if (!Char.IsNumber(c) && !seperators.Contains(c))
        {
          seperators.Add(c);
        }
      }
      return seperators;
    }

    private List<Symbol> ExtractAllSymbols(string[] allLines, Func<Char, bool> match)
    {
      var symbols = new List<Symbol>();
      for (int row = 0; row < allLines.Length; row++)
      {
        for (int col = 0; col < allLines[row].Length; col++)
        {
          Char c = allLines[row][col];
          if (!Char.IsNumber(c) && match(c))
          {
            symbols.Add(new Symbol { Row = row, Col = col });
          }
        }
      }
      return symbols;
    }

    private List<Symbol> ExtractAllGearSymbols(string[] allLines) 
    {
      return ExtractAllSymbols(allLines, (char c) => c == '*');
    }

    public void Solve2(string[] allLines)
    {
      List<Symbol> symbols = ExtractAllGearSymbols(allLines);
      List<Number> numbers = ExtractAllNumbers(allLines);
      var sum = 0;
      foreach (var symbol in symbols)
      {
        var vals = new List<Number>();
        foreach (var number in numbers)
        {
          if (symbol.SymbolTouchesNumber(number))
          {
            vals.Add(number);
          }
        }
        if (vals.Count == 2)
        {
          sum += vals[0].Value * vals[1].Value;
        }
      }
      Console.WriteLine($"Task1: {sum}");
      return;
    }
  }
}
