using System.Data;
using System.Linq;
using System.Collections.Generic;

namespace AdventOfCode;

public class Day10 : BaseDay
{
    private readonly string _input;
    private readonly string[] _lines;
    private readonly char[][] _map;

    private readonly Coordinate _start;

    public Day10()
    {
        _input = File.ReadAllText(InputFilePath);
        _lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        _map = _lines.Select(line => line.ToCharArray()).ToArray();
        _start = GetStart();
    }

    public override ValueTask<string> Solve_1()
    {
        var loop = new Loop(_start);
        // Lets build the loop going right
        loop.BuildLoop(_map, new Coordinate(_start.X + 1, _start.Y));

        var loop2 = new Loop(_start);
        // Lets build the loop going down
        loop2.BuildLoop(_map, new Coordinate(_start.X, _start.Y + 1));

        var step = 1;
        var pipe1 = loop.Pipes[1];
        var pipe2 = loop2.Pipes[1];
        while (pipe1.Coordinate != pipe2.Coordinate)
        {
            step++;
            pipe1 = loop.Pipes[step];
            pipe2 = loop2.Pipes[step];

        }

        return new(step.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var loop = new Loop(_start);
        // Lets build the loop going right
        loop.BuildLoop(_map, new Coordinate(_start.X + 1, _start.Y));

        var vertices = GetVertices(loop);
        var area = PolygonArea(vertices.Select(v => (double)v.X).ToArray(), vertices.Select(v => (double)v.Y).ToArray(), vertices.Count);

        var insideCoordinates = new List<Coordinate>();
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                if (IsPointInPolygon4(vertices.ToArray(), new Coordinate(j, i)))
                {
                    insideCoordinates.Add(new Coordinate(j, i));
                }
            }
        }

        // remove all pipe coordinates from inside coordinates
        var pipeCoordinates = loop.Pipes.Select(pipe => pipe.Coordinate).ToList();
        insideCoordinates.RemoveAll(l => pipeCoordinates.Contains(l));


        DrawMap(loop, vertices, insideCoordinates);

        return new(insideCoordinates.Count.ToString());
    }

    private static List<Coordinate> GetVertices(Loop loop)
    {
        var symbols = new HashSet<char> { 'S', '7', 'L', 'F', 'J' };

        return loop.Pipes
            .Where(pipe => symbols.Contains(pipe.Symbol))
            .Select(pipe => pipe.Coordinate)
            .ToList();
    }

    /// <summary>
    /// Determines if the given point is inside the polygon
    /// </summary>
    /// <param name="polygon">the vertices of polygon</param>
    /// <param name="testPoint">the given point</param>
    /// <returns>true if the point is inside the polygon; otherwise, false</returns>
    public static bool IsPointInPolygon4(Coordinate[] polygon, Coordinate testPoint)
    {
        bool result = false;
        int j = polygon.Length - 1;
        for (int i = 0; i < polygon.Length; i++)
        {
            if (polygon[i].Y < testPoint.Y && polygon[j].Y >= testPoint.Y ||
                polygon[j].Y < testPoint.Y && polygon[i].Y >= testPoint.Y)
            {
                if (polygon[i].X + (testPoint.Y - polygon[i].Y) /
                   (polygon[j].Y - polygon[i].Y) *
                   (polygon[j].X - polygon[i].X) < testPoint.X)
                {
                    result = !result;
                }
            }
            j = i;
        }
        return result;
    }

    public void DrawMap(Loop loop, List<Coordinate> vertices, List<Coordinate> insideCoordinates)
    {
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                if (loop.Pipes.Any(pipe => pipe.Coordinate == new Coordinate(j, i)))
                {
                    if (vertices.Any(v => v == new Coordinate(j, i)))
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                else
                {
                    if (insideCoordinates.Any(v => v == new Coordinate(j, i)))
                    {

                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
                Console.Write(_map[i][j]);
            }
            Console.WriteLine();
        }

    }

    public Coordinate GetStart()
    {
        for (int i = 0; i < _map.Length; i++)
        {
            for (int j = 0; j < _map[i].Length; j++)
            {
                if (_map[i][j] == 'S')
                {
                    return new Coordinate(j, i);
                }
            }
        }

        throw new InvalidDataException("No start found");
    }

    public static double PolygonArea(double[] X,
                               double[] Y, int n)
    {

        // Initialize area
        double area = 0.0;

        // Calculate value of shoelace formula
        int j = n - 1;

        for (int i = 0; i < n; i++)
        {
            area += (X[j] + X[i]) * (Y[j] - Y[i]);

            // j is previous vertex to i
            j = i;
        }

        // Return absolute value
        return Math.Abs(area / 2.0);
    }
}

public record Coordinate(int X, int Y);

public class Loop
{
    public Coordinate Start { get; private set; }

    public List<Pipe> Pipes { get; private set; } = [];

    public Loop(Coordinate start)
    {
        Start = start;
        Pipes.Add(new Pipe(start, start, 'S'));
    }

    public void BuildLoop(char[][] map, Coordinate next)
    {
        var previous = Start;

        while (next != Start)
        {
            var nextPipe = new Pipe(next, previous, map[next.Y][next.X]);
            Pipes.Add(nextPipe);
            previous = nextPipe.Coordinate;
            next = nextPipe.Exit;
        }
    }

}

public class Pipe
{
    public Coordinate Entrance { get; private set; }
    public Coordinate Exit { get; private set; }
    public Coordinate Coordinate { get; private set; }
    public char Symbol { get; private set; }

    public Pipe(Coordinate coordinate, Coordinate entrance, char symbol)
    {
        Entrance = entrance;
        Coordinate = coordinate;
        Symbol = symbol;

        switch (symbol)
        {
            case 'S':
                break;
            case '|':
                if (entrance.Y < coordinate.Y)
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y + 1);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y - 1);
                }
                break;
            case '-':
                if (entrance.X < coordinate.X)
                {
                    Exit = new Coordinate(coordinate.X + 1, coordinate.Y);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X - 1, coordinate.Y);
                }
                break;
            case 'L':
                if (entrance.Y < coordinate.Y)
                {
                    Exit = new Coordinate(coordinate.X + 1, coordinate.Y);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y - 1);
                }
                break;
            case 'J':
                if (entrance.X < coordinate.X)
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y - 1);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X - 1, coordinate.Y);
                }
                break;
            case '7':
                if (entrance.X < coordinate.X)
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y + 1);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X - 1, coordinate.Y);
                }
                break;
            case 'F':
                if (entrance.X > coordinate.X)
                {
                    Exit = new Coordinate(coordinate.X, coordinate.Y + 1);
                }
                else
                {
                    Exit = new Coordinate(coordinate.X + 1, coordinate.Y);
                }
                break;
            default:
                throw new InvalidDataException("Invalid character");
        }
    }
}

