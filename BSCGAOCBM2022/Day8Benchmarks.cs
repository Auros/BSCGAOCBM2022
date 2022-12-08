using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;

namespace BSCGAOCBM2022;

[MemoryDiagnoser]
public class Day8Benchmarks
{
    private string _inputText = null!;
    private string[] _inputLines = null!;
    private IEnumerable<string> _inputEnumerable = null!;

    [GlobalSetup]
    public void Setup()
    {
        _inputText = File.ReadAllText(@"Input\8.txt");
        _inputLines = File.ReadAllLines(@"Input\8.txt");
        _inputEnumerable = _inputLines.AsEnumerable();
    }

    readonly record struct Auros_Coordinate(int X, int Y);

    [Benchmark]
    public int Auros_Part1()
    {
        var width = _inputLines[0].Length;
        var height = _inputLines.Length;
        int endWidth = width - 1;
        int endHeight = height - 1;

        void ScanForVisibleTrees(int x, int y, int xIncrement, int yIncrement, int limit, HashSet<Auros_Coordinate> collector)
        {
            var maximumTree = -1;
            while (x >= 0 && y >= 0 && x < limit && y < limit && maximumTree is not 57 /* 57 is 9, the tallest tree possible. No point in looking further beyond. */)
            {
                var currentTree = _inputLines[x][y];

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

    [Benchmark]
    public int Auros_Part2()
    {
        int bestScore = 0;
        var width = _inputLines[0].Length;
        var height = _inputLines.Length;

        int ScanUntilViewBlocked(int x, int y, int xIncrement, int yIncrement, int limit, int maximum)
        {
            x += xIncrement;
            y += yIncrement;
            int viewDistance = 0;

            while (x >= 0 && y >= 0 && x < limit && y < limit)
            {
                var currentTree = _inputLines![x][y];

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
            var value = _inputLines[x][y];
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

    [Benchmark]
    public int Caeden_Part1()
    {
        var treeHeightmapWidth = _inputLines[0].Length;
        var treeHeightmapHeight = _inputLines.Length;

        var visibleTrees = 0;

        for (var y = 0; y < treeHeightmapHeight; y++)
            for (var x = 0; x < treeHeightmapWidth; x++)
            {
                var treeHeight = _inputLines[y][x];

                var isTreeVisible = x == 0 || y == 0 || x == treeHeightmapWidth - 1 || y == treeHeightmapHeight - 1
                || IsTreeBlocked(treeHeight, Enumerable.Range(0, x).Select(left => _inputLines[y][left]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(x + 1, treeHeightmapWidth - x - 1).Select(right => _inputLines[y][right]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(0, y).Select(up => _inputLines[up][x]))
                || IsTreeBlocked(treeHeight, Enumerable.Range(y + 1, treeHeightmapHeight - y - 1).Select(down => _inputLines[down][x]));

                if (isTreeVisible)
                {
                    visibleTrees++;
                }
            }

        bool IsTreeBlocked(char tree, IEnumerable<char> treesToEdge) => treesToEdge.Max() < tree;

        return visibleTrees;
    }

    [Benchmark]
    public int Caeden_Part2()
    {
        var treeHeightmapWidth = _inputLines[0].Length;
        var treeHeightmapHeight = _inputLines.Length;

        var maxScenicScore = 0;
        for (var y = 0; y < treeHeightmapHeight; y++)
            for (var x = 0; x < treeHeightmapWidth; x++)
            {
                var treeHeight = _inputLines[y][x];

                var scenicScore = GetTreeViewDistance(treeHeight, Enumerable.Range(0, x).Select(left => _inputLines[y][left]).Reverse())
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(x + 1, treeHeightmapWidth - x - 1).Select(right => _inputLines[y][right]))
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(0, y).Select(up => _inputLines[up][x]).Reverse())
                    * GetTreeViewDistance(treeHeight, Enumerable.Range(y + 1, treeHeightmapHeight - y - 1).Select(down => _inputLines[down][x]));

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

    [Benchmark]
    public int Eris_Part1_Sol1()
    {
        var treeHeightMap = Eris_ParseTreeHeightMap_Sol1();

        var forestHeight = treeHeightMap.Length;
        var forestWidth = treeHeightMap[0].Length;
        var visibleTreeCount = 0;

        for (var y = 0; y < forestHeight; y++)
            for (var x = 0; x < forestWidth; x++)
            {
                if (Eris_CheckTreeVisible_Sol1(treeHeightMap, x, y))
                {
                    visibleTreeCount++;
                }
            }

        return visibleTreeCount;
    }

    [Benchmark]
    public int Eris_Part2_Sol1()
    {
        var treeHeightMap = Eris_ParseTreeHeightMap_Sol1();

        IEnumerable<int> ScenicScoreEnumerator()
        {
            var forestHeight = treeHeightMap.Length;
            var forestWidth = treeHeightMap[0].Length;

            for (var x = 2; x < forestWidth; x++)
                for (var y = 3; y < forestHeight; y++)
                {
                    yield return Eris_CalculateScenicScore_Sol1(treeHeightMap, x, y);
                }
        }

        return ScenicScoreEnumerator().Max();
    }

    private int[][] Eris_ParseTreeHeightMap_Sol1()
    {
        return _inputEnumerable
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
}