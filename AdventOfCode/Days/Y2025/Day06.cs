using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Threading;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day06 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source.SplitNewLineThenWhitespace(StringSplitOptions.RemoveEmptyEntries);
			var input = instructions.Take(instructions.Length - 1)
				.Select(y => y.Select(long.Parse).ToArray())
				.ToArray();
			var action = instructions[^1];
			long grandTotal = 0;

			for (int actionIndex = 0; actionIndex < action.Length; ++actionIndex)
			{
				bool isMultiply = action[actionIndex] == "*";
				long total = input[0][actionIndex];

				for (int inputIndex = 1; inputIndex < input.Length; ++inputIndex)
				{
					if (isMultiply)
					{
						total *= input[inputIndex][actionIndex];
					}
					else
					{
						total += input[inputIndex][actionIndex];
					}
				}
				
				grandTotal += total;
			}

			return Task.FromResult<object>(
				grandTotal
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries);
			long grandTotal = 0;

			Parallel.For(
				0, instructions[^1].Length,
				(charIndex) =>
				{
					if (instructions[^1][charIndex] == '*' || instructions[^1][charIndex] == '+')
					{
						var numberChars = new List<char[]>();
						long total = instructions[^1][charIndex] == '*'
							? 1
							: 0;
						
						// find length of last line
						int nextIndex = instructions[^1].IndexOfAny(['*', '+'], charIndex + 1);
						int length = nextIndex != -1
							? nextIndex - charIndex - 1
							: instructions[^1].Length - charIndex + 1;

						for (int rowIndex = 0; rowIndex < instructions.Length - 1; ++rowIndex)
						{
							numberChars.Add(
								instructions[rowIndex]
									.Skip(charIndex)
									.Take(length)
									.ToArray()
							);
						}

						for (int index = numberChars[^1].Length - 1; index >= 0; --index)
						{
							long number = 0;
							int offset = 0;

							for (int rowIndex = numberChars.Count - 1; rowIndex >= 0; --rowIndex)
							{
								if ((index <= numberChars[rowIndex].Length) && (numberChars[rowIndex][index] != ' '))
								{
									number += numberChars[rowIndex][index].AsLong() * (long)Math.Pow(10, offset);
									++offset;
								}
							}

							if (instructions[^1][charIndex] == '*')
							{
								total *= number;
							}
							else
							{
								total += number;
							}
						}
						
						Interlocked.Add(ref grandTotal, total);
					}
				}
			);

			return Task.FromResult<object>(
				grandTotal
			);
		}
	}
}
