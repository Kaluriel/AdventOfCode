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
	public sealed class Day10 : Day
	{
		private int[][] Map;
		private List<Point2D> StartPoints = new List<Point2D>();
		private List<Point2D> EndPoints = new List<Point2D>();

		protected override Task ExecuteSharedAsync()
		{
			Map = CreateIntMapFromSource(Source)
				.Select(x => x.ToArray())
				.ToArray();

			StartPoints.Clear();
			EndPoints.Clear();
			for (int y = 0; y < Map.Length; ++y)
			{
				for (int x = 0; x < Map[y].Length; ++x)
				{
					switch (Map[y][x])
					{
						case 0:
							StartPoints.Add(new Point2D(x, y));
							break;
						
						case 9:
							EndPoints.Add(new Point2D(x, y));
							break;
						
						default:
							break;
					}
				}
			}
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetSumOfTrailScores(false)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				GetSumOfTrailScores(true)
			);
		}

		private int GetSumOfTrailScores(bool newVisitedPerBranch)
		{
			return StartPoints
				.Select(x => GetTrailScore(x, newVisitedPerBranch))
				.Sum();
		}

		private int GetTrailScore(Point2D startPoint, bool newVisitedPerBranch)
		{
			HashSet<Point2D> visited = new HashSet<Point2D>();
			return VisitPoints(visited, startPoint, newVisitedPerBranch);
		}

		private int VisitPoints(HashSet<Point2D> visited, Point2D point, bool newVisitedPerBranch)
		{
			int ret = 0;

			visited.Add(point);

			if (!EndPoints.Contains(point))
			{
				var current = Map[point.Y][point.X];

				for (int i = 0; i < Point2D.OrthoDirections.Count; ++i)
				{
					var dir = point + Point2D.OrthoDirections[i];
					if (dir.Y < 0 || dir.Y >= Map.Length || dir.X < 0 || dir.X >= Map[dir.Y].Length ||
					    visited.Contains(dir))
					{
						continue;
					}

					var next = Map[dir.Y][dir.X];
					if ((next - current) != 1)
					{
						continue;
					}

					ret += VisitPoints(
						newVisitedPerBranch
							? new HashSet<Point2D>(visited)
							: visited,
						dir,
						newVisitedPerBranch
					);
				}
			}
			else
			{
				ret = 1;
			}

			return ret;
		}
	}
}
