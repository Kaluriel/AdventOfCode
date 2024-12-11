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
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day11 : Day
	{
		private Int64[] EngravedStones;

		protected override Task ExecuteSharedAsync()
		{
			EngravedStones = Source
				.Split(' ', StringSplitOptions.RemoveEmptyEntries)
				.Select(Int64.Parse)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountEngravedStonesAfterBlinks(25)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountEngravedStonesAfterBlinks(75)
			);
		}

		private Int64 CountEngravedStonesAfterBlinks(int blinkCount)
		{
			Dictionary<Int64, Int64> engravedStones = new Dictionary<Int64, Int64>(1000000);
			Dictionary<Int64, Int64> engravedStonesTmp = new Dictionary<Int64, Int64>(1000000);

			foreach (var stone in EngravedStones)
			{
				engravedStones.Add(stone, 1);
			}

			for (int blink = 0; blink < blinkCount; ++blink)
			{
				engravedStonesTmp.Clear();
				
				foreach (var stone in engravedStones)
				{
					if (stone.Key == 0)
					{
						if (!engravedStonesTmp.TryAdd(1, stone.Value))
						{
							engravedStonesTmp[1] += stone.Value;
						}
					}
					else
					{
						Int64 digitCount = MathExt.DigitCount(stone.Key);
						if ((digitCount & 1) == 0)
						{
							Int64 halfDigits = (Int64)Math.Pow(10, digitCount * 0.5);

							Int64 leftEngravedValue = stone.Key % halfDigits;
							if (!engravedStonesTmp.TryAdd(leftEngravedValue, stone.Value))
							{
								engravedStonesTmp[leftEngravedValue] += stone.Value;
							}

							Int64 rightEngravedValue = stone.Key / halfDigits;
							if (!engravedStonesTmp.TryAdd(rightEngravedValue, stone.Value))
							{
								engravedStonesTmp[rightEngravedValue] += stone.Value;
							}
						}
						else
						{
							Int64 engravedValue = stone.Key * 2024;
							if (!engravedStonesTmp.TryAdd(engravedValue, stone.Value))
							{
								engravedStonesTmp[engravedValue] += stone.Value;
							}
						}
					}
				}

				(engravedStones, engravedStonesTmp) = (engravedStonesTmp, engravedStones);
			}

			return engravedStones.Sum(x => x.Value);
		}
	}
}
