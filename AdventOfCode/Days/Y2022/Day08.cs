using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public sealed class Day08 : Day
	{
		private int[][] TreeHeights = new int[0][];

		protected override Task ExecuteSharedAsync()
		{
			TreeHeights = GetTreeHeights();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			bool[][] treeVisible = TreeHeights.Select(x => x.Select(x => false)
															.ToArray())
											  .ToArray();

			// outside
			for (int y = 0; y < treeVisible.Length; ++y)
			{
				for (int x = 0; x < treeVisible[y].Length; ++x)
				{
					if ((x == 0) || (y == 0) || ((x + 1) == treeVisible[y].Length) || ((y + 1) == treeVisible.Length))
					{
						treeVisible[y][x] = true;
					}
				}
			}

			// horizontal
			for (int y = 0; y < treeVisible.Length; ++y)
			{
				// left
				int tallest = TreeHeights[y][0];

				for (int x = 1; x < treeVisible[y].Length; ++x)
				{
					if (TreeHeights[y][x] > tallest)
					{
						treeVisible[y][x] = true;
					}

					tallest = Math.Max(tallest, TreeHeights[y][x]);
				}

				// right
				tallest = TreeHeights[y][treeVisible[y].Length - 1];

				for (int x = treeVisible[y].Length - 2; x >= 0; --x)
				{
					if (TreeHeights[y][x] > tallest)
					{
						treeVisible[y][x] = true;
					}

					tallest = Math.Max(tallest, TreeHeights[y][x]);
				}
			}

			// horizontal
			for (int x = 0; x < treeVisible[0].Length; ++x)
			{
				// top
				int tallest = TreeHeights[0][x];

				for (int y = 1; y < treeVisible.Length; ++y)
				{
					if (TreeHeights[y][x] > tallest)
					{
						treeVisible[y][x] = true;
					}

					tallest = Math.Max(tallest, TreeHeights[y][x]);
				}

				// bottom
				tallest = TreeHeights[treeVisible.Length - 1][x];

				for (int y = treeVisible.Length - 2; y >= 0; --y)
				{
					if (TreeHeights[y][x] > tallest)
					{
						treeVisible[y][x] = true;
					}

					tallest = Math.Max(tallest, TreeHeights[y][x]);
				}
			}

			return Task.FromResult<object>(
				treeVisible.Sum(x => x.Select(x => x ? 1 : 0)
									  .Sum())
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			int[][] scenicScore = TreeHeights.Select(x => x.Select(x => 0)
														   .ToArray())
											 .ToArray();

			for (int yo = 0; yo < scenicScore.Length; ++yo)
			{
				for (int xo = 0; xo < scenicScore[yo].Length; ++xo)
				{
					// up
					int upCount = 0;

					for (int y = yo - 1; y >= 0; --y)
					{
						++upCount;

						if (TreeHeights[yo][xo] <= TreeHeights[y][xo])
						{
							break;
						}
					}

					// left
					int leftCount = 0;

					for (int x = xo - 1; x >= 0; --x)
					{
						++leftCount;

						if (TreeHeights[yo][xo] <= TreeHeights[yo][x])
						{
							break;
						}
					}

					// down
					int downCount = 0;

					for (int y = yo + 1; y < scenicScore.Length; ++y)
					{
						++downCount;

						if (TreeHeights[yo][xo] <= TreeHeights[y][xo])
						{
							break;
						}
					}

					// right
					int rightCount = 0;

					for (int x = xo + 1; x < scenicScore[yo].Length; ++x)
					{
						++rightCount;

						if (TreeHeights[yo][xo] <= TreeHeights[yo][x])
						{
							break;
						}
					}

					scenicScore[yo][xo] = upCount * leftCount * downCount * rightCount;
				}
			}

			return Task.FromResult<object>(
				scenicScore.Max(x => x.Max())
			);
		}

		private int[][] GetTreeHeights()
		{
			return Source.SplitNewLine()
						 .Select(x => x.ToCharArray()
									   .Select(char.ToString)
									   .Select(int.Parse)
									   .ToArray())
						 .ToArray();
		}
	}
}
