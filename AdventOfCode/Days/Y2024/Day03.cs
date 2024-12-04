using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day03 : DayBase2024
	{
		private readonly Regex Regex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfMultiplications()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfMultiplicationsWithConditionals()
			);
		}

		private int SumOfMultiplications()
		{
			return Regex.Matches(Source)
				.Select(x => x.Groups.Values
					.Skip(1)
					.Select(y => int.Parse(y.Value))
					.Aggregate(1, (a, b) => a * b)
				)
				.Sum();
		}

		private int SumOfMultiplicationsWithConditionals()
		{
			return Regex.Matches(
				Source
					.Split("do()")
					.Select(x => x.Split("don't()").First())
					.Aggregate("", (a, b) => a + b)
				)
				.Select(x => x.Groups.Values
					.Skip(1)
					.Select(y => int.Parse(y.Value))
					.Aggregate(1, (a, b) => a * b)
				)
				.Sum();
		}
	}
}
