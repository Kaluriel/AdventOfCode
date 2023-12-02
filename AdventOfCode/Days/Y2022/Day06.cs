using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public class Day06 : DayBase2022
	{
		protected override Task<object> ExecutePart1Async()
		{
			int marker = FindDistinctCharacters(4);

			return Task.FromResult<object>(
				marker
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			int marker = FindDistinctCharacters(14);

			return Task.FromResult<object>(
				marker
			);
		}

		private int FindDistinctCharacters(int distinctCount)
		{
			// without custom enumerator
			/*return Source.Skip(distinctCount)
						 .Select(
							 (x, i) => new
							 {
								 Index = i + distinctCount,
								 Count = Source.Skip(i)
											   .Take(distinctCount)
											   .Distinct()
											   .Count(),
							 }
						 )
						 .First(x => x.Count == distinctCount)
						 .Index;*/

			// with custom enumerator
			return Source.Chunk(chunksize: distinctCount, stride: 1)
						 .Select(
							 (x, i) => new
							 {
								 Index = i + distinctCount,
								 DistinctCount = x.Distinct()
												  .Count(),
							 }
						 )
						 .First(x => x.DistinctCount == distinctCount)
						 .Index;
		}
	}
}
