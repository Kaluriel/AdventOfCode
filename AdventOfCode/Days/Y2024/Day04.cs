using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public class Day04 : DayBase2024
	{
		private readonly string FindWordP1 = "XMAS";
		private readonly string FindWordP2 = "MAS";
		private char[][] WordSearch;

		protected override Task ExecuteSharedAsync()
		{
			WordSearch = Source
				.SplitNewLine()
				.Select(x => x.ToCharArray())
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		private bool IsWordInGrid(string word, int startX, int startY, int dX, int dY)
		{
			bool ret = true;

			for (int i = 1; i < word.Length; ++i)
			{
				startX += dX;
				startY += dY;

				if ((startY < 0) || (startY >= WordSearch.Length) ||
				    (startX < 0) || (startX >= WordSearch[startY].Length) ||
				    (WordSearch[startY][startX] != word[i]))
				{
					ret = false;
					break;
				}
			}

			return ret;
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountWordsInWordSearch()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				CountX_MASInWordSearch()
			);
		}

		private int CountWordsInWordSearch()
		{
			int count = 0;

			for (int y = 0; y < WordSearch.Length; y++)
			{
				for (int x = 0; x < WordSearch[y].Length; x++)
				{
					// skip until we find the starting letter
					if (WordSearch[y][x] != FindWordP1[0])
					{
						continue;
					}

					bool canLeft = x >= FindWordP1.Length - 1;
					bool canRight = x <= (WordSearch[y].Length - FindWordP1.Length);
					bool canUp = y >= (FindWordP1.Length - 1);
					bool canDown = y <= (WordSearch.Length - FindWordP1.Length);
					int instanceCount = 0;

					if (canUp)
					{
						// vertical up
						if (IsWordInGrid(FindWordP1, x, y, 0, -1))
						{
							++instanceCount;
						}
					}
					
					if (canDown)
					{
						// vertical down
						if (IsWordInGrid(FindWordP1, x, y, 0, 1))
						{
							++instanceCount;
						}
					}

					if (canLeft)
					{
						// horizontal backwards
						if (IsWordInGrid(FindWordP1, x, y, -1, 0))
						{
							++instanceCount;
						}

						if (canUp)
						{
							// diagonal up left
							if (IsWordInGrid(FindWordP1, x, y, -1, -1))
							{
								++instanceCount;
							}
						}

						if (canDown)
						{
							// diagonal down left
							if (IsWordInGrid(FindWordP1, x, y, -1, 1))
							{
								++instanceCount;
							}
						}
					}

					if (canRight)
					{
						// horizontal forwards
						if (IsWordInGrid(FindWordP1, x, y, 1, 0))
						{
							++instanceCount;
						}

						if (canUp)
						{
							// diagonal up right
							if (IsWordInGrid(FindWordP1, x, y, 1, -1))
							{
								++instanceCount;
							}
						}

						if (canDown)
						{
							// diagonal down right
							if (IsWordInGrid(FindWordP1, x, y, 1, 1))
							{
								++instanceCount;
							}
						}
					}

					count += instanceCount;
				}
			}
			
			return count;
		}

		private int CountX_MASInWordSearch()
		{
			int count = 0;

			for (int y = 0; y < WordSearch.Length; y++)
			{
				for (int x = 0; x < WordSearch[y].Length; x++)
				{
					int index = FindWordP2.IndexOf(WordSearch[y][x]);
					if (index == -1)
					{
						continue;
					}
					
					bool hasTopLeft = false;
					bool hasTopRight = false;
					bool hasBottomLeft = false;
					bool hasBottomRight = false;
					
					if ((index > 0) && ((index + 1) < FindWordP2.Length))
					{
						string left = FindWordP2.Substring(0, index + 1).Reverse();
						string right = FindWordP2.Substring(index);

						if (IsWordInGrid(left, x, y, -1, -1) && IsWordInGrid(right, x, y, 1, 1))
						{
							hasTopLeft = true;
							hasBottomRight = true;
						}
						else if (IsWordInGrid(right, x, y, -1, -1) && IsWordInGrid(left, x, y, 1, 1))
						{
							hasTopLeft = true;
							hasBottomRight = true;
						}
						
						if (IsWordInGrid(left, x, y, -1, 1) && IsWordInGrid(right, x, y, 1, -1))
						{
							hasTopRight = true;
							hasBottomLeft = true;
						}
						else if (IsWordInGrid(right, x, y, -1, 1) && IsWordInGrid(left, x, y, 1, -1))
						{
							hasTopRight = true;
							hasBottomLeft = true;
						}
					}
					/*else
					{
						string word = (index == 0)
							? FindWordP2
							: FindWordP2.Reverse();

						hasTopLeft = IsWordInGrid(word, x, y, -1, -1);
						hasTopRight = IsWordInGrid(word, x, y, 1, -1);
						hasBottomLeft = IsWordInGrid(word, x, y, 1, 1);						
						hasBottomRight = IsWordInGrid(word, x, y, 1, 1);
					}*/

					if (hasTopLeft && hasTopRight && hasBottomLeft && hasBottomRight)
					{
						++count;
					}
				}
			}
			
			return count;
		}
	}
}
