using BenchmarkDotNet.Attributes;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection.PortableExecutable;

namespace BSCGAOCBM2022;

[MemoryDiagnoser]
public class Day9Benchmarks
{
    private string _inputText = null!;
    private string[] _inputLines = null!;
    private Memory<string> _inputMemory = null!;

    [GlobalSetup]
    public void Setup()
    {
        _inputText = File.ReadAllText(@"Input\9.txt");
        _inputLines = File.ReadAllLines(@"Input\9.txt");
        _inputMemory = new Memory<string>(_inputLines);
    }

    [Benchmark]
    public int Auros_Part1()
    {
        HashSet<(int, int)> visited = new();
        (int, int) currentHeadLocation = (0, 0);
        (int, int) currentTailLocation = (0, 0);

        foreach (var line in _inputLines)
        {
            var (stepX, stepY) = line[0] switch
            {
                'U' => (0, 1),
                'D' => (0, -1),
                'L' => (-1, 0),
                'R' => (1, 0),
                _ => throw new NotImplementedException(),
            };

            var steps = int.Parse(line[2..]);

            visited.Add(currentTailLocation);

            for (int i = 0; i < steps; i++)
            {
                var (headX, headY) = currentHeadLocation;
                var (tailX, tailY) = currentTailLocation;
                var previousHeadLocation = currentHeadLocation;
                currentHeadLocation = (headX + stepX, headY + stepY);
                (headX, headY) = currentHeadLocation;

                var diffX = headX - tailX;
                var diffY = headY - tailY;

                // If the tail is out of range of the head, move it.
                if (diffX > 1 || diffX < -1 || diffY > 1 || diffY < -1)
                {
                    currentTailLocation = previousHeadLocation;
                    visited.Add(currentTailLocation);
                }
            }
        }

        return visited.Count;
    }

    [Benchmark]
    public int Auros_Part2()
    {
        HashSet<(int, int)> visited = new();
        Span<(int, int)> rope = stackalloc (int, int)[10];

        foreach (var line in _inputLines)
        {
            var (stepX, stepY) = line[0] switch
            {
                'U' => (0, 1),
                'D' => (0, -1),
                'L' => (-1, 0),
                'R' => (1, 0),
                _ => throw new NotImplementedException(),
            };

            var steps = int.Parse(line[2..]);

            for (int i = 0; i < steps; i++)
            {
                // Move the head
                var (headX, headY) = rope[0];
                var targetLocation = (headX + stepX, headY + stepY);
                var (targetX, targetY) = targetLocation;
                rope[0] = targetLocation;

                for (int c = 1; c < rope.Length; c++)
                {
                    var (prevX, prevY) = rope[c - 1];
                    var (knotX, knotY) = rope[c];

                    var diffX = knotX - prevX;
                    var diffY = knotY - prevY;

                    // If the current knot is out of range of the previous knot, move it.
                    if (diffX > 1 || diffX < -1 || diffY > 1 || diffY < -1)
                        rope[c] = (knotX + (diffX is 0 ? 0 : (diffX > 0 ? -1 : 1)), knotY + (diffY is 0 ? 0 : (diffY > 0 ? -1 : 1)));
                }

                var tail = rope[^1];
                visited.Add(tail);
            }
        }

        return visited.Count;
    }

    [Benchmark]
    public int Caeden_Part1()
    {
        const int tailLength = 1; // Change to 1 for part 1, 9 for part 2
        HashSet<int> visitedPositions = new(_inputMemory.Span.Length);
        Span<Caeden_Vector2Int> rope = stackalloc Caeden_Vector2Int[tailLength + 1];

        for (var line = 0; line < _inputMemory.Span.Length; line++)
        {
            var currentLine = _inputMemory.Span[line];

            // Get around int.Parse by manually parsing the amount ourselves
            var movementAmount = currentLine.Length == 4
                ? ((currentLine[2] - '0') * 10) + currentLine[3] - '0'
                : currentLine[2] - '0';

            var direction = currentLine[0] switch
            {
                'R' => new Caeden_Vector2Int(1, 0),
                'L' => new Caeden_Vector2Int(-1, 0),
                'U' => new Caeden_Vector2Int(0, 1),
                'D' => new Caeden_Vector2Int(0, -1),
                _ => throw new UnreachableException("huh")
            };

            for (var i = 0; i < movementAmount; i++)
            {
                rope[0] += direction;

                for (var j = 0; j < tailLength; j++)
                {
                    var distance = rope[j] - rope[j + 1];
                    if (distance.Length > 1)
                    {
                        rope[j + 1] += distance.Direction;
                    }
                }

                visitedPositions.Add(rope[^1].GetHashCode());
            }
        }
        return visitedPositions.Count;
    }

    [Benchmark]
    public int Caeden_Part2()
    {
        const int tailLength = 9; // Change to 1 for part 1, 9 for part 2
        HashSet<int> visitedPositions = new(_inputMemory.Span.Length);
        Span<Caeden_Vector2Int> rope = stackalloc Caeden_Vector2Int[tailLength + 1];

        for (var line = 0; line < _inputMemory.Span.Length; line++)
        {
            var currentLine = _inputMemory.Span[line];

            // Get around int.Parse by manually parsing the amount ourselves
            var movementAmount = currentLine.Length == 4
                ? ((currentLine[2] - '0') * 10) + currentLine[3] - '0'
                : currentLine[2] - '0';

            var direction = currentLine[0] switch
            {
                'R' => new Caeden_Vector2Int(1, 0),
                'L' => new Caeden_Vector2Int(-1, 0),
                'U' => new Caeden_Vector2Int(0, 1),
                'D' => new Caeden_Vector2Int(0, -1),
                _ => throw new UnreachableException("huh")
            };

            for (var i = 0; i < movementAmount; i++)
            {
                rope[0] += direction;

                for (var j = 0; j < tailLength; j++)
                {
                    var distance = rope[j] - rope[j + 1];
                    if (distance.Length > 1)
                    {
                        rope[j + 1] += distance.Direction;
                    }
                }

                visitedPositions.Add(rope[^1].GetHashCode());
            }
        }
        return visitedPositions.Count;
    }

    struct Caeden_Vector2Int
    {
        public int Length => (X * X + Y * Y) >> 1;
        public Caeden_Vector2Int Direction => new(Math.Sign(X), Math.Sign(Y));

        public int X;
        public int Y;

        public Caeden_Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Caeden_Vector2Int operator +(Caeden_Vector2Int left, Caeden_Vector2Int right) => new(left.X + right.X, left.Y + right.Y);

        public static Caeden_Vector2Int operator -(Caeden_Vector2Int left, Caeden_Vector2Int right) => new(left.X - right.X, left.Y - right.Y);

        public override int GetHashCode()
        {
            var hash = Math.Abs(X) << 16;
            hash += Math.Abs(Y) << 2;
            hash += (Math.Sign(X) >= 0 ? 1 : 0) << 1;
            return hash + (Math.Sign(Y) >= 0 ? 1 : 0);
        }
    }
}