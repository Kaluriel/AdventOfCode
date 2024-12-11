using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Days.Y2022
{
	public sealed class Day04 : Day
	{
		private IEnumerable<KeyValuePair<Line1D, Line1D>> AssignmentPairs = Enumerable.Empty<KeyValuePair<Line1D, Line1D>>();

		protected override Task ExecuteSharedAsync()
		{
			AssignmentPairs = GetAssignmentPairs();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				AssignmentPairs.Count(x => x.Key.Contains(x.Value) || x.Value.Contains(x.Key))
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				AssignmentPairs.Count(x => x.Key.Overlaps(x.Value))
			);
		}

		private IEnumerable<KeyValuePair<Line1D, Line1D>> GetAssignmentPairs()
		{
			return Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
						 .Select(x => x.Split(new char[] { ',', '-' })
									   .Select(int.Parse)
									   .ToArray())
						 .Select(x => new KeyValuePair<Line1D, Line1D>(
										  new Line1D(x[0], x[1]),
										  new Line1D(x[2], x[3])
									  ));
		}
	}
}
