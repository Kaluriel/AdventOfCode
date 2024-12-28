using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day20 : Day
	{
#if TEST
		private const int SaveAtLeastCount = 20;
#else
		private const int SaveAtLeastCount = 100;
#endif
		private HashSet<Point2D> RaceTrack = new HashSet<Point2D>();
		private Point2D Start = Point2D.Zero;
		private Point2D End = Point2D.Zero;
		private int Width = 0;
		private int Height = 0;

		protected override Task ExecuteSharedAsync()
		{
			var raceTrack = CreateCharMapFromSource(Source)
				.Select(x => x.ToArray())
				.ToArray();
			
			RaceTrack.Clear();

			Height = raceTrack.Length;
			Width = raceTrack[0].Length;

			for (int y = 0; y < raceTrack.Length; ++y)
			{
				for (int x = 0; x < raceTrack[y].Length; ++x)
				{
					switch (raceTrack[y][x])
					{
						case 'S':
							Start = new Point2D(x, y);
							RaceTrack.Add(new Point2D(x, y));
							break;
						case 'E':
							End = new Point2D(x, y);
							RaceTrack.Add(new Point2D(x, y));
							break;
						case '.':
							RaceTrack.Add(new Point2D(x, y));
							break;
					}
				}
			}

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var nodes = DijkstraAlgorithm.CreateNodes(RaceTrack);
			var shortedPath = new List<DijkstraAlgorithm.DijkstraNode>();
			var cheats = new Dictionary<int, int>();
			int raceTrackCost = 0;
			
			DijkstraAlgorithm.Build(nodes, Start, true);
			DijkstraAlgorithm.FindShortestRoute(nodes, Start, End, shortedPath, false);
			raceTrackCost = shortedPath.Count;

			for (int y = 0; y < Height; ++y)
			//for (int y = 1; y <= 1; ++y)
			{
				for (int x = 0; x < Width; ++x)
				//for (int x = 8; x <= 8; ++x)
				{
					var point = new Point2D(x, y);

					// skip if its part of the path
					if (RaceTrack.Contains(point))
					{
						continue;
					}

					if (nodes.TryGetValue(point + Point2D.Left, out var leftNode) && nodes.TryGetValue(point + Point2D.Right, out var rightNode))
					{
						int distanceSaved = Math.Abs(rightNode.DistanceFromStart - leftNode.DistanceFromStart);
						
						if (distanceSaved > 0)
						{
							if (!cheats.TryAdd(distanceSaved - 2, 1))
							{
								++cheats[distanceSaved - 2];
							}
						}
					}
					
					if (nodes.TryGetValue(point + Point2D.Up, out var upNode) && nodes.TryGetValue(point + Point2D.Down, out var downNode))
					{
						/*nodes.Add(point, new DijkstraAlgorithm.DijkstraNode(point));
						DijkstraAlgorithm.Build(nodes, Start, End, true);
						DijkstraAlgorithm.FindShortestRoute(nodes, Start, End, shortedPath, false);
						nodes.Remove(point);
						
						int newShortedPath = shortedPath.Count;
						
						if (newShortedPath < raceTrackCost)
						{
							if (!cheats.TryAdd(raceTrackCost - newShortedPath, 1))
							{
								++cheats[raceTrackCost - newShortedPath];
							}
						}*/
						int distanceSaved = Math.Abs(downNode.DistanceFromStart - upNode.DistanceFromStart);
						
						if (distanceSaved > 0)
						{
							if (!cheats.TryAdd(distanceSaved - 2, 1))
							{
								++cheats[distanceSaved - 2];
							}
						}
					}
				}
			}

			return Task.FromResult<object>(
				cheats
					.Where(x => x.Key >= SaveAtLeastCount)
					.Sum(x => x.Value)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				1
			);
		}
	}
}
