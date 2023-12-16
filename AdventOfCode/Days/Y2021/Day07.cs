using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day07 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var positions = Source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .OrderBy(x => x)
                .ToArray();
            int leastFuel = int.MaxValue;

            for (int i = 0; i <= positions.Last(); ++i)
            {
                int fuel = 0;

                foreach (var position in positions)
                {
                    fuel += Math.Abs(position - i);
                }

                if (fuel < leastFuel)
                {
                    leastFuel = fuel;
                }
            }

            return Task.FromResult<object>(
                leastFuel
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var positions = Source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse)
                .OrderBy(x => x)
                .ToArray();
            int leastFuel = int.MaxValue;

            for (int i = 0; i <= positions.Last(); ++i)
            {
                int fuel = 0;

                foreach (var position in positions)
                {
                    int n = Math.Abs(position - i);
                    fuel += (n * (n + 1)) / 2;
                }

                if (fuel < leastFuel)
                {
                    leastFuel = fuel;
                }
            }

            return Task.FromResult<object>(
                leastFuel
            );
        }
    }
}
