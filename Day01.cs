using System.Text;

namespace AdventOfCode;

public class Day01 : BaseDay
{
    private readonly string _input;

    public Day01()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var result = 0;
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            var numbers = line
            .Where(c => int.TryParse(c.ToString(), out _)); //it would be better to filter out ascii values for integers

            if (numbers.Any())
            {
                result += int.Parse(numbers.First().ToString() + numbers.Last().ToString());
            }
        }

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var numberList = new Dictionary<string, int>
    {
        { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 },
        { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 },
        { "1", 1 }, { "2", 2 }, { "3", 3 }, { "4", 4 }, { "5", 5 },
        { "6", 6 }, { "7", 7 }, { "8", 8 }, { "9", 9 }
    };
        var result = 0;
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        foreach (var line in lines)
        {
            int firstIndex = 9999;
            int lastIndex = -1;
            string firstNumber = null;
            string lastNumber = null;
            foreach (var (item, index, index2) in from item in numberList.Keys
                                                  let index = line.IndexOf(item)
                                                  let index2 = line.LastIndexOf(item)
                                                  select (item, index, index2))
            {
                if (index != -1 && index < firstIndex)
                {
                    firstIndex = index;
                    firstNumber = item;
                }

                if (index2 != -1 && index2 > lastIndex)
                {
                    lastIndex = index2;
                    lastNumber = item;
                }
            }

            // Its possible that a line only has one number
            lastNumber ??= firstNumber;

            var combinedNumber = new StringBuilder()
                .Append(numberList[firstNumber])
                .Append(numberList[lastNumber])
                .ToString();

            result += int.Parse(combinedNumber);
        }

        return new ValueTask<string>(result.ToString());
    }
}
