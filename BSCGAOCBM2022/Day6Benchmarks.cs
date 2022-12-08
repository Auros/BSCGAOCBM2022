using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BSCGAOCBM2022;

[MemoryDiagnoser]
public class Day6Benchmarks
{
    private string _inputText = null!;
    private string[] _inputLines = null!;
    private MemoryStream _inputStream = null!;
    private IEnumerable<string> _inputEnumerable = null!;

    [GlobalSetup]
    public void Setup()
    {
        _inputText = File.ReadAllText(@"Input\6.txt");
        _inputLines = File.ReadAllLines(@"Input\6.txt");
        _inputEnumerable = _inputLines.AsEnumerable();
        _inputStream = new MemoryStream(Encoding.UTF8.GetBytes(_inputText));
    }

    [Benchmark]
    public int? Auros_Part1()
    {
        int? GetMarker(char[] buffer)
        {
            int? index = null;
            for (int i = 0; i < _inputText.Length - buffer.Length; i++)
            {
                if (buffer.Distinct().Count() == buffer.Length)
                {
                    index = i + buffer.Length;
                    break;
                }

                for (int c = 0; c < buffer.Length; c++)
                    buffer[c] = _inputText[i + c + 1];
            }
            return index;
        }

        char[] buffer = _inputText.Take(4).ToArray();
        int? index = GetMarker(buffer);
        return index;
    }

    [Benchmark]
    public int? Auros_Part2()
    {
        int? GetMarker(char[] buffer)
        {
            int? index = null;
            for (int i = 0; i < _inputText.Length - buffer.Length; i++)
            {
                if (buffer.Distinct().Count() == buffer.Length)
                {
                    index = i + buffer.Length;
                    break;
                }

                for (int c = 0; c < buffer.Length; c++)
                    buffer[c] = _inputText[i + c + 1];
            }
            return index;
        }

        char[] buffer = _inputText.Take(14).ToArray();
        int? index = GetMarker(buffer);
        return index;
    }

    [Benchmark]
    public int Caeden_Part1()
    {
        const int messageLength = 4;
        var totalChars = 0;
        var currentChar = 0;
        var bitfield = 0u;
        var i = 0;
        Span<int> processing = stackalloc int[(int)_inputStream.Length];

        while ((currentChar = _inputStream.ReadByte()) > -1)
        {
            if (currentChar > 'z') continue;
            processing[totalChars] = currentChar;
            totalChars++;
            if (totalChars >= messageLength)
            {
                bitfield = 0;

                for (i = totalChars - messageLength; i < totalChars; i++) bitfield |= 1u << (processing[i] - 'a');

                if (System.Numerics.BitOperations.PopCount(bitfield) == messageLength) break;
            }
        }

        return totalChars;
    }

    [Benchmark]
    public int Caeden_Part2()
    {
        const int messageLength = 14;
        var totalChars = 0;
        var currentChar = 0;
        var bitfield = 0u;
        var i = 0;
        Span<int> processing = stackalloc int[(int)_inputStream.Length];

        while ((currentChar = _inputStream.ReadByte()) > -1)
        {
            if (currentChar > 'z') continue;
            processing[totalChars] = currentChar;
            totalChars++;
            if (totalChars >= messageLength)
            {
                bitfield = 0;

                for (i = totalChars - messageLength; i < totalChars; i++) bitfield |= 1u << (processing[i] - 'a');

                if (System.Numerics.BitOperations.PopCount(bitfield) == messageLength) break;
            }
        }

        return totalChars;
    }

    [Benchmark]
    public int Eris_Part1()
    {
        return Eris_Check(4);
    }

    [Benchmark]
    public int Eris_Part2()
    {
        return Eris_Check(14);
    }

    private int Eris_Check(int markerSize)
    {
        var dataStreamSpan = _inputText.AsSpan();

        var i = 0;
        var buffer = new char[markerSize];
        do
        {
            dataStreamSpan.Slice(i, markerSize).CopyTo(buffer);
            if (Eris_HasUniqueCharacters(buffer))
            {
                break;
            }

            i++;
        } while (i < dataStreamSpan.Length - markerSize);

        return i + markerSize;
    }

    private static bool Eris_HasUniqueCharacters(char[] input)
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
}
