using System.Linq;
using System.Text;

namespace AdventOfCode;

public class Day03 : BaseDay
{
    private readonly string _input;
    private readonly List<char> _parts = ['*', '+', '#', '$', '/', '-', '=', '@', '%', '&'];

    public Day03()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var matrix = lines.Select(line => line.ToCharArray()).ToArray();

        var result = GetNumbers(matrix)
         .Where(number => HasAdjacentPart(number.Item2, number.Item3, number.Item1, matrix))
         .Sum(number => int.Parse(number.Item1));

        return new(result.ToString());
    }
    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var matrix = lines.Select(line => line.ToCharArray()).ToArray();

        var result = GetGears(matrix)
         .Sum(number =>
         {
             var numbers = GetAdjacentNumbers(number.Item2, number.Item3, matrix).ToList();
             return numbers.Count == 2 ? numbers[0] * numbers[1] : 0;
         });

        return new(result.ToString());
    }


    private static List<int> GetAdjacentNumbers(int i, int j, char[][] matrix)
    {
        //first get numbers in the 3 lines where gear is
        var numbers = Enumerable.Range(i - 1, 3)
                        .SelectMany(line => GetNumbersByLine(matrix, line))
                        .ToList();

        var adjacentNumbers = numbers
            .Where(number => IsAdjacent(number, (i, j)))
            .Select(number => int.Parse(number.Item1))
            .ToList();

        return adjacentNumbers;
    }

    private static bool IsAdjacent((string, int, int) number, (int, int) gear)
    {
        var (numberString, iNumber, jNumber) = number;
        var (iGear, jGear) = gear;


        var numberCoordinates = Enumerable.Range(jNumber, numberString.Length)
                                          .Select(j => (iNumber, j))
                                          .ToList();

        //get all adjacent coordinates for gear
        var adjacentCoordinatesGear = Enumerable.Range(iGear - 1, 3)
                                                .SelectMany(i => Enumerable.Range(jGear - 1, 3)
                                                .Select(j => (i, j)))
                                                .ToList();

        //check if they intersect
        var intersect = numberCoordinates.Intersect(adjacentCoordinatesGear).Any();
        return intersect;
    }

    private bool HasAdjacentPart(int i, int j, string number, char[][] matrix)
    {
        var surroundElements = new List<char>();

        //try to get above row
        if (i > 0)
        {
            var above = matrix[i - 1][j..(j + number.Length)];
            surroundElements.AddRange(above);
        }

        //try to get under row
        if (i < matrix.Length - 1)
        {
            //can get lower left
            var under = matrix[i + 1][j..(j + number.Length)];
            surroundElements.AddRange(under);
        }

        //try to get left column
        if (j > 0)
        {
            if (i > 0)
            {
                var aboveLeft = matrix[i - 1][j - 1];
                surroundElements.Add(aboveLeft);
            }

            var left = matrix[i][j - 1];
            surroundElements.Add(left);

            if (i < matrix.Length - 1)
            {
                var underLeft = matrix[i + 1][j - 1];
                surroundElements.Add(underLeft);
            }
        }

        //try to get right column
        if (j + number.Length < matrix[i].Length - 1)
        {
            if (i > 0)
            {
                var aboveRight = matrix[i - 1][j + number.Length];
                surroundElements.Add(aboveRight);
            }

            var right = matrix[i][j + number.Length];
            surroundElements.Add(right);

            if (i < matrix.Length - 1)
            {
                var underRight = matrix[i + 1][j + number.Length];
                surroundElements.Add(underRight);
            }
        }


        return surroundElements.Any(x => _parts.Any(y => y == x));
    }

    private static IEnumerable<(string, int, int)> GetGears(char[][] matrix)
    {
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                if (matrix[i][j] == '*')
                {
                    yield return ("*", i, j);
                }
            }
        }
    }

    private static IEnumerable<(string, int, int)> GetNumbers(char[][] matrix)
    {
        var number = new StringBuilder();
        int initialI = 0;
        int initialJ = 0;
        bool fetchingNumber = false;

        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                var cell = matrix[i][j];
                if (char.IsDigit(cell))
                {
                    if (!fetchingNumber)
                    {
                        fetchingNumber = true;
                        initialI = i;
                        initialJ = j;
                    }

                    number.Append(cell);
                }
                else if (fetchingNumber)
                {
                    yield return (number.ToString(), initialI, initialJ);
                    number.Clear();
                    fetchingNumber = false;
                }
            }
        }

        if (fetchingNumber)
        {
            yield return (number.ToString(), initialI, initialJ);
        }
    }

    private static IEnumerable<(string, int, int)> GetNumbersByLine(char[][] matrix, int i)
    {
        var number = new StringBuilder();
        int initialI = 0;
        int initialJ = 0;
        bool fetchingNumber = false;


        for (int j = 0; j < matrix[i].Length; j++)
        {
            var cell = matrix[i][j];
            if (char.IsDigit(cell))
            {
                if (!fetchingNumber)
                {
                    fetchingNumber = true;
                    initialI = i;
                    initialJ = j;
                }

                number.Append(cell);
            }
            else if (fetchingNumber)
            {
                yield return (number.ToString(), initialI, initialJ);
                number.Clear();
                fetchingNumber = false;
            }
        }

        if (fetchingNumber)
        {
            yield return (number.ToString(), initialI, initialJ);
        }
    }

}
