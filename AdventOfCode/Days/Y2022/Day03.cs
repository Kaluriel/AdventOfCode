using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2022
{
	public class Day03 : DayBase2022
	{
		private IEnumerable<string> RuckSacks = Enumerable.Empty<string>();

		protected override Task ExecuteSharedAsync()
		{
			RuckSacks = GetRuckSacks();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			return Task.FromResult<object>(
				RuckSacks
					  .Select(
					  	  x => x[..(x.Length / 2)].Intersect(x[(x.Length / 2)..])
					  							  .First()
					  )
					  .Select(CalculatePriority)
					  .Sum()
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			return Task.FromResult<object>(
				RuckSacks.Chunk(3)
						 .Select(
					  		 x => x.Skip(1)
								   .Aggregate(
									   x.First().AsEnumerable(),
									   (a, b) => a.Intersect(b)
								   )
								   .First()
						 )
						 .Select(CalculatePriority)
						 .Sum()
			);
		}

		private IEnumerable<string> GetRuckSacks()
		{
			return Source.SplitNewLine();
		}

		private static int CalculatePriority(char x)
		{
			return x - ((x > 96) ? 96 : 38);
		}
	}
}
