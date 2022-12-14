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
	public class Day14 : DayBase2022
	{
		private IEnumerable<Point2D[]> RockPaths = Enumerable.Empty<Point2D[]>();

		protected override Task ExecuteSharedAsync()
		{
			RockPaths = GetRockPaths();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
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

			bool foundAbyss = false;
			int unit = 0;

			do
			{
				Point2D sand = sandSource + new Point2D(0, 0);

				do
				{
					if ((sand.Y + 1) >= caveLayout.Length)
					{
						foundAbyss = true;
						break;
					}
					else if (caveLayout[sand.Y + 1][sand.X - minX] == '.')
					{
						sand.Y += 1;
					}
					else
					{
						if ((sand.X - 1 - minX) < 0)
						{
							foundAbyss = true;
							break;
						}
						else if (caveLayout[sand.Y + 1][sand.X - 1 - minX] == '.')
						{
							sand.X -= 1;
							sand.Y += 1;
						}
						else if ((sand.X + 1 - minX) >= caveLayout[sand.Y].Length)
						{
							foundAbyss = true;
							break;
						}
						else if (caveLayout[sand.Y + 1][sand.X + 1 - minX] == '.')
						{
							sand.X += 1;
							sand.Y += 1;
						}
						else
						{
							break;
						}
					}

				}
				while (true);

				if (!foundAbyss)
				{
					caveLayout[sand.Y][sand.X - minX] = 'O';
					++unit;
				}

				//DrawCave(caveLayout);
			}
			while (!foundAbyss);

			return Task.FromResult<object>(
				unit
			);
		}

		protected override Task<object> ExecutePart2Async()
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

			Point2D sand;
			int unit = 0;

			do
			{
				sand = sandSource + new Point2D(0, 0);

				do
				{
					if ((sand.Y + 1) >= caveLayout.Length)
					{
						throw new Exception("Should be no abyss");
					}
					else if (caveLayout[sand.Y + 1][sand.X - minX] == '.')
					{
						sand.Y += 1;
					}
					else
					{
						if ((sand.X - 1 - minX) < 0)
						{
							throw new Exception("Should be no abyss");
						}
						else if (caveLayout[sand.Y + 1][sand.X - 1 - minX] == '.')
						{
							sand.X -= 1;
							sand.Y += 1;
						}
						else if ((sand.X + 1 - minX) >= caveLayout[sand.Y].Length)
						{
							throw new Exception("Should be no abyss");
						}
						else if (caveLayout[sand.Y + 1][sand.X + 1 - minX] == '.')
						{
							sand.X += 1;
							sand.Y += 1;
						}
						else
						{
							break;
						}
					}

				}
				while (true);

				//DrawCave(caveLayout);

				caveLayout[sand.Y][sand.X - minX] = 'O';
				++unit;
			}
			while (sand != sandSource);

			//DrawCave(caveLayout);

			return Task.FromResult<object>(
				unit
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
