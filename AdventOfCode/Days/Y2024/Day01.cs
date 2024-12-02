using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day01 : DayBase2024
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetTotalDistance()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetSimilarityScore()
			);
		}

		private int GetTotalDistance()
		{
			return Source
				.SplitNewLineAndSpaces(StringSplitOptions.RemoveEmptyEntries)
				.Select((x, index) => new { Value = int.Parse(x), ListGroup = index & 1 })
				.GroupBy(x => x.ListGroup, x => x.Value)
				.Select(
					g => g
						.OrderBy(x => x)
						.Select((x, index) => new { Value = x, Index = index })
				)
				.SelectWithNext(
					(listX, listY) => listX
						.GroupJoin(listY, x => x.Index, y => y.Index, (x, y) => new { a = x.Value, b = y.First().Value })
						.Select(x => Math.Abs(x.a - x.b))
						.Sum()
				)
				.First();
		}

		private int GetSimilarityScore()
		{
			return Source
				.SplitNewLineAndSpaces(StringSplitOptions.RemoveEmptyEntries)
				.Select((x, index) => new { Value = int.Parse(x), ListGroup = index & 1 })
				.GroupBy(x => x.ListGroup, x => x.Value)
				.SelectWithNext(
					(listX, listY) => listX
						.Select(x => x * listY.Count(y => y == x))
						.Sum()
				)
				.First();
		}
	}
}
