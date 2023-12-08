using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AoC2023
{
  internal class Day8 : ISolver
  {
    class Node
    {
      public string Key { get; set; }
      public Node Left { get; set; }
      public Node Right { get; set; }

      public override string ToString() => $"{Key} - {Left.Key},{Right.Key}";
    }

    private Dictionary<string, Node> ReadInput(string[] lines)
    {
      var rc = new Dictionary<string, Node>();
      for (int i = 2; i < lines.Length; i++)
      {
        var parts = lines[i].Split(' ', '(', ',', ')', '=');
        var key = parts[0];
        var left = parts[4];
        var right = parts[6];
        if (!rc.ContainsKey(key))
          rc.Add(key, new Node() { Key = key });
        if (!rc.ContainsKey(left))
          rc.Add(left, new Node() { Key = left });
        if (!rc.ContainsKey(right))
          rc.Add(right, new Node() { Key = right });
        rc[key].Left = rc[left];
        rc[key].Right = rc[right];
      }
      return rc;
    }

    public void Solve1(string[] lines)
    {
      return;
      var directions = lines[0];

      Dictionary<string, Node> nodes = ReadInput(lines);
      var node = nodes["AAA"];
      var noOfSteps = 0;
      bool found = false;
      while (!found)
      {
        for (int i = 0; i < directions.Length; i++)
        {
          if (directions[i] == 'L')
            node = node.Left;
          else
            node = node.Right;

          if (node.Key == "ZZZ")
          {
            noOfSteps += i + 1;
            found = true;
            break;
          }
        }
        if(!found)
          noOfSteps+=directions.Length;
      }

      Console.WriteLine($"Task1: {noOfSteps}");
    }

    public void Solve2(string[] lines)
    {
      var directions = lines[0];

      Dictionary<string, Node> nodes = ReadInput(lines);
      var ghostStartNodes = nodes.Values.Where(x => x.Key.EndsWith("A")).ToList();
      var ghostNodes = new List<Node>();
      ghostNodes.AddRange(ghostStartNodes); ;
      var noOfSteps = 0;
      bool found = false;
      while (!found)
      {
        for (int i = 0; i < directions.Length; i++)
        {
          var nextGhostNodes = new List<Node>();
          foreach (var ghostNode in ghostNodes)
          {
            var nextNode = directions[i] == 'L' ? ghostNode.Left : ghostNode.Right;
            nextGhostNodes.Add(nextNode);
          }
          ghostNodes = nextGhostNodes;

          if (ghostNodes.All (gn => gn.Key.EndsWith ("Z")))
          {
            noOfSteps += i + 1;
            found = true;
            break;
          }
          else
          {
            //Console.WriteLine($"{string.Join("", ghostNodes.Select(gn => gn.Key[2]))}");
          }
        }
        if (!found)
        {
          noOfSteps += directions.Length;
          Console.WriteLine($"Steps: {noOfSteps}");
        }
      }
      Console.WriteLine($"Task2: {noOfSteps}");
    }
  }
}
