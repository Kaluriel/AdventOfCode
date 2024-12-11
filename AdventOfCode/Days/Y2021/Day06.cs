using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public sealed class Day06 : Day
    {
        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var fish_ages = Source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
            int[] ages = new int[9];

            for (int i = 0; i < 9; ++i)
            {
                ages[i] = fish_ages.Count(x => x == i);
            }

            for (int day = 0; day < 80; ++day)
            {
                int newFish = ages[0];

                for (int i = 1; i < 9; ++i)
                {
                    ages[i - 1] = ages[i];
                }
             
                ages[6] += newFish;
                ages[8] = newFish;
            }

            int totalFish = ages.Sum(x => x);

            return Task.FromResult<object>(
                totalFish
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var fish_ages = Source.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x));
            long[] ages = new long[9];

            for (int i = 0; i < 9; ++i)
            {
                ages[i] = fish_ages.Count(x => x == i);
            }

            for (int day = 0; day < 256; ++day)
            {
                long newFish = ages[0];

                for (int i = 1; i < 9; ++i)
                {
                    ages[i - 1] = ages[i];
                }

                ages[6] += newFish;
                ages[8] = newFish;
            }

            long totalFish = ages.Sum(x => x);

            return Task.FromResult<object>(
                totalFish
            );
        }
    }
}
