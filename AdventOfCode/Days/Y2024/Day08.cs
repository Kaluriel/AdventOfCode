using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day08 : DayBase2024
	{
		private Dictionary<char, Point2D[]> Antennas = new Dictionary<char, Point2D[]>();
		private char[][] Map;

		protected override Task ExecuteSharedAsync()
		{
			Map = Source
				.SplitNewLine()
				.Select(x => x.ToCharArray())
				.ToArray();
			
			Antennas = Map
				.SelectMany(
					(row, y) => row
						.Select((p, x) => (p, x, y))
						.Where(item => char.IsLetterOrDigit(item.p))
				)
				.GroupBy(p => p.p)
				.ToDictionary(
					g => g.Key,
					g => g
						.Select(p => new Point2D(p.x, p.y))
						.ToArray()
				);

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumUniqueAntinodeLocations()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumUniqueAntinodeLocationsIgnoreDistance()
			);
		}

		private int NumUniqueAntinodeLocations()
		{
			HashSet<Point2D> uniqueLocations = new HashSet<Point2D>();

			foreach (var antenna in Antennas)
			{
				for (int i = 0; i < antenna.Value.Length; ++i)
				{
					for (int j = i + 1; j < antenna.Value.Length; ++j)
					{
						var diff = antenna.Value[i] - antenna.Value[j];
						uniqueLocations.Add(antenna.Value[i] + diff);
						uniqueLocations.Add(antenna.Value[j] - diff);
					}
				}
			}

			uniqueLocations.RemoveWhere(p => p.Y < 0 || p.Y >= Map.Length || p.X < 0 || p.X >= Map[p.Y].Length);

			return uniqueLocations.Count;
		}

		private int NumUniqueAntinodeLocationsIgnoreDistance()
		{
			HashSet<Point2D> uniqueLocations = new HashSet<Point2D>();

			foreach (var antenna in Antennas)
			{
				for (int i = 0; i < antenna.Value.Length; ++i)
				{
					for (int j = i + 1; j < antenna.Value.Length; ++j)
					{
						var diff = antenna.Value[i] - antenna.Value[j];

						var start = antenna.Value[i];
						while (start.Y >= 0 && start.Y < Map.Length && start.X >= 0 && start.X < Map[start.Y].Length)
						{
							uniqueLocations.Add(start);
							start += diff;
						}
						
						start = antenna.Value[j];
						while (start.Y >= 0 && start.Y < Map.Length && start.X >= 0 && start.X < Map[start.Y].Length)
						{
							uniqueLocations.Add(start);
							start -= diff;
						}
					}
				}
			}

			return uniqueLocations.Count;
		}
	}
}
