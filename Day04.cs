using System.Linq;

namespace AdventOfCode;

public class Day04 : BaseDay
{
    private readonly string _input;

    public Day04()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var result = lines
            .Select(line => GetNumbers(line).Intersect(GetWinningsNumbers(line)))
            .Select(GetScore)
            .Sum();

        return new(result.ToString());


        static List<int> GetWinningsNumbers(string line)
        {
            var result = line.Split("|", StringSplitOptions.TrimEntries)[1]
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            return result;
        }

        static List<int> GetNumbers(string line)
        {
            var result = line.Split("|", StringSplitOptions.TrimEntries)[0]
                .Split(":", StringSplitOptions.TrimEntries)[1]
                .Split(" ", StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList();
            return result;
        }

        static int GetScore(IEnumerable<int> numbers)
        {
            if (!numbers.Any())
            {
                return 0;
            }
            var result = (int)Math.Pow(2, numbers.Count() - 1);
            return result;
        }
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

}
