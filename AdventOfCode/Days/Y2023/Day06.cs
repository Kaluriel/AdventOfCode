using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day06 : DayBase2023
	{
		private int[][] RaceDataP1;
		private Int64[] RaceDataP2;
		
		protected override Task ExecuteSharedAsync()
		{
			RaceDataP1 = Source.SplitNewLine()
							   .Select(x => x.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
												  .Skip(1)
												  .Select(int.Parse)
												  .ToArray())
							   .ToArray();
			RaceDataP2 = Source.SplitNewLine()
							   .Select(x => x.Replace(" ", "")
												  .Split(new[] { ':' }, StringSplitOptions.RemoveEmptyEntries)
												  .Skip(1)
												  .Select(Int64.Parse))
							   .Select(x => x.First())
							   .ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			int[] errorMargin = new int[RaceDataP1[0].Length];

			for (int i = 0; i < RaceDataP1[0].Length; ++i)
			{
				int recordDistance = RaceDataP1[1][i];

				for (int chargeTime = 0; chargeTime < RaceDataP1[0][i]; ++chargeTime)
				{
					int distance = chargeTime * (RaceDataP1[0][i] - chargeTime);
					if (distance > recordDistance)
					{
						++errorMargin[i];
					}
				}
			}
			
			return Task.FromResult<object>(
				errorMargin.Aggregate(1, (x, y) => x * y)
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			Int64 errorMargin = 0;

			Int64 recordDistance = RaceDataP2[1];

			for (int chargeTime = 0; chargeTime < RaceDataP2[0]; ++chargeTime)
			{
				Int64 distance = chargeTime * (RaceDataP2[0] - chargeTime);
				if (distance > recordDistance)
				{
					++errorMargin;
				}
			}
			
			return Task.FromResult<object>(
				errorMargin
			);
		}
	}
}
