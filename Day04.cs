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
        var results = new Dictionary<int, int>();
        var queue = new Queue<int>();

        //get the winning numbers for each line
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries)
                  .Select((line, index) => (index + 1, Numbers: GetNumbers(line).Intersect(GetWinningsNumbers(line)).Count()))
                  .ToList();

        // Initialize the dictionary with the line number and the number of winning numbers
        Enumerable.Range(1, lines.Count)
        .ToList()
        .ForEach(x =>
        {
            results.Add(x, 1);
            queue.Enqueue(x);
        });

        while (queue.Any())
        {
            var current = queue.Dequeue();
            var (_, numbers) = lines[current - 1];
            foreach (var number in Enumerable.Range(current + 1, numbers))
            {
                queue.Enqueue(number);
                if (!results.TryAdd(number, 1))
                {
                    results[number] += 1;
                }
            }
        }

        return new(results.Values.Sum().ToString());
    }


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
}
