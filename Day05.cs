﻿using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Spectre.Console;

namespace AdventOfCode;

public class Day05 : BaseDay
{
    private readonly string _input;

    public record RangeRecord(long Source, long Destination, long Range);

    public Day05()
    {
        _input = File.ReadAllText(InputFilePath);
    }

    public override ValueTask<string> Solve_1()
    {
        var lines = _input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        var seeds = ParseSeeds(lines.First());
        var maps = ParseMaps(lines.Skip(1));
        var results = new List<long>();

        foreach (var seed in seeds)
        {
            long currentLocationValue = seed;
            foreach (var map in maps)
            {
                var rangeRecord = map.FirstOrDefault(item => currentLocationValue >= item.Source && currentLocationValue < item.Source + item.Range);

                if (rangeRecord == null)
                {
                    continue;
                }

                var difference = currentLocationValue - rangeRecord.Source;
                currentLocationValue = rangeRecord.Destination + difference;
            }

            results.Add(currentLocationValue);
        }

        return new(results.Min().ToString());
    }

    private List<List<RangeRecord>> ParseMaps(IEnumerable<string> lines)
    {
        var pattern = @"(.+)-to-(.+) map:";
        var maps = new List<List<RangeRecord>>();
        List<RangeRecord> currentMap = null;
        foreach (var line in lines)
        {
            var match = Regex.Match(line, pattern);
            if (match.Success)
            {
                //it's a new map, so add the current one to the list and start a new one
                if (currentMap != null)
                {
                    maps.Add(currentMap.OrderBy(x => x.Source).ToList());
                }

                currentMap = [];
            }
            else if (currentMap != null)
            {
                //it's a range, so add it to the current map
                var range = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                .Select(long.Parse)
                                .ToList();

                currentMap.Add(new RangeRecord(range[1], range[0], range[2]));
            }
        }

        maps.Add(currentMap.OrderBy(x => x.Source).ToList());
        return maps;
    }

    private static long[] ParseSeeds(string line)
    {
        return line.Split(':')[1]
        .Split(' ', StringSplitOptions.RemoveEmptyEntries)
        .Select(long.Parse)
        .ToArray();
    }

    public override ValueTask<string> Solve_2()
    {
        return new(0.ToString());
    }
}
