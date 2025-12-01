using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day01 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source
				.SplitNewLineAndSpaces(StringSplitOptions.RemoveEmptyEntries)
				.Select((x) => MathF.Sign(x[0] - 'M') * int.Parse(x.Substring(1)))
				.ToArray();
			int dial = 50;
			int count = 0;

			for (int i = 0; i < instructions.Length; i++)
			{
				var turn = instructions[i];
				dial = MathExt.AbsMod(dial + turn, 100);
				if (dial == 0)
				{
					++count;
				}
			}

			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source
				.SplitNewLineAndSpaces(StringSplitOptions.RemoveEmptyEntries)
				.Select((x) => MathF.Sign(x[0] - 'M') * int.Parse(x.Substring(1)))
				.ToArray();
			int dial = 50;
			int count = 0;

			foreach (var turn in instructions)
			{
				int oldDial = dial;
				int newDial = oldDial + turn;

				if (turn > 0)
				{
					count += newDial / 100;
				}
				else if (turn < 0)
				{
					if ((oldDial != 0) && (newDial) <= 0)
					{
						count += 1 + Math.Abs(newDial) / 100;
					}
					else if ((oldDial == 0) && (newDial <= -100))
					{
						count += Math.Abs(newDial) / 100;
					}
				}

				dial = MathExt.AbsMod(newDial, 100);
			}

			return Task.FromResult<object>(
				count
			);
		}
	}
}
