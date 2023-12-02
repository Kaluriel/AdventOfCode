using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day11 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var grid = Source.SplitNewLine()
                             .Select(
                                 x => x.ToCharArray()
                                       .Select(char.ToString)
                                       .Select(int.Parse)
                                       .ToArray()
                             )
                             .ToArray();

            int flashCount = 0;

            for (int i = 0; i < 100; ++i)
            {
                // increase energy
                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        ++grid[y][x];
                    }
                }

                // flash
                bool hasFlashed;

                do
                {
                    hasFlashed = false;

                    for (int y = 0; y < grid.Length; ++y)
                    {
                        for (int x = 0; x < grid[y].Length; ++x)
                        {
                            if (grid[y][x] == 10)
                            {
                                FlashOctopus(x, y, grid);
                                hasFlashed = true;
                                ++grid[y][x];
                                ++flashCount;
                            }
                        }
                    }
                }
                while (hasFlashed);

                // reset
                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        if (grid[y][x] > 9)
                        {
                            grid[y][x] = 0;
                        }
                    }
                }

                // debug
                /*Console.WriteLine($"Step {i + 1}");

                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        Console.Write($"{grid[y][x]}");
                    }

                    Console.WriteLine();
                }*/
            }

            return Task.FromResult<object>(
                flashCount
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var grid = Source.SplitNewLine()
                             .Select(
                                 x => x.ToCharArray()
                                       .Select(char.ToString)
                                       .Select(int.Parse)
                                       .ToArray()
                             )
                             .ToArray();

            bool allFlashing = false;
            int i = 0;

            do
            {
                // increase energy
                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        ++grid[y][x];
                    }
                }

                // flash
                int flashCount = 0;
                bool hasFlashed;

                do
                {
                    hasFlashed = false;

                    for (int y = 0; y < grid.Length; ++y)
                    {
                        for (int x = 0; x < grid[y].Length; ++x)
                        {
                            if (grid[y][x] == 10)
                            {
                                FlashOctopus(x, y, grid);
                                hasFlashed = true;
                                ++flashCount;
                                ++grid[y][x];
                            }
                        }
                    }
                }
                while (hasFlashed);

                // reset
                for (int y = 0; y < grid.Length; ++y)
                {
                    for (int x = 0; x < grid[y].Length; ++x)
                    {
                        if (grid[y][x] > 9)
                        {
                            grid[y][x] = 0;
                        }
                    }
                }

                allFlashing = (flashCount == 100);
                ++i;
            }
            while (!allFlashing);

            return Task.FromResult<object>(
                i
            );
        }

        private static void FlashOctopus(int x, int y, int[][] grid)
        {
            for (int y2 = Math.Max(0, y - 1); y2 <= Math.Min(y + 1, grid.Length - 1); ++y2)
            {
                for (int x2 = Math.Max(0, x - 1); x2 <= Math.Min(x + 1, grid[y].Length - 1); ++x2)
                {
                    if (grid[y2][x2] <= 9)
                    {
                        ++grid[y2][x2];
                    }
                }
            }
        }
    }
}
