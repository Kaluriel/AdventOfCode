using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public sealed class Day04 : Day
	{
		class Card
		{
			public int Id { get; set; }
			public int[] WinningNumbers { get; set; }
			public int[] Numbers { get; set; }
		}

		private Card[] Scratchcards;

		protected override Task ExecuteSharedAsync()
		{
			Scratchcards = Source.SplitNewLine()
								 .Select(x => x.Split(new[] { ": ", "|" }, StringSplitOptions.RemoveEmptyEntries)
													.Select(y => y.Split(new [] { ' '}, StringSplitOptions.RemoveEmptyEntries)))
								 .Select(
									 x => new Card()
									 {
										 Id = int.Parse(x.First()
														 .Skip(1)
														 .First()),
										 WinningNumbers = x.Skip(1)
														   .First()
														   .Select(int.Parse)
														   .ToArray(),
										 Numbers = x.Skip(2)
													.First()
													.Select(int.Parse)
													.ToArray(),
									 }
								 )
								 .ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				Scratchcards.Select(x => x.Numbers.Count(y => x.WinningNumbers.Contains(y)))
							.Sum(x => (int)Math.Pow(2, x - 1))
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			int[] winningCounts = Scratchcards.Select(x => x.Numbers.Count(y => x.WinningNumbers.Contains(y)))
											  .ToArray();
			int[] cardCounts = new int[Scratchcards.Length];
			for (int i = 0; i < cardCounts.Length; ++i)
			{
				cardCounts[i] = 1;
			}
			
			for (int i = 0; i < winningCounts.Length; ++i)
			{
				int start = i + 1;
				int end = Math.Min(start + winningCounts[i], cardCounts.Length);

				for (int c = start; c < end; ++c)
				{
					cardCounts[c] += cardCounts[i];
				}
			}
			
			return Task.FromResult<object>(
				cardCounts.Sum()
			);
		}
	}
}
