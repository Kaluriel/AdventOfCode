using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public sealed class Day01 : Day
	{
		private readonly KeyValuePair<string, string>[] kNumberAsWords = new[]
		{
			new KeyValuePair<string, string>("one", "o1e"),
			new KeyValuePair<string, string>("two", "t2o"),
			new KeyValuePair<string, string>("three", "t3e"),
			new KeyValuePair<string, string>("four", "f4r"),
			new KeyValuePair<string, string>("five", "f5e"),
			new KeyValuePair<string, string>("six", "s6x"),
			new KeyValuePair<string, string>("seven", "s7n"),
			new KeyValuePair<string, string>("eight", "e8t"),
			new KeyValuePair<string, string>("nine", "n9e"),
		};

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetCalibrationValue(false)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetCalibrationValue(true)
			);
		}

		private int GetCalibrationValue(bool replaceNumberWords)
		{
			return Source.SplitNewLine()
						 .Select(x => replaceNumberWords ? x.Replace(kNumberAsWords) : x)
						 .Select(s => s.Where(char.IsDigit).Select(c => c - '0'))
						 .Select(n => n.First() * 10 + n.Last())
						 .Sum();
		}
	}
}
