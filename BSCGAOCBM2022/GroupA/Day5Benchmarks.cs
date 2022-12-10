using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Text;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day5Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(5);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public string Auros_Part1()
    {
        int maxStackWidthSize = 4;
        var stacksInput = _input.Lines.Where(i => i.Contains('['));

        var stacksInputSeparated = stacksInput.Select(s => s.Chunk(maxStackWidthSize).ToArray()).ToArray();

        int numberOfStacks = (int)Math.Ceiling((double)stacksInput.First().Length / maxStackWidthSize);

        Stack<char>[] stacks = new Stack<char>[numberOfStacks];
        for (int i = 0; i < stacks.Length; i++)
            stacks[i] = new Stack<char>();

        var instructionsInput = _input.Lines.Where(i => i.StartsWith("move")).Select(instructionInput =>
        {
            var splits = instructionInput.Split(' ');
            var quantity = int.Parse(splits[1]);
            var start = stacks[int.Parse(splits[3]) - 1];
            var end = stacks[int.Parse(splits[5]) - 1];
            return (quantity, start, end);
        });

        for (int i = stacksInputSeparated.Length - 1; i >= 0; i--)
        {
            var row = stacksInputSeparated[i];
            for (int c = 0; c < row.Length; c++)
            {
                var crate = row[c];
                var id = crate[1];
                if (id == ' ')
                    continue;

                stacks[c].Push(id);
            }
        }

        foreach (var (quantity, start, end) in instructionsInput)
        {
            for (int i = 0; i < quantity; i++)
            {
                var crate = start.Pop();
                end.Push(crate);
            }
        }

        string firstCode = new(stacks.Select(s => s.Pop()).ToArray());
        return firstCode;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Auros_Part2()
    {
        int maxStackWidthSize = 4;
        var stacksInput = _input.Lines.Where(i => i.Contains('['));

        var stacksInputSeparated = stacksInput.Select(s => s.Chunk(maxStackWidthSize).ToArray()).ToArray();

        int numberOfStacks = (int)Math.Ceiling((double)stacksInput.First().Length / maxStackWidthSize);

        Stack<char>[] stacks = new Stack<char>[numberOfStacks];
        for (int i = 0; i < stacks.Length; i++)
            stacks[i] = new Stack<char>();

        var instructionsInput = _input.Lines.Where(i => i.StartsWith("move")).Select(instructionInput =>
        {
            var splits = instructionInput.Split(' ');
            var quantity = int.Parse(splits[1]);
            var start = stacks[int.Parse(splits[3]) - 1];
            var end = stacks[int.Parse(splits[5]) - 1];
            return (quantity, start, end);
        });

        for (int i = stacksInputSeparated.Length - 1; i >= 0; i--)
        {
            var row = stacksInputSeparated[i];
            for (int c = 0; c < row.Length; c++)
            {
                var crate = row[c];
                var id = crate[1];
                if (id == ' ')
                    continue;

                stacks[c].Push(id);
            }
        }

        foreach (var (quantity, start, end) in instructionsInput)
        {
            var moving = start.Take(quantity).ToArray();
            for (int i = 0; i < quantity; i++)
                _ = start.Pop();

            for (int i = moving.Length - 1; i >= 0; i--)
                end.Push(moving[i]);
        }

        string code = new(stacks.Select(s => s.Pop()).ToArray());
        return code;
    }

    #endregion

    #region Caeden
    /*
    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public string Caeden_Part1()
    {

    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Caeden_Part2()
    {

    }
    */
    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public string Eris_Part1()
    {
        var (stacks, instructionSet) = ParseAssetData();
        foreach (var instruction in instructionSet)
        {
            for (var i = 0; i < instruction.amountToMove; i++)
            {
                var crate = stacks[instruction.fromStack].Pop();
                stacks[instruction.toStack].Push(crate);
            }
        }

        return CombineTopStacks(stacks);
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Eris_Part2()
    {
        var (stacks, instructionSet) = ParseAssetData();
        foreach (var instruction in instructionSet)
        {
            var moveBuffer = new Stack<char>(instruction.amountToMove);
            for (var i = 0; i < instruction.amountToMove; i++)
            {
                var crate = stacks[instruction.fromStack].Pop();
                moveBuffer.Push(crate);
            }

            for (var i = 0; i < instruction.amountToMove; i++)
            {
                var crate = moveBuffer.Pop();
                stacks[instruction.toStack].Push(crate);
            }
        }

        return CombineTopStacks(stacks);
    }

    private (List<Stack<char>> stacks, IEnumerable<(int amountToMove, int fromStack, int toStack)> instructionSet) ParseAssetData()
    {
        var data = _input.Enumerable.ToList();
        var dataSeparatorIndex = data.FindIndex(string.IsNullOrWhiteSpace);

        var stacks = new List<Stack<char>>();
        var stackLayerData = data.GetRange(0, dataSeparatorIndex - 1);
        stackLayerData.Reverse();
        foreach (var stackLayer in stackLayerData)
        {
            stackLayer
                .ToCharArray()
                .Chunk(4)
                .Select((crateInfo, index) => (crate: crateInfo[1], Index: index))
                .ToList()
                .ForEach((crateInfo) =>
                {
                    var (crate, index) = crateInfo;

                    var crateStack = stacks.ElementAtOrDefault(index);
                    if (crateStack == null)
                    {
                        crateStack = new Stack<char>();
                        stacks.Add(crateStack);
                    }

                    if (!char.IsWhiteSpace(crate))
                    {
                        crateStack.Push(crate);
                    }
                });
        }

        var instructionSet = data.GetRange(dataSeparatorIndex + 1, data.Count - dataSeparatorIndex - 1)
            .Select(instruction => instruction.Split(' '))
            .Select(instructionParts => (
                amountToMove: int.Parse(instructionParts[1]),
                fromStack: int.Parse(instructionParts[3]) - 1,
                toStack: int.Parse(instructionParts[5]) - 1));

        return (stacks, instructionSet);
    }

    private static string CombineTopStacks(List<Stack<char>> stacks)
    {
        var stringBuilder = new StringBuilder();
        foreach (var stack in stacks)
        {
            stringBuilder.Append(stack.Peek());
        }

        return stringBuilder.ToString();
    }

    #endregion

    #region Goobie
    /*
    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public string Goobie_Part1()
    {

    }

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Goobie_Part2()
    {

    }
    */
    #endregion
}