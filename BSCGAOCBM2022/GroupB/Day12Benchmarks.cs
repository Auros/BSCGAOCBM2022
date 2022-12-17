using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Runtime.CompilerServices;

namespace BSCGAOCBM2022.GroupB;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day12Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(12);

    #region Auros

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        var input = _input.Lines;

        var rowLength = input[0].Length;
        var columnLength = input.Length;
        var totalMountainSize = input.Length * rowLength;
        Span<char> mountain = stackalloc char[totalMountainSize];

        int startIndex = -1;
        int endIndex = -1;

        for (int i = 0; i < mountain.Length; i++)
        {
            var row = i / rowLength;
            var current = input[row][i - (row * rowLength)];
            ref var mountainCell = ref mountain[i];
            if (current is 'S')
            {
                startIndex = i;
                mountainCell = 'a';
            }
            else if (current is 'E')
            {
                endIndex = i;
                mountainCell = 'z';
            }
            else
            {
                mountainCell = current;
            }
        }

        var minimumPathFromStart = Auros_GetMinimumPathValue(startIndex, endIndex, rowLength, columnLength, ref mountain);
        return minimumPathFromStart;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        var input = _input.Lines;

        var rowLength = input[0].Length;
        var columnLength = input.Length;
        var totalMountainSize = input.Length * rowLength;
        Span<char> mountain = stackalloc char[totalMountainSize];

        int startIndex = -1;
        int endIndex = -1;

        for (int i = 0; i < mountain.Length; i++)
        {
            var row = i / rowLength;
            var current = input[row][i - (row * rowLength)];
            ref var mountainCell = ref mountain[i];
            if (current is 'S')
            {
                startIndex = i;
                mountainCell = 'a';
            }
            else if (current is 'E')
            {
                endIndex = i;
                mountainCell = 'z';
            }
            else
            {
                mountainCell = current;
            }
        }

        int lowestScore = int.MaxValue;
        for (int i = 0; i < mountain.Length; i++)
        {
            ref var cell = ref mountain[i];
            if (cell is not 'b')
                continue;

            var score = Auros_GetMinimumPathValue(i, endIndex, rowLength, columnLength, ref mountain);
            if (lowestScore > score)
                lowestScore = score;
        }
        lowestScore++; // Since we base it off the second lowest elevation, we need to add one since it's a guarantee that the path we found is connected to the lowest.
        return lowestScore;
    }

    static int Auros_GetMinimumPathValue(int startIndex, int endIndex, int rowLength, int columnLength, ref Span<char> mountain)
    {
        int end = -1;
        Span<int> layer = stackalloc int[rowLength * 4 - 4]; // Maximum layer search depth will be the perimeter of the grid? Probably?
        Span<int> layerBuffer = stackalloc int[layer.Length];
        Span<int> mountainPathValues = stackalloc int[mountain.Length];
        mountainPathValues[startIndex] = -1;
        layer[0] = startIndex;
        int layerSize = 1;
        int depth = 1;

        while (end is -1)
        {
            int bufferPos = 0;
            for (int i = 0; i < layerSize; i++)
            {
                ref var layerElementIndex = ref layer[i];
                ref var layerElement = ref mountain[layerElementIndex];
                var maxIncline = layerElement + 1;

                var currentElementRow = layerElementIndex / rowLength;
                var currentElementCol = layerElementIndex % rowLength;

                // Look left:
                if (currentElementCol is not 0)
                {
                    var elementIndex = layerElementIndex - 1;
                    ref var element = ref mountain[elementIndex];
                    if (maxIncline >= element) // Is this element at a valid depth?
                    {
                        ref var elementPathValue = ref mountainPathValues[elementIndex];
                        if (elementPathValue is 0) // Ensure this path has not been visited.
                        {
                            elementPathValue = depth;
                            layerBuffer[bufferPos++] = elementIndex;

                            if (elementIndex == endIndex)
                                end = elementIndex;
                        }
                    }
                }

                // Look right:
                if (currentElementCol != rowLength - 1)
                {
                    var elementIndex = layerElementIndex + 1;
                    ref var element = ref mountain[elementIndex];
                    if (maxIncline >= element) // Is this element at a valid depth?
                    {
                        ref var elementPathValue = ref mountainPathValues[elementIndex];
                        if (elementPathValue is 0) // Ensure this path has not been visited.
                        {
                            elementPathValue = depth;
                            layerBuffer[bufferPos++] = elementIndex;

                            if (elementIndex == endIndex)
                                end = elementIndex;
                        }
                    }
                }

                // Look up:
                if (currentElementRow is not 0)
                {
                    var elementIndex = layerElementIndex - rowLength;
                    ref var element = ref mountain[elementIndex];
                    if (maxIncline >= element) // Is this element at a valid depth?
                    {
                        ref var elementPathValue = ref mountainPathValues[elementIndex];
                        if (elementPathValue is 0) // Ensure this path has not been visited.
                        {
                            elementPathValue = depth;
                            layerBuffer[bufferPos++] = elementIndex;

                            if (elementIndex == endIndex)
                                end = elementIndex;
                        }
                    }
                }

                // Look down:
                if (currentElementRow != columnLength - 1)
                {
                    var elementIndex = layerElementIndex + rowLength;
                    ref var element = ref mountain[elementIndex];
                    if (maxIncline >= element) // Is this element at a valid depth?
                    {
                        ref var elementPathValue = ref mountainPathValues[elementIndex];
                        if (elementPathValue is 0) // Ensure this path has not been visited.
                        {
                            elementPathValue = depth;
                            layerBuffer[bufferPos++] = elementIndex;

                            if (elementIndex == endIndex)
                                end = elementIndex;
                        }
                    }
                }
            }
            depth++;
            layerSize = bufferPos;
            for (int i = 0; i < bufferPos; i++)
            {
                ref var localLayer = ref layer[i];
                localLayer = layerBuffer[i];
            }
        }

        return mountainPathValues[end];
    }

    #endregion

    #region Caeden
    
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var inputLines = _input.Lines;
        var inputStr = _input.Text;

        var xLength = inputLines[0].Length;
        var yLength = inputLines.Length;

        // Helper methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetArrayIdxForPosition(int x, int y) => (y * xLength) + x + (y * 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Caeden_Vector2Int GetPositionForArrayIdx(int idx) => new(idx % (xLength + 2), idx / (xLength + 2));

        var startPosition = inputStr.IndexOf('S');
        var endPosition = inputStr.IndexOf('E');

        Span<bool> visitedPositions = stackalloc bool[inputStr.Length];

        // Idea if I want to get rid of allocations: pool vectors using stackalloced arrays
        var activePaths = new List<Caeden_Vector2Int>() { GetPositionForArrayIdx(startPosition) };
        visitedPositions[startPosition] = true;

        var steps = 0;
        var atEnd = false;
        while (true)
        {
            var beginningCount = activePaths.Count;
            for (var i = 0; i < beginningCount; i++)
            {
                var path = activePaths[0];

                bool TestNewDirection(ref Span<bool> visitedPositions, int addX, int addY)
                {
                    // Bounds checking
                    if (path.X + addX < 0 || path.Y + addY < 0 || path.X + addX >= xLength || path.Y + addY >= yLength) return false;

                    var hashedPosition = GetArrayIdxForPosition(path.X, path.Y);
                    var hashedNewPosition = GetArrayIdxForPosition(path.X + addX, path.Y + addY);

                    var strValue = inputStr[hashedPosition];
                    if (strValue == 'S') strValue = 'a';

                    var newStrValue = inputStr[hashedNewPosition];
                    if (newStrValue == 'E') newStrValue = 'z';

                    if (!visitedPositions[hashedNewPosition] && newStrValue - strValue <= 1)
                    {
                        if (hashedNewPosition == endPosition) return true;

                        visitedPositions[hashedNewPosition] = true;
                        activePaths!.Add(new(path.X + addX, path.Y + addY));
                    }
                    return false;
                }

                activePaths.RemoveAt(0);

                atEnd |= TestNewDirection(ref visitedPositions, 1, 0)
                    || TestNewDirection(ref visitedPositions, -1, 0)
                    || TestNewDirection(ref visitedPositions, 0, 1)
                    || TestNewDirection(ref visitedPositions, 0, -1);
            }

            steps++;
            if (atEnd) break;
        }

        return steps;
    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        var inputLines = _input.Lines;
        var inputStr = _input.Text;

        var xLength = inputLines[0].Length;
        var yLength = inputLines.Length;

        // Helper methods
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int GetArrayIdxForPosition(int x, int y) => (y * xLength) + x + (y * 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        Caeden_Vector2Int GetPositionForArrayIdx(int idx) => new(idx % (xLength + 2), idx / (xLength + 2));

        var startPosition = inputStr.IndexOf('S');
        var endPosition = inputStr.IndexOf('E');

        Span<bool> visitedPositions = stackalloc bool[inputStr.Length];
        var activePaths = new List<Caeden_Vector2Int>();

        for (var i = 0; i < inputStr.Length; i++)
        {
            if (inputStr[i] == 'a')
            {
                activePaths.Add(GetPositionForArrayIdx(i));
                visitedPositions[i] = true;
            }
        }

        var steps = 0;
        var atEnd = false;
        while (true)
        {
            var beginningCount = activePaths.Count;
            for (var i = 0; i < beginningCount; i++)
            {
                var path = activePaths[0];

                bool TestNewDirection(ref Span<bool> visitedPositions, int addX, int addY)
                {
                    // Bounds checking
                    if (path.X + addX < 0 || path.Y + addY < 0 || path.X + addX >= xLength || path.Y + addY >= yLength) return false;

                    var hashedPosition = GetArrayIdxForPosition(path.X, path.Y);
                    var hashedNewPosition = GetArrayIdxForPosition(path.X + addX, path.Y + addY);

                    var strValue = inputStr[hashedPosition];
                    if (strValue == 'S') strValue = 'a';

                    var newStrValue = inputStr[hashedNewPosition];
                    if (newStrValue == 'E') newStrValue = 'z';

                    if (!visitedPositions[hashedNewPosition] && newStrValue - strValue <= 1)
                    {
                        if (hashedNewPosition == endPosition) return true;

                        visitedPositions[hashedNewPosition] = true;
                        activePaths!.Add(new(path.X + addX, path.Y + addY));
                    }
                    return false;
                }

                activePaths.RemoveAt(0);

                atEnd |= TestNewDirection(ref visitedPositions, 1, 0)
                    || TestNewDirection(ref visitedPositions, -1, 0)
                    || TestNewDirection(ref visitedPositions, 0, 1)
                    || TestNewDirection(ref visitedPositions, 0, -1);
            }

            steps++;
            if (atEnd) break;
        }

        return steps;
    }

    // Data structures (modified from day 9)
    struct Caeden_Vector2Int
    {
        public int X;
        public int Y;

        public Caeden_Vector2Int(int x, int y, bool hashed = false)
        {
            if (!hashed)
            {
                X = x;
                Y = y;
            }
            else
            {

            }
        }
    }
    
    #endregion

    #region Eris

    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        ReadOnlySpan<string> terrainRowDataSpans = _input.Lines;
        var terrainHeight = terrainRowDataSpans.Length;
        var terrainWidth = terrainRowDataSpans[0].Length;
        var terrainSize = terrainHeight * terrainWidth;

        // var terrainData = GC.AllocateUninitializedArray<int>(terrainSize).AsSpan();
        Span<int> terrainData = stackalloc int[terrainSize];

        var startingIndex = Eris_PrepareDataAndFindStartingIndex(ref terrainRowDataSpans, terrainHeight, terrainWidth, terrainData, 'E');

        return Eris_FindShortestPathDescending(terrainData, terrainHeight, terrainWidth, terrainSize, startingIndex, 'S');
    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        ReadOnlySpan<string> terrainRowDataSpans = _input.Lines;
        var terrainHeight = terrainRowDataSpans.Length;
        var terrainWidth = terrainRowDataSpans[0].Length;
        var terrainSize = terrainHeight * terrainWidth;

        // var terrainData = GC.AllocateUninitializedArray<int>(terrainSize).AsSpan();
        Span<int> terrainData = stackalloc int[terrainSize];

        var startingIndex = Eris_PrepareDataAndFindStartingIndex(ref terrainRowDataSpans, terrainHeight, terrainWidth, terrainData, 'E');

        return Eris_FindShortestPathDescending(terrainData, terrainHeight, terrainWidth, terrainSize, startingIndex, 'a');
    }

    private static int Eris_PrepareDataAndFindStartingIndex(ref ReadOnlySpan<string> terrainDataRaw, int terrainHeight, int terrainWidth, scoped Span<int> terrainData, char startingChar)
    {
        var startingIndex = -1;
        var terrainIndex = 0;
        for (var rowIndex = 0; rowIndex < terrainHeight; rowIndex++)
        {
            var rawRowData = terrainDataRaw[rowIndex];
            for (var columnIndex = 0; columnIndex < terrainWidth; columnIndex++)
            {
                var terrainValue = rawRowData[columnIndex];
                if (terrainValue == startingChar)
                {
                    startingIndex = terrainIndex;
                }

                // Rewrite the start and end points data so it's just 1 lower/higher compared to the second lowest/highest point in the map
                terrainData[terrainIndex++] = Eris_RemapTerrainValue(terrainValue);
            }
        }

        return startingIndex;
    }

    // ReSharper disable once CognitiveComplexity
    private static int Eris_FindShortestPathDescending(ReadOnlySpan<int> terrainData, int terrainHeight, int terrainWidth, int terrainSize, int startingIndex, char endingChar)
    {
        var endingValue = Eris_RemapTerrainValue(endingChar);

        // According to actual testing against the puzzle input, 20 should be enough to reach the end
        // But given that the input is not guaranteed to be solvable within that buffer size, we'll use a much higher value
        // const int waveSearchBufferMaxSize = 20;
        var waveSearchBufferMaxSize = terrainHeight * 2 + terrainWidth * 2;

        // Queue-like stoof for BFS
        // Contains the indexes of map tiles that need to have their neighbours searched for the current waveStep
        var waveSearchBufferSize = 0;
        Span<int> waveSearchBuffer = stackalloc int[waveSearchBufferMaxSize];
        // Placeholder buffer for the next waveStep round checks
        var nextWaveSearchBufferSize = 0;
        Span<int> nextWaveSearchBuffer = stackalloc int[waveSearchBufferMaxSize];

        // Indicates whether a given index has already been visited
        Span<bool> terrainVisitedData = stackalloc bool[terrainSize];

        // Initialize the first waveSearchBuffer with the starting index
        waveSearchBuffer[waveSearchBufferSize++] = startingIndex;
        terrainVisitedData[startingIndex] = true;

        for (var currentWaveStep = 1;; currentWaveStep++)
        {
            for (var waveSearchBufferIndex = 0; waveSearchBufferIndex < waveSearchBufferSize; waveSearchBufferIndex++)
            {
                ref var terrainIndex = ref waveSearchBuffer[waveSearchBufferIndex];

                // Get the x-y coordinates for the current index
                var terrainRow = terrainIndex / terrainWidth;
                var terrainColumn = terrainIndex % terrainWidth;

                var terrainValue = terrainData[terrainIndex];
                var declineThreshold = terrainValue - 1;

                // Check top
                if (terrainRow > 0)
                {
                    var topIndex = terrainIndex - terrainWidth;
                    var topValue = terrainData[topIndex];

                    if (!terrainVisitedData[topIndex] && topValue >= declineThreshold)
                    {
                        if (topValue == endingValue)
                        {
                            return currentWaveStep;
                        }

                        terrainVisitedData[topIndex] = true;
                        nextWaveSearchBuffer[nextWaveSearchBufferSize++] = topIndex;
                    }
                }

                // Check bottom
                if (terrainRow < terrainHeight - 1)
                {
                    var bottomIndex = terrainIndex + terrainWidth;
                    var bottomValue = terrainData[bottomIndex];

                    if (!terrainVisitedData[bottomIndex] && bottomValue >= declineThreshold)
                    {
                        if (bottomValue == endingValue)
                        {
                            return currentWaveStep;
                        }

                        terrainVisitedData[bottomIndex] = true;
                        nextWaveSearchBuffer[nextWaveSearchBufferSize++] = bottomIndex;
                    }
                }

                // Check left
                if (terrainColumn > 0)
                {
                    var leftIndex = terrainIndex - 1;
                    var leftValue = terrainData[leftIndex];

                    if (!terrainVisitedData[leftIndex] && leftValue >= declineThreshold)
                    {
                        if (leftValue == endingValue)
                        {
                            return currentWaveStep;
                        }

                        terrainVisitedData[leftIndex] = true;
                        nextWaveSearchBuffer[nextWaveSearchBufferSize++] = leftIndex;
                    }
                }

                // Check right
                if (terrainColumn < terrainWidth - 1)
                {
                    var rightIndex = terrainIndex + 1;
                    var rightValue = terrainData[rightIndex];

                    if (!terrainVisitedData[rightIndex] && rightValue >= declineThreshold)
                    {
                        if (rightValue == endingValue)
                        {
                            return currentWaveStep;
                        }

                        terrainVisitedData[rightIndex] = true;
                        nextWaveSearchBuffer[nextWaveSearchBufferSize++] = rightIndex;
                    }
                }
            }

            // Copy the nextWaveSearchBuffer to the waveSearchBuffer so we can reuse it
            nextWaveSearchBuffer[..nextWaveSearchBufferSize].CopyTo(waveSearchBuffer);
            waveSearchBufferSize = nextWaveSearchBufferSize;
            nextWaveSearchBufferSize = 0;
        }
    }

    private static int Eris_RemapTerrainValue(char terrainValue) => terrainValue switch
    {
        'S' => 'a' - 1, // Start point is supposedly equal to a, but bc the algo searches from highest to lowest, we need to make it lower than the lowest point
        'E' => 'z', // End point height is equal to z
        _ => terrainValue
    };

    #endregion

    #region Goobie
    /*
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Goobie_Part1()
    {

    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Goobie_Part2()
    {

    }
    */
    #endregion
}