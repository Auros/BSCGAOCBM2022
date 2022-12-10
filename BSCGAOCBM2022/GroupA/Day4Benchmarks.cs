using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day4Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(4);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        var overlappingPairCount = _input.Lines.Count(line =>
        {
            var (larger, smaller) = Auros_GetSectionWorkPairs(line);
            return smaller.All(larger.Contains);
        });
        return overlappingPairCount;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        var overlappingPairRangeCount = _input.Lines.Count(line =>
        {
            var (larger, smaller) = Auros_GetSectionWorkPairs(line);
            return smaller.Any(larger.Contains);
        });
        return overlappingPairRangeCount;
    }

    static Auros_SectionWorkPair Auros_GetSectionWorkPairs(string line)
    {
        var rawPairs = line.Split(',');
        var rawFirst = rawPairs[0].Split('-');
        var rawSecond = rawPairs[1].Split('-');

        var firstStart = int.Parse(rawFirst[0]);
        var secondStart = int.Parse(rawSecond[0]);
        var firstEnd = int.Parse(rawFirst[1]);
        var secondEnd = int.Parse(rawSecond[1]);

        var firstSize = firstEnd - firstStart + 1;
        var secondSize = secondEnd - secondStart + 1;

        var firstSectionRange = Enumerable.Range(firstStart, firstSize);
        var secondSectionRange = Enumerable.Range(secondStart, secondSize);

        var larger = firstSize > secondSize ? firstSectionRange : secondSectionRange;
        var smaller = larger == secondSectionRange ? firstSectionRange : secondSectionRange;

        return new Auros_SectionWorkPair(larger, smaller);
    }

    readonly record struct Auros_SectionWorkPair(IEnumerable<int> Larger, IEnumerable<int> Smaller);

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var expandedPairs = _input.Text.Split(Environment.NewLine)
            .Select(pairs => pairs.Split(','))
            .Select(elf => elf
                .Select(range => range.Split('-'))
                .Select(endpoints => endpoints
                    .Select(endpointsStr => int.Parse(endpointsStr))
                    .ToArray())
                .Select(range => Enumerable.Range(range[0], range[1] - range[0] + 1))
                .ToArray());

        var rendundantPairs = expandedPairs
            .Where(section =>
                section[0].All(x => section[1].Contains(x))
                || section[1].All(x => section[0].Contains(x)));

        return expandedPairs.Count();
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        var expandedPairs = _input.Text.Split(Environment.NewLine)
            .Select(pairs => pairs.Split(','))
            .Select(elf => elf
                .Select(range => range.Split('-'))
                .Select(endpoints => endpoints
                    .Select(endpointsStr => int.Parse(endpointsStr))
                    .ToArray())
                .Select(range => Enumerable.Range(range[0], range[1] - range[0] + 1))
                .ToArray());

        var overlappingPairs = expandedPairs
            .Where(it =>
            it[0].Any(x => it[1].Contains(x))
            || it[1].Any(x => it[0].Contains(x)));

        return overlappingPairs.Count();
    }

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        return Eris_ParseAssetData().Count(tuples =>
        {
            var (first, second) = tuples;
            return Eris_IsFullyContainedIn(first, second);
        });
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        return Eris_ParseAssetData().Count(sections =>
        {
            var (first, second) = sections;
            return Eris_IsOverlapping(first, second);
        });
    }

    private IEnumerable<((int start, int end) first, (int start, int end) second)> Eris_ParseAssetData() =>
        _input.Enumerable
            .SelectMany(line => line
                .Split(',')
                .Select(y => y
                    .Split('-')
                    .Select(int.Parse)
                    .ToArray())
                .Select(sections => (start: sections.First(), end: sections.Last()))
                .Chunk(2))
            .Select(sections => (first: sections.First(), second: sections.Last()));

    private static bool Eris_IsContainedIn((int start, int end) a, (int start, int end) b) =>
        a.start >= b.start && a.end <= b.end;

    private static bool Eris_IsFullyContainedIn((int start, int end) a, (int start, int end) b) =>
        Eris_IsContainedIn(a, b) || Eris_IsContainedIn(b, a);

    private static bool Eris_IsOverlapping((int start, int end) a, (int start, int end) b) =>
        (a.start <= b.end && b.start <= a.end);

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