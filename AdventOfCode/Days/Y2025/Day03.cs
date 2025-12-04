using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day03 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Select(CharExt.AsInt).ToArray())
				.ToArray();
			int totalJoltage = 0;

			for (int y = 0; y < instructions.Length; ++y)
			{
				int maxFirstIndex = 0;

				for (int x = 1; x < instructions[y].Length - 1; ++x)
				{
					if (instructions[y][x] > instructions[y][maxFirstIndex])
					{
						maxFirstIndex = x;
					}
				}
				
				int maxSecondIndex = maxFirstIndex + 1;
				
				for (int x = maxSecondIndex + 1; x < instructions[y].Length; ++x)
				{
					if (instructions[y][x] > instructions[y][maxSecondIndex])
					{
						maxSecondIndex = x;
					}
				}

				int joltage = (instructions[y][maxFirstIndex] * 10) + instructions[y][maxSecondIndex];
				totalJoltage += joltage;
			}

			return Task.FromResult<object>(
				totalJoltage
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Select(CharExt.AsULong).ToArray())
				.ToArray();
			ulong totalJoltage = 0;
			const int BatteryCount = 12;

			for (int y = 0; y < instructions.Length; ++y)
			{
				var batteries = new ulong[BatteryCount];
				int startIndex = 0;

				for (int batteryIndex = 0; batteryIndex < BatteryCount; ++batteryIndex)
				{
					int rangeMax = (instructions[y].Length - startIndex) - (BatteryCount - (batteryIndex + 1));
					var availableNums = instructions[y]
						.Skip(startIndex)
						.Take(rangeMax)
						.ToArray();
					ulong highestNumber = availableNums.Max();
					
					batteries[batteryIndex] = highestNumber;
					
					startIndex = Array.IndexOf(instructions[y], highestNumber, startIndex) + 1;
				}

				ulong joltage = 0;

				for (int batteryIndex = 0; batteryIndex < BatteryCount; ++batteryIndex)
				{
					joltage += batteries[batteryIndex] * (ulong)Math.Pow(10, BatteryCount - batteryIndex - 1);
				}
				
				totalJoltage += joltage;
			}

			return Task.FromResult<object>(
				totalJoltage
			);
		}
	}
}
