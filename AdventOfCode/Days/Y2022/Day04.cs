using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2022
{
	public class Day04 : DayBase2022
	{
		private IEnumerable<KeyValuePair<Range, Range>> AssignmentPairs = Enumerable.Empty<KeyValuePair<Range, Range>>();

		protected override Task ExecuteSharedAsync()
		{
			AssignmentPairs = GetAssignmentPairs();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			return Task.FromResult<object>(
				AssignmentPairs.Count(
								   x => x.Key.Contains(x.Value) || x.Value.Contains(x.Key)
							   )
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			return Task.FromResult<object>(
				AssignmentPairs.Count(
								   x => x.Key.Overlaps(x.Value)
							   )
			);
		}

		private IEnumerable<KeyValuePair<Range, Range>> GetAssignmentPairs()
		{
			return Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
						 .Select(
							  x => x.Split(',')
									.Select(y => y.Split('-').Select(int.Parse).ToArray())
									.ToArray()
						 )
						 .Select(
							 x => new KeyValuePair<Range, Range>(
								 new Range(x[0][0], x[0][1]),
								 new Range(x[1][0], x[1][1])
							 )
						 );
		}
	}
}
