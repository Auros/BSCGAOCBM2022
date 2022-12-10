using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day3Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(3);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        var input = _input.Lines;
        var priorityLookup = Enumerable.Range(97, 26).Union(Enumerable.Range(65, 26)).ToList();

        int GetPriority(char item) => item is char.MinValue ? 0 : priorityLookup.IndexOf(item) + 1;

        var rucksacks = input.Select(content => new Auros_Rucksack(content));
        var rucksackUniqueSum = rucksacks.Sum(r => GetPriority(r.FirstBag.Intersect(r.SecondBag).FirstOrDefault()));

        return rucksackUniqueSum;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        var input = _input.Lines;
        var priorityLookup = Enumerable.Range(97, 26).Union(Enumerable.Range(65, 26)).ToList();

        int GetPriority(char item) => item is char.MinValue ? 0 : priorityLookup.IndexOf(item) + 1;

        var elfGroups = input.Chunk(3).Select(contents => new Auros_ElfGroup(contents));
        var elfGroupUniqueSum = elfGroups.Sum(g => GetPriority(g.Elf1.Intersect(g.Elf2).Intersect(g.Elf3).First()));

        return elfGroupUniqueSum;
    }

    internal class Auros_Rucksack
    {
        public IReadOnlyList<char> FirstBag { get; }

        public IReadOnlyList<char> SecondBag { get; }

        public Auros_Rucksack(string content)
        {
            var chunks = content.Chunk(content.Length / 2);
            FirstBag = chunks.First();
            SecondBag = chunks.Last();
        }
    }

    internal class Auros_ElfGroup
    {
        public IReadOnlyList<char> Elf1 { get; }
        public IReadOnlyList<char> Elf2 { get; }
        public IReadOnlyList<char> Elf3 { get; }

        public Auros_ElfGroup(IEnumerable<string> elfContent)
        {
            var elves = elfContent.ToArray();
            Elf1 = elves[0].ToList();
            Elf2 = elves[1].ToList();
            Elf3 = elves[2].ToList();
        }
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        const string prioritiesTable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var rucksacks = _input.Text.Split(Environment.NewLine).ToList();

        var sumPriorities = rucksacks
            .Select(rucksack =>
            {
                var rucksackSize = rucksack.Length / 2;
                return new[] { rucksack[0..rucksackSize], rucksack[rucksackSize..] };
            })
            .SelectMany(compartment => compartment[0].Intersect(compartment[1]))
            .Sum(sharedItem => prioritiesTable.IndexOf(sharedItem) + 1);

        return sumPriorities;
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        const string prioritiesTable = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        var rucksacks = _input.Text.Split(Environment.NewLine).ToList();

        var sumGroupProorities = rucksacks
            .GroupBy(rucksack => rucksacks.IndexOf(rucksack) / 3)
            .Select(group => group.ToArray())
            .Where(group => group.Length == 3)
            .SelectMany(group => group[0].Intersect(group[1]).Intersect(group[2]))
            .Sum(groupBadge => prioritiesTable.IndexOf(groupBadge) + 1);

        return sumGroupProorities;
    }

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public uint Eris_Part1()
    {
        return _input.Enumerable.Aggregate<string, uint>(0, (acc, ruckSack) =>
         {
             var ruckSackCompartimentLength = ruckSack.Length / 2;
             var ruckSackCompartimentA = ruckSack[..ruckSackCompartimentLength];
             var ruckSackCompartimentB = ruckSack[ruckSackCompartimentLength..];
             return acc + Eris_FindDuplicatesAndSumReduce(ruckSackCompartimentA, ruckSackCompartimentB);
         });
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public uint Eris_Part2()
    {
        return _input.Enumerable
            .Chunk(3)
            .Aggregate<string[], uint>(0, (acc, bags) => acc + Eris_FindDuplicatesAndSumReduce(bags));
    }

    private static uint Eris_FindDuplicatesAndSumReduce(params string[] dataSets)
    {
        var duplicates = dataSets[0].Intersect(dataSets[1]);
        for (var i = 2; i < dataSets.Length; i++)
        {
            duplicates = duplicates.Intersect(dataSets[i]);
        }

        return duplicates.Aggregate<char, uint>(0, (acc, c) => acc + Eris_ConvertToPriority(c));
    }

    private static uint Eris_ConvertToPriority(char c)
    {
        const int asciiUpperCaseOffset = 64;
        const int asciiLowerCaseOffset = 96;
        const int additionalAsciiOffset = 26;

        if (char.IsAsciiLetterUpper(c))
        {
            return (uint)(c - asciiUpperCaseOffset + additionalAsciiOffset);
        }

        if (char.IsAsciiLetterLower(c))
        {
            return (uint)(c - asciiLowerCaseOffset);
        }

        throw new ArgumentException("Invalid character");
    }

    #endregion

    #region Goobie

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Goobie_Part1()
    {
        int priorities = 0;
        foreach (var line in _input.Lines)
        {
            Goobie_Items meewewew = Goobie_Items.cumm;
            int length = line.Length;
            int length2 = length / 2;

            for (int i = 0; i < length2; i++)
            {
                int meow;
                if (line[i] < 91)
                    meow = line[i] - 39;
                else
                    meow = line[i] - 97;

                Goobie_Items mewww = (Goobie_Items)((ulong)0x1 << meow);
                meewewew |= mewww;
            }

            for (int i = length2; i < length; i++)
            {
                int meow;
                if (line[i] < 91)
                    meow = line[i] - 39;
                else
                    meow = line[i] - 97;

                Goobie_Items mewww = (Goobie_Items)((ulong)0x1 << meow);
                if (meewewew.HasFlag(mewww))
                {
                    priorities += meow + 1;
                    break;
                }
            }
        }
        return priorities;
    }

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Goobie_Part2()
    {
        var lines = _input.Lines;
        int priorities2 = 0;
        for (int i = 0; i < lines.Length / 3; i++)
        {
            Goobie_Items mewwwww = Goobie_Items.All;
            for (int x = 0; x < 3; x++)
            {
                var line = lines[i * 3 + x];
                Goobie_Items hhhhh = Goobie_Items.cumm;

                foreach (var character in line)
                {
                    int meow;
                    if (character < 91)
                        meow = character - 39;
                    else
                        meow = character - 97;

                    Goobie_Items graaaaah = (Goobie_Items)((ulong)0x1 << meow);
                    if (x == 2 && mewwwww.HasFlag(graaaaah))
                    {
                        priorities2 += meow + 1;
                        break;
                    }

                    hhhhh |= graaaaah;
                }
                mewwwww &= hhhhh;
            }
        }
        return priorities2;
    }

    [Flags]
    internal enum Goobie_Items : ulong
    {
        cumm = 0,
        a = 1,
        b = 2,
        c = 4,
        d = 8,
        e = 16,
        f = 32,
        g = 64,
        h = 128,
        i = 256,
        j = 512,
        k = 1024,
        l = 2048,
        m = 4096,
        n = 8192,
        o = 16384,
        p = 32768,
        q = 65536,
        r = 131072,
        s = 262144,
        t = 524288,
        u = 1048576,
        v = 2097152,
        w = 4194304,
        x = 8388608,
        y = 16777216,
        z = 33554432,
        A = 67108864,
        B = 134217728,
        C = 268435456,
        D = 536870912,
        E = 1073741824,
        F = 2147483648,
        G = 4294967296,
        H = 8589934592,
        I = 17179869184,
        J = 34359738368,
        K = 68719476736,
        L = 137438953472,
        M = 274877906944,
        N = 549755813888,
        O = 1099511627776,
        P = 2199023255552,
        Q = 4398046511104,
        R = 8796093022208,
        S = 17592186044416,
        T = 35184372088832,
        U = 70368744177664,
        V = 140737488355328,
        W = 281474976710656,
        X = 562949953421312,
        Y = 1125899906842624,
        Z = 2251799813685248,
        All = 4_503_599_627_370_496
    }

    #endregion
}