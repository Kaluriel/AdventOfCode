using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Threading;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day05 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);
			int count = 0;
			
			var ranges = instructions[0].SplitNewLine()
				.Select(x => x.Split('-').Select(ulong.Parse).ToArray())
				.ToArray();
			
			var numbers = instructions[1].SplitNewLine()
				.Select(ulong.Parse)
				.ToArray();

			foreach (var number in numbers)
			{
				foreach (var range in ranges)
				{
					if ((number >= range[0]) && (number <= range[1]))
					{
						++count;
						break;
					}
				}
			}

			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);
			BigInteger count = 0;
			
			var ranges = instructions[0].SplitNewLine()
				.Select(x => x.Split('-').Select(ulong.Parse).ToArray())
				.OrderBy(x => x[0])
				.ThenBy(x => x[1])
				.ToList();

			bool hasChanged;
			do
			{
				hasChanged = false;
				
				for (int r1 = 0; r1 < ranges.Count; ++r1)
				{
					for (int r2 = r1 + 1; r2 < ranges.Count; ++r2)
					{
						if (ranges[r1][0] <= ranges[r2][1] && ranges[r2][0] <= ranges[r1][1])
						{
							ulong newStart = Math.Min(ranges[r1][0], ranges[r2][0]);
							ulong newEnd = Math.Max(ranges[r1][1], ranges[r2][1]);
							
							ranges[r1] =
							[
								newStart,
								newEnd
							];

							ranges.RemoveAt(r2);
							--r2;
							hasChanged = true;
						}
					}
				}
			} while (hasChanged);

			foreach (var range in ranges)
			{
				count += range[1];
				count -= range[0];
				count += 1;
			}

			return Task.FromResult<object>(
				count
			);
		}
	}
}
