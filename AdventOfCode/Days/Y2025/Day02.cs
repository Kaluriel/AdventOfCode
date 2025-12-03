using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day02 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source
				.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Split('-').Select(ulong.Parse).ToArray())
				.ToArray();
			ulong count = 0;

			foreach (var instruction in instructions)
			{
				for (ulong i = instruction[0]; i <= instruction[1]; ++i)
				{
					string num = i.ToString();
					if ((num.Length % 2) == 0)
					{
						if (string.Compare(num, 0, num, num.Length / 2, num.Length / 2) == 0)
						{
							count += i;
						}
					}
				}
			}

			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source
				.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Select(x => x.Split('-').Select(ulong.Parse).ToArray())
				.ToArray();
			ulong count = 0;

			foreach (var instruction in instructions)
			{
				for (ulong i = instruction[0]; i <= instruction[1]; ++i)
				{
					string num = i.ToString();
					int halfLength = num.Length / 2;

					for (int subLength = 1; subLength <= halfLength; ++subLength)
					{
						if ((num.Length % subLength) != 0)
						{
							continue;
						}

						bool fullMatch = true;

						for (int offset = subLength; offset <= (num.Length - subLength); offset += subLength)
						{
							if (string.Compare(num, 0, num, offset, subLength) != 0)
							{
								fullMatch = false;
								break;
							}
						}

						if (fullMatch)
						{
							count += i;
							break;
						}
					}
				}
			}
			
			return Task.FromResult<object>(
				count
			);
		}
	}
}
