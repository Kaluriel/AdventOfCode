using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.DataTypes;
using System.Text;
using System.Text.Json;


namespace AdventOfCode.Days.Y2022
{
	public sealed class Day14 : Day
	{
		private static readonly IReadOnlyList<Point2D> SandActions = new Point2D[]
		{
			new Point2D( 0, 1),
			new Point2D(-1, 1),
			new Point2D( 1, 1),
		};

		private IEnumerable<Point2D[]> RockPaths = Enumerable.Empty<Point2D[]>();

		protected override Task ExecuteSharedAsync()
		{
			RockPaths = GetRockPaths();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			int minX = RockPaths.Min(x => x.Min(y => y.X));
			int maxX = RockPaths.Max(x => x.Max(y => y.X + 1));
			int maxY = RockPaths.Max(x => x.Max(y => y.Y + 1));

			char[][] caveLayout = new char[maxY][];
			for (int y = 0; y < caveLayout.Length; ++y)
			{
				caveLayout[y] = new char[maxX - minX];

				for (int x = 0; x < caveLayout[y].Length; ++x)
				{
					caveLayout[y][x] = '.';
				}
			}

			Point2D sandSource = new Point2D(500, 0);
			caveLayout[sandSource.Y][sandSource.X - minX] = '+';

			foreach (var rockPath in RockPaths)
			{
				for (int part = 1; part < rockPath.Length; ++part)
				{
					Point2D dir = new Point2D(
						Math.Sign(rockPath[part].X - rockPath[part - 1].X),
						Math.Sign(rockPath[part].Y - rockPath[part - 1].Y)
					);
					Point2D endPoint = rockPath[part] + dir;
					Point2D currentPoint = rockPath[part - 1];

					do
					{
						caveLayout[currentPoint.Y][currentPoint.X - minX] = '#';
						currentPoint += dir;
					}
					while (currentPoint != endPoint);
				}
			}

			//DrawCave(caveLayout);

			Point2D sand = sandSource;
			bool foundAbyss = false;
			int sandUnits = 0;

			do
			{
				int i = 0;

				for (; i < SandActions.Count; ++i)
				{
					Point2D nextPosition = sand + SandActions[i];

					if ((nextPosition.X < minX) || (nextPosition.X >= maxX) || (nextPosition.Y >= caveLayout.Length))
					{
						foundAbyss = true;
						break;
					}
					else if (caveLayout[nextPosition.Y][nextPosition.X - minX] == '.')
					{
						sand = nextPosition;
						break;
					}
				}

				if (i == SandActions.Count)
				{
					caveLayout[sand.Y][sand.X - minX] = 'O';
					++sandUnits;

					//DrawCave(caveLayout);

					sand = sandSource;
				}
			}
			while (!foundAbyss);

			return Task.FromResult<object>(
				sandUnits
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			int maxY = RockPaths.Max(x => x.Max(y => y.Y + 1)) + 2;
			int minX = RockPaths.Min(x => x.Min(y => y.X)) - maxY;
			int maxX = RockPaths.Max(x => x.Max(y => y.X + 1)) + maxY;

			char[][] caveLayout = new char[maxY][];
			for (int y = 0; y < caveLayout.Length; ++y)
			{
				caveLayout[y] = new char[maxX - minX];

				for (int x = 0; x < caveLayout[y].Length; ++x)
				{
					caveLayout[y][x] = '.';
				}
			}

			Point2D sandSource = new Point2D(500, 0);
			caveLayout[sandSource.Y][sandSource.X - minX] = '+';

			foreach (var rockPath in RockPaths)
			{
				for (int part = 1; part < rockPath.Length; ++part)
				{
					Point2D dir = new Point2D(
						Math.Sign(rockPath[part].X - rockPath[part - 1].X),
						Math.Sign(rockPath[part].Y - rockPath[part - 1].Y)
					);
					Point2D endPoint = rockPath[part] + dir;
					Point2D currentPoint = rockPath[part - 1];

					do
					{
						caveLayout[currentPoint.Y][currentPoint.X - minX] = '#';
						currentPoint += dir;
					}
					while (currentPoint != endPoint);
				}
			}

			// ground
			for (int x = 0; x < caveLayout[^1].Length; ++x)
			{
				caveLayout[^1][x] = '#';
			}

			//DrawCave(caveLayout);

			Point2D sand = sandSource;
			bool foundAbyss = false;
			int sandUnits = 0;

			do
			{
				int i = 0;

				for (; i < SandActions.Count; ++i)
				{
					Point2D nextPosition = sand + SandActions[i];

					if ((nextPosition.X < minX) || (nextPosition.X >= maxX) || (nextPosition.Y >= caveLayout.Length))
					{
						throw new Exception("Should be no abyss");
					}
					else if (caveLayout[nextPosition.Y][nextPosition.X - minX] == '.')
					{
						sand = nextPosition;
						break;
					}
				}

				if (i == SandActions.Count)
				{
					caveLayout[sand.Y][sand.X - minX] = 'O';
					++sandUnits;

					//DrawCave(caveLayout);

					if (sand == sandSource)
					{
						break;
					}

					sand = sandSource;
				}
			}
			while (!foundAbyss);

			return Task.FromResult<object>(
				sandUnits
			);
		}

		private IEnumerable<Point2D[]> GetRockPaths()
		{
			return Source.SplitNewLine()
						 .Select(x => x.Split(" -> ")
									   .Select(y => y.Split(',')
													 .Select(int.Parse))
									   .Select(y => new Point2D(y.First(), y.Skip(1).First()))
									   .ToArray());
		}

		private void DrawCave(char[][] cave)
		{
			StringBuilder strBuilder = new StringBuilder();

			for (int y = 0; y < cave.Length; ++y)
			{
				for (int x = 0; x < cave[y].Length; ++x)
				{
					strBuilder.Append(cave[y][x]);
				}

				strBuilder.AppendLine();
			}

			System.Diagnostics.Debug.WriteLine(strBuilder.ToString());
		}
	}
}
