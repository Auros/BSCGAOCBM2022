using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day1Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(1);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int? Auros_Part1()
    {
        List<Auros_Elf> elves = new();
        Auros_Elf? currentElf = null;
        foreach (var line in _input.Lines)
        {
            // Once we reach the end of a grouping of calories,
            // unassign the current elf. The ending is represented
            // as an empty line.
            if (line == string.Empty)
            {
                currentElf = null;
                continue;
            }

            // If we have a calorie value (this line isn't empty) and there isn't
            // an elf assign, we assign it and add it to the collection of elves.
            if (currentElf is null)
            {
                currentElf = new Auros_Elf();
                elves.Add(currentElf);
            }

            // Parse the calories as an integer.
            int calories = int.Parse(line);

            // Add the calories to the current elf.
            currentElf.AddCalories(calories);
        }

        // Find the elf with the most calories
        Auros_Elf? mostCaloricElf = null;
        for (int i = 0; i < elves.Count; i++)
        {
            var elf = elves[i];
            if (mostCaloricElf is null || elf.Calories > mostCaloricElf.Calories)
                mostCaloricElf = elf;
        }

        return mostCaloricElf?.Calories;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        List<Auros_Elf> elves = new();
        Auros_Elf? currentElf = null;
        foreach (var line in _input.Lines)
        {
            // Once we reach the end of a grouping of calories,
            // unassign the current elf. The ending is represented
            // as an empty line.
            if (line == string.Empty)
            {
                currentElf = null;
                continue;
            }

            // If we have a calorie value (this line isn't empty) and there isn't
            // an elf assign, we assign it and add it to the collection of elves.
            if (currentElf is null)
            {
                currentElf = new Auros_Elf();
                elves.Add(currentElf);
            }

            // Parse the calories as an integer.
            int calories = int.Parse(line);

            // Add the calories to the current elf.
            currentElf.AddCalories(calories);
        }

        const int topElvesCount = 3;
        int topCalorieCount = default;

        // Sort the elves from most amount of calories to least.
        elves.Sort((a, b) => b.Calories - a.Calories);

        // Add the top X elves calorie counts. 
        for (int i = 0; i < topElvesCount || i >= elves.Count; i++)
            topCalorieCount += elves[i].Calories;

        return topCalorieCount;
    }

    internal class Auros_Elf
    {
        public int Calories { get; private set; }

        public void AddCalories(int calories) => Calories += calories;
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var totalCaloriesPerElf = _input.Text
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(it => it.Split(Environment.NewLine).Where(it => !string.IsNullOrWhiteSpace(it)))
            .Select(it => it.Select(str => int.Parse(str)).Sum())
            .OrderByDescending(it => it)
            .ToList();

        return totalCaloriesPerElf.Max();
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        var totalCaloriesPerElf = _input.Text
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(it => it.Split(Environment.NewLine).Where(it => !string.IsNullOrWhiteSpace(it)))
            .Select(it => it.Select(str => int.Parse(str)).Sum())
            .OrderByDescending(it => it)
            .ToList();

        return totalCaloriesPerElf.Take(3).Sum();
    }

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public uint Eris_Part1()
    {
        return Eris_ParseCaloriesData().Max();
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public long Eris_Part2()
    {
        return Eris_ParseCaloriesData().OrderDescending().Take(3).Sum(x => x);
    }

    private IEnumerable<uint> Eris_ParseCaloriesData()
    {
        var currentCalories = 0u;
        foreach (var calorieRaw in _input.Enumerable)
        {
            if (string.IsNullOrWhiteSpace(calorieRaw))
            {
                yield return currentCalories;
                currentCalories = 0;
            }
            else
            {
                currentCalories += uint.Parse(calorieRaw);
            }
        }
    }

    #endregion

    #region Goobie

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Goobie_Part1()
    {
        var elves = new List<Goobie_Elf>();

        Goobie_Elf elf = new();
        foreach (var line in _input.Lines)
        {
            if (line == "")
            {
                elves.Add(elf);
                elf = new();
                continue;
            }

            var meow = int.Parse(line);
            elf.foods.Add(meow);
        }

        elves.Sort((a, b) =>
        {
            return b.Calories() - a.Calories();
        });

        return elves.First().Calories();
    }

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Goobie_Part2()
    {
        var elves = new List<Goobie_Elf>();

        Goobie_Elf elf = new();
        foreach (var line in _input.Lines)
        {
            if (line == "")
            {
                elves.Add(elf);
                elf = new();
                continue;
            }

            var meow = int.Parse(line);
            elf.foods.Add(meow);
        }

        elves.Sort((a, b) =>
        {
            return b.Calories() - a.Calories();
        });

        return elves[0].Calories() + elves[1].Calories() + elves[2].Calories();
    }

    public class Goobie_Elf
    {
        public List<int> foods { get; set; } = new List<int>();

        public int Calories() => foods.Aggregate((a, b) => a + b);
    }

    #endregion
}