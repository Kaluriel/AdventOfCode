using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day12 : DayBase2021
    {
        class Cave
        {
            public string name;
            public List<Cave> connections { get; } = new List<Cave>();
            public bool IsSmallCave => name.All(c => char.IsLower(c));
            public bool IsBig => name.All(c => char.IsUpper(c));
            public bool IsStart => name == "start";
            public bool IsEnd => name == "end";

            public Cave(string _name)
            {
                name = _name;
            }

            public override string ToString()
            {
                return name;
            }
        }

        protected override Task<object> ExecutePart1Async()
        {
            var caveMapping = Source.SplitNewLine()
                                    .Select(x => x.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries))
                                    .Select(
                                        x => (x[1] == "start") || (x[1] == "end") || (x[0].CompareTo(x[1]) > 0)
                                            ? new { start = x[1], end = x[0] }
                                            : new { start = x[0], end = x[1] }
                                    ).ToArray();

            var caves = new Dictionary<string, Cave>();

            foreach (var caveMap in caveMapping)
            {
                Cave currentCave;
                Cave nextCave;

                if (!caves.TryGetValue(caveMap.start, out currentCave))
                {
                    currentCave = new Cave(caveMap.start);
                    caves.Add(currentCave.name, currentCave);
                }

                if (!caves.TryGetValue(caveMap.end, out nextCave))
                {
                    nextCave = new Cave(caveMap.end);
                    caves.Add(nextCave.name, nextCave);
                }

                if (!currentCave.IsEnd && !nextCave.IsStart && !currentCave.connections.Contains(nextCave))
                {
                    currentCave.connections.Add(nextCave);
                }

                if (!nextCave.IsEnd && !currentCave.IsStart && !nextCave.connections.Contains(currentCave))
                {
                    nextCave.connections.Add(currentCave);
                }
            }

            var visitedCaves = new List<Cave>()
            {
                caves["start"]
            };

            int pathCount = PathCount(caves["start"], visitedCaves, true);

            return Task.FromResult<object>(
                pathCount
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var caveMapping = Source.SplitNewLine()
                                    .Select(x => x.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries))
                                    .Select(
                                        x => (x[1] == "start") || (x[1] == "end") || (x[0].CompareTo(x[1]) > 0)
                                            ? new { start = x[1], end = x[0] }
                                            : new { start = x[0], end = x[1] }
                                    );

            var caves = new Dictionary<string, Cave>();

            foreach (var caveMap in caveMapping)
            {
                Cave currentCave;
                Cave nextCave;

                if (!caves.TryGetValue(caveMap.start, out currentCave))
                {
                    currentCave = new Cave(caveMap.start);
                    caves.Add(currentCave.name, currentCave);
                }

                if (!caves.TryGetValue(caveMap.end, out nextCave))
                {
                    nextCave = new Cave(caveMap.end);
                    caves.Add(nextCave.name, nextCave);
                }

                if (!currentCave.IsEnd && !nextCave.IsStart && !currentCave.connections.Contains(nextCave))
                {
                    currentCave.connections.Add(nextCave);
                }

                if (!nextCave.IsEnd && !currentCave.IsStart && !nextCave.connections.Contains(currentCave))
                {
                    nextCave.connections.Add(currentCave);
                }
            }

            var visitedCaves = new List<Cave>()
            {
                caves["start"]
            };

            int pathCount = PathCount(caves["start"], visitedCaves, false);

            return Task.FromResult<object>(
                pathCount
            );
        }

        private static int PathCount(Cave start, List<Cave> visitedCaves, bool hadSecondSmallVisit)
        {
            int endReachCount = 0;

            if (start.IsEnd)
            {
                //Console.WriteLine($"{string.Join("-", visitedCaves)}");
                ++endReachCount;
            }
            else
            {
                foreach (var conn in start.connections)
                {
                    if (!conn.IsSmallCave || (!visitedCaves.Contains(conn) || !hadSecondSmallVisit))
                    {
                        bool _hadSecondSmallVisit = hadSecondSmallVisit;

                        if (conn.IsSmallCave && !hadSecondSmallVisit && visitedCaves.Contains(conn))
                        {
                            _hadSecondSmallVisit = true;
                        }

                        var list = new List<Cave>(visitedCaves);
                        list.Add(conn);
                        endReachCount += PathCount(conn, list, _hadSecondSmallVisit);
                    }
                }
            }

            return endReachCount;
        }
    }
}
