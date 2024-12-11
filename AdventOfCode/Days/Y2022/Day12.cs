using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.DataTypes;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public sealed class Day12 : Day
	{
		private struct Location
		{
			public Point2D Coord { get; set; }
			public int Height { get; set; }

			public Location(Point2D coord, int height)
			{
				Coord = coord;
				Height = height;
			}
		}
		private Location[][] Map = new Location[0][];
		private Point2D StartLoc = new Point2D();
		private Point2D EndLoc = new Point2D();

		protected override Task ExecuteSharedAsync()
		{
			Map = GetMap(ref StartLoc, ref EndLoc);
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				FindShortestPath(StartLoc)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				Map.SelectMany(x => x.Where(x => x.Height == 0))
				   .Min(x => FindShortestPath(x.Coord))
			);
		}

		private int FindShortestPath(Point2D start)
		{
			bool[][] visited = Map.Select(x => x.Select(y => start == y.Coord)
												.ToArray())
								  .ToArray();
			var travellerLocations = new List<Point2D>()
			{
				start
			};

			for (int stepCount = 1; travellerLocations.Any(); ++stepCount)
			{
				for (int t = 0; t < travellerLocations.Count; ++t)
				{
					Point2D lastLoc = travellerLocations[t];

					foreach (var direction in Point2D.OrthoDirections)
					{
						Point2D nextLoc = lastLoc + direction;

						if (CanTravelToPoint(lastLoc, nextLoc, visited))
						{
							if (nextLoc == EndLoc)
							{
								return stepCount;
							}

							travellerLocations.Insert(t, nextLoc);
							visited[nextLoc.Y][nextLoc.X] = true;
							++t;
						}
					}

					travellerLocations.RemoveAt(t);
					--t;
				}
			}

			return int.MaxValue;
		}

		private bool CanTravelToPoint(Point2D lastLoc, Point2D nextLoc, bool[][] visited)
		{
				   // within range
			return (nextLoc.Y >= 0) && (nextLoc.X >= 0) && (nextLoc.Y < Map.Length) && (nextLoc.X < Map[nextLoc.Y].Length) &&
				   // haven't already visited
				   !visited[nextLoc.Y][nextLoc.X] &&
				   // can traverse to it
				   (Map[nextLoc.Y][nextLoc.X].Height <= (Map[lastLoc.Y][lastLoc.X].Height + 1));
		}

		private Location[][] GetMap(ref Point2D start, ref Point2D end)
		{
			char[][] map = Source.SplitNewLine()
								 .Select(x => x.ToCharArray())
								 .ToArray();

			for (int y = 0; y < map.Length; ++y)
			{
				for (int x = 0; x < map[y].Length; ++x)
				{
					if (map[y][x] == 'S')
					{
						start = new Point2D(x, y);
						map[y][x] = 'a';
					}
					else if (map[y][x] == 'E')
					{
						end = new Point2D(x, y);
						map[y][x] = 'z';
					}
				}
			}

			return map.Select((row, y) => row.Select((height, x) => new Location(new Point2D(x, y), height - 'a'))
											 .ToArray())
					  .ToArray();
		}
	}
}
