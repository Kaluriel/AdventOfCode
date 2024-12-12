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
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day12 : Day
	{
		private char[][] GardenMap;
		private List<(char, HashSet<Point2D>)> Regions = new List<(char, HashSet<Point2D>)>();

		protected override Task ExecuteSharedAsync()
		{
			GardenMap = CreateCharMapFromSource(Source)
				.Select(x => x.ToArray())
				.ToArray();
			
			Dictionary<char, HashSet<Point2D>> plantLists = new Dictionary<char, HashSet<Point2D>>();

			for (int y = 0; y < GardenMap.Length; ++y)
			{
				for (int x = 0; x < GardenMap[y].Length; ++x)
				{
					char regionId = GardenMap[y][x];
					var point = new Point2D(x, y);

					if (plantLists.ContainsKey(regionId))
					{
						plantLists[regionId].Add(point);
					}
					else
					{
						plantLists.Add(regionId, [point]);
					}
				}
			}
			
			Regions = new List<(char, HashSet<Point2D>)>();

			foreach (var plantList in plantLists)
			{
				while (plantList.Value.Count > 0)
				{
					var pointList = new List<Point2D>()
					{
						plantList.Value.First()
					};
					
					// first entry
					plantList.Value.Remove(pointList[0]);

					// find adjacent points
					for (int index = 0; index < pointList.Count; ++index)
					{
						for (int i = 0; i < Point2D.OrthoDirections.Count; ++i)
						{
							Point2D adjPoint = pointList[index] + Point2D.OrthoDirections[i];
							if (plantList.Value.Contains(adjPoint))
							{
								plantList.Value.Remove(adjPoint);
								pointList.Add(adjPoint);
							}
						}
					}

					Regions.Add((plantList.Key, [..pointList]));
				}
			}

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				TotalCostOfFencing()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				TotalCostOfFencingWithBulkDiscount()
			);
		}

		private Int64 TotalCostOfFencing()
		{
			return Regions.Sum(region => region.Item2.Count * MathExt.CalculatePerimeter(region.Item2, true));
		}

		private Int64 TotalCostOfFencingWithBulkDiscount()
		{
			return Regions.Sum(region => region.Item2.Count * MathExt.CountEdges(region.Item2, true));
		}
	}
}
