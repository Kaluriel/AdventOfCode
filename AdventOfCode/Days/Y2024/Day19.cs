using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day19 : Day
	{
		private string[] TowelPatterns;
		private string[] Towels;

		protected override Task ExecuteSharedAsync()
		{
			var parts = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);
			TowelPatterns = parts[0]
				.Split(", ", StringSplitOptions.RemoveEmptyEntries)
				.OrderBy(x => x)
				.ToArray();
			Towels = parts[1].SplitNewLine(StringSplitOptions.RemoveEmptyEntries);

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			int possibleDesignCount = 0;

			foreach (var towel in Towels)
			{
				(int, HashSet<string>)[] towelPatterns = TowelPatterns
					.Where(towel.Contains)
					.GroupBy(tp => tp.Length)
					.OrderByDescending(tpg => tpg.Key)
					.Select(tpg => (tpg.Key, tpg.ToHashSet()))
					.ToArray();
				if (CanBuildTowel(towel, towelPatterns, new Dictionary<string, bool>()))
				{
					++possibleDesignCount;
				}
			}
			
			return Task.FromResult<object>(
				possibleDesignCount
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			long sumOfPossibleDesignCombinations = 0;

			foreach (var towel in Towels)
			{
				(int, HashSet<string>)[] towelPatterns = TowelPatterns
					.Where(towel.Contains)
					.GroupBy(tp => tp.Length)
					.OrderByDescending(tpg => tpg.Key)
					.Select(tpg => (tpg.Key, tpg.ToHashSet()))
					.ToArray();
				long ret = CountBuildTowel(towel, towelPatterns, new Dictionary<string, long>());
				sumOfPossibleDesignCombinations += ret;
			}
			
			return Task.FromResult<object>(
				sumOfPossibleDesignCombinations
			);
		}

		private bool CanBuildTowel(string towel, (int, HashSet<string>)[] towelPatternGroups, Dictionary<string, bool> cache)
		{
			if (cache.TryGetValue(towel, out var ret))
			{
				return ret;
			}

			towelPatternGroups = towelPatternGroups
				.Where(x => x.Item1 <= towel.Length)
				.ToArray();

			for (int i = 0; i < towelPatternGroups.Length; ++i)
			{
				var towelPattern = towel[..towelPatternGroups[i].Item1];

				if (towelPatternGroups[i].Item2.Contains(towelPattern))
				{
					if ((towel.Length == towelPatternGroups[i].Item1) ||
					    CanBuildTowel(towel[towelPatternGroups[i].Item1..], towelPatternGroups, cache))
					{
						ret = true;
						break;
					}
				}
			}

			cache.Add(towel, ret);
			return ret;
		}

		private long CountBuildTowel(string towel, (int, HashSet<string>)[] towelPatternGroups, Dictionary<string, long> cache)
		{
			if (towel.Length == 0)
			{
				return 1;
			}
			
			if (cache.TryGetValue(towel, out var ret))
			{
				return ret;
			}

			towelPatternGroups = towelPatternGroups
				.Where(x => x.Item1 <= towel.Length)
				.ToArray();

			for (int i = 0; i < towelPatternGroups.Length; ++i)
			{
				var towelPattern = towel[..towelPatternGroups[i].Item1];

				if (towelPatternGroups[i].Item2.Contains(towelPattern))
				{
					ret += CountBuildTowel(towel[towelPatternGroups[i].Item1..], towelPatternGroups, cache);
				}
			}

			cache.Add(towel, ret);
			return ret;
		}
	}
}
