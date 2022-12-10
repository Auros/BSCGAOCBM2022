using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Runtime.CompilerServices;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day8Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(8);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        var width = _input.Lines[0].Length;
        var height = _input.Lines.Length;
        int endWidth = width - 1;
        int endHeight = height - 1;

        void ScanForVisibleTrees(int x, int y, int xIncrement, int yIncrement, int limit, HashSet<Auros_Coordinate> collector)
        {
            var maximumTree = -1;
            while (x >= 0 && y >= 0 && x < limit && y < limit && maximumTree is not 57 /* 57 is 9, the tallest tree possible. No point in looking further beyond. */)
            {
                var currentTree = _input.Lines[x][y];

                if (currentTree > maximumTree)
                {
                    collector.Add(new Auros_Coordinate(x, y));
                    maximumTree = currentTree;
                }

                x += xIncrement;
                y += yIncrement;
            }
        }

        HashSet<Auros_Coordinate> visibleCoordinates = new();
        for (int i = 0; i < width; i++)
        {
            ScanForVisibleTrees(0, i, 1, 0, height, visibleCoordinates);
            ScanForVisibleTrees(endWidth, i, -1, 0, height, visibleCoordinates);
        }
        for (int i = 0; i < height; i++)
        {
            ScanForVisibleTrees(i, 0, 0, 1, width, visibleCoordinates);
            ScanForVisibleTrees(i, endHeight, 0, -1, width, visibleCoordinates);
        }
        return visibleCoordinates.Count;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        int bestScore = 0;
        var width = _input.Lines[0].Length;
        var height = _input.Lines.Length;

        int ScanUntilViewBlocked(int x, int y, int xIncrement, int yIncrement, int limit, int maximum)
        {
            x += xIncrement;
            y += yIncrement;
            int viewDistance = 0;

            while (x >= 0 && y >= 0 && x < limit && y < limit)
            {
                var currentTree = _input.Lines![x][y];

                viewDistance++;
                if (currentTree >= maximum)
                    break;

                x += xIncrement;
                y += yIncrement;
            }
            return viewDistance;
        }

        int CalculateScenicScore(int x, int y)
        {
            var value = _input.Lines[x][y];
            int score = ScanUntilViewBlocked(x, y, 1, 0, height, value);
            score *= ScanUntilViewBlocked(x, y, -1, 0, height, value);
            score *= ScanUntilViewBlocked(x, y, 0, 1, width, value);
            score *= ScanUntilViewBlocked(x, y, 0, -1, width, value);
            return score;
        }

        for (int i = 0; i < width; i++)
        {
            for (int c = 0; c < height; c++)
            {
                var score = CalculateScenicScore(i, c);
                if (score > bestScore)
                    bestScore = score;
            }
        }

        return bestScore;
    }

    readonly record struct Auros_Coordinate(int X, int Y);

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var treeHeightmapWidth = _input.Lines[0].Length;
        var treeHeightmapHeight = _input.Lines.Length;

        var visibleTrees = 0;

        for (var y = 0; y < treeHeightmapHeight; y++)
            for (var x = 0; x < treeHeightmapWidth; x++)
            {
                var treeHeight = _input.Lines[y][x];

                var isTreeVisible = x == 0 || y == 0 || x == treeHeightmapWidth - 1 || y == treeHeightmapHeight - 1
                || IsTreeBlocked(treeHeight, Enumerable.Range(0, x).Select(left => _input.Lines[y][left]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(x + 1, treeHeightmapWidth - x - 1).Select(right => _input.Lines[y][right]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(0, y).Select(up => _input.Lines[up][x]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(y + 1, treeHeightmapHeight - y - 1).Select(down => _input.Lines[down][x]));

                if (isTreeVisible)
                {
                    visibleTrees++;
                }
            }

        bool IsTreeBlocked(char tree, IEnumerable<char> treesToEdge) => treesToEdge.Max() < tree;

        return visibleTrees;
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        var treeHeightmapWidth = _input.Lines[0].Length;
        var treeHeightmapHeight = _input.Lines.Length;

        var maxScenicScore = 0;
        for (var y = 0; y < treeHeightmapHeight; y++)
            for (var x = 0; x < treeHeightmapWidth; x++)
            {
                var treeHeight = _input.Lines[y][x];

                var scenicScore = GetTreeViewDistance(treeHeight, Enumerable.Range(0, x).Select(left => _input.Lines[y][left]).Reverse())
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(x + 1, treeHeightmapWidth - x - 1).Select(right => _input.Lines[y][right]))
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(0, y).Select(up => _input.Lines[up][x]).Reverse())
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(y + 1, treeHeightmapHeight - y - 1).Select(down => _input.Lines[down][x]));

                if (scenicScore > maxScenicScore) maxScenicScore = scenicScore;
            }

        int GetTreeViewDistance(char targetTree, IEnumerable<char> treesToEdge)
        {
            var distance = 0;
            foreach (var tree in treesToEdge)
            {
                distance++;
                if (tree >= targetTree) break;
            }
            return distance;
        }
        return maxScenicScore;
    }

    #endregion

    #region Eris Solution 1

    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1_Sol1()
    {
        var treeHeightMap = Eris_ParseTreeHeightMap_Sol1();

        var forestHeight = treeHeightMap.Length;
        var forestWidth = treeHeightMap[0].Length;
        var visibleTreeCount = 2 * forestHeight + 2 * forestWidth - 4;
        // -4 so we don't count the corners twice

        for (var y = 1; y < forestHeight - 1; y++)
            for (var x = 1; x < forestWidth - 1; x++)
            {
                if (Eris_CheckTreeVisible_Sol1(treeHeightMap, x, y))
                {
                    visibleTreeCount++;
                }
            }

        return visibleTreeCount;
    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2_Sol1()
    {
        var treeHeightMap = Eris_ParseTreeHeightMap_Sol1();

        IEnumerable<int> ScenicScoreEnumerator()
        {
            var forestHeight = treeHeightMap.Length;
            var forestWidth = treeHeightMap[0].Length;

            for (var x = 1; x < forestWidth - 1; x++)
                for (var y = 1; y < forestHeight - 1; y++)
                {
                    yield return Eris_CalculateScenicScore_Sol1(treeHeightMap, x, y);
                }
        }

        return ScenicScoreEnumerator().Max();
    }

    private int[][] Eris_ParseTreeHeightMap_Sol1()
    {
        return _input.Enumerable
            .Select(treeLine => treeLine.Select(treeHeight => (int)char.GetNumericValue(treeHeight)).ToArray())
            .ToArray();
    }

    private static bool Eris_CheckTreeVisible_Sol1(int[][] treeHeightMap, int x, int y)
    {
        var visible = false;

        var treeRow = treeHeightMap[y];
        // Check if numbers to the left are lower than the number from current position
        visible |= Eris_CheckTreeVisibilityInDirection_Sol1(treeRow, x, -1);

        // Check if numbers to the right are lower than the number from current position
        visible |= Eris_CheckTreeVisibilityInDirection_Sol1(treeRow, x, 1);

        var treeColumn = treeHeightMap.Select(line => line[x]).ToArray();
        // Check if numbers to the top are lower than the number from current position
        visible |= Eris_CheckTreeVisibilityInDirection_Sol1(treeColumn, y, -1);

        // Check if numbers to the bottom are lower than the number from current position
        visible |= Eris_CheckTreeVisibilityInDirection_Sol1(treeColumn, y, 1);

        return visible;
    }

    private static bool Eris_CheckTreeVisibilityInDirection_Sol1(int[] treeHeightsForOrientation, int index, int direction)
    {
        var heightThreshold = treeHeightsForOrientation[index];
        index += direction;

        bool CanContinue() => direction switch
        {
            -1 => index >= 0,
            1 => index < treeHeightsForOrientation.Length,
            _ => throw new NotSupportedException()
        };

        while (CanContinue())
        {
            var localThreeHeight = treeHeightsForOrientation[index];
            if (localThreeHeight >= heightThreshold)
            {
                return false;
            }

            index += direction;
        }

        return true;
    }

    private static int Eris_CalculateScenicScore_Sol1(int[][] treeHeightMap, int x, int y)
    {
        var scenicScore = 1;

        var treeRow = treeHeightMap[y];
        // Calculate scenic score for trees to the left of the current position
        scenicScore *= Eris_CalculateScenicScoreInDirection_Sol1(treeRow, x, -1);

        // Calculate scenic score for trees to the right of the current position
        scenicScore *= Eris_CalculateScenicScoreInDirection_Sol1(treeRow, x, 1);

        var treeColumn = treeHeightMap.Select(line => line[x]).ToArray();
        // Calculate scenic score for trees above the current position
        scenicScore *= Eris_CalculateScenicScoreInDirection_Sol1(treeColumn, y, -1);

        // Calculate scenic score for trees below the current position
        scenicScore *= Eris_CalculateScenicScoreInDirection_Sol1(treeColumn, y, 1);

        return scenicScore;
    }

    private static int Eris_CalculateScenicScoreInDirection_Sol1(int[] treeHeightsForOrientation, int index, int direction)
    {
        var heightThreshold = treeHeightsForOrientation[index];
        index += direction;

        bool CanContinue() => direction switch
        {
            -1 => index >= 0,
            1 => index < treeHeightsForOrientation.Length,
            _ => throw new NotSupportedException()
        };

        var scenicScore = 0;
        while (CanContinue())
        {
            scenicScore++;
            var localThreeHeight = treeHeightsForOrientation[index];
            if (localThreeHeight >= heightThreshold)
            {
                return scenicScore;
            }

            index += direction;
        }

        return scenicScore;
    }

    #endregion

    #region Eris Solution 2

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1_Sol2()
    {
        var (treeHeightMap, height, width) = Eris_ParseTreeHeightMap_Sol2();
        var visibleTreeCount = 2 * height + 2 * width - 4;
        // -4 so we don't count the corners twice

        ReadOnlySpan<byte> treeHeightSpan = treeHeightMap.AsSpan();

        for (var y = 1; y < height - 1; y++)
            for (var x = 1; x < width - 1; x++)
            {
                if (Eris_CheckTreeVisible_Sol2(ref treeHeightSpan, width, x, y))
                {
                    visibleTreeCount++;
                }
            }

        return visibleTreeCount;
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2_Sol2()
    {
        var (treeHeightMap, height, width) = Eris_ParseTreeHeightMap_Sol2();
        ReadOnlySpan<byte> treeHeightSpan = treeHeightMap.AsSpan();

        var max = 0;
        for (var x = 1; x < width - 1; x++)
            for (var y = 1; y < height - 1; y++)
            {
                var score = Eris_CalculateScenicScore_Sol2(ref treeHeightSpan, width, x, y);
                if (score > max)
                {
                    max = score;
                }
            }

        return max;
    }

    private (byte[] treeHeightMap, int height, int width) Eris_ParseTreeHeightMap_Sol2()
    {
        var treeHeightMapData = _input.Lines;
        return (
            treeHeightMap: treeHeightMapData
                .SelectMany(treeLine => treeLine.Select(treeHeight => (byte)treeHeight))
                .ToArray(),
            height: treeHeightMapData.Length,
            width: treeHeightMapData[0].Length);
    }

    // Helpers part 1
    private static bool Eris_CheckTreeVisible_Sol2(ref ReadOnlySpan<byte> treeHeightList, int width, int x, int y)
    {
        var treeRow = treeHeightList.Slice(y * width, width);
        // Check if numbers to the left are lower than the number from current position
        if (Eris_CheckTreeVisibilityInDirection_Sol2(ref treeRow, x, -1) ||
            // Check if numbers to the right are lower than the number from current position
            Eris_CheckTreeVisibilityInDirection_Sol2(ref treeRow, x, 1))
        {
            return true;
        }

        var treeColumn = treeHeightList[x..];
        // Check if numbers to the top are lower than the number from current position
        return Eris_CheckTreeVisibilityInDirection_Sol2(ref treeColumn, y * width, -width) ||
               // Check if numbers to the bottom are lower than the number from current position
               Eris_CheckTreeVisibilityInDirection_Sol2(ref treeColumn, y * width, width);
    }

    private static bool Eris_CheckTreeVisibilityInDirection_Sol2(ref ReadOnlySpan<byte> treeHeightsForOrientation, int index, int offset)
    {
        var heightThreshold = treeHeightsForOrientation[index];
        index += offset;

        while (offset switch
        {
            < 0 => index >= 0,
            > 0 => index < treeHeightsForOrientation.Length,
            _ => throw new NotSupportedException()
        })
        {
            var localThreeHeight = treeHeightsForOrientation[index];
            if (localThreeHeight >= heightThreshold)
            {
                return false;
            }

            index += offset;
        }

        return true;
    }

    // Helpers part 2
    private static int Eris_CalculateScenicScore_Sol2(ref ReadOnlySpan<byte> treeHeightList, int width, int x, int y)
    {
        var treeRow = treeHeightList.Slice(y * width, width);
        var treeColumn = treeHeightList[x..];

        return Eris_CalculateScenicScoreInBiDirectional_Sol2(ref treeRow, x, 1) * // Check if numbers to the left are lower than the number from current position
               Eris_CalculateScenicScoreInBiDirectional_Sol2(ref treeColumn, y * width, width); // Check if numbers to the bottom are lower than the number from current position
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int Eris_CalculateScenicScoreInBiDirectional_Sol2(ref ReadOnlySpan<byte> treeHeightsForOrientation, int index, int offset)
    {
        return Eris_CalculateScenicScoreInDirection_Sol2(ref treeHeightsForOrientation, index, -offset) *
               Eris_CalculateScenicScoreInDirection_Sol2(ref treeHeightsForOrientation, index, offset);
    }

    private static int Eris_CalculateScenicScoreInDirection_Sol2(ref ReadOnlySpan<byte> treeHeightsForOrientation, int index, int offset)
    {
        var heightThreshold = treeHeightsForOrientation[index];
        index += offset;

        var scenicScore = 0;

        while (offset switch
        {
            < 0 => index >= 0,
            > 0 => index < treeHeightsForOrientation.Length,
            _ => throw new NotSupportedException()
        })
        {
            scenicScore++;
            var localThreeHeight = treeHeightsForOrientation[index];
            if (localThreeHeight >= heightThreshold)
            {
                return scenicScore;
            }

            index += offset;
        }

        return scenicScore;
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