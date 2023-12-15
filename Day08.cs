using System.Text;

namespace AdventOfCode;

public class Day08 : BaseDay
{
    private readonly string _input;
    private readonly string[] _lines;
    private readonly string endNode = "ZZZ";
    private readonly string rootNode = "AAA";

    public Day08()
    {
        _input = File.ReadAllText(InputFilePath);
        _lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }

    public override ValueTask<string> Solve_1()
    {
        var instructions = _lines.First().ToCharArray();
        var nodes = GetNodes();

        var currentNode = rootNode;
        var steps = 0;
        int currentInstructionIndex = 0;
        //now transverse the tree
        while (currentNode != endNode)
        {
            var currentInstruction = instructions[currentInstructionIndex % instructions.Length];
            var node = nodes[currentNode];
            currentNode = currentInstruction == 'L' ? node.Left : node.Right;

            steps++;
            currentInstructionIndex++;
        }

        return new(steps.ToString());

        Dictionary<string, Node> GetNodes()
        {
            return _lines.Skip(1)
            .Select(x =>
            {
                var split = x.Split('=', StringSplitOptions.RemoveEmptyEntries);
                var root = split[0].Trim();
                var split2 = split[1].Split(',', StringSplitOptions.RemoveEmptyEntries);
                var left = split2[0].Trim().Trim('(');
                var right = split2[1].Trim(')').Trim();
                return new Node(root, left, right);
            })
            .ToDictionary(x => x.Root, x => x);
        }
    }

    public override ValueTask<string> Solve_2()
    {
        return new(_input.Length.ToString());
    }
}

public record Node(string Root, string Left, string Right);