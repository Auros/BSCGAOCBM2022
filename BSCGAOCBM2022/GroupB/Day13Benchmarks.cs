using System.Text.Json;
using BenchmarkDotNet.Attributes;

namespace BSCGAOCBM2022.GroupB;

public class Day13Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(13);

    #region Auros

    /*[Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
    }

    [Benchmark(Baseline = true)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
    }*/
    
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

    [Benchmark]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        ReadOnlySpan<string> packetDataRaw = _input.Lines;

        var packetsInOrderIndicesSum = 0;
        for (var i = 0; i < packetDataRaw.Length; i += 3)
        {
            var leftPacket = packetDataRaw[i];
            var rightPacket = packetDataRaw[i + 1];

            var leftPacketParsed = JsonDocument.Parse(leftPacket).RootElement;
            var rightPacketParsed = JsonDocument.Parse(rightPacket).RootElement;

            if (Eris_ComparePackets(leftPacketParsed, rightPacketParsed) < 0)
            {
                packetsInOrderIndicesSum += i / 3 + 1;
            }
        }

        return packetsInOrderIndicesSum;
    }

    [Benchmark]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        var dividerPacket1 = JsonDocument.Parse("[[2]]").RootElement;
        var dividerPacket2 = JsonDocument.Parse("[[6]]").RootElement;

        ReadOnlySpan<string> packetDataRaw = _input.Lines;

        var totalPacketCount = ((packetDataRaw.Length / 3) + 1) * 2 + 2;
        var packetData = new JsonElement[totalPacketCount];

        var packetDataIndex = 0;
        for (var i = 0; i < packetDataRaw.Length; i += 3)
        {
            packetData[packetDataIndex++] = JsonDocument.Parse(packetDataRaw[i]).RootElement;
            packetData[packetDataIndex++] = JsonDocument.Parse(packetDataRaw[i + 1]).RootElement;
        }

        packetData[packetDataIndex++] = dividerPacket1;
        packetData[packetDataIndex] = dividerPacket2;

        Array.Sort(packetData, Eris_PacketComparison);

        var dividerPacket1Index = Array.IndexOf(packetData, dividerPacket1) + 1;
        var dividerPacket2Index = Array.IndexOf(packetData, dividerPacket2) + 1;

        return dividerPacket1Index * dividerPacket2Index;
    }

    private static readonly Comparison<JsonElement> Eris_PacketComparison = Eris_ComparePackets;

    // ReSharper disable once CognitiveComplexity
    private static int Eris_ComparePackets(JsonElement leftPacket, JsonElement rightPacket)
    {
        while (true)
        {
            if (leftPacket.ValueKind == JsonValueKind.Number && rightPacket.ValueKind == JsonValueKind.Number)
            {
                return leftPacket.GetInt32().CompareTo(rightPacket.GetInt32());
            }

            if (leftPacket.ValueKind == JsonValueKind.Number)
            {
                leftPacket = Eris_ConvertToArrayPacket(leftPacket);
            }
            else if (rightPacket.ValueKind == JsonValueKind.Number)
            {
                rightPacket = Eris_ConvertToArrayPacket(rightPacket);
            }
            else
            {
                foreach (var (nextLeft, nextRight) in leftPacket.EnumerateArray().Zip(rightPacket.EnumerateArray()))
                {
                    var comparisonResult = Eris_ComparePackets(nextLeft, nextRight);
                    if (comparisonResult != 0)
                    {
                        return comparisonResult;
                    }
                }

                return leftPacket.GetArrayLength() - rightPacket.GetArrayLength();
            }
        }
    }

    private static JsonElement Eris_ConvertToArrayPacket(JsonElement packet)
    {
        return JsonDocument.Parse($"[{packet.GetInt32()}]").RootElement;
    }

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