using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public sealed class Day15 : Day
    {
        class AStarNode
        {
            public Point2D Coord { get; set; }
            public long H { get; set; }
            public long C { get; set; }

            public AStarNode(int x, int y, long cost)
            {
                Coord = new Point2D(x, y);
                H = 0;
                C = cost;
            }
        }

        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var map = Source.SplitNewLine()
                            .Select((x, y) => x.ToCharArray()
                                               .Select((c, x) => new AStarNode(x, y, int.Parse(c.ToString())))
                                               .ToArray()
                            )
                            .ToArray();

            var start = map[0][0];
            var finish = map[map.Length - 1][map[0].Length - 1];

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    map[y][x].H = Math.Abs(finish.Coord.Y - y) + Math.Abs(finish.Coord.X - x);
                }
            }

            var path = A_Star(start, finish, (x) => x.H, map);
            var risk = path.Sum(x => x.C) - start.C;

            //
            return Task.FromResult<object>(
                risk
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var map = Source.SplitNewLine()
                .Select((x, y) => x.ToCharArray()
                    .Select((c, x) => new AStarNode(x, y, int.Parse(c.ToString())))
                    .ToArray()
                )
                .ToArray();

            int sizeY = map.Length;
            int sizeX = map[0].Length;

            Array.Resize(ref map, sizeY * 5);
            for (int y = 0; y < map.Length; ++y)
            {
                int x = 0;

                Array.Resize(ref map[y], sizeX * 5);

                if (y < sizeY)
                {
                    x = sizeX;
                }

                for (; x < map[y].Length; ++x)
                {
                    long cost;

                    if (y < sizeY)
                    {
                        cost = map[y][x - sizeX].C;
                    }
                    else
                    {
                        cost = map[y - sizeY][x].C;
                    }

                    ++cost;
                    if (cost >= 10)
                    {
                        cost = 1;
                    }

                    map[y][x] = new AStarNode(x, y, cost);
                }
            }

            var start = map[0][0];
            var finish = map[map.Length - 1][map[0].Length - 1];

            for (int y = 0; y < map.Length; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    map[y][x].H = Math.Abs(finish.Coord.Y - y) + Math.Abs(finish.Coord.X - x);
                }
            }

            var path = A_Star(start, finish, (x) => x.H, map);
            var risk = path.Sum(x => x.C) - start.C;

            //
            return Task.FromResult<object>(
                risk
            );
        }

        static List<AStarNode> A_Star(AStarNode start, AStarNode goal, Func<AStarNode, long> h, AStarNode[][] map)
        {
            // The set of discovered nodes that may need to be (re-)expanded.
            // Initially, only the start node is known.
            // This is usually implemented as a min-heap or priority queue rather than a hash-set.
            var openSet = new List<AStarNode>()
            {
                start
            };

            // For node n, cameFrom[n] is the node immediately preceding it on the cheapest path from start
            // to n currently known.
            var cameFrom = new Dictionary<AStarNode, AStarNode>();

            // For node n, gScore[n] is the cost of the cheapest path from start to n currently known.
            var gScore = new Dictionary<AStarNode, long>();// map with default value of Infinity
            gScore[start] = 0;

            // For node n, fScore[n] := gScore[n] + h(n). fScore[n] represents our current best guess as to
            // how short a path from start to finish can be if it goes through n.
            var fScore = new Dictionary<AStarNode, long>();//:= map with default value of Infinity
            fScore[start] = h(start);

            while (openSet.Any())
            {
                // This operation can occur in O(1) time if openSet is a min-heap or a priority queue
                var current = openSet.OrderBy(x => fScore[x]).FirstOrDefault();
                if (current == goal)
                {
                    return reconstruct_path(cameFrom, current);
                }

                openSet.Remove(current);

                foreach (var neighbor in GetNeighbours(current, map))
                {
                    // d(current,neighbor) is the weight of the edge from current to neighbor
                    // tentative_gScore is the distance from start to the neighbor through current
                    long tentative_gScore = gScore[current] + d(current, neighbor);
                    long neighbour_gScore = long.MaxValue;
                    if (gScore.ContainsKey(neighbor))
                    {
                        neighbour_gScore = gScore[neighbor];
                    }

                    if (tentative_gScore < neighbour_gScore)
                    {
                        // This path to neighbor is better than any previous one. Record it!
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentative_gScore;
                        fScore[neighbor] = tentative_gScore + h(neighbor);

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            // Open set is empty but goal was never reached
            return null;
        }

        static IEnumerable<AStarNode> GetNeighbours(AStarNode node, AStarNode[][] map)
        {
            int x = node.Coord.X;
            int y = node.Coord.Y;

            if (y > 0)
            {
                yield return map[y - 1][x];
            }

            if (x > 0)
            {
                yield return map[y][x - 1];
            }

            if (y < (map.Length - 1))
            {
                yield return map[y + 1][x];
            }

            if (x < (map[y].Length - 1))
            {
                yield return map[y][x + 1];
            }
        }

        static long d(AStarNode current, AStarNode neighbour)
        {
            return neighbour.C;
        }

        static List<AStarNode> reconstruct_path(Dictionary<AStarNode, AStarNode> cameFrom, AStarNode current)
        {
            var total_path = new List<AStarNode>()
            {
                current
            };

            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                total_path.Insert(0, current);
            };

            return total_path;
        }
}
}
