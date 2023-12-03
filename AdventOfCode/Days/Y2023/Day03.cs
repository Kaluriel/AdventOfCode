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
		List<KeyValuePair<string, List<Point2D>>> Numbers = new List<KeyValuePair<string, List<Point2D>>>();
		List<KeyValuePair<Point2D, char>> Symbols = new List<KeyValuePair<Point2D, char>>();
		
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

						string valStr = lines[y].Substring(startX, x - startX);
						List<Point2D> points = new List<Point2D>(valStr.Length);

						for (int i = 0; i < valStr.Length; ++i)
						{
							points.Add(new Point2D(startX + i, y));
						}
						
						Numbers.Add(
							new KeyValuePair<string, List<Point2D>>(valStr, points)
						);

						--x;
					}
					else if (lines[y][x] != '.')
					{
						Symbols.Add(
							new KeyValuePair<Point2D, char>(new Point2D(x, y), lines[y][x])
						);
					}
				}
			}
			
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			return Task.FromResult<object>(
				Numbers.Where(
							number => number.Value.Any(
								numberLoc => Symbols.Any(
									symbol => numberLoc.IsAdjacent(symbol.Key)
								)
							)
						)
					   .Select(x => int.Parse(x.Key))
					   .Sum()
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			return Task.FromResult<object>(
				Symbols.Select(
					symbol => Numbers.Where(
						number => number.Value.Any(
							numberLoc => numberLoc.IsAdjacent(symbol.Key)
                        )
					)
				)
				.Where(gearCodes => gearCodes.Count() == 2)
				.Select(gearCodes => gearCodes.Aggregate(1, (x, gearCode) => x * int.Parse(gearCode.Key)))
				.Sum()
			);
		}
	}
}
