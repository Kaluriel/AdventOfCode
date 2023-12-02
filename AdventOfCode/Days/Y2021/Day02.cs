using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day02 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var vals = Source.SplitNewLine()
                             .Select(x => x.Split(' '))
                             .Select(x => new KeyValuePair<string, int>(x[0], int.Parse(x[1])))
                             .ToArray();
            int x = 0;
            int y = 0;

            foreach (var val in vals)
            {
                switch (val.Key)
                {
                    case "down":
                        y += val.Value;
                        break;

                    case "forward":
                        x += val.Value;
                        break;

                    case "up":
                        y -= val.Value;
                        break;
                }
            }

            return Task.FromResult<object>(
                x * y
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var vals = Source.SplitNewLine()
                .Select(x => x.Split(' '))
                .Select(x => new KeyValuePair<string, int>(x[0], int.Parse(x[1])))
                .ToArray();
            int x = 0;
            int y = 0;
            int aim = 0;

            foreach (var val in vals)
            {
                switch (val.Key)
                {
                    case "down":
                        aim += val.Value;
                        break;

                    case "up":
                        aim -= val.Value;
                        break;

                    case "forward":
                        x += val.Value;
                        y += aim * val.Value;
                        break;
                }
            }

            return Task.FromResult<object>(
                x * y
            );
        }
    }
}
