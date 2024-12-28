using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Utils;

public static class DijkstraAlgorithm
{
	public class DijkstraNode(Point2D position)
	{
		public Point2D Position = position;
		public int DistanceFromStart = int.MaxValue;

		public void Reset()
		{
			DistanceFromStart = int.MaxValue;
		}
	}

	public static Dictionary<Point2D, DijkstraNode> CreateNodes<T>(T[][] map, Func<T, bool> canTravelFunc)
	{
		Dictionary<Point2D, DijkstraNode> ret = new Dictionary<Point2D, DijkstraNode>();
		
		for (int y = 0; y < map.Length; ++y)
		{
			for (int x = 0; x < map[y].Length; ++x)
			{
				if (canTravelFunc(map[y][x]))
				{
					var point = new Point2D(x, y);
					ret.Add(
						point,
						new DijkstraNode(point)
					);
				}
			}
		}

		return ret;
	}

	public static Dictionary<Point2D, DijkstraNode> CreateNodes(HashSet<Point2D> path)
	{
		Dictionary<Point2D, DijkstraNode> ret = new Dictionary<Point2D, DijkstraNode>();

		foreach (var point in path)
		{
			ret.Add(
				point,
				new DijkstraNode(point)
			);
		}

		return ret;
	}

	public static void Build(Dictionary<Point2D, DijkstraNode> nodes, Point2D start, bool reset = false)
	{
		var unvistedSet = new Dictionary<Point2D, DijkstraNode>(nodes);

		if (reset)
		{
			foreach (var node in nodes)
			{
				node.Value.Reset();
			}
		}

		nodes[start].DistanceFromStart = 0;

		var currentNode = unvistedSet
			.OrderBy(x => x.Value.DistanceFromStart)
			.FirstOrDefault();
		while (unvistedSet.Count > 0)
		{
			foreach (var orthoDirection in Point2D.OrthoDirections)
			{
				var nextPosition = currentNode.Key + orthoDirection;
				if (unvistedSet.ContainsKey(nextPosition))
				{
					int newDistance = currentNode.Value.DistanceFromStart + 1;
					if (newDistance < unvistedSet[nextPosition].DistanceFromStart)
					{
						unvistedSet[nextPosition].DistanceFromStart = newDistance;
					}
				}
			}

			unvistedSet.Remove(currentNode.Key);

			currentNode = unvistedSet
				.OrderBy(x => x.Value.DistanceFromStart)
				.FirstOrDefault();
		}
	}

	public static List<DijkstraNode> FindShortestRoute(Dictionary<Point2D, DijkstraNode> nodes, Point2D start, Point2D end, List<DijkstraNode> shortestRouteBuffer = null, bool routeStartToEnd = true)
	{
		var shortestRoute = shortestRouteBuffer ?? new List<DijkstraNode>();
		shortestRouteBuffer?.Clear();

		shortestRoute.Add(nodes[end]);

		while (shortestRoute[^1].Position != start)
		{
			var node = Point2D.OrthoDirections
				.Select(x => shortestRoute[^1].Position + x)
				.Where(nodes.ContainsKey)
				.Select(x => nodes[x])
				.FirstOrDefault(x => x.DistanceFromStart == (shortestRoute[^1].DistanceFromStart - 1));
			if (node == null)
			{
				shortestRoute.Clear();
				break;
			}

			shortestRoute.Add(node);
		}

		if (routeStartToEnd)
		{
			shortestRoute.Reverse();
		}

		return shortestRoute;
	}

	public static List<DijkstraNode> FindShortestManhattanRoute(Dictionary<Point2D, DijkstraNode> nodes, Point2D start, Point2D end, bool routeStartToEnd = true)
	{
		var shortestRoutes = new List<List<DijkstraNode>>();

		shortestRoutes.Add([nodes[end]]);

		while (shortestRoutes.Any(list => list[^1].Position != start))
		{
			for (int i = 0; i < shortestRoutes.Count; ++i)
			{
				var foundNodes = Point2D.OrthoDirections
					.Select(x => shortestRoutes[i][^1].Position + x)
					.Where(nodes.ContainsKey)
					.Select(x => nodes[x])
					.Where(x => x.DistanceFromStart == (shortestRoutes[i][^1].DistanceFromStart - 1))
					.ToArray();
				if (foundNodes.Length == 0)
				{
					shortestRoutes.RemoveAt(i);
					--i;
					continue;
				}

				for (int j = 1; j < foundNodes.Length; ++j)
				{
					var newList = new List<DijkstraNode>(shortestRoutes[i]) { foundNodes[j] };
					shortestRoutes.Insert(0, newList);
					++i;
				}

				shortestRoutes[i].Add(foundNodes[0]);
			}
		}

		if (shortestRoutes.Count == 0)
		{
			return new List<DijkstraNode>();
		}

		if (shortestRoutes.Count > 1)
		{
			shortestRoutes.Sort(
				(x, y) =>
				{
					int x1 = x
						.SelectWithNext((v1, v2) => v1.Position - v2.Position)
						.SelectWithNext((v1, v2) => (v1.X == v2.X) || (v1.Y == v2.Y))
						.Count(v => v);
					int y1 = y
						.SelectWithNext((v1, v2) => v1.Position - v2.Position)
						.SelectWithNext((v1, v2) => (v1.X == v2.X) || (v1.Y == v2.Y))
						.Count(v => v);
					return y1.CompareTo(x1);
				}
			);
		}

		if (routeStartToEnd)
		{
			shortestRoutes[0].Reverse();
		}

		return shortestRoutes[0];
	}
}
