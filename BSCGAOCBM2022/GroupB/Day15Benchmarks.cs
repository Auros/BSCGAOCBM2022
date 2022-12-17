using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupB;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day15Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(15);

    #region Auros

    /*[Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part1)]
    public long Auros_Part1()
    {
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part2)]
    public long Auros_Part2()
    {
    }*/

    #endregion

    #region Caeden

    /*
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public long Caeden_Part1()
    {

    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public long Caeden_Part2()
    {

    }
    */

    #endregion

    #region Eris

    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public long Eris_Part1()
    {
        const int rowToCheckAgainst = 2000000;

        ReadOnlySpan<string> sensorReports = _input.Lines;

        var sensorReportsCoordinateBufferSize = sensorReports.Length * 2;
        Span<Eris_Coordinate> sensorReportsCoordinateBuffer = stackalloc Eris_Coordinate[sensorReportsCoordinateBufferSize];
        Span<int> sensorReportsCoordinateDistanceBuffer = stackalloc int[sensorReports.Length];

        Eris_PrepareSensorReportsCoordinateBuffer(ref sensorReports, sensorReportsCoordinateBuffer, sensorReportsCoordinateDistanceBuffer);

        Span<Eris_CoordinateRange> sensorRowData = stackalloc Eris_CoordinateRange[sensorReports.Length];
        HashSet<Eris_Coordinate> uniqueBeaconCoordinatesForRow = new();

        var sensorRowDataIndex = 0;
        for (var i = 0; i < sensorReports.Length; i++)
        {
            var coordinatesSlice = sensorReportsCoordinateBuffer.Slice(i * 2, 2);
            var sensorCoordinate = coordinatesSlice[0];

            var beaconCoordinate = coordinatesSlice[1];
            if (beaconCoordinate.Y == rowToCheckAgainst)
            {
                uniqueBeaconCoordinatesForRow.Add(beaconCoordinate);
            }

            var distanceBetweenSensorAndBeacon = sensorReportsCoordinateDistanceBuffer[i];

            var distanceToTargetRow = Math.Abs(sensorCoordinate.Y - rowToCheckAgainst);
            var remainingDistance = distanceBetweenSensorAndBeacon - distanceToTargetRow;
            if (remainingDistance >= 0)
            {
                var startingPosition = sensorCoordinate.X - remainingDistance;
                var endingPosition = sensorCoordinate.X + remainingDistance;

                sensorRowData[sensorRowDataIndex++] = new Eris_CoordinateRange(startingPosition, endingPosition);
            }
        }

        var usedSensorRowData = sensorRowData[..sensorRowDataIndex];
        usedSensorRowData.Sort(ErisCoordinateRangeAscendingXComparer);

        var minX = usedSensorRowData[0].StartIndex;

        var maxX = int.MinValue;
        for (var i = 0; i < usedSensorRowData.Length; i++)
        {
            if (usedSensorRowData[i].EndIndex > maxX)
            {
                maxX = usedSensorRowData[i].EndIndex;
            }
        }

        return maxX - minX - uniqueBeaconCoordinatesForRow.Count + 1;
    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public long Eris_Part2()
    {
        const int minBound = 0;
        const int maxBound = 4000000;
        const long tuningFrequency = 4000000;

        ReadOnlySpan<string> sensorReports = _input.Lines;

        var sensorReportsCoordinateBufferSize = sensorReports.Length * 2;
        Span<Eris_Coordinate> sensorReportsCoordinateBuffer = stackalloc Eris_Coordinate[sensorReportsCoordinateBufferSize];
        Span<int> sensorReportsCoordinateDistanceBuffer = stackalloc int[sensorReports.Length];

        Eris_PrepareSensorReportsCoordinateBuffer(ref sensorReports, sensorReportsCoordinateBuffer, sensorReportsCoordinateDistanceBuffer);

        Span<Eris_CoordinateRange> sensorRowData = stackalloc Eris_CoordinateRange[sensorReports.Length];

        // No larger than 4000000
        for (var row = minBound; row <= maxBound; row++)
        {
            var sensorRowDataIndex = 0;
            for (var i = 0; i < sensorReports.Length; i++)
            {
                var coordinatesSlice = sensorReportsCoordinateBuffer.Slice(i * 2, 2);
                var sensorCoordinate = coordinatesSlice[0];

                var distanceBetweenSensorAndBeacon = sensorReportsCoordinateDistanceBuffer[i];

                var distanceToTargetRow = Math.Abs(sensorCoordinate.Y - row);
                var remainingDistance = distanceBetweenSensorAndBeacon - distanceToTargetRow;
                if (remainingDistance >= 0)
                {
                    var startingPosition = sensorCoordinate.X - remainingDistance;
                    var endingPosition = sensorCoordinate.X + remainingDistance;

                    sensorRowData[sensorRowDataIndex++] = new Eris_CoordinateRange(startingPosition, endingPosition);
                }
            }

            var usedSensorRowData = sensorRowData[..sensorRowDataIndex];
            // MemoryExtensions.Sort(usedSensorRowData, ErisCoordinateRangeAscendingXComparer);
            usedSensorRowData.Sort(ErisCoordinateRangeAscendingXComparer);

            var lastX = minBound - 1;

            for (var i = 0; i < usedSensorRowData.Length; i++)
            {
                var currentRange = usedSensorRowData[i];
                if (currentRange.EndIndex < minBound)
                {
                    continue;
                }

                if (currentRange.StartIndex > maxBound)
                {
                    break;
                }

                if (currentRange.StartIndex - lastX > 1)
                {
                    return (lastX + 1) * tuningFrequency + row;
                }

                if (lastX < currentRange.EndIndex)
                {
                    lastX = currentRange.EndIndex;
                }
            }
        }

        throw new UnreachableException("Bonk!");
    }

    private static void Eris_PrepareSensorReportsCoordinateBuffer(ref ReadOnlySpan<string> sensorReportsRaw,
        scoped Span<Eris_Coordinate> sensorReportsCoordinateBuffer,
        scoped Span<int> sensorReportsCoordinateDistanceBuffer)
    {
        const int sensorAtOffset = 12;
        const int closestBeaconOffset = 25;

        var sensorReportsCoordinateBufferIndex = 0;
        for (var i = 0; i < sensorReportsRaw.Length; i++)
        {
            var sensorReportRaw = sensorReportsRaw[i].AsSpan()[sensorAtOffset..];

            var sensorReportRawTraversalIndex = 0;

            ref var sensorCoordinate = ref sensorReportsCoordinateBuffer[sensorReportsCoordinateBufferIndex++];
            Eris_ExtractAndParseCoordinate(sensorReportRaw, ref sensorReportRawTraversalIndex, ref sensorCoordinate);

            sensorReportRawTraversalIndex += closestBeaconOffset;

            ref var beaconCoordinate = ref sensorReportsCoordinateBuffer[sensorReportsCoordinateBufferIndex++];
            Eris_ExtractAndParseCoordinate(sensorReportRaw, ref sensorReportRawTraversalIndex, ref beaconCoordinate);

            sensorReportsCoordinateDistanceBuffer[i] = sensorCoordinate - beaconCoordinate;
        }
    }

    private static void Eris_ExtractAndParseCoordinate(
        ReadOnlySpan<char> sensorReportRaw, ref int sensorReportRawTraversalIndex, ref Eris_Coordinate coordinate)
    {
        var startIndex = sensorReportRawTraversalIndex;

        do
        {
            sensorReportRawTraversalIndex++;
        } while (sensorReportRaw[sensorReportRawTraversalIndex] != ',');

        var x = Eris_SpecializedCaedenIntParser(sensorReportRaw.Slice(startIndex, sensorReportRawTraversalIndex - startIndex));

        sensorReportRawTraversalIndex += 4;
        startIndex = sensorReportRawTraversalIndex;

        do
        {
            sensorReportRawTraversalIndex++;
        } while (sensorReportRawTraversalIndex < sensorReportRaw.Length && sensorReportRaw[sensorReportRawTraversalIndex] != ':');

        var y = Eris_SpecializedCaedenIntParser(sensorReportRaw.Slice(startIndex, sensorReportRawTraversalIndex - startIndex));

        coordinate = new Eris_Coordinate(x, y);
    }

    private static int Eris_SpecializedCaedenIntParser(ReadOnlySpan<char> span)
    {
        if (span[0] == '-')
        {
            return -Eris_SpecializedCaedenIntParser(span[1..]);
        }

        return span.Length switch
        {
            7 => (span[0] - '0') * 1000000 + (span[1] - '0') * 100000 + (span[2] - '0') * 10000 + (span[3] - '0') * 1000 + (span[4] - '0') * 100 + (span[5] - '0') * 10 + (span[6] - '0'),
            6 => (span[0] - '0') * 100000 + (span[1] - '0') * 10000 + (span[2] - '0') * 1000 + (span[3] - '0') * 100 + (span[4] - '0') * 10 + (span[5] - '0'),
            5 => (span[0] - '0') * 10000 + (span[1] - '0') * 1000 + (span[2] - '0') * 100 + (span[3] - '0') * 10 + (span[4] - '0'),
            _ => throw new UnreachableException("Bonk!")
        };
    }

    private readonly struct Eris_Coordinate : IEquatable<Eris_Coordinate>
    {
        public readonly int X;
        public readonly int Y;

        public Eris_Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static int operator -(Eris_Coordinate left, Eris_Coordinate right)
        {
            return Math.Abs(left.X - right.X) + Math.Abs(left.Y - right.Y);
        }

        public bool Equals(Eris_Coordinate other)
        {
            return X == other.X && Y == other.Y;
        }
    }

    private readonly struct Eris_CoordinateRange
    {
        public readonly int StartIndex;
        public readonly int EndIndex;

        public Eris_CoordinateRange(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }
    }

    private static readonly Comparison<Eris_CoordinateRange> ErisCoordinateRangeAscendingXComparer = (r1, r2) => r1.StartIndex - r2.StartIndex;

    #endregion

    #region Goobie

    /*
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public long Goobie_Part1()
    {

    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public long Goobie_Part2()
    {

    }
    */

    #endregion
}