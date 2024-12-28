using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day18 : Day
	{
#if TEST
		const int MemorySpaceWidth = 7;
		const int MemorySpaceHeight = 7;
			const int Steps = 12;
#else
		const int MemorySpaceWidth = 71;
		const int MemorySpaceHeight = 71;
		const int Steps = 1024;
#endif
		private char[][] MemorySpace;
		private Point2D[] IncomingBytes;
		private readonly Point2D Entrance = Point2D.Zero;
		private readonly Point2D Exit = new Point2D(MemorySpaceWidth - 1, MemorySpaceHeight - 1);

		protected override Task ExecuteSharedAsync()
		{
			MemorySpace = new char[MemorySpaceHeight][];
			for (int y = 0; y < MemorySpaceHeight; ++y)
			{
				MemorySpace[y] = new char[MemorySpaceWidth];
				for (int x = 0; x < MemorySpaceWidth; ++x)
				{
					MemorySpace[y][x] = '.';
				}
			}

			IncomingBytes = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line
					.Split(',')
					.Select(int.Parse)
					.ToArray())
				.Select(x => new Point2D(x))
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			for (int i = 0; i < Steps; ++i)
			{
				var incomingByte = IncomingBytes[i];

				MemorySpace[incomingByte.Y][incomingByte.X] = '#';
			}
			
			Logging.LogMap(MemorySpace);

			var nodes = DijkstraAlgorithm.CreateNodes(MemorySpace, (x) => x == '.');
			DijkstraAlgorithm.Build(nodes, Entrance);
			List<DijkstraAlgorithm.DijkstraNode>? shortestRoute = DijkstraAlgorithm.FindShortestRoute(nodes, Entrance, Exit);

			return Task.FromResult<object>(
				shortestRoute.Count - 1
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			object _lockObj = new();
			int ret = int.MaxValue;

			Parallel.For(
				Steps,
				IncomingBytes.Length,
				(index) =>
				{
					var memorySpace = MemorySpace.Select(x => x.ToArray()).ToArray();

					for (int i = 0; i <= index; ++i)
					{
						var incomingByte = IncomingBytes[i];
						memorySpace[incomingByte.Y][incomingByte.X] = '#';
					}

					var nodes = DijkstraAlgorithm.CreateNodes(memorySpace, (x) => x == '.');
					DijkstraAlgorithm.Build(nodes, Entrance);
					List<DijkstraAlgorithm.DijkstraNode>? shortestRoute = DijkstraAlgorithm.FindShortestRoute(nodes, Entrance, Exit);
					if (shortestRoute.Count == 0)
					{
						lock (_lockObj)
						{
							if (index < ret)
							{
								ret = index;
							}
						}
					}
				}
			);

			return Task.FromResult<object>(
				$"{IncomingBytes[ret].X},{IncomingBytes[ret].Y}"
			);
		}
	}
}
