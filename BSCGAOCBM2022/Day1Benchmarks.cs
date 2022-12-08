using BenchmarkDotNet.Attributes;

namespace BSCGAOCBM2022;

[MemoryDiagnoser]
public class Day1Benchmarks
{
    private string _inputText = null!;
    private string[] _inputLines = null!;

    [GlobalSetup]
    public void Setup()
    {
        _inputText = File.ReadAllText(@"Input\1.txt");
        _inputLines = File.ReadAllLines(@"Input\1.txt");
    }

    [Benchmark]
    public int? Auros_Part1()
    {
        List<Auros.Day1_Elf> elves = new();
        Auros.Day1_Elf? currentElf = null;
        foreach (var line in _inputLines)
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
                currentElf = new Auros.Day1_Elf();
                elves.Add(currentElf);
            }

            // Parse the calories as an integer.
            int calories = int.Parse(line);

            // Add the calories to the current elf.
            currentElf.AddCalories(calories);
        }

        // Find the elf with the most calories
        Auros.Day1_Elf? mostCaloricElf = null;
        for (int i = 0; i < elves.Count; i++)
        {
            var elf = elves[i];
            if (mostCaloricElf is null || elf.Calories > mostCaloricElf.Calories)
                mostCaloricElf = elf;
        }

        return mostCaloricElf?.Calories;
    }

    [Benchmark]
    public int Auros_Part2()
    {
        List<Auros.Day1_Elf> elves = new();
        Auros.Day1_Elf? currentElf = null;
        foreach (var line in _inputLines)
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
                currentElf = new Auros.Day1_Elf();
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

    [Benchmark]
    public int Caeden_Part1()
    {
        var totalCaloriesPerElf = _inputText
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(it => it.Split(Environment.NewLine).Where(it => !string.IsNullOrWhiteSpace(it)))
            .Select(it => it.Select(str => int.Parse(str)).Sum())
            .OrderByDescending(it => it)
            .ToList();

        return totalCaloriesPerElf.Max();
    }

    [Benchmark]
    public int Caeden_Part2()
    {
        var totalCaloriesPerElf = _inputText
            .Split(Environment.NewLine + Environment.NewLine)
            .Select(it => it.Split(Environment.NewLine).Where(it => !string.IsNullOrWhiteSpace(it)))
            .Select(it => it.Select(str => int.Parse(str)).Sum())
            .OrderByDescending(it => it)
            .ToList();

        return totalCaloriesPerElf.Take(3).Sum();
    }

    [Benchmark]
    public uint Eris_Part1()
    {
        return Eris_ParseCaloriesData().Max();
    }

    [Benchmark]
    public long Eris_Part2()
    {
        return Eris_ParseCaloriesData().OrderDescending().Take(3).Sum(x => x);
    }

    private IEnumerable<uint> Eris_ParseCaloriesData()
    {
        var caloriesList = new List<uint> { 0 };
        foreach (var calorieRaw in _inputLines)
        {
            if (string.IsNullOrWhiteSpace(calorieRaw))
            {
                caloriesList.Add(0);
            }
            else
            {
                caloriesList[^1] += uint.Parse(calorieRaw);
            }
        }
        return caloriesList;
    }
}