using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day04 : Day
	{
		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var instructions = Source.SplitNewLineThenChars(StringSplitOptions.RemoveEmptyEntries);
			int count = 0;

			Parallel.For(
				0, instructions.Length,
				y =>
				{
					int yStart = Math.Max(0, y - 1);
					int yEnd = Math.Min(instructions.Length, y + 2);

					Parallel.For(
						0, instructions[y].Length,
						x =>
						{
							if (instructions[y][x] != '@')
							{
								return;
							}

							int rollCount = 0;

							for (int y1 = yStart; y1 < yEnd; ++y1)
							{
								int xStart = Math.Max(0, x - 1);
								int xEnd = Math.Min(instructions[y1].Length, x + 2);

								for (int x1 = xStart; x1 < xEnd; ++x1)
								{
									if ((x == x1) && (y == y1))
									{
										continue;
									}

									if (instructions[y1][x1] == '@')
									{
										++rollCount;
										if (rollCount == 4)
										{
											break;
										}
									}
								}
							}

							if (rollCount < 4)
							{
								Interlocked.Increment(ref count);
							}
						}
					);
				}
			);

			return Task.FromResult<object>(
				count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var instructions = Source.SplitNewLineThenChars(StringSplitOptions.RemoveEmptyEntries);
			int lastCount = 0;
			int count = 0;

			do
			{
				lastCount = count;

				Parallel.For(
					0, instructions.Length,
					y =>
					{
						int yStart = Math.Max(0, y - 1);
						int yEnd = Math.Min(instructions.Length, y + 2);

						Parallel.For(
							0, instructions[y].Length,
							x =>
							{
								if (instructions[y][x] != '@')
								{
									return;
								}

								int rollCount = 0;

								for (int y1 = yStart; y1 < yEnd; ++y1)
								{
									int xStart = Math.Max(0, x - 1);
									int xEnd = Math.Min(instructions[y1].Length, x + 2);

									for (int x1 = xStart; x1 < xEnd; ++x1)
									{
										if ((x == x1) && (y == y1))
										{
											continue;
										}

										if (instructions[y1][x1] == '@')
										{
											++rollCount;
											if (rollCount == 4)
											{
												break;
											}
										}
									}
								}

								if (rollCount < 4)
								{
									Interlocked.Increment(ref count);
									instructions[y][x] = '.';
								}
							}
						);
					}
				);
			} while (lastCount != count);
			
			return Task.FromResult<object>(
				count
			);
		}
	}
}
