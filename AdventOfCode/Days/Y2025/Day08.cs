using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2025
{
	public sealed class Day08 : Day
	{
		private Point3D[] _Points;

		protected override async Task ExecuteSharedAsync()
		{
			await base.ExecuteSharedAsync();
			
			_Points = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(x => new  Point3D(x))
				.ToArray();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var junctionCircuit = new Dictionary<Point3D, HashSet<Point3D>>();
			var closestPoints = new List<(Point3D, Point3D, float)>();
			
			// determine distances
			for (int p1 = 0; p1 < _Points.Length; ++p1)
			{
				for (int p2 = p1 + 1; p2 < _Points.Length; ++p2)
				{
					float distance = _Points[p1].GetDistance(_Points[p2]);
					closestPoints.Add(
						(_Points[p1], _Points[p2], distance)
					);
				}
			}

			// sort for the closest
			closestPoints.Sort((a, b) => a.Item3.CompareTo(b.Item3));
			
			// limit how many we're checking
#if TEST
			closestPoints.RemoveRange(10, closestPoints.Count - 10);
#else
			closestPoints.RemoveRange(1000, closestPoints.Count - 1000);
#endif

			// prepopulate so we save a bunch of unnecessary null checks
			foreach (var point in _Points)
			{
				junctionCircuit.Add(point, [point]);
			}

			foreach (var closestPair in closestPoints)
			{
				var circuit1 = junctionCircuit[closestPair.Item1];
				var circuit2 = junctionCircuit[closestPair.Item2];

				// already joined?
				if (circuit1 == circuit2)
				{
					continue;
				}

				// combine circuits
				circuit1.UnionWith(circuit2);

				// update dictionary with the new parent circuit
				foreach (var point in circuit2)
				{
					junctionCircuit[point] = circuit1;
				}
			}

			return Task.FromResult<object>(
				junctionCircuit.Values.Distinct()
					.OrderByDescending(x => x.Count)
					.Take(3)
					.Aggregate(1, (x, y) => x * y.Count)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var junctionCircuit = new Dictionary<Point3D, HashSet<Point3D>>();
			var closestPoints = new List<(Point3D, Point3D, float)>();
			int lastTwoXProduct = 0;
			
			// determine distances
			for (int p1 = 0; p1 < _Points.Length; ++p1)
			{
				for (int p2 = p1 + 1; p2 < _Points.Length; ++p2)
				{
					float distance = _Points[p1].GetDistance(_Points[p2]);
					closestPoints.Add(
						(_Points[p1], _Points[p2], distance)
					);
				}
			}

			// sort for the closest
			closestPoints.Sort((a, b) => a.Item3.CompareTo(b.Item3));

			// prepopulate so we save a bunch of unnecessary null checks
			foreach (var point in _Points)
			{
				junctionCircuit.Add(point, [point]);
			}

			foreach (var closestPair in closestPoints)
			{
				var circuit1 = junctionCircuit[closestPair.Item1];
				var circuit2 = junctionCircuit[closestPair.Item2];

				// already joined?
				if (circuit1 == circuit2)
				{
					continue;
				}

				// combine circuits
				circuit1.UnionWith(circuit2);

				// update dictionary with the new parent circuit
				foreach (var point in circuit2)
				{
					junctionCircuit[point] = circuit1;
				}

				// keep calculating the product so it's eventually left on the product of the last two
				lastTwoXProduct = closestPair.Item1.X * closestPair.Item2.X;
			}

			return Task.FromResult<object>(lastTwoXProduct);
		}
	}
}
