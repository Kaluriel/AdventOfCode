using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day09 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var grid = Source.SplitNewLine()
                             .Select(
                                 x => x.Select(c => int.Parse(c.ToString()))
                                       .ToArray()
                             )
                             .ToArray();

            var riskLevels = GetLowestLocations(grid)
                .Select(loc => grid[loc.Y][loc.X])
                .Sum(x => x + 1);

            return Task.FromResult<object>(
                riskLevels
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var grid = Source.SplitNewLine()
                .Select(
                    x => x.Select(c => int.Parse(c.ToString()))
                          .ToArray()
                )
                .ToArray();

            int val = GetLowestLocations(grid)
                .Select(x => GetBasinSize(x, grid))
                .OrderByDescending(x => x)
                .Take(3)
                .Aggregate(1, (x, y) => x * y);

            return Task.FromResult<object>(
                val
            );
        }

        private static IEnumerable<Point2D> GetLowestLocations(int[][] grid)
        {
            for (int y = 0; y < grid.Length; ++y)
            {
                for (int x = 0; x < grid[y].Length; ++x)
                {
                    bool isLowest = grid[y][x] < 9;

                    if ((((y - 1) >= 0) && (grid[y - 1][x] < grid[y][x])) ||
                        (((y + 1) < grid.Length) && (grid[y + 1][x] < grid[y][x])) ||
                        (((x - 1) >= 0) && (grid[y][x - 1] < grid[y][x])) ||
                        (((x + 1) < grid[y].Length) && (grid[y][x + 1] < grid[y][x])))
                    {
                        isLowest = false;
                    }

                    if (isLowest)
                    {
                        //Console.WriteLine($"{x},{y}");
                        yield return new Point2D(x, y);
                    }
                }
            }
        }

        private static int GetBasinSize(Point2D point, int[][] grid)
        {
            // no out of bounds or 9's
            if ((point.X < 0) || (point.Y < 0) ||
                (point.Y >= grid.Length) ||
                (point.X >= grid[point.Y].Length) ||
                (grid[point.Y][point.X] == 9))
            {
                return 0;
            }

            // mark as traversed
            grid[point.Y][point.X] = 9;

            // this basic point counts
            int i = 1;

            // and adjacent recursively until we can't go anymore
            i += GetBasinSize(new Point2D(point.X, point.Y - 1), grid);
            i += GetBasinSize(new Point2D(point.X, point.Y + 1), grid);
            i += GetBasinSize(new Point2D(point.X - 1, point.Y), grid);
            i += GetBasinSize(new Point2D(point.X + 1, point.Y), grid);

            return i;
        }
    }
}
