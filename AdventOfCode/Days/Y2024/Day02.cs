using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day02 : DayBase2024
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountSafeReports()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountSafeReportsWithProblemDampener()
			);
		}

		private int CountSafeReports()
		{
			return Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(
					x => x
						.Split(" ", StringSplitOptions.RemoveEmptyEntries)
						.Select(int.Parse)
				)
				.Select(
					x => x.SelectWithNext((a, b) => a - b)
				)
				.Where(x => x.Select(Math.Sign).Distinct().Count() == 1)
				.Count(
					x => x
						.Select(a => Math.Abs(a) >= 1 && Math.Abs(a) <= 3)
						.All(a => a)
				);
		}

		private int CountSafeReportsWithProblemDampener()
		{
			return Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(
					x => x
						.Split(" ", StringSplitOptions.RemoveEmptyEntries)
						.Select(int.Parse)
						.PermutationsOfOneRemoved()
						.Select(y => y.SelectWithNext((a, b) => a - b))
						.Where(y => y.Select(Math.Sign).Distinct().Count() == 1)
				)
				.Count(
					x => x.Any(y => y
						.Select(a => Math.Abs(a) >= 1 && Math.Abs(a) <= 3)
						.All(a => a)
					)
				);
		}
	}
}
