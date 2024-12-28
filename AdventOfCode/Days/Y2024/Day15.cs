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
	public sealed class Day15 : Day
	{
		private char[][] Warehouse;
		private Point2D[] Directions;

		protected override Task ExecuteSharedAsync()
		{
			var sections = Source.SplitDoubleNewLine();
			Warehouse = CreateCharMapFromSource(sections[0])
				.Select(x => x.ToArray())
				.ToArray();

			Directions = sections[1]
				.Replace("\r\n", "")
				.Replace("\r", "")
				.Replace("\n", "")
				.Select(x => x switch
				{
					'^' => Point2D.Up,
					'v' => Point2D.Down,
					'>' => Point2D.Right,
					'<' => Point2D.Left,
					_ => throw new ArgumentException($"Unknown direction character '{x}'")
				})
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			HashSet<Point2D> walls = new HashSet<Point2D>();
			HashSet<Point2D> boxes = new HashSet<Point2D>();
			Point2D robot = Point2D.Zero;

			for (int y = 0; y < Warehouse.Length; ++y)
			{
				for (int x = 0; x < Warehouse[y].Length; ++x)
				{
					switch (Warehouse[y][x])
					{
						case '#':
							walls.Add(new Point2D(x, y));
							break;
						case 'O':
							boxes.Add(new Point2D(x, y));
							break;
						case '@':
							robot = new Point2D(x, y);
							break;
					}
				}
			}
			
			List<Point2D> boxesToMove = new List<Point2D>(boxes.Count);

			foreach (var direction in Directions)
			{
				Point2D newRobot = robot + direction;
				Point2D nextPoint = newRobot;
				
				boxesToMove.Clear();
				
nextPointP1:
				if (walls.Contains(nextPoint))
				{
					// can't move, so goto next instruction
					continue;
				}
				
				if (boxes.Contains(nextPoint))
				{
					boxesToMove.Add(nextPoint);
					nextPoint += direction;
					goto nextPointP1;
				}

				foreach (var box in boxesToMove.AsEnumerable().Reverse())
				{
					boxes.Remove(box);
					boxes.Add(box + direction);
				}

				robot = newRobot;
			}

			string str = "";
			for (int y = 0; y < Warehouse.Length; ++y)
			{
				for (int x = 0; x < Warehouse[y].Length; ++x)
				{
					var p = new Point2D(x, y);
					if (walls.Contains(p))
					{
						str += "#";
					}
					else if (boxes.Contains(p))
					{
						str += "O";
					}
					else if (robot == p)
					{
						str += "@";
					}
					else
					{
						str += ".";
					}
				}
				
				str += "\r\n";
			}
			
			Logging.Log(str);
			Logging.Log("");

			return Task.FromResult<object>(
				boxes
					.Select(box => box.Y * 100 + box.X)
					.Sum()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			/*HashSet<Point2D> walls = new HashSet<Point2D>();
			HashSet<Point2D> boxLefts = new HashSet<Point2D>();
			HashSet<Point2D> boxRights = new HashSet<Point2D>();
			Point2D robot = Point2D.Zero;

			for (int y = 0; y < Warehouse.Length; ++y)
			{
				for (int x = 0; x < Warehouse[y].Length; ++x)
				{
					switch (Warehouse[y][x])
					{
						case '#':
							walls.Add(new Point2D(x * 2, y));
							walls.Add(new Point2D(x * 2 + 1, y));
							break;
						case 'O':
							boxLefts.Add(new Point2D(x * 2, y));
							boxRights.Add(new Point2D(x * 2 + 1, y));
							break;
						case '@':
							robot = new Point2D(x * 2, y);
							break;
					}
				}
			}
			
			List<Point2D> boxesToMove = new List<Point2D>(boxLefts.Count);

			foreach (var direction in Directions)
			{
				Point2D newRobot = robot + direction;
				List<Point2D> nextPoints = new List<Point2D>()
				{
					newRobot
				};
				
				boxesToMove.Clear();
				
nextPointP2:
				if (walls.Any(w => nextPoints.Contains(w)))
				{
					// can't move, so goto next instruction
					continue;
				}
				
				if (boxLefts.Contains(nextPoint))
				{
					boxesToMove.Add(nextPoint);
					nextPoint += direction;
					goto nextPointP2;
				}
				else if (boxRights.Contains(nextPoint))
				{
					boxesToMove.Add(nextPoint);
					nextPoint += direction * new Point2D(2, 1);
					goto nextPointP2;
				}

				foreach (var box in boxesToMove.AsEnumerable().Reverse())
				{
					boxLefts.Remove(box);
					boxLefts.Remove(box + Point2D.Right);
					boxLefts.Add(box + direction);
					boxRights.Add(box + direction + Point2D.Right);
				}

				robot = newRobot;
			}

			string str = "";
			for (int y = 0; y < Warehouse.Length; ++y)
			{
				for (int x = 0; x < Warehouse.Length*2; ++x)
				{
					var p = new Point2D(x, y);
					if (walls.Contains(p))
					{
						str += "#";
					}
					else if (boxLefts.Contains(p))
					{
						str += "[";
					}
					else if (boxRights.Contains(p))
					{
						str += "]";
					}
					else if (robot == p)
					{
						str += "@";
					}
					else
					{
						str += ".";
					}
				}
				
				str += "\r\n";
			}
			
			Logging.Log(str);
			Logging.Log("");*/
			
			return Task.FromResult<object>(
				1
			);
		}
	}
}
