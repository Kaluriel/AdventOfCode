using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public sealed class Day11 : Day
	{
		private Point2D[] Galaxies;

		protected override Task ExecuteSharedAsync()
		{
			Galaxies = Source.SplitNewLine()
							 .SelectMany((line, y) => line.ToCharArray()
															  .Select((c, x) => (x, y, c))
															  .Where(item => item.c == '#')
															  .Select(item => new Point2D(item.x, item.y)))
							 .ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			Point2D[] expandedGalaxies = Galaxies.Select(g => g + new Point2D(g.X - Galaxies.GroupBy(x => x.X)
																									 .Select(x => x.First())
																									 .Where(x => x.X < g.X)
																									 .Count(),
																					 g.Y - Galaxies.GroupBy(x => x.Y)
																									 .Select(x => x.First())
																									 .Where(x => x.Y < g.Y)
																									 .Count()))
												 .ToArray();

			Int64 sum = 0;

			for (int i = 0; i < expandedGalaxies.Length; ++i)
			{
				for (int j = i + 1; j < expandedGalaxies.Length; ++j)
				{
					sum += expandedGalaxies[j].GetManhattenDistance(expandedGalaxies[i]);
				}
			}

			return Task.FromResult<object>(
				sum
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			Point2D[] expandedGalaxies = Galaxies.Select(g => g + new Point2D(g.X - Galaxies.GroupBy(x => x.X)
																									 .Select(x => x.First())
																									 .Where(x => x.X < g.X)
																									 .Count(),
																					 g.Y - Galaxies.GroupBy(x => x.Y)
																									 .Select(x => x.First())
																									 .Where(x => x.Y < g.Y)
																									 .Count()) * 999999)
												 .ToArray();

			Int64 sum = 0;

			for (int i = 0; i < expandedGalaxies.Length; ++i)
			{
				for (int j = i + 1; j < expandedGalaxies.Length; ++j)
				{
					sum += expandedGalaxies[j].GetManhattenDistance(expandedGalaxies[i]);
				}
			}

			return Task.FromResult<object>(
				sum
			);
		}
	}
}
