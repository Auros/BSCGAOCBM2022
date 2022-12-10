using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day6Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(6);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int? Auros_Part1()
    {
        int? GetMarker(char[] buffer)
        {
            int? index = null;
            for (int i = 0; i < _input.Text.Length - buffer.Length; i++)
            {
                if (buffer.Distinct().Count() == buffer.Length)
                {
                    index = i + buffer.Length;
                    break;
                }

                for (int c = 0; c < buffer.Length; c++)
                    buffer[c] = _input.Text[i + c + 1];
            }
            return index;
        }

        char[] buffer = _input.Text.Take(4).ToArray();
        int? index = GetMarker(buffer);
        return index;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int? Auros_Part2()
    {
        int? GetMarker(char[] buffer)
        {
            int? index = null;
            for (int i = 0; i < _input.Text.Length - buffer.Length; i++)
            {
                if (buffer.Distinct().Count() == buffer.Length)
                {
                    index = i + buffer.Length;
                    break;
                }

                for (int c = 0; c < buffer.Length; c++)
                    buffer[c] = _input.Text[i + c + 1];
            }
            return index;
        }

        char[] buffer = _input.Text.Take(14).ToArray();
        int? index = GetMarker(buffer);
        return index;
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        const int messageLength = 4;
        var totalChars = 0;
        var currentChar = 0;
        var bitfield = 0u;
        var i = 0;
        Span<int> processing = stackalloc int[_input.Bytes.Length];

        for (int z = 0; z < _input.Bytes.Length; z++)
        {
            currentChar = _input.Bytes[z];

            if (currentChar > 'z') continue;
            processing[totalChars] = currentChar;
            totalChars++;
            if (totalChars >= messageLength)
            {
                bitfield = 0;

                for (i = totalChars - messageLength; i < totalChars; i++) bitfield |= 1u << processing[i] - 'a';

                if (System.Numerics.BitOperations.PopCount(bitfield) == messageLength) break;
            }
        }

        return totalChars;
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        const int messageLength = 14;
        var totalChars = 0;
        var currentChar = 0;
        var bitfield = 0u;
        var i = 0;
        Span<int> processing = stackalloc int[_input.Bytes.Length];

        for (int z = 0; z < _input.Bytes.Length; z++)
        {
            currentChar = _input.Bytes[z];

            if (currentChar > 'z') continue;
            processing[totalChars] = currentChar;
            totalChars++;
            if (totalChars >= messageLength)
            {
                bitfield = 0;

                for (i = totalChars - messageLength; i < totalChars; i++) bitfield |= 1u << processing[i] - 'a';

                if (System.Numerics.BitOperations.PopCount(bitfield) == messageLength) break;
            }
        }

        return totalChars;
    }

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        return Eris_Check(4);
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        return Eris_Check(14);
    }

    private int Eris_Check(int markerSize)
    {
        var dataStreamSpan = _input.Text.AsSpan();

        var i = 0;
        do
        {
            var bufferAsSpan = dataStreamSpan.Slice(i, markerSize);
            if (Eris_HasUniqueCharacters(ref bufferAsSpan))
            {
                break;
            }

            i++;
        } while (i < dataStreamSpan.Length - markerSize);

        return i + markerSize;
    }

    private static bool Eris_HasUniqueCharacters(ref ReadOnlySpan<char> input)
    {
        for (var i = 0; i < input.Length; i++)
            for (var j = i + 1; j < input.Length; j++)
            {
                if (input[i] == input[j])
                {
                    return false;
                }
            }

        return true;
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
