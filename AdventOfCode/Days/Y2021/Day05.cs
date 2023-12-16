using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day05 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var rows = Source.SplitNewLine()
                             .Select(
                                 x =>
                                 {
                                     var seg = x.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                                     var xy1 = seg[0].Split(new char[] { ',' });
                                     var xy2 = seg[2].Split(new char[] { ',' });
                                     return new
                                     {
                                         x1 = int.Parse(xy1[0]),
                                         y1 = int.Parse(xy1[1]),
                                         x2 = int.Parse(xy2[0]),
                                         y2 = int.Parse(xy2[1]),
                                     };
                                 }
                             ).ToArray();
            int maxX = rows.Max(x => Math.Max(x.x1, x.x2)) + 1;
            int maxY = rows.Max(x => Math.Max(x.y1, x.y2)) + 1;

            int[][] diagram = new int[maxX][];
            for (int x = 0; x < maxX; ++x)
            {
                diagram[x] = new int[maxY];
            }

            foreach (var row in rows)
            {
                if ((row.x1 != row.x2) && (row.y1 != row.y2))
                {
                    continue;
                }

                int dx = Math.Sign(row.x2 - row.x1);
                int dy = Math.Sign(row.y2 - row.y1);
                int x = row.x1;
                int y = row.y1;

                while (true)
                {
                    ++diagram[x][y];
                    if (x == row.x2 && y == row.y2)
                    {
                        break;
                    }
                    x += dx;
                    y += dy;
                }
            }

            int count = 0;

            for (int y = 0; y < maxY; ++y)
            {
                for (int x = 0; x < maxX; ++x)
                {
                    if (diagram[x][y] >= 2)
                    {
                        ++count;
                    }
                    //Console.Write($"{diagram[x][y]}");
                }

                //Console.WriteLine();
            }

            return Task.FromResult<object>(
                count
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var rows = Source.SplitNewLine()
                .Select(
                    x =>
                    {
                        var seg = x.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        var xy1 = seg[0].Split(new char[] { ',' });
                        var xy2 = seg[2].Split(new char[] { ',' });
                        return new
                        {
                            x1 = int.Parse(xy1[0]),
                            y1 = int.Parse(xy1[1]),
                            x2 = int.Parse(xy2[0]),
                            y2 = int.Parse(xy2[1]),
                        };
                    }
                ).ToArray();
            int maxX = rows.Max(x => Math.Max(x.x1, x.x2)) + 1;
            int maxY = rows.Max(x => Math.Max(x.y1, x.y2)) + 1;

            int[][] diagram = new int[maxX][];
            for (int x = 0; x < maxX; ++x)
            {
                diagram[x] = new int[maxY];
            }

            foreach (var row in rows)
            {
                int dx = Math.Sign(row.x2 - row.x1);
                int dy = Math.Sign(row.y2 - row.y1);
                int x = row.x1;
                int y = row.y1;

                while (true)
                {
                    ++diagram[x][y];
                    if (x == row.x2 && y == row.y2)
                    {
                        break;
                    }
                    x += dx;
                    y += dy;
                }
            }

            int count = 0;

            for (int y = 0; y < maxY; ++y)
            {
                for (int x = 0; x < maxX; ++x)
                {
                    if (diagram[x][y] >= 2)
                    {
                        ++count;
                    }
                    //Console.Write($"{diagram[x][y]}");
                }

                //Console.WriteLine();
            }

            return Task.FromResult<object>(
                count
            );
        }
    }
}
