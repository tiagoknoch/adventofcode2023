namespace AdventOfCode;

public class Day09 : BaseDay
{
    private readonly string _input;
    private readonly string[] _lines;

    public Day09()
    {
        _input = File.ReadAllText(InputFilePath);
        _lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

    }

    static List<int> CalculateConsecutiveDifferences(IEnumerable<int> list)
    {
        return list.Skip(1).Select((item, index) => item - list.ElementAt(index)).ToList();
    }

    public override ValueTask<string> Solve_1()
    {

        var lastItems = new List<int>();
        foreach (var line in _lines)
        {
            var items = line.Split(" ").Select(int.Parse).ToList();
            List<int> results = [-1];
            var allDifferences = new List<List<int>>
            {
                items
            };

            while (!results.TrueForAll(x => x == 0))
            {
                results = CalculateConsecutiveDifferences(items);
                allDifferences.Add(results);
                items = results;
            }

            int currentValue = allDifferences
                .Select(list => list[^1])
                .Sum();

            lastItems.Add(currentValue);
        }

        var finalResult = lastItems.Sum();
        return new(finalResult.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }
}
