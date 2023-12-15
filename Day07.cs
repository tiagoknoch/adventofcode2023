namespace AdventOfCode;

public class Day07 : BaseDay
{
    private readonly string _input;

    public Day07()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var games = ParseInput(lines)
            .Select(x => new Game(x.Item1, x.Item2))
            .ToList();

        games.Sort();

        var result = games
        .Select((game, index) => (index + 1) * game.Bid)
        .Sum();

        return new(result.ToString());
    }

    public override ValueTask<string> Solve_2()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var games = ParseInput(lines)
            .Select(x => new Game2(x.Item1, x.Item2))
            .ToList();

        games.Sort();

        var result = games
        .Select((game, index) => (index + 1) * game.Bid)
        .Sum();

        return new(result.ToString());
    }

    private static IEnumerable<(string, int)> ParseInput(string[] lines)
    {
        return lines.Select(line =>
        {
            var parts = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            var hand = parts[0];
            var value = int.Parse(parts[1]);
            return (hand, value);
        });
    }

}

public class Game : IComparable<Game>
{
    enum Type
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }

    private static readonly Dictionary<char, int> values = new(){
        { 'A', 14},
        { 'K', 13},
        { 'Q', 12},
        { 'J', 11},
        { 'T', 10},
        { '9', 9},
        { '8', 8},
        { '7', 7},
        { '6', 6},
        { '5', 5},
        { '4', 4},
        { '3', 3},
        { '2', 2}
    };

    private readonly Type _type;

    private readonly char[] _hand;

    public string OriginalHand { get; }
    public int Bid { get; }

    public Game(string hand, int bid)
    {
        OriginalHand = hand;
        _hand = hand.ToCharArray();
        Bid = bid;
        _type = GetTypeOfHand(hand);

    }

    private Type GetTypeOfHand(string hand)
    {
        var sortedHand = SortHand(hand);
        var groups = sortedHand.GroupBy(x => x).ToList();
        var groupCounts = groups.Select(x => x.Count()).ToList();

        if (groupCounts.Count == 1 && groupCounts[0] == 5)
        {
            return Type.FiveOfAKind;
        }

        if (groupCounts.Contains(4))
        {
            return Type.FourOfAKind;
        }

        if (groupCounts.Contains(3) && groupCounts.Contains(2))
        {
            return Type.FullHouse;
        }

        if (groupCounts.Contains(3))
        {
            return Type.ThreeOfAKind;
        }

        if (groupCounts.Count(x => x == 2) == 2)
        {
            return Type.TwoPair;
        }

        if (groupCounts.Contains(2))
        {
            return Type.OnePair;
        }

        return Type.HighCard;
    }

    public int CompareTo(Game other)
    {
        if (_type != other._type)
        {
            return _type.CompareTo(other._type);
        }

        // same type, check cards individually
        for (int i = 0; i < _hand.Length; i++)
        {
            int card = values[_hand[i]];
            int otherCard = values[other._hand[i]];

            if (card != otherCard)
            {
                return card.CompareTo(otherCard);
            }
        }

        return 0;
    }

    private char[] SortHand(string hand)
    {
        return hand.ToCharArray()
            .OrderByDescending(x => values[x])
            .ToArray();
    }
}


public class Game2(string hand, int bid) : IComparable<Game2>
{
    enum Type
    {
        FiveOfAKind = 7,
        FourOfAKind = 6,
        FullHouse = 5,
        ThreeOfAKind = 4,
        TwoPair = 3,
        OnePair = 2,
        HighCard = 1
    }

    private static readonly Dictionary<char, int> values = new(){
        { 'A', 13},
        { 'K', 12},
        { 'Q', 11},
        { 'T', 10},
        { '9', 9},
        { '8', 8},
        { '7', 7},
        { '6', 6},
        { '5', 5},
        { '4', 4},
        { '3', 3},
        { '2', 2},
        { 'J', 1}
    };

    private readonly Type _type = GetTypeOfHand(hand);

    private readonly char[] _hand = hand.ToCharArray();

    public string OriginalHand { get; } = hand;
    public int Bid { get; } = bid;

    private static Type GetTypeOfHand(string hand)
    {
        // Handle joker, aka, replace with other chards
        if (!hand.Contains('J'))
        {
            return CalculateType(hand);
        }

        if (hand == "JJJJJ")
        {
            return Type.FiveOfAKind;
        }

        var distinctChars = hand.Distinct().Except(['J']).ToList();
        var newHands = distinctChars.Select(c =>
        {
            var newHand = hand.Replace('J', c);
            return (newHand, CalculateType(newHand));
        })
        .OrderByDescending(x => x.Item2)
        .ToList();

        return newHands[0].Item2;
    }

    public int CompareTo(Game2 other)
    {
        if (_type != other._type)
        {
            return _type.CompareTo(other._type);
        }

        // same type, check cards individually
        for (int i = 0; i < _hand.Length; i++)
        {
            int card = values[_hand[i]];
            int otherCard = values[other._hand[i]];

            if (card != otherCard)
            {
                return card.CompareTo(otherCard);
            }
        }

        return 0;
    }

    private static char[] SortHand(string hand)
    {
        return hand.ToCharArray()
            .OrderByDescending(x => values[x])
            .ToArray();
    }

    private static Type CalculateType(string hand)
    {
        var sortedHand = SortHand(hand);
        var groups = sortedHand.GroupBy(x => x).ToList();
        var groupCounts = groups.Select(x => x.Count()).ToList();

        if (groupCounts.Count == 1 && groupCounts[0] == 5)
        {
            return Type.FiveOfAKind;
        }

        if (groupCounts.Contains(4))
        {
            return Type.FourOfAKind;
        }

        if (groupCounts.Contains(3) && groupCounts.Contains(2))
        {
            return Type.FullHouse;
        }

        if (groupCounts.Contains(3))
        {
            return Type.ThreeOfAKind;
        }

        if (groupCounts.Count(x => x == 2) == 2)
        {
            return Type.TwoPair;
        }

        if (groupCounts.Contains(2))
        {
            return Type.OnePair;
        }

        return Type.HighCard;
    }
}