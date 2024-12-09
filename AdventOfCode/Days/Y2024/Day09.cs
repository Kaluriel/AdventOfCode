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
		private struct Block
		{
			public int File { get; set; }
			public int Count { get; set; }
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var fileSystem = Source
					.SelectMany(
						(f, i) => Enumerable.Repeat<Int64>(
							((i & 1) == 0)
								? i / 2
								: -1,
							f - '0'
						)
					)
					.ToList();

			for (int i = 0; i < fileSystem.Count; ++i)
			{
				if (fileSystem[i] == -1)
				{
					int lastIndex = fileSystem.FindLastIndex(x => x != -1);
					if ((lastIndex == -1) || (i >= lastIndex))
					{
						break;
					}

					(fileSystem[i], fileSystem[lastIndex]) = (fileSystem[lastIndex], fileSystem[i]);
				}
			}
			
			return Task.FromResult<object>(
				CalculateChecksum(
					fileSystem
				)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var fileSystem = Source
				.Select(
					(f, i) => new Block
					{
						File = ((i & 1) == 0)
							? i / 2
							: -1,
						Count = f - '0',
					}
				)
				.Where(f => f.Count > 0)
				.ToList();
			
			for (int i = fileSystem.Count - 1; i >= 0; --i)
			{ 
				if (fileSystem[i].File != -1)
				{
					int blockCount = fileSystem[i].Count;

					int index = fileSystem.FindIndex(x => (x.File == -1) && (x.Count >= blockCount));
					if ((index == -1) || (index >= i))
					{
						continue;
					}

					int freeBlockSize = fileSystem[index].Count;

					fileSystem[index] = fileSystem[i];
					fileSystem[i] = new Block
					{
						File = -1,
						Count = blockCount,
					};

					if (freeBlockSize > blockCount)
					{
						fileSystem.Insert(index + 1, new Block { File = -1, Count = freeBlockSize - blockCount });
						++i;
					}
				}
			}
			
			return Task.FromResult<object>(
				CalculateChecksum(
					fileSystem.SelectMany(x => Enumerable.Repeat<Int64>(x.File, x.Count))
				)
			);
		}

		private Int64 CalculateChecksum(IEnumerable<Int64> values)
		{
			return values
				.Select((f, i) => f * i)
				.Where(x => x >= 0)
				.Sum();
		}
	}
}
