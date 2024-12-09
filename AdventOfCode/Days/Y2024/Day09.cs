using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day09 : DayBase2024
	{
		protected override Task ExecuteSharedAsync()
		{
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumUniqueAntinodeLocations()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumUniqueAntinodeLocationsIgnoreDistance()
			);
		}

		private int NumUniqueAntinodeLocations()
		{
			return 1;
		}

		private int NumUniqueAntinodeLocationsIgnoreDistance()
		{
			return 1;
		}
	}
}
