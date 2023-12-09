using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day09 : DayBase2023
	{
		private Int64[][] Numbers;

		protected override Task ExecuteSharedAsync()
		{
			Numbers = Source.SplitNewLine()
							.Select(x => x.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries)
											   .Select(Int64.Parse)
											   .ToArray())
							.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			List<List<Int64>>[] numbers = Numbers.Select(
													 x => new List<List<Int64>>()
													 {
														 new List<Int64>(x)
													 }
												 )
												 .ToArray();

			for (int i = 0; i < numbers.Length; ++i)
			{
				while (numbers[i][^1].Any(c => c != 0))
				{
					var newList = new List<long>();
					{
						for (int n = 1; n < numbers[i][^1].Count; ++n)
						{
							newList.Add(numbers[i][^1][n] - numbers[i][^1][n - 1]);
						}
					}
					numbers[i].Add(newList);
				}

				for (int n = numbers[i].Count - 2; n >= 0; --n)
				{
					numbers[i][n].Add(numbers[i][n][^1] + numbers[i][n + 1][^1]);
				}
			}
			
			return Task.FromResult<object>(
				numbers.Sum(x => x[0][^1])
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			List<List<Int64>>[] numbers = Numbers.Select(
													 x => new List<List<Int64>>()
													 {
														 new List<Int64>(x)
													 }
												 )
												 .ToArray();

			for (int i = 0; i < numbers.Length; ++i)
			{
				while (numbers[i][^1].Any(c => c != 0))
				{
					var newList = new List<long>();
					{
						for (int n = 1; n < numbers[i][^1].Count; ++n)
						{
							newList.Add(numbers[i][^1][n] - numbers[i][^1][n - 1]);
						}
					}
					numbers[i].Add(newList);
				}

				for (int n = numbers[i].Count - 2; n >= 0; --n)
				{
					numbers[i][n].Insert(0, numbers[i][n][0] - numbers[i][n + 1][0]);
				}
			}
			
			return Task.FromResult<object>(
				numbers.Sum(x => x[0][0])
			);
		}
	}
}
