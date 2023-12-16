using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Days.Y2022
{
	public class Day09 : DayBase2022
	{
		private IReadOnlyDictionary<char, Point2D> Movement = new Dictionary<char, Point2D>()
		{
			{ 'U', Point2D.Up },
			{ 'D', Point2D.Down },
			{ 'L', Point2D.Left },
			{ 'R', Point2D.Right },
		};

		private IEnumerable<KeyValuePair<char, int>> Instructions = Enumerable.Empty<KeyValuePair<char, int>>();

		protected override Task ExecuteSharedAsync()
		{
			Instructions = GetInstructions();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				CalculateUniqueTailPositions(2)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				CalculateUniqueTailPositions(10)
			);
		}

		private IEnumerable<KeyValuePair<char, int>> GetInstructions()
		{
			return Source.SplitNewLine()
						 .Select(x => x.Split(' '))
						 .Select(
							 y => new KeyValuePair<char, int>(
								 y[0][0],
								 int.Parse(y[1])
							 )
						 );
		}

		private int CalculateUniqueTailPositions(int knotCount)
		{
			Point2D[] knots = new Point2D[knotCount];

			var visitedTailPositions = new HashSet<Point2D>()
			{
				knots[^1]
			};

			foreach (var instruction in Instructions)
			{
				for (int count = 0; count < instruction.Value; ++count)
				{
					// move head
					knots[0] += Movement[instruction.Key];

					// update knots after head
					int j = 1;

					for (; j < knots.Length; ++j)
					{
						Point2D dir = knots[j - 1] - knots[j];

						// if the knot doesn't move, then break out early
						if ((Math.Abs(dir.X) < 2) && (Math.Abs(dir.Y) < 2))
						{
							break;
						}

						// otherwise move towards the knot ahead
						knots[j] += dir.Sign();
					}

					// if tail moves, add to the list of positions
					if (j == knots.Length)
					{
						visitedTailPositions.Add(knots[^1]);
					}
				}
			}

			return visitedTailPositions.Count;
		}
	}
}
