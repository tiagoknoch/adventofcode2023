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

    }

    // I had to check online how to solve it, brute force was not an option
    public override ValueTask<string> Solve_2()
    {
        var instructions = _lines.First().ToCharArray();
        var nodes = GetNodes();

        var rootNodes = nodes.Keys.Where(x => x.EndsWith('A')).ToList();
        var trees = rootNodes.Select(x => new Tree(nodes, x)).ToList();

        int currentInstructionIndex = 0;
        //now transverse the tree
        while (!trees.All(tree => tree.Finished))
        {
            var currentInstruction = instructions[currentInstructionIndex % instructions.Length];
            foreach (var tree in trees)
            {
                tree.Transverse(currentInstruction == 'L');
            }

            currentInstructionIndex++;
        }

        var result = trees.Select(x => x.Steps)
            .Aggregate(Lcm);

        return new(result.ToString());
    }

    static long Gcf(long a, long b)
    {
        while (b != 0)
        {
            long temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    static long Lcm(long a, long b)
    {
        return (a / Gcf(a, b)) * b;
    }


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

public class Tree(Dictionary<string, Node> nodes, string root)
{
    private readonly Dictionary<string, Node> nodes = nodes;
    private readonly string root = root;
    public long Steps { get; set; } = 0;

    public bool Finished { get; set; }

    public string CurrentNode { get; set; } = root;

    public string Root => root;

    public void Transverse(bool left)
    {
        if (Finished)
        {
            return;
        }

        var node = nodes[CurrentNode];
        CurrentNode = left ? node.Left : node.Right;
        Steps++;
        if (CurrentNode.EndsWith('Z'))
        {
            Finished = true;
        }
    }
}

public record Node(string Root, string Left, string Right);