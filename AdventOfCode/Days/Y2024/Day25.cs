using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day25 : Day
	{
		private int[][] Locks;
		private int[][] Keys;
		private const int MaxHeight = 5;

		protected override Task ExecuteSharedAsync()
		{
			var locksAndKeys = Source
				.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(lines => lines
					.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
					.SelectMany((line, rowIndex) => line
						.Select((character, colIndex) => (character, colIndex, rowIndex)))
					.OrderBy(x => x.rowIndex)
					.GroupBy(i => i.colIndex, i => i.character)
					.ToArray())
				.Select(grp => (
					grp.First().First() == '#',
					grp.Select(x => x.Count(c => c == '#') - 1).ToArray())
				)
				.ToArray();
			
			Locks = locksAndKeys
				.Where(x => x.Item1)
				.Select(x => x.Item2)
				.ToArray();
			
			Keys = locksAndKeys
				.Where(x => !x.Item1)
				.Select(x => x.Item2)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			int count = 0;

			foreach (var @lock in Locks)
			{
				foreach (var key in Keys)
				{
					bool allOkay = true;

					for (int i = 0; i < key.Length; ++i)
					{
						allOkay = allOkay && ((@lock[i] + key[i]) <= MaxHeight);
					}

					if (allOkay)
					{
						++count;
					}
				}
			}
			
			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				1
			);
		}
	}
}
