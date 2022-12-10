using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Text;

namespace BSCGAOCBM2022.GroupB;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day10enchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(10);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        int reader = 0;
        int current = 1;
        Span<int> register = stackalloc int[_input.Lines.Length * 2]; // Maximum possible size since each instruction can use at most two cycles. 

        foreach (var line in _input.Lines)
        {
            if (line[0] is 'n')
            {
                // noop

                // Consume one cycle and don't do anything
                register[reader++] = current;
            }
            else
            {
                // addx
                bool negative = line[5] == '-';
                bool doubleDigit = line.Length == 8 && negative || line.Length == 7 && !negative;
                var value = line[^1] - '0';
                if (doubleDigit)
                    value += (line[^2] - '0') * 10;

                if (negative)
                    value *= -1;

                // Consume two cycles and assign the incremented register on the second.
                register[reader++] = current;
                register[reader++] = current += value;
            }
        }

        int sum = 0;
        for (int i = 19; i < 220; i += 40)
            sum += register[i - 1] * (i + 1); // This doesn't feel right...

        return sum;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Auros_Part2()
    {
        int reader = 0;
        int current = 1;
        Span<int> register = stackalloc int[_input.Lines.Length * 2]; // Maximum possible size since each instruction can use at most two cycles. 

        foreach (var line in _input.Lines)
        {
            if (line[0] is 'n')
            {
                // noop

                // Consume one cycle and don't do anything
                register[reader++] = current;
            }
            else
            {
                // addx
                bool negative = line[5] == '-';
                bool doubleDigit = line.Length == 8 && negative || line.Length == 7 && !negative;
                var value = line[^1] - '0';
                if (doubleDigit)
                    value += (line[^2] - '0') * 10;

                if (negative)
                    value *= -1;

                // Consume two cycles and assign the incremented register on the second.
                register[reader++] = current;
                register[reader++] = current += value;
            }
        }

        int currentRow = 0;
        int currentCrt = 0;
        Span<char> crt = stackalloc char[247]; // :tf:
        crt[currentCrt++] = register[0] == 1 || register[0] == 0 ? '#' : '.';
        for (int i = 1; i < reader; i++)
        {
            if (i % 40 == 0)
            {
                crt[currentCrt++] = '\n';
                currentRow += 40;
            }

            var value = register[i - 1] + currentRow;
            crt[currentCrt++] = (value == i || value + 1 == i || value - 1 == i) ? '#' : '.';
        }

        return new string(crt);
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var inputLength = _input.Lines.Length;

        Span<int> xRegOverTime = stackalloc int[inputLength * 2];
        Span<int> signalStrengthOverTime = stackalloc int[inputLength * 2];

        var clockCycle = 1;
        var xReg = 1;

        for (int i = 0; i < inputLength; i++)
        {
            if (_input.Lines[i][0] == 'a')
            {
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);

                // Fuck int.Parse!!
                var isNegative = false;
                var result = 0;
                for (var j = 5; j < _input.Lines[i].Length; j++)
                {
                    switch (_input.Lines[i][j])
                    {
                        case '-':
                            isNegative = true;
                            break;
                        default:
                            result *= 10;
                            result += _input.Lines[i][j] - '0';
                            break;
                    }
                }
                xReg += result * (isNegative ? -1 : 1);
            }
            else
            {
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);
            }
        }

        var signalStrengthSum = signalStrengthOverTime[20]
            + signalStrengthOverTime[60]
            + signalStrengthOverTime[100]
            + signalStrengthOverTime[140]
            + signalStrengthOverTime[180]
            + signalStrengthOverTime[220];

        return signalStrengthSum;
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Caeden_Part2()
    {
        var inputLength = _input.Lines.Length;

        Span<int> xRegOverTime = stackalloc int[inputLength * 2];
        Span<int> signalStrengthOverTime = stackalloc int[inputLength * 2];

        var clockCycle = 1;
        var xReg = 1;

        for (int i = 0; i < inputLength; i++)
        {
            if (_input.Lines[i][0] == 'a')
            {
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);

                // Fuck int.Parse!!
                var isNegative = false;
                var result = 0;
                for (var j = 5; j < _input.Lines[i].Length; j++)
                {
                    switch (_input.Lines[i][j])
                    {
                        case '-':
                            isNegative = true;
                            break;
                        default:
                            result *= 10;
                            result += _input.Lines[i][j] - '0';
                            break;
                    }
                }
                xReg += result * (isNegative ? -1 : 1);
            }
            else
            {
                IncrementClockCycle(in xRegOverTime, in signalStrengthOverTime, in xReg, ref clockCycle);
            }
        }

        var sb = new StringBuilder(240 + (240 / 40));
        for (int i = 0; i < 240; i++)
        {
            var cycle = i % 40;
            var diff = xRegOverTime[i + 1] - cycle;
            sb.Append((diff <= 1 && diff >= -1) ? '#' : '.');
            if (cycle == 39) sb.AppendLine();
        }

        return sb.ToString();
    }

    static void IncrementClockCycle(in Span<int> xRegSpan, in Span<int> signalStrengthSpan, in int xReg, ref int clockCycle)
    {
        xRegSpan[clockCycle] = xReg;
        signalStrengthSpan[clockCycle] = clockCycle * xReg;
        clockCycle++;
    }

    #endregion

    #region Eris
    /*
    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {

    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public string Eris_Part2()
    {

    }
    */
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
    public string Goobie_Part2()
    {

    }
    */
    #endregion
}