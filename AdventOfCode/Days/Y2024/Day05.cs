using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day05 : Day
	{
		private int[][] PageOrderingRules;
		private int[][] PagesToProduceInEachUpdate;

		protected override Task ExecuteSharedAsync()
		{
			var sourceParts = Source.SplitDoubleNewLine();
			PageOrderingRules = sourceParts[0].SplitNewLine()
				.Select(x => x
					.Split("|")
					.Select(int.Parse)
					.ToArray()
				)
				.ToArray();
			PagesToProduceInEachUpdate = sourceParts[1].SplitNewLine()
				.Select(x => x
					.Split(",")
					.Select(int.Parse)
					.ToArray()
				)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfMiddlePageNumbers()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfMiddlePageNumbersForInccorrect()
			);
		}

		private int SumOfMiddlePageNumbers()
		{
			return PagesToProduceInEachUpdate
					.Where(
						updatePages => PageOrderingRules
							.Where(rulePages => rulePages.All(updatePages.Contains))
							.All(rulePages => Array.IndexOf(updatePages, rulePages[0]) < Array.IndexOf(updatePages, rulePages[1]))
					)
					.Select(
						updatePages => updatePages
							.Skip(updatePages.Count() / 2)
							.First()
					)
					.Sum();
		}

		private class PageOrderComparer(int[][] pageOrderingRules)
			: IComparer<int>
		{
			public int Compare(int x, int y)
			{
				for (int i = 0; i < pageOrderingRules.Length; ++i)
				{
					if ((pageOrderingRules[i][0] == x) && (pageOrderingRules[i][1] == y))
					{
						return -1;
					}
					else if ((pageOrderingRules[i][0] == y) && (pageOrderingRules[i][1] == x))
					{
						return 1;
					}
				}
				return 0;
			}
		}

		private int SumOfMiddlePageNumbersForInccorrect()
		{
			return PagesToProduceInEachUpdate
				.Where(
					updatePages => !PageOrderingRules
						.Where(rulePages => rulePages.All(updatePages.Contains))
						.All(rulePages => Array.IndexOf(updatePages, rulePages[0]) < Array.IndexOf(updatePages, rulePages[1]))
				)
				.Select(
					updatePages => updatePages
						.OrderBy(x => x, new PageOrderComparer(PageOrderingRules))
						.Skip(updatePages.Count() / 2)
						.First()
				)
				.Sum();
		}
	}
}
