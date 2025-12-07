using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day07 : Day
	{
		private bool[][] _Splitters;
		private int _StartX;

		protected override async Task ExecuteSharedAsync()
		{
			var instructions = Source.SplitNewLineThenChars(StringSplitOptions.RemoveEmptyEntries);
			
			_Splitters = instructions
				.Skip(2)
				.Select(row => row.Select(column => column == '^').ToArray())
				.ToArray();
			
			_StartX = instructions[0].IndexOf(['S']);
			
			await base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var buffer = new bool[_Splitters[0].Length];
			long count = 0;

			buffer[_StartX] = true;

			for (int y = 0; y < _Splitters.Length; ++y)
			{
				for (int x = 0; x < _Splitters[y].Length; ++x)
				{
					if (!buffer[x])
					{
						continue;
					}
					
					if (_Splitters[y][x])
					{
						buffer[x - 1] = true;
						buffer[x] = false;
						buffer[x + 1] = true;
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
			var countBuffer = new []
			{
				new long[_Splitters[0].Length],
				new long[_Splitters[0].Length],
			};
			int writeIndex = 1;
			int readIndex = 0;

			countBuffer[readIndex][_StartX] = 1;

			for (int y = 0; y < _Splitters.Length; ++y)
			{
				Array.Clear(countBuffer[writeIndex], 0, countBuffer[writeIndex].Length);

				for (int x = 0; x < _Splitters[y].Length; ++x)
				{
					if (countBuffer[readIndex][x] <= 0)
					{
						continue;
					}
					
					if (_Splitters[y][x])
					{
						countBuffer[writeIndex][x - 1] += countBuffer[readIndex][x];
						countBuffer[writeIndex][x + 1] += countBuffer[readIndex][x];
					}
					else
					{
						countBuffer[writeIndex][x] += countBuffer[readIndex][x];
					}
				}

				(readIndex, writeIndex) = (writeIndex, readIndex);
			}

			return Task.FromResult<object>(
				countBuffer[readIndex].Sum()
			);
		}
	}
}
