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
    /*
    This is a brute force solution, I'm sure there is a better way but this solution is still under 1s
    on my machine so I'm not going to worry about it.
    */
    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var race = Parse2(lines);
        var result = ProcessRace(race);

        return new(result.ToString());
    }

    private static int ProcessRace((long Time, long Record) race)
    {
        var (time, record) = race;
        var waysToWin = EnumerableRangeLong(1, time - 2)
                                  .Count(i => (time - i) * i > record);

        return waysToWin;
    }

    private static List<(long, long)> Parse(string[] lines)
    {
        var times = lines[0]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();
        var distances = lines[1]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(long.Parse)
            .ToList();

        return times.Zip(distances, (time, distance) => (time, distance)).ToList();
    }

    private static (long, long) Parse2(string[] lines)
    {
        var times = lines[0]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);


        var distances = lines[1]
            .Split(':')[1]
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);


        var time = long.Parse(string.Join("", times));
        var distance = long.Parse(string.Join("", distances));
        return (time, distance);
    }

    private static IEnumerable<long> EnumerableRangeLong(long start, long count)
    {
        var end = start + count;
        for (var current = start; current < end; ++current)
        {
            yield return current;
        }
    }
}
