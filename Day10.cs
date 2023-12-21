using System.Data;

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
        return new(_input.Length.ToString());
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