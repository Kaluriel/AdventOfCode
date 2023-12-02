using AdventOfCode.Ext;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
	public class Day01 : DayBase2021
	{
		protected override Task<object> ExecutePart1Async()
		{
			var vals = Source.SplitNewLine()
							 .Select(int.Parse)
							 .ToArray();
			int count = 0;

			for (int i = 1; i < vals.Length; ++i)
			{
				count += (vals[i] > vals[i - 1]) ? 1 : 0;
			}

			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			var vals = Source.SplitNewLine().Select(int.Parse).ToArray();
			int count = 0;

			for (int i = 1; i < (vals.Length - 2); i += 1)
			{
				int prev = day1_Sum(vals, i - 1, i + 2);
				int curr = day1_Sum(vals, i, i + 3);
				count += (curr > prev) ? 1 : 0;
			}

			return Task.FromResult<object>(
				count
			);
		}

		private static int day1_Sum(int[] list, int start, int end)
		{
			int ret = 0;

			while (start < end)
			{
				ret += list[start];
				++start;
			}

			return ret;
		}
	}
}
