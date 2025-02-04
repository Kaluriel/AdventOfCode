﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2022
{
	public sealed class Day01 : Day
	{
		private IEnumerable<int> TotalCalories = Enumerable.Empty<int>();

		protected override Task ExecuteSharedAsync()
		{
			TotalCalories = GetTotalCalories();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				TotalCalories.Max()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				TotalCalories.OrderByDescending(x => x)
							 .Take(3)
							 .Sum()
			);
		}

		private IEnumerable<int> GetTotalCalories()
		{
			return Source.SplitDoubleNewLine()
						 .Select(x => x.SplitNewLine()
						 			   .Select(int.Parse)
						 			   .Sum());
		}
	}
}
