using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day9Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(9);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        HashSet<(int, int)> visited = new();
        (int, int) currentHeadLocation = (0, 0);
        (int, int) currentTailLocation = (0, 0);

        foreach (var line in _input.Lines)
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

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        HashSet<(int, int)> visited = new();
        Span<(int, int)> rope = stackalloc (int, int)[10];

        foreach (var line in _input.Lines)
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
                        rope[c] = (knotX + (diffX is 0 ? 0 : diffX > 0 ? -1 : 1), knotY + (diffY is 0 ? 0 : diffY > 0 ? -1 : 1));
                }

                var tail = rope[^1];
                visited.Add(tail);
            }
        }

        return visited.Count;
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        const int tailLength = 1; // Change to 1 for part 1, 9 for part 2
        HashSet<int> visitedPositions = new(_input.Memory.Span.Length);
        Span<Caeden_Vector2Int> rope = stackalloc Caeden_Vector2Int[tailLength + 1];

        for (var line = 0; line < _input.Memory.Span.Length; line++)
        {
            var currentLine = _input.Memory.Span[line];

            // Get around int.Parse by manually parsing the amount ourselves
            var movementAmount = currentLine.Length == 4
                ? (currentLine[2] - '0') * 10 + currentLine[3] - '0'
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

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        const int tailLength = 9; // Change to 1 for part 1, 9 for part 2
        HashSet<int> visitedPositions = new(_input.Memory.Span.Length);
        Span<Caeden_Vector2Int> rope = stackalloc Caeden_Vector2Int[tailLength + 1];

        for (var line = 0; line < _input.Memory.Span.Length; line++)
        {
            var currentLine = _input.Memory.Span[line];

            // Get around int.Parse by manually parsing the amount ourselves
            var movementAmount = currentLine.Length == 4
                ? (currentLine[2] - '0') * 10 + currentLine[3] - '0'
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
        public int Length => X * X + Y * Y >> 1;
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

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        var instructions = Eris_GetInstructions();

        var uniquePositions = new HashSet<Vector2>();
        Vector2 headPosition = new(0, 0);
        Vector2 tailPosition = new(0, 0);
        foreach (var (moveDirection, stepCount) in instructions)
        {
            var moveVector = Eris_ConvertDirectionToMoveVector(moveDirection);
            for (var i = 0; i < stepCount; i++)
            {
                headPosition += moveVector;

                var diff = Eris_CalculatePositionDifference(headPosition, tailPosition);
                Eris_MoveTailIfNeeded(ref tailPosition, diff);

                uniquePositions.Add(tailPosition);
            }
        }

        return uniquePositions.Count;
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        var instructions = Eris_GetInstructions();

        var knots = new Vector2[10];
        for (var i = 0; i < 10; i++)
        {
            knots[i] = new Vector2(0, 0);
        }

        var uniquePositions = new HashSet<Vector2>();
        foreach (var (moveDirection, stepCount) in instructions)
        {
            var moveVector = Eris_ConvertDirectionToMoveVector(moveDirection);
            for (var i = 0; i < stepCount; i++)
            {
                knots[0] += moveVector;

                for (var j = 1; j < knots.Length; j++)
                {
                    var diff = Eris_CalculatePositionDifference(knots[j - 1], knots[j]);
                    if (!Eris_MoveTailIfNeeded(ref knots[j], diff))
                    {
                        break;
                    }
                }

                uniquePositions.Add(knots.Last());
            }
        }

        return uniquePositions.Count;
    }

    public IEnumerable<(char moveDirection, int stepCount)> Eris_GetInstructions() => _input.Enumerable.Select(instruction => (instruction[0], int.Parse(instruction[2..])));

    private static bool Eris_MoveTailIfNeeded(ref Vector2 tailPosition, Vector2 diff)
    {
        bool MoveInternal(float diffLeadingPart, ref float positionLeadingPart, float diffLesserPart, ref float positionLesserPart)
        {
            if (diffLeadingPart is > 1 or < -1)
            {
                positionLeadingPart += diffLeadingPart - Math.Sign(diffLeadingPart);

                if (diffLesserPart != 0)
                {
                    positionLesserPart += Math.Sign(diffLesserPart);
                }

                return true;
            }

            return false;
        }

        return Math.Abs(diff.X) > Math.Abs(diff.Y)
            ? MoveInternal(diff.X, ref tailPosition.X, diff.Y, ref tailPosition.Y)
            : MoveInternal(diff.Y, ref tailPosition.Y, diff.X, ref tailPosition.X);
    }

    private static Vector2 Eris_CalculatePositionDifference(Vector2 headPosition, Vector2 tailPosition) => headPosition - tailPosition;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Vector2 Eris_ConvertDirectionToMoveVector(char moveDirection)
    {
        return moveDirection switch
        {
            'U' => new Vector2(0, 1),
            'D' => new Vector2(0, -1),
            'R' => new Vector2(1, 0),
            'L' => new Vector2(-1, 0),
            _ => throw new InvalidOperationException("Bonk")
        };
    }

    #endregion

    #region Goobie
    /*
    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Goobie_Part1()
    {

    }

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Goobie_Part2()
    {

    }
    */
    #endregion
}