using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day03 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var vals = Source.SplitNewLine()
                             .ToArray();

            string gammaRateBinary = GetMostCommonBits(vals);
            string epsilonRateBinary = InvertBits(gammaRateBinary);

            int gammaRate = Convert.ToInt32(gammaRateBinary, 2);
            int epsilonRate = Convert.ToInt32(epsilonRateBinary, 2);

            return Task.FromResult<object>(
                gammaRate * epsilonRate
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var vals = Source.SplitNewLine()
                .ToArray();

            var oxygenRatingBinary = FilterBitCriteria(vals, false);
            var co2ScubberRatingBinary = FilterBitCriteria(vals, true);

            int oxygenRating = Convert.ToInt32(oxygenRatingBinary, 2);
            int co2ScubberRating = Convert.ToInt32(co2ScubberRatingBinary, 2);

            return Task.FromResult<object>(
                oxygenRating * co2ScubberRating
            );
        }

        static string FilterBitCriteria(string[] list, bool invert)
        {
            int count = list[0].Length;
            string str = string.Empty;

            for (int index = 0; (index < count) && (list.Length > 1); ++index)
            {
                char mcb = GetMostCommonBit(list, index);
                if (invert)
                {
                    mcb = InvertBit(mcb);
                }
                list = list.Where(x => x[index] == mcb).ToArray();
            }

            return list.FirstOrDefault();
        }

        static char GetMostCommonBit(string[] list, int bit)
        {
            char str = '1';

            int oneCount = list.Count(x => x[bit] == '1');
            int zeroCount = list.Length - oneCount;

            if (oneCount > zeroCount)
            {
                str = '1';
            }
            else if (oneCount < zeroCount)
            {
                str = '0';
            }

            return str;
        }

        static string GetMostCommonBits(string[] list)
        {
            string str = string.Empty;

            for (int index = 0; index < list[0].Length; ++index)
            {
                str += GetMostCommonBit(list, index);
            }

            return str;
        }

        static string InvertBits(string bin)
        {
            string str = string.Empty;

            for (int index = 0; index < bin.Length; ++index)
            {
                str += InvertBit(bin[index]);
            }

            return str;
        }

        static char InvertBit(char bit)
        {
            char str = '0';

            if (bit == '0')
            {
                str = '1';
            }

            return str;
        }
    }
}
