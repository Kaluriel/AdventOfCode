using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2022
{
	public class Day02 : DayBase2022
	{
		private static IReadOnlyDictionary<char, IReadOnlyDictionary<char, int>> WinLUT = new Dictionary<char, IReadOnlyDictionary<char, int>>()
		{
			{
				'A',
				new Dictionary<char, int>()
				{
					{ 'X', 0 },
					{ 'Y', 1 },
					{ 'Z', -1 },
				}
			},
			{
				'B',
				new Dictionary<char, int>()
				{
					{ 'X', -1 },
					{ 'Y', 0 },
					{ 'Z', 1 },
				}
			},
			{
				'C',
				new Dictionary<char, int>()
				{
					{ 'X', 1 },
					{ 'Y', -1 },
					{ 'Z', 0 },
				}
			},
		};
		private static IReadOnlyDictionary<char, IReadOnlyDictionary<char, char>> EndLUT = new Dictionary<char, IReadOnlyDictionary<char, char>>()
		{
			{
				'A',
				new Dictionary<char, char>()
				{
					{ 'X', 'Z' },
					{ 'Y', 'X' },
					{ 'Z', 'Y' },
				}
			},
			{
				'B',
				new Dictionary<char, char>()
				{
					{ 'X', 'X' },
					{ 'Y', 'Y' },
					{ 'Z', 'Z' },
				}
			},
			{
				'C',
				new Dictionary<char, char>()
				{
					{ 'X', 'Y' },
					{ 'Y', 'Z' },
					{ 'Z', 'X' },
				}
			},
		};
		private IEnumerable<KeyValuePair<char, char>> RPSPairs = Enumerable.Empty<KeyValuePair<char, char>>();

		protected override Task ExecuteSharedAsync()
		{
			RPSPairs = GetRPSPairs();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				RPSPairs.Select(CalculateScore)
						.Sum()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				RPSPairs.Select(
							x => new KeyValuePair<char, char>(x.Key, EndLUT[x.Key][x.Value])
						)
						.Select(CalculateScore)
						.Sum()
			);
		}

		private static int CalculateScore(KeyValuePair<char, char> rpsPair)
		{
			int letterScore = 1 + (int)(rpsPair.Value - 'X'); // X = 1, Y = 2, Z = 3
			int outcome = WinLUT[rpsPair.Key][rpsPair.Value];
			int outcomeScore = (outcome + 1) * 3; // -1 = 0, 0 = 3, 1 = 6
			return letterScore + outcomeScore;
		}

		private IEnumerable<KeyValuePair<char, char>> GetRPSPairs()
		{
			return Source.SplitNewLine()
					     .Select(
						     x => new KeyValuePair<char, char>(x[0], x[2])
					     );
		}
	}
}
