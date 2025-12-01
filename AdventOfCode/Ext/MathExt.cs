using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Ext
{
	public static class MathExt
	{
		public static int AbsMod(int x, int m)
		{
			return ((x % m) + m) % m;
		}
		
		public static Int64 LowestCommonMultiple(this IEnumerable<Int64> values)
		{
			return values.Aggregate(LowestCommonMultiple);
		}
		
		public static Int64 LowestCommonMultiple(Int64 a, Int64 b)
		{
			return Math.Abs(a * b) / GreatestCommonDivider(a, b);
		}

		public static BigInteger Min(BigInteger a, BigInteger b)
		{
			return (a < b) ? a : b;
		}
		
		public static Int64 GreatestCommonDivider(Int64 a, Int64 b)
		{
			return (b == 0)
				? a
				: GreatestCommonDivider(b, a % b);
		}
		
		public static bool IsInsidePolygon(Point2D test, Point2D[] points)
		{
			bool result = false;

			for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
			{
				if ((((points[i].Y <= test.Y) && (test.Y < points[j].Y)) ||
				     ((points[j].Y <= test.Y) && (test.Y < points[i].Y))) &&
				    (test.X < (points[j].X - points[i].X) * (test.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
				{
					result = !result;
				}
			}

			return result;
		}

		public static int DigitCount(double value)
		{
			return (int)Math.Floor(Math.Log10(value) + 1);
		}
		
		public static int CalculatePerimeter(HashSet<Point2D> points, bool includeInternal)
		{
			return InternalGetOrthoEdgePoints(points, includeInternal).Count;
		}
		
		public static int CountEdges(HashSet<Point2D> points, bool includeInternal)
		{
			var edgePoints = InternalGetOrthoEdgePoints(points, includeInternal).ToList();
			var connectedEdges = new List<Point2D>();

			for (int i = 0; i < edgePoints.Count; ++i)
			{
				int directionIndex = edgePoints[i].Item1;
				Point2D point = edgePoints[i].Item2;
				
				// clear previous edges
				connectedEdges.Clear();
				
				// add this as the initial edge
				connectedEdges.Add(point);
				edgePoints.RemoveAt(i);

				// remove edges adjacent to this one
				for (int j = 0; j < connectedEdges.Count; ++j)
				{
					switch (directionIndex)
					{
						// edge goes up or down
						case 0:
						case 2:
							{
								// check right
								var p1 = (directionIndex, connectedEdges[j] + Point2D.OrthoDirections[1]);
								if (edgePoints.Contains(p1))
								{
									connectedEdges.Add(p1.Item2);
									edgePoints.Remove(p1);
								}

								// check left
								var p2 = (directionIndex, connectedEdges[j] + Point2D.OrthoDirections[3]);
								if (edgePoints.Contains(p2))
								{
									connectedEdges.Add(p2.Item2);
									edgePoints.Remove(p2);
								}
							}
							break;
						
						// edge goes right or left
						case 1:
						case 3:
							{
								// check above
								var p1 = (directionIndex, connectedEdges[j] + Point2D.OrthoDirections[0]);
								if (edgePoints.Contains(p1))
								{
									connectedEdges.Add(p1.Item2);
									edgePoints.Remove(p1);
								}

								// check below
								var p2 = (directionIndex, connectedEdges[j] + Point2D.OrthoDirections[2]);
								if (edgePoints.Contains(p2))
								{
									connectedEdges.Add(p2.Item2);
									edgePoints.Remove(p2);
								}
							}
							break;
					}
				}
				
				// readd this edge so we can use it in our results
				edgePoints.Insert(i, (directionIndex, point));
			}

			return edgePoints.Count;
		}
		
		private static HashSet<(int, Point2D)> InternalGetOrthoEdgePoints(HashSet<Point2D> points, bool includeInternal)
		{
			HashSet<(int, Point2D)> visited = new HashSet<(int, Point2D)>();

			if (points.Count > 0)
			{
				if (includeInternal)
				{
					foreach (var p in points)
					{
						for (int i = 0; i < Point2D.OrthoDirections.Count; ++i)
						{
							var orthoDirection = Point2D.OrthoDirections[i];

							if (!points.Contains(p + orthoDirection))
							{
								visited.Add((i, p));
							}
						}
					}
				}
				else
				{
					Point2D point = points.First();
					int directionIndex = 0;

					foreach (var p in points)
					{
						if (p.Y < point.Y)
						{
							directionIndex = 0;
							point = p;
						}
					}

					while (!visited.Contains((directionIndex, point)))
					{
						Point2D inFrontPoint = point + Point2D.OrthoDirections[directionIndex];
						
						if (points.Contains(inFrontPoint))
						{
							point = inFrontPoint;
							directionIndex += Point2D.OrthoDirections.Count - 1;
						}
						else
						{
							visited.Add((directionIndex, point));
							++directionIndex;
						}
						
						directionIndex %= Point2D.OrthoDirections.Count;
					}
				}
			}

			return visited;
		}
	}
}
