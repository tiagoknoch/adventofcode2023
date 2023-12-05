using System.Collections.Immutable;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace AdventOfCode;

public class Day02 : BaseDay
{
    private readonly string _input;

    public Day02()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var cubeCounts = new Dictionary<string, int>
        {
            { "red", 12 },
            { "green", 13 },
            { "blue", 14 }
        };

        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var result = lines
            .Select(line => line.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .Where(gameSetsSplit => IsGamePossible(gameSetsSplit[1], int.Parse(gameSetsSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1])))
            .Sum(gameSetsSplit => int.Parse(gameSetsSplit[0].Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]));

        return new ValueTask<string>(result.ToString());

        bool IsGamePossible(string sets, int gameId)
        {
            return sets.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .All(set => set.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(i => i.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries))
                .All(item => IsCubeValid(item[1], int.Parse(item[0]))));
        }

        bool IsCubeValid(string color, int nrCubes)
        {
            return nrCubes <= 15 && nrCubes <= cubeCounts[color];
        }
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

        var result = lines
            .Select(line => line.Split(':', StringSplitOptions.RemoveEmptyEntries))
            .Select(gameSetsSplit => GePowerMaxCubes(gameSetsSplit[1]))
            .Sum();

        return new ValueTask<string>(result.ToString());

        static int GePowerMaxCubes(string sets)
        {
            return sets.Split(';', StringSplitOptions.RemoveEmptyEntries)
                .Select(set => set.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(i =>
                {
                    var cubes = i.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    return (Color: cubes[1], Cubes: int.Parse(cubes[0]));
                }
                )
                .ToList())
                .SelectMany(i => i)
                .GroupBy(i => i.Color)
                .Select(i => i.Max(i => i.Cubes))
                .Aggregate((a, x) => a * x);
        }
    }
}
