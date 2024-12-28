using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day16 : Day
	{
		private class Route
		{
			public List<Point2D> Visited;
			public Point2D Position;
			public int Direction;
			public int Cost = 0;

			public Route(Point2D position, int direction, List<Point2D> visited = null)
			{
				Visited = new List<Point2D>((visited?.Count ?? 0) + 1);
				if (visited != null)
				{
					Visited.AddRange(visited);
				}
				Visited.Add(position);

				Position = position;
				Direction = direction;
			}
		}

		private HashSet<Point2D> Walls { get; } = new HashSet<Point2D>();
		private Point2D StartTile;
		private Point2D EndTile;
		private const int StartDirection = 1;
		private const int TurnCost = 1000;
		private const int MoveCost = 1;

		protected override Task ExecuteSharedAsync()
		{
			var map = CreateCharMapFromSource(Source)
				.Select(x => x.ToArray())
				.ToArray();
			
			Walls.Clear();

			for (var y = 0; y < map.Length; ++y)
			{
				for (var x = 0; x < map[y].Length; ++x)
				{
					switch (map[y][x])
					{
						case '#':
							Walls.Add(new Point2D(x, y));
							break;
						
						case 'S':
							StartTile = new Point2D(x, y);
							break;
						
						case 'E':
							EndTile = new Point2D(x, y);
							break;
					}
				}
			}

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			List<Route> routes = new List<Route>();
			Route bestRoute = new Route(EndTile, StartDirection)
			{
				Cost = int.MaxValue
			};

			for (int i = 0; i < Point2D.OrthoDirections.Count; ++i)
			{
				int directionIndex = (StartDirection + i) % Point2D.OrthoDirections.Count;
				Point2D newPosition = StartTile + Point2D.OrthoDirections[directionIndex];
				
				if (!Walls.Contains(newPosition))
				{
					routes.Add(
						new Route(newPosition, directionIndex, [StartTile])
						{
							Cost = (Math.Abs(directionIndex - StartDirection) * TurnCost) + MoveCost
						}
					);
				}
			}
			
			routes.Sort((a, b) => a.Cost.CompareTo(b.Cost));

			while (routes.Count > 0)
			{
				if (routes[0].Position == new Point2D(1, 9))
				{
					routes[0].Position = routes[0].Position;
				}

				for (int i = Point2D.OrthoDirections.Count - 1; i >= 0; --i)
				{
					int directionIndex = (routes[0].Direction + i) % Point2D.OrthoDirections.Count;
					Point2D newPosition = routes[0].Position + Point2D.OrthoDirections[directionIndex];
					int newCost = routes[0].Cost +(Math.Abs(directionIndex - routes[0].Direction) * TurnCost) + MoveCost;

					if (newPosition == EndTile)
					{
						if (newCost < bestRoute.Cost)
						{
							bestRoute = new Route(newPosition, directionIndex, routes[0].Visited)
							{
								Cost = newCost
							};
						}

						if (i == 0)
						{
							routes[0].Visited.Add(newPosition);
							routes[0].Position = newPosition;
							//Logging.Log(routes[0].Visited.Aggregate("", (current, visited) => current + $"({visited}), "));
							routes.RemoveAt(0);
						}

						break;
					}
					
					if (routes[0].Visited.Contains(newPosition) || Walls.Contains(newPosition))
					{
						if (i != 0)
						{
							continue;
						}

						routes[0].Visited.Add(newPosition);
						routes[0].Position = newPosition;
						//Logging.Log(routes[0].Visited.Aggregate("", (current, visited) => current + $"({visited}), "));
						routes.RemoveAt(0);
						break;
					}

					if (i == 0)
					{
						routes[0].Visited.Add(newPosition);
						routes[0].Position = newPosition;
						routes[0].Cost = newCost;
					}
					else
					{
						routes.Insert(
							1,
							new Route(newPosition, directionIndex, routes[0].Visited)
							{
								Cost = newCost
							}
						);
					}
				}
				
				routes.RemoveAll(route => route.Cost > bestRoute.Cost);
				routes.Sort((a, b) => a.Cost.CompareTo(b.Cost));
			}

			return Task.FromResult<object>(
				bestRoute.Cost
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
