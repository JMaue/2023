using System;
using System.IO;
using System.Linq;
using System.Text;

namespace AoC2023
{
  public enum Direction
  {
    Up,
    Down,
    Left,
    Right
  }

  public class Coord
  {
    public int X { get; set; }
    public int Y { get; set; }
  }

  public class PipeMap
  {
    public PipeMap(string[] allLines)
    {
      AllLines = allLines;
      FindStart();
    }

    private String[] AllLines;
    private Char[,] _map;
    private Coord _coord;
    private int _countLeft;
    private int _countRight;
    private int _maxX;
    private int _maxY;

    private const Char Start = 'S'; 
    public int CountLeft => _countLeft;
    public int CountRight => _countRight;

    public void Erase()
    {
      _countLeft = 0;
      _countRight = 0;

      _map = new Char[AllLines.Length, AllLines[0].Length];
      _maxX = AllLines[0].Length;
      _maxY = AllLines.Length;
      for (int i = 0; i < AllLines.Length; i++)
      {
        var line = AllLines[i];
        for (int c = 0; c < line.Length; c++)
        {
          var ch = line[c];
          _map[i, c] = ch == Start ? ch : ' ';
        }
      }
    }

    public void MarkPosition()
    {
      MarkPosition(_coord.X, _coord.Y, '.');
    }

    public void MarkPosition(int x, int y, char mark)
    {
      _map[y, x] = mark;
    }

    public void FindStart()
    {
      for (int i = 0; i < AllLines.Length; i++)
      {
        if (!AllLines[i].Contains(Start)) 
          continue;

        _coord = new Coord { X = AllLines[i].IndexOf(Start), Y = i};
      }
    }

    public void Write(string fileName)
    {
      string[] lines = new string[_map.GetLength(0)];
      for (int i = 0; i < _map.GetLength(0); i++)
      {
        var sb = new StringBuilder();
        for (int j = 0; j < _map.GetLength(1); j++)
        {
          sb.Append(_map[i, j]);
        }
        lines[i] = sb.ToString() ;
      }
      File.WriteAllLines(fileName, lines);
    }

    public char GetNextChar(Direction dir) => dir switch
    {
      Direction.Up => _coord.Y == 0 ? ' ' : AllLines[_coord.Y - 1][_coord.X],
      Direction.Down => AllLines[_coord.Y + 1][_coord.X],
      Direction.Left => AllLines[_coord.Y][_coord.X - 1],
      Direction.Right => AllLines[_coord.Y][_coord.X + 1],
      _ => throw new ArgumentOutOfRangeException(nameof(dir), dir, null)
    };

    public Direction? CanMove(Direction? dir, Char nextChar)
    {
      switch (dir)
      {
        case Direction.Up:
          if (nextChar == '|' || nextChar == '7' || nextChar == 'F')
          {
            _coord.Y--;
            return nextChar == '|' ? Direction.Up : nextChar == '7' ? Direction.Left : Direction.Right;
          }
          return null;
        case Direction.Down:
          if (nextChar == '|' || nextChar == 'L' || nextChar == 'J')
          {
            _coord.Y++;
            return nextChar == '|' ? Direction.Down : nextChar == 'J' ? Direction.Left : Direction.Right;
          }
          return null;
        case Direction.Left:
          if (nextChar == '-' || nextChar == 'L' || nextChar == 'F')
          {
            _coord.X--;
            return nextChar == '-' ? Direction.Left : nextChar == 'L' ? Direction.Up : Direction.Down;
          }
          return null;
        case Direction.Right:
          if (nextChar == '-' || nextChar == '7' || nextChar == 'J')
          {
            _coord.X++;
            return nextChar == '-' ? Direction.Right : nextChar == 'J' ? Direction.Up : Direction.Down;
          }
          return null;
        default:
          return null;
      }
    }

