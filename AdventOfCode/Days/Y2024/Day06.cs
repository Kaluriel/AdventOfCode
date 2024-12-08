using System;
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
	public class Day06 : DayBase2024
	{
		private char[][] Map;
		private Point2D StartPoint;
		private int StartDirectionIndex = 0;

		protected override Task ExecuteSharedAsync()
		{
			Map = Source
				.SplitNewLine()
				.Select(x => x.ToCharArray())
				.ToArray();

			for (int y = 0; y < Map.Length; ++y)
			{
				for (int x = 0; x < Map[y].Length; ++x)
				{
					if (Map[y][x] == '^')
					{
						StartPoint = new Point2D(x, y);
						Map[y][x] = '.';
						break;
					}
				}
			}

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumberOfDistinctPositions()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				NumberOfLoopObstaclePositions()
			);
		}

		private int NumberOfDistinctPositions()
		{
			List<Point2D> points = new List<Point2D>();
			GetNavigationPoints(points, Map);
			return points.Count;
		}

		private int NumberOfLoopObstaclePositions()
		{
			List<Point2D> points = new List<Point2D>();
			GetNavigationPoints(points, Map);

			int loopObstacleCount = 0;

			Parallel.For(
				0,
				points.Count,
				i =>
				{
					var map = Map.Select(x => x.ToArray()).ToArray();
					map[points[i].Y][points[i].X] = '#';

					bool leftMap = GetNavigationPoints(points, map);
					if (!leftMap)
					{
						Interlocked.Increment(ref loopObstacleCount);
					}
				}
			);
			
			return loopObstacleCount;
		}
		
		private bool GetNavigationPoints(List<Point2D> points, char[][] map)
		{
			//map = map.Select(x => x.ToArray()).ToArray();
			HashSet<(Point2D, int)> visited = new HashSet<(Point2D, int)>();
			Point2D loc = StartPoint;
			int directionIndex = StartDirectionIndex;
			bool leftMap = false;

			do
			{
				if (!points.Contains(loc))
				{
					points.Add(loc);
				}
				
				/*map[loc.Y][loc.X] = 'X';

				Log(
					DrawGrid(map)
				);
				Log("");*/

				try_again:
				if (!visited.Add((loc, directionIndex)))
				{
					break;
				}

				Point2D nextLoc = loc + Point2D.OrthoDirections[directionIndex];
				if (nextLoc.Y < 0 ||
				    nextLoc.Y >= map.Length ||
				    nextLoc.X < 0 ||
				    nextLoc.X >= map[nextLoc.Y].Length)
				{
					leftMap = true;
					break;
				}

				if (map[nextLoc.Y][nextLoc.X] == '#')
				{
					directionIndex = (directionIndex + 1) % Point2D.OrthoDirections.Count;
					goto try_again;
				}
				
				loc = nextLoc;
			}
			while (!visited.Contains((loc, directionIndex)));

			return leftMap;
		}
	}
}
