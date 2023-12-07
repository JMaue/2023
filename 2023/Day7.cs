using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AoC2023
{
  internal class Day7 : ISolver
  {
    public enum Rank
    {
      HighCard = 1, OnePair, TwoPairs, ThreeOfAKind, FullHouse, FourOfAKind, FiveOfAKind
    }

    internal static class Strength
    {
      private static Dictionary<char, int> _strength1 = new Dictionary<char, int>()
      {
          { 'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'J', 11 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, { '7', 7 }, { '6', 6 }, { '5', 5 },
          { '4', 4 }, { '3', 3 }, { '2', 2 }
      };
      private static Dictionary<char, int> _strength2 = new Dictionary<char, int>()
      {
          { 'A', 14 }, { 'K', 13 }, { 'Q', 12 }, { 'T', 10 }, { '9', 9 }, { '8', 8 }, { '7', 7 }, { '6', 6 }, { '5', 5 },
          { '4', 4 }, { '3', 3 }, { '2', 2 }, { 'J', 1 }
      };

      internal static int CompareStrength1(char v1, char v2) => _strength1[v1].CompareTo(_strength1[v2]);
      internal static int CompareStrength2(char v1, char v2) => _strength2[v1].CompareTo(_strength2[v2]);
    }

    internal class Hand
    {
      private String Cards { get; set; }
      private Dictionary<char, int> Occurances { get; set; }
      public Hand(string cards, int ruleset)
      {
        Cards = cards;
        if (ruleset == 1)
          ReadOccurrances1();
        else
          ReadOccurrances2();
      }

      private void ReadOccurrances1()
      {
        Occurances = new Dictionary<char, int>();
        foreach (var c in Cards)
        {
          if (Occurances.ContainsKey(c))
            Occurances[c]++;
          else
            Occurances.Add(c, 1);
        }
      }

      private void ReadOccurrances2()
      {
        Occurances = new Dictionary<char, int>();
        int noOfJokers = Cards.Count(c => c == 'J');
        if (noOfJokers == 5)
        {
          Occurances.Add('J', noOfJokers);
        }
        else
        {
          foreach (var c in Cards.Where(c => c != 'J'))
          {
            if (Occurances.ContainsKey(c))
              Occurances[c]++;
            else
              Occurances.Add(c, 1);
          }

          if (noOfJokers > 0)
          {
            var max = Occurances.Values.Max();
            var key = Occurances.FirstOrDefault(x => x.Value == max).Key;
            Occurances[key] += noOfJokers;
          }
        }
      }

      public override string ToString() => Cards;
      public bool IsFiveOfAKind() => Occurances.Where(o => o.Value == 5).Any();
           
      public bool IsFourOfAKind() => Occurances.Where(o => o.Value == 4).Any();

      public bool IsFullHouse() => Occurances.Where(o => o.Value == 3).Count() == 1 && Occurances.Where(o => o.Value == 2).Count() == 1;

      public bool IsThreeOfAKind() => Occurances.Where(o => o.Value == 3).Any() && Occurances.Where(o => o.Value == 1).Count() == 2;

      public bool IsTwoPairs() => Occurances.Where(o => o.Value == 2).Count() == 2;
      public bool IsOnePair() => Occurances.Where(o => o.Value == 2).Count() == 1 && Occurances.Where(o => o.Value == 1).Count() == 3;
      public bool IsHighCard() => Occurances.Where(o => o.Value == 1).Count() == 5;

      public Rank Rank => IsFiveOfAKind() ? Rank.FiveOfAKind :
        IsFourOfAKind() ? Rank.FourOfAKind : 
        IsFullHouse() ? Rank.FullHouse :
        IsThreeOfAKind() ? Rank.ThreeOfAKind :
        IsTwoPairs() ? Rank.TwoPairs : 
        IsOnePair() ? Rank.OnePair : Rank.HighCard;

      internal static int CompareHands1(Tuple<Hand, int> x, Tuple<Hand, int> y)
      {
        if (x.Item1.Rank != y.Item1.Rank)
          return x.Item1.Rank.CompareTo(y.Item1.Rank);

        for (int i = 0; i < x.Item1.Cards.Length; i++)
        {
          if (x.Item1.Cards[i] != y.Item1.Cards[i])
            return Strength.CompareStrength1 (x.Item1.Cards[i], y.Item1.Cards[i]);
        }
        return 0;
      }

      internal static int CompareHands2(Tuple<Hand, int> x, Tuple<Hand, int> y)
      {
        if (x.Item1.Rank != y.Item1.Rank)
          return x.Item1.Rank.CompareTo(y.Item1.Rank);

        for (int i = 0; i < x.Item1.Cards.Length; i++)
        {
          if (x.Item1.Cards[i] != y.Item1.Cards[i])
            return Strength.CompareStrength2(x.Item1.Cards[i], y.Item1.Cards[i]);
        }
        return 0;
      }
    }

    public List<Tuple<Hand, int>> ReadInput(string[] allLines, int ruleset)
    {
      var rc = new List<Tuple<Hand, int>>();
      foreach (var line in allLines)
      {
        var row = line.Split(' ');
        rc.Add(new Tuple<Hand, int> (new Hand(row[0], ruleset), int.Parse(row[1])));
      }
      return rc;
    }

    public long Solve(string[] allLines, int ruleset)
    {
      var allHands = ReadInput(allLines, ruleset);
      if (ruleset == 1)
        allHands.Sort(Hand.CompareHands1);
      else
        allHands.Sort(Hand.CompareHands2);

      var sum = 0;
      for (int i = 0; i < allHands.Count; i++)
      {
        sum += (i + 1) * allHands[i].Item2;
      }

      return sum;
    }

    public void Solve1(string[] allLines)
    {
      var sum = Solve(allLines, 1);
      Console.WriteLine($"Task1: {sum}");
    }

    public void Solve2(string[] allLines)
    {
      var sum = Solve(allLines, 2);
      Console.WriteLine($"Task2: {sum}");
    }
  }
}
