using AdventOfCode.DataTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day17 : DayBase2021
    {
        struct D17Range
        {
            public int min { get; set; }
            public int max { get; set; }

            public D17Range(int _start, int _end)
            {
                min = Math.Min(_start, _end);
                max = Math.Max(_start, _end);
            }
        }

        protected override Task<object> ExecutePart1Async()
        {
            var parts = Source.Split(',')
                                  .Select(x => x.Split('=')[1])
                                  .Select(x => x.Split("..")
                                                .Select(int.Parse)
                                                .ToArray()
                                  )
                                  .ToArray();
            D17Range xRange = new D17Range(parts[0][0], parts[0][1]);
            D17Range yRange = new D17Range(parts[1][0], parts[1][1]);

            // x = (n * (n + 1)) / 2;
            const float quarter = 1.0f / 4.0f;
            const float half = 1.0f / 2.0f;
            int xLow = (int)(Math.Sqrt(2.0f * (float)xRange.min + quarter) - half);
            int xHigh = (int)(Math.Sqrt(2.0f * (float)xRange.max + quarter) - half);

            Point2D start = new Point2D(0, 0);
            int maxY = 0;

            for (int xIV = xLow; xIV <= xHigh; ++xIV)
            {
                for (int yIV = 0; yIV < 1000; ++yIV)
                {
                    int highestY = int.MinValue;
                    int xV = xIV;
                    int yV = yIV;
                    int x = 0;
                    int y = 0;

                    while (y >= yRange.min)
                    {
                        x += xV;
                        y += yV;

                        highestY = Math.Max(highestY, y);

                        if (IsInRange(x, y, xRange, yRange))
                        {
                            maxY = Math.Max(highestY, maxY);
                            break;
                        }

                        xV = Math.Max(xV - 1, 0);
                        --yV;
                    }
                }
            }

            //
            return Task.FromResult<object>(
                maxY
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var parts = Source.Split(',')
                              .Select(x => x.Split('=')[1])
                              .Select(x => x.Split("..")
                                            .Select(int.Parse)
                                            .ToArray()
                              )
                              .ToArray();
            D17Range xRange = new D17Range(parts[0][0], parts[0][1]);
            D17Range yRange = new D17Range(parts[1][0], parts[1][1]);

            // x = (n * (n + 1)) / 2;
            const float quarter = 1.0f / 4.0f;
            const float half = 1.0f / 2.0f;
            int xLow = (int)(Math.Sqrt(2.0f * (float)xRange.min + quarter) - half);
            int xHigh = (int)(Math.Sqrt(2.0f * (float)xRange.max + quarter) - half);

            Point2D start = new Point2D(0, 0);
            int count = 0;

            for (int xIV = -1000; xIV < 1000; ++xIV)
            {
                for (int yIV = -1000; yIV < 1000; ++yIV)
                {
                    int xV = xIV;
                    int yV = yIV;
                    int x = 0;
                    int y = 0;

                    while ((x <= xRange.max) && (y >= yRange.min))
                    {
                        x += xV;
                        y += yV;

                        if (IsInRange(x, y, xRange, yRange))
                        {
                            ++count;
                            break;
                        }

                        xV = Math.Max(xV - 1, 0);
                        --yV;
                    }
                }
            }

            //
            return Task.FromResult<object>(
                count
            );
        }

        private static bool IsInRange(int x, int y, D17Range xRange, D17Range yRange)
        {
            if ((x >= xRange.min) && (x <= xRange.max) &&
                (y >= yRange.min) && (y <= yRange.max))
            {
                return true;
            }

            return false;
        }
    }
}
