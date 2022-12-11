﻿using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupB;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day11Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(11);

    #region Auros

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part1)]
    public long Auros_Part1()
    {
        var input = _input.Lines;
        Span<Auros_Monkey> monkeys = stackalloc Auros_Monkey[(input.Length + 1) / 7];
        var holdingSum = Auros_CalculateHoldingSum(input, ref monkeys);
        Span<long> items = stackalloc long[holdingSum];
        Span<int> itemHolders = stackalloc int[holdingSum];

        Auros_ParseInput(input, ref monkeys, ref items, ref itemHolders);
        var monkeyBusinessPart1 = Auros_CalculateMonkeyBusiness(20, 3, ref monkeys, ref items, ref itemHolders);
        return monkeyBusinessPart1;
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part2)]
    public long Auros_Part2()
    {
        var input = _input.Lines;
        Span<Auros_Monkey> monkeys = stackalloc Auros_Monkey[(input.Length + 1) / 7];
        var holdingSum = Auros_CalculateHoldingSum(input, ref monkeys);
        Span<long> items = stackalloc long[holdingSum];
        Span<int> itemHolders = stackalloc int[holdingSum];

        Auros_ParseInput(input, ref monkeys, ref items, ref itemHolders);
        var monkeyBusinessPart2 = Auros_CalculateMonkeyBusiness(10_000, 1, ref monkeys, ref items, ref itemHolders);
        return monkeyBusinessPart2;
    }
    
    const int AUROS_TEST_OFFSET = 21;
    const int AUROS_TRUE_LOCATION = 29;
    const int AUROS_FALSE_LOCATION = 30;
    const int AUROS_OPERATION_LOCATION = 25;
    const int AUROS_STARTING_ITEMS_OFFSET = 18;
    const int AUROS_OPERATION_OPERATOR_LOCATION = 23;

    static int Auros_FastParseDoubleDigits(char tens, char ones) => Auros_FastParseSingleDigit(ones) + (Auros_FastParseSingleDigit(tens) * 10);

    static int Auros_FastParseSingleDigit(char ones) => ones - '0';

    static int Auros_FastParseDigits(ref ReadOnlySpan<char> inputChar)
    {
        bool doubleDigit = inputChar.Length == 2;
        if (doubleDigit)
            return Auros_FastParseDoubleDigits(inputChar[0], inputChar[1]);
        return Auros_FastParseSingleDigit(inputChar[0]);
    }

    static int Auros_CalculateHoldingSum(string[] input, ref Span<Auros_Monkey> monkeys)
    {
        int holdingSum = 0;
        int currentMonkey = 0;
        for (int i = 1; i < input.Length; i += 7)
        {
            var holding = (input[i].Length - AUROS_STARTING_ITEMS_OFFSET + 2) / 4; // Calculates how many starting items there are.
            monkeys[currentMonkey++].Holding = holding;
            holdingSum += holding;
        }
        return holdingSum;
    }

    static void Auros_ParseInput(string[] input, ref Span<Auros_Monkey> monkeys, ref Span<long> items, ref Span<int> itemHolders)
    {
        int nextItem = 0;
        for (int i = 0; i < monkeys.Length; i++)
        {
            var offset = i * 7;
            var startingItemsLine = input[++offset];
            var operationLine = input[++offset];
            var testLine = input[++offset];
            var trueLine = input[++offset];
            var falseLine = input[++offset];

            for (int c = AUROS_STARTING_ITEMS_OFFSET; c < startingItemsLine.Length; c += 4)
            {
                var item = Auros_FastParseDoubleDigits(startingItemsLine[c], startingItemsLine[c + 1]);
                itemHolders[nextItem] = i;
                items[nextItem++] = item;
            }

            var op = operationLine[AUROS_OPERATION_OPERATOR_LOCATION];
            var opValueRaw = operationLine[AUROS_OPERATION_LOCATION];
            var opValue = 0;
            if (opValueRaw is not 'o')
            {
                var opSpan = operationLine.AsSpan(AUROS_OPERATION_LOCATION);
                opValue = Auros_FastParseDigits(ref opSpan);
            }
            monkeys[i].Operation = op == '*' ? opValue * -1 : opValue;

            var testSpan = testLine.AsSpan(AUROS_TEST_OFFSET);
            monkeys[i].Test = Auros_FastParseDigits(ref testSpan);

            monkeys[i].True = Auros_FastParseSingleDigit(trueLine[AUROS_TRUE_LOCATION]);
            monkeys[i].False = Auros_FastParseSingleDigit(falseLine[AUROS_FALSE_LOCATION]);
        }
    }

    static long Auros_CalculateMonkeyBusiness(int rounds, int calmingEffect, ref Span<Auros_Monkey> monkeys, ref Span<long> items, ref Span<int> itemHolders)
    {
        int lcm = 1;
        for (int i = 0; i < monkeys.Length; i++)
            lcm *= monkeys[i].Test;

        for (int i = 0; i < rounds; i++)
        {
            for (int c = 0; c < monkeys.Length; c++)
            {
                var monkey = monkeys[c];
                for (int q = 0; q < items.Length && monkey.Holding is not 0; q++)
                {
                    // If the item we're looking for is not 
                    if (itemHolders[q] != c)
                        continue;

                    // Calculate the new worry level
                    var item = items[q];

                    item = monkey.Calculate(item) / calmingEffect % lcm;

                    // Perform test
                    var targetMonkeyIndex = item % monkey.Test is 0 ? monkey.True : monkey.False;
                    monkeys[targetMonkeyIndex].Holding++;
                    itemHolders[q] = targetMonkeyIndex;
                    monkey.Holding--;
                    monkey.Inspections++;
                    monkeys[c] = monkey;
                    items[q] = item;
                }
            }
        }

        int first = 0;
        int second = 0;

        for (int i = 0; i < monkeys.Length; i++)
        {
            var inspections = monkeys[i].Inspections;
            if (inspections > first)
            {
                second = first;
                first = inspections;
            }
            else if (inspections > second)
                second = inspections;
        }

        return (long)first * second;
    }

    struct Auros_Monkey
    {
        public int Holding;

        public int Operation;

        public int Test;

        public int True;

        public int False;

        public int Inspections;

        public long Calculate(long old)
        {
            return Operation switch
            {
                0 => old * old,
                > 0 => old + Operation,
                _ => old * Operation * -1
            };
        }
    }

    #endregion

    #region Caeden
    /*
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {

    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {

    }
    */
    #endregion

    #region Eris
    /*
    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {

    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {

    }
    */
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