using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using System.Diagnostics;

namespace BSCGAOCBM2022.GroupA;

[MemoryDiagnoser]
[CategoriesColumn]
[Config(typeof(CustomConfig))]
[GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
public class Day2Benchmarks
{
    private Input _input = null!;

    [GlobalSetup]
    public void Setup() => _input = Helpers.GetInput(2);

    #region Auros

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Auros_Part1()
    {
        var rounds = _input.Lines.Select(static line =>
        {
            var first = Auros_ConvertToRPS(line[0]);
            var second = Auros_ConvertToRPS(line[2]);
            return new
            {
                First = first,
                Second = second
            };
        });

        var totalSum = rounds.Select(static round =>
        {
            var first = round.First;
            var second = round.Second;

            return Auros_PointValueForChoice(second) + Auros_GetMatchResultValue(first, second);
        }).Sum();

        return totalSum;
    }

    [Benchmark(Baseline = Helpers.AurosIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Auros_Part2()
    {
        var rounds = _input.Lines.Select(static line =>
        {
            var first = Auros_ConvertToRPS(line[0]);
            var second = Auros_ConvertToRPS(line[2]);
            return new
            {
                First = first,
                Second = second
            };
        });

        var riggedScore = rounds.Select(static round =>
        {
            var first = round.First;
            var result = (Auros_Result)round.Second;

            var move = result is Auros_Result.Draw ? first : (first, result) switch
            {
                (Auros_RPS.Rock, Auros_Result.Lose) or (Auros_RPS.Paper, Auros_Result.Win) => Auros_RPS.Scissors,
                (Auros_RPS.Paper, Auros_Result.Lose) or (Auros_RPS.Scissors, Auros_Result.Win) => Auros_RPS.Rock,
                (Auros_RPS.Scissors, Auros_Result.Lose) or (Auros_RPS.Rock, Auros_Result.Win) => Auros_RPS.Paper,
                _ => throw new NotImplementedException(),
            };

            return Auros_PointValueForChoice(move) + Auros_GetMatchResultValue(first, move);
        }).Sum();

        return riggedScore;
    }

    static Auros_RPS Auros_ConvertToRPS(char input)
    {
        return input switch
        {
            'A' or 'X' => Auros_RPS.Rock,
            'B' or 'Y' => Auros_RPS.Paper,
            'C' or 'Z' => Auros_RPS.Scissors,
            _ => throw new NotImplementedException(),
        };
    }

    static int Auros_PointValueForChoice(Auros_RPS rps)
    {
        return rps switch
        {
            Auros_RPS.Rock => 1,
            Auros_RPS.Paper => 2,
            Auros_RPS.Scissors => 3,
            _ => throw new NotImplementedException(),
        };
    }

    static int Auros_GetMatchResultValue(Auros_RPS first, Auros_RPS second)
    {
        return first == second ? 3 : (first, second) switch
        {
            (Auros_RPS.Rock, Auros_RPS.Paper) or (Auros_RPS.Paper, Auros_RPS.Scissors) or (Auros_RPS.Scissors, Auros_RPS.Rock) => 6,
            _ => 0
        };
    }

    internal enum Auros_RPS
    {
        Rock,
        Paper,
        Scissors
    }

    internal enum Auros_Result
    {
        Lose,
        Draw,
        Win
    }

    #endregion

    #region Caeden

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Caeden_Part1()
    {
        var pointsWithRespondingShape = 0;
        for (int i = 0; i < _input.Lines.Length; i++)
        {
            var line = _input.Lines[i];
            pointsWithRespondingShape += line[0] switch
            {
                'A' => line[2] switch
                {
                    'X' => 1 + 3,
                    'Y' => 2 + 6,
                    'Z' => 3 + 0,
                    _ => throw new UnreachableException("huh?")
                },
                'B' => line[2] switch
                {
                    'X' => 1 + 0,
                    'Y' => 2 + 3,
                    'Z' => 3 + 6,
                    _ => throw new UnreachableException("huh?")
                },
                'C' => line[2] switch
                {
                    'X' => 1 + 6,
                    'Y' => 2 + 0,
                    'Z' => 3 + 3,
                    _ => throw new UnreachableException("huh?")
                },
                _ => throw new UnreachableException("huh?")
            };
        }
        return pointsWithRespondingShape;
    }

    [Benchmark(Baseline = Helpers.CaedenIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Caeden_Part2()
    {
        var pointsWithDeterminedResult = 0;
        for (int i = 0; i < _input.Lines.Length; i++)
        {
            var line = _input.Lines[i];
            pointsWithDeterminedResult += line[0] switch
            {
                'A' => line[2] switch
                {
                    'X' => 3 + 0,
                    'Y' => 1 + 3,
                    'Z' => 2 + 6,
                    _ => throw new UnreachableException("huh?")
                },
                'B' => line[2] switch
                {
                    'X' => 1 + 0,
                    'Y' => 2 + 3,
                    'Z' => 3 + 6,
                    _ => throw new UnreachableException("huh?")
                },
                'C' => line[2] switch
                {
                    'X' => 2 + 0,
                    'Y' => 3 + 3,
                    'Z' => 1 + 6,
                    _ => throw new UnreachableException("huh?")
                },
                _ => throw new UnreachableException("huh?")
            };
        }
        return pointsWithDeterminedResult;
    }

    #endregion

    #region Eris

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Eris_Part1()
    {
        var totalScore = 0;

        var rawPlays = _input.Enumerable;
        foreach (var rawPlay in rawPlays)
        {
            var player1MoveRaw = rawPlay[0] - ERIS_ASCII_OFFSET;
            var player2MoveRaw = rawPlay[^1] - ERIS_ASCII_OFFSET - ERIS_ADDITIONAL_ASCII_OFFSET;

            var outcome = Eris_CalculateOutcome((Eris_Move)player1MoveRaw, (Eris_Move)player2MoveRaw);

            totalScore += player2MoveRaw + 1 + Eris_MapOutcome(outcome);
        }

        return totalScore;
    }

    [Benchmark(Baseline = Helpers.ErisIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Eris_Part2()
    {
        var totalScore = 0;

        var rawPlays = _input.Enumerable;
        foreach (var rawPlay in rawPlays)
        {
            var player1MoveRaw = rawPlay[0] - ERIS_ASCII_OFFSET;
            var strategyRaw = rawPlay[^1] - ERIS_ASCII_OFFSET - ERIS_ADDITIONAL_ASCII_OFFSET;

            // The strategy offset is calculated by subtracting one and inverting the result
            // -1 means a winning move
            // 0 means a draw
            // 1 means a losing move
            var strategyOffset = -strategyRaw - 1;

            // Calculate our move by offsetting the raw opponent move by 3 (the amount of possible moves)
            // as well as adding the strategy offset, resulting in our own move
            var player2MoveRaw = (player1MoveRaw + 3 - strategyOffset) % 3;

            var outcome = Eris_CalculateOutcome((Eris_Move)player1MoveRaw, (Eris_Move)player2MoveRaw);

            totalScore += player2MoveRaw + 1 + Eris_MapOutcome(outcome);
        }

        return totalScore;
    }

    private const int ERIS_ASCII_OFFSET = 65;
    private const int ERIS_ADDITIONAL_ASCII_OFFSET = 23;

    private static Eris_Outcome Eris_CalculateOutcome(Eris_Move player1Move, Eris_Move player2Move)
    {
        if (player1Move == player2Move)
        {
            return Eris_Outcome.Draw;
        }

        return (player1Move + 3 - player2Move) % 3 == 1 ? Eris_Outcome.Player1Won : Eris_Outcome.Player2Won;
    }

    private static int Eris_MapOutcome(Eris_Outcome outcome) => outcome switch
    {
        Eris_Outcome.Player1Won => 0,
        Eris_Outcome.Draw => 3,
        Eris_Outcome.Player2Won => 6,
        _ => throw new ArgumentOutOfRangeException(nameof(outcome), outcome, null)
    };

    // ReSharper disable thrice UnusedMember.Local
    private enum Eris_Move
    {
        Rock,
        Paper,
        Scissors
    }

    private enum Eris_Outcome
    {
        Player1Won,
        Player2Won,
        Draw
    }

    #endregion

    #region Goobie

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part1)]
    public int Goobie_Part1()
    {
        var score = 0;
        foreach (var line in _input.Lines)
        {
            score += line[2] switch
            {
                'X' => 1,
                'Y' => 2,
                'Z' => 3,
                _ => 0
            };

            score += line switch
            {
                "A X" => 3,
                "B Y" => 3,
                "C Z" => 3,
                "A Y" => 6,
                "B Z" => 6,
                "C X" => 6,
                _ => 0
            };
        }
        return score;
    }

    [Benchmark(Baseline = Helpers.GoobieIsBaseline)]
    [BenchmarkCategory(Helpers.Part2)]
    public int Goobie_Part2()
    {
        var score2 = 0;
        foreach (var line in _input.Lines)
        {
            score2 += line[2] switch
            {
                'X' => 0,
                'Y' => 3,
                'Z' => 6,
                _ => 0
            };

            score2 += line switch
            {
                "A Y" => 1,
                "B X" => 1,
                "C Z" => 1,
                "A Z" => 2,
                "B Y" => 2,
                "C X" => 2,
                _ => 3
            };
        }
        return score2;
    }

    #endregion
}