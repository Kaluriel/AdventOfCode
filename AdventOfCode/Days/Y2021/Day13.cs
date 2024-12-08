using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day13 : DayBase2021
    {
        struct Fold
        {
            static readonly char[] FoldSplit = new char[] { '=' };

            public bool isXAxis;
            public int axisValue;

            public Fold(string str)
            {
                var parts = str.Split(FoldSplit);
                isXAxis = parts[0] == "x";
                axisValue = int.Parse(parts[1]);
            }
        }

        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var data = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .GroupBy(
                                 x => x.StartsWith("fold along "),
                                 (k, g) => new { IsInstructions = k, Items = g.Select(x => x.Split(new char[] { ' ' })[^1]) }
                             )
                             .Aggregate(
                                 new { Points = new List<Point2D>(), Instructions = new List<Fold>() },
                                 (x, y) =>
                                 {
                                     if (y.IsInstructions)
                                     {
                                         x.Instructions.AddRange(
                                             y.Items.Select(x => new Fold(x))
                                         );
                                     }
                                     else
                                     {
                                         x.Points.AddRange(
                                             y.Items.Select(x => new Point2D(x))
                                         );
                                     }
                             
                                     return x;
                                 }
                             );

            // create grid
            int limitX = data.Points.Max(p => p.X) + 1;
            int limitY = data.Points.Max(p => p.Y) + 1;
            var grid = CreateGrid(limitX, limitY);

            // fill grid
            foreach (var point in data.Points)
            {
                grid[point.Y][point.X] = '#';
            }

            //DrawGrid(grid);

            // Fold and count
            int dotCount = 0;

            grid = FoldGrid(grid, data.Instructions.First());
            //DrawGrid(grid);

            for (int y = 0; y < grid.Length; ++y)
            {
                for (int x = 0; x < grid[y].Length; ++x)
                {
                    if (grid[y][x] == '#')
                    {
                        ++dotCount;
                    }
                }
            }

            //
            return Task.FromResult<object>(
                dotCount
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var data = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .GroupBy(
                                 x => x.StartsWith("fold along "),
                                 (k, g) => new { IsInstructions = k, Items = g.Select(x => x.Split(new char[] { ' ' })[^1]) }
                             )
                             .Aggregate(
                                 new { Points = new List<Point2D>(), Instructions = new List<Fold>() },
                                 (x, y) =>
                                 {
                                     if (y.IsInstructions)
                                     {
                                         x.Instructions.AddRange(
                                             y.Items.Select(x => new Fold(x))
                                         );
                                     }
                                     else
                                     {
                                         x.Points.AddRange(
                                             y.Items.Select(x => new Point2D(x))
                                         );
                                     }
                             
                                     return x;
                                 }
                             );

            // create grid
            int limitX = data.Points.Max(p => p.X) + 1;
            int limitY = data.Points.Max(p => p.Y) + 1;
            var grid = CreateGrid(limitX, limitY);

            // fill grid
            foreach (var point in data.Points)
            {
                grid[point.Y][point.X] = '#';
            }

            //DrawGrid(grid);

            // fold
            foreach (var instruction in data.Instructions)
            {
                grid = FoldGrid(grid, instruction);
                //DrawGrid(grid);
            }

            return Task.FromResult<object>(
                DrawGrid(grid)
            );
        }

        private static char[][] CreateGrid(int sizeX, int sizeY)
        {
            var grid = new char[sizeY][];

            for (int y = 0; y < grid.Length; ++y)
            {
                grid[y] = new char[sizeX];

                for (int x = 0; x < grid[y].Length; ++x)
                {
                    grid[y][x] = '.';
                }
            }

            return grid;
        }

        private static char[][] FoldGrid(char[][] grid, Fold fold)
        {
            char[][] newGrid = CreateGrid(
                fold.isXAxis ? fold.axisValue : grid[0].Length,
                fold.isXAxis ? grid.Length : fold.axisValue
            );

            for (int y = 0; y < newGrid.Length; ++y)
            {
                for (int x = 0; x < newGrid[y].Length; ++x)
                {
                    newGrid[y][x] = grid[y][x];
                }
            }

            for (int y = 0; y < newGrid.Length; ++y)
            {
                for (int x = 0; x < newGrid[y].Length; ++x)
                {
                    if (newGrid[y][x] != '.')
                    {
                        continue;
                    }

                    if (fold.isXAxis)
                    {
                        newGrid[y][x] = grid[y][grid[y].Length - 1 - x];
                    }
                    else
                    {
                        newGrid[y][x] = grid[grid.Length - 1 - y][x];
                    }
                }
            }

            return newGrid;
        }
    }
}
