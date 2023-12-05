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
        var result = 0;
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var matrix = lines.Select(line => line.ToCharArray()).ToArray();

        foreach (var number in GetNumbers(matrix))
        {
            if (HasAdjacentPart(number.Item2, number.Item3, number.Item1, matrix))
            {
                result += int.Parse(number.Item1);
            }
        }

        return new(result.ToString());
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

    public static IEnumerable<(string, int, int)> GetNumbers(char[][] matrix)
    {
        bool fetchingNumber = false;
        string number = "";
        int initialI = 0;
        int initialJ = 0;
        for (int i = 0; i < matrix.Length; i++)
        {
            for (int j = 0; j < matrix[i].Length; j++)
            {
                var cell = matrix[i][j];
                if (fetchingNumber)
                {
                    if (int.TryParse(cell.ToString(), out var _))
                    {
                        number += cell.ToString();
                    }
                    else
                    {
                        yield return (number, initialI, initialJ);
                        //reset
                        fetchingNumber = false;
                        number = "";
                        initialI = 0;
                        initialJ = 0;
                    }
                }
                else
                {
                    if (int.TryParse(cell.ToString(), out var _))
                    {
                        fetchingNumber = true;
                        initialI = i;
                        initialJ = j;
                        number = cell.ToString();
                    }
                }
            }
        }
    }

    public override ValueTask<string> Solve_2()
    {
        throw new NotImplementedException();
    }

    public static IEnumerable<char[][]> GetAdjacentParts(char[][] matrix)
    {
        for (int i = 1; i < matrix.Length - 1; i++)
        {
            for (int j = 1; j < matrix[i].Length - 1; j++)
            {
                var current = matrix[i][j];
                if (current != '.' && !int.TryParse(current.ToString(), out _))
                {
                    //its a part, return the adjacent elements
                    yield return new char[][] { [matrix[i - 1][j - 1], matrix[i - 1][j], matrix[i -1][j + 1]],
                        [matrix[i][j-1], matrix[i][j], matrix[i][j+1]],
                        [matrix[i + 1][j - 1], matrix[i+1][j ], matrix[i + 1][j + 1]] };
                }
            }
        }
    }
}
