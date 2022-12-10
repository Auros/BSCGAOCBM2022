namespace BSCGAOCBM2022;

public static class Helpers
{
    public const string Part1 = "Part 1";
    public const string Part2 = "Part 2";

    public const bool AurosIsBaseline = true;
    public const bool CaedenIsBaseline = false;
    public const bool ErisIsBaseline = false;
    public const bool GoobieIsBaseline = false;

    public static Input GetInput(int day)
    {
        var inputText = File.ReadAllText(@$"Input\{day}.txt");
        var inputLines = File.ReadAllLines(@$"Input\{day}.txt");
        var inputEnumerable = inputLines.AsEnumerable();
        return new(inputText, inputLines, inputEnumerable);
    }
}