    public Direction? CountTiles(Direction? dir, Char nextChar)
    {
      Direction? nextDir = null;
      switch (dir)
      {
        case Direction.Up:
          if (nextChar == '|' || nextChar == '7' || nextChar == 'F')
          {
           _coord.Y--;
           nextDir = nextChar == '|' ? Direction.Up : nextChar == '7' ? Direction.Left : Direction.Right;
          }
          break;
        case Direction.Down:
          if (nextChar == '|' || nextChar == 'L' || nextChar == 'J')
          {
            _coord.Y++;
            nextDir =  nextChar == '|' ? Direction.Down : nextChar == 'J' ? Direction.Left : Direction.Right;
          }
          break;
        case Direction.Left:
          if (nextChar == '-' || nextChar == 'L' || nextChar == 'F')
          {
            _coord.X--;
            nextDir = nextChar == '-' ? Direction.Left : nextChar == 'L' ? Direction.Up : Direction.Down;
          }
          break;
        case Direction.Right:
          if (nextChar == '-' || nextChar == '7' || nextChar == 'J')
          { 
            nextDir = nextChar == '-' ? Direction.Right : nextChar == 'J' ? Direction.Up : Direction.Down;
            _coord.X++;
          }
          break;
        default:
          break;
      }
      Calc(nextDir.Value); 
      return nextDir;
    }

    public void Count2Right()
    {
      // count all tiles to the right
      var x = _coord.X + 1;
      while (x >= 0 && x < _maxX && _map[_coord.Y, x] != '.')
      {
        if (_map[_coord.Y, x] == ' ')
          _countRight++;
        MarkPosition(x, _coord.Y, 'I');
        x++;
      }
    }

    public void Count2Left()
    {
      // count all tiles to the left
      var x = _coord.X - 1;
      while (x >= 0 && x < _maxX && _map[_coord.Y, x] != '.')
      {
        if (_map[_coord.Y, x] == ' ')
          _countRight++;
        MarkPosition(x, _coord.Y, 'I');
        x--;
      }
    }

    public void CountUp()
    {
      // count all tiles upwards
      var y = _coord.Y - 1;
      while (y >= 0 && y < _maxY && _map[y, _coord.X] != '.')
      {
        if (_map[y, _coord.X] == ' ')
          _countRight++;
        MarkPosition(_coord.X, y, 'I');
        y++;
      }
    }

    public void CountDwn()
    {
      // count all tiles downwards
      var y = _coord.Y + 1;
      while (y >= 0 && y < _maxY && _map[y, _coord.X] != '.')
      {
        if (_map[y, _coord.X] == ' ')
          _countRight++;
        MarkPosition(_coord.X, y, 'I');
        y--;
      }
    }

    public void Calc (Direction nextDir)
    {
      if (nextDir == Direction.Up)
      {
        CountDwn();
        Count2Right();
      }
      if (nextDir == Direction.Down)
      {
        CountUp();
        Count2Left();
      }
      if (nextDir == Direction.Left)
      {
        Count2Right();
        CountUp();
      }
      if (nextDir == Direction.Right)
      {
        Count2Left();
        CountDwn();
      }
    }
  }

  internal class Day10 : ISolver
  {
    Direction startDir;

    public void Solve1(string[] allLines)
    {
      var map = new PipeMap(allLines);
      bool loopComplete = false;
      var count = 0;
      foreach (var dir in Enum.GetValues(typeof(Direction)))
      {
        if (loopComplete)
          break;

        count = 0;
        Direction? nextDir = (Direction) dir;
        startDir = (Direction)dir;
        while (nextDir != null)
        {
          count++;
          var nextChar = map.GetNextChar(nextDir.Value);
          if (nextChar == 'S')
          {
            loopComplete = true;
            break;
          }
          nextDir = map.CanMove(nextDir, nextChar);
        }
      }

      Console.WriteLine($"Task1 {count/2}");
    }

    public void Solve2(string[] allLines)
    {
      //var map = new PipeMap(allLines);
      var newMap = new PipeMap(allLines);
      newMap.Erase();

      startDir = Direction.Right;
      Direction? nextDir = startDir;
      while (nextDir != null)
      {
        var nextChar = newMap.GetNextChar(nextDir.Value);
        if (nextChar == 'S')
        {
          break;
        }
        nextDir = newMap.CanMove(nextDir, nextChar);
        newMap.MarkPosition();
      }
      newMap.Write("Task2a.txt");
      nextDir = startDir;
      newMap.FindStart();
      while (nextDir != null)
      {
        var nextChar = newMap.GetNextChar(nextDir.Value);
        if (nextChar == 'S')
        {
          break;
        }
        nextDir = newMap.CountTiles(nextDir, nextChar);
      }

      newMap.Write("Task2b.txt");

      Console.WriteLine($"Task1 {newMap.CountRight}");
    }
  }
}
