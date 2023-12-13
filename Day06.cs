namespace AdventOfCode;

public class Day06 : BaseDay
{
    private readonly string _input;

    public Day06()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var pairs = Parse(lines);

        var result = pairs
            .Select(ProcessRace)
            .Aggregate((a, x) => a * x);

        return new(result.ToString());
    }

    private int ProcessRace((int Time, int Record) race)
    {
        var (time, record) = race;
        var waysToWin = Enumerable.Range(1, time - 2)
                                  .Count(i => (time - i) * i > record);

        return waysToWin;
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    public static List<(int, int)> Parse(string[] lines)
    {
        var times = lines[0]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();
        var distances = lines[1]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(int.Parse)
            .ToList();

        return times.Zip(distances, (time, distance) => (time, distance)).ToList();
    }
}
