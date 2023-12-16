using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day03 : DayBase2023
	{
		struct Number
		{
			public int Value { get; set; }
			public List<Point2D> Locations { get; set; }
		}

		struct Symbol
		{
			public char Value { get; set; }
			public Point2D Location { get; set; }
		}

		private const char kGearSymbol = '*';
		
		List<Number> Numbers = new List<Number>();
		List<Symbol> Symbols = new List<Symbol>();
		
		protected override Task ExecuteSharedAsync()
		{
			string[] lines = Source.SplitNewLine();

			for (int y = 0; y < lines.Length; ++y)
			{
				for (int x = 0; x < lines.Length; ++x)
				{
					if (char.IsDigit(lines[y][x]))
					{
						int startX = x;
						
						for (; x < lines.Length; ++x)
						{
							if (!char.IsDigit(lines[y][x]))
							{
								break;
							}
						}

						ReadOnlySpan<char> numberSpan = lines[y].AsSpan(startX, x - startX);
						List<Point2D> points = new List<Point2D>(numberSpan.Length);

						for (int i = 0; i < numberSpan.Length; ++i)
						{
							points.Add(new Point2D(startX + i, y));
						}
						
						Numbers.Add(
							new Number()
							{
								Value = int.Parse(numberSpan),
								Locations = points
							}
						);

						--x;
					}
					else if (lines[y][x] != '.')
					{
						Symbols.Add(
							new Symbol()
							{
								Value = lines[y][x],
								Location = new Point2D(x, y)
							}
						);
					}
				}
			}
			
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
						// find valid part numbers..
				Numbers.Where(
							// ..by checking if they are adjacent to symbols
							number => number.Locations.Any(
								numberLoc => Symbols.Any(
									symbol => numberLoc.IsAdjacent(symbol.Location)
								)
							)
						)
						// sum up all these valid part numbers
					   .Sum(x => x.Value)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				// only check gears
				Symbols.Where(x => x.Value == kGearSymbol)
                // select numbers..
				.Select(
	                // ..that are adjacent to the gear
					symbol => Numbers.Where(
						number => number.Locations.IsAdjacent(symbol.Location)
					)
					// ..take three at most so we know if it is invalid
					.Take(3)
				)
                // gears are only valid if there is exactly two numbers adjacent
				.Where(gearCodes => gearCodes.Count() == 2)
                // multiply numbers together
				.Select(gearCodes => gearCodes.Aggregate(1, (x, gearCode) => x * gearCode.Value))
                // add the resulting values together
				.Sum()
			);
		}
	}
}
