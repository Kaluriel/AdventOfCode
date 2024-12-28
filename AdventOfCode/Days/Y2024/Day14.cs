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
	public sealed class Day14 : Day
	{
		private struct RobotInfo
		{
			public Point2D Position;
			public Point2D Velocity;
		}
		const int Seconds = 100;
#if TEST
		const int MapWidth = 11;
		const int MapHeight = 7;
#else
		const int MapWidth = 101;
		const int MapHeight = 103;
#endif
		const int QuadrantWidth = (MapWidth - 1) / 2;
		const int QuadrantHeight = (MapHeight - 1) / 2;

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			RobotInfo[] robots = Source
				.SplitNewLine()
				.Select(line => line
					.Split(['=', ',', ' ', 'p', 'v'], StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse))
				.Select(v => new RobotInfo
					{
						Position = new Point2D(v
							.Take(2)
							.ToArray()),
						Velocity = new Point2D(v
							.Skip(2)
							.Take(2)
							.ToArray())
					}
				)
				.ToArray();

			for (int i = 0; i < Seconds; ++i)
			{
				for (int r = 0; r < robots.Length; ++r)
				{
					robots[r].Position += robots[r].Velocity;
					
					if ((robots[r].Position.X < 0) || (robots[r].Position.X >= MapWidth))
					{
						robots[r].Position = new Point2D(
							(robots[r].Position.X + MapWidth) % MapWidth,
							robots[r].Position.Y
						);
					}
					
					if ((robots[r].Position.Y < 0) || (robots[r].Position.Y >= MapHeight))
					{
						robots[r].Position = new Point2D(
							robots[r].Position.X,
							(robots[r].Position.Y + MapHeight) % MapHeight
						);
					}
				}
			}

			int q1 = robots.Count(r => r.Position is { X: < QuadrantWidth, Y: < QuadrantHeight });
			int q2 = robots.Count(r => r.Position is { X: > QuadrantWidth, Y: < QuadrantHeight });
			int q3 = robots.Count(r => r.Position is { X: < QuadrantWidth, Y: > QuadrantHeight });
			int q4 = robots.Count(r => r.Position is { X: > QuadrantWidth, Y: > QuadrantHeight });
			
			return Task.FromResult<object>(
				q1 * q2 * q3 * q4
			);
		}

		private void LogRobots(RobotInfo[] robots, int seconds)
		{
			Logging.Log("Seconds: " + seconds);
			for (int y = 0; y < MapHeight; ++y)
			{
				string line = "";
				
				for (int x = 0; x < MapWidth; ++x)
				{
					int v = robots.Count(r => r.Position.X == x && r.Position.Y == y);
					if (v > 0)
					{
						line += v.ToString();
					}
					else
					{
						line += ".";
					}
				}
				
				Logging.Log(line);
			}
			
			Logging.Log("");
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			RobotInfo[] robots = Source
				.SplitNewLine()
				.Select(line => line
					.Split(['=', ',', ' ', 'p', 'v'], StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse))
				.Select(v => new RobotInfo
					{
						Position = new Point2D(v
							.Take(2)
							.ToArray()),
						Velocity = new Point2D(v
							.Skip(2)
							.Take(2)
							.ToArray())
					}
				)
				.ToArray();

			bool foundChristmasTree = false;
			int seconds = 0;
			do
			{
				++seconds;

				for (int r = 0; r < robots.Length; ++r)
				{
					robots[r].Position += robots[r].Velocity;

					if ((robots[r].Position.X < 0) || (robots[r].Position.X >= MapWidth))
					{
						robots[r].Position = new Point2D(
							(robots[r].Position.X + MapWidth) % MapWidth,
							robots[r].Position.Y
						);
					}

					if ((robots[r].Position.Y < 0) || (robots[r].Position.Y >= MapHeight))
					{
						robots[r].Position = new Point2D(
							robots[r].Position.X,
							(robots[r].Position.Y + MapHeight) % MapHeight
						);
					}
				}
				

				int q1 = robots.Count(r => r.Position is { X: < QuadrantWidth, Y: < QuadrantHeight });
				int q2 = robots.Count(r => r.Position is { X: > QuadrantWidth, Y: < QuadrantHeight });
				int q3 = robots.Count(r => r.Position is { X: < QuadrantWidth, Y: > QuadrantHeight });
				int q4 = robots.Count(r => r.Position is { X: > QuadrantWidth, Y: > QuadrantHeight });
				//Logging.Log("Seconds: " + seconds + " " + (q1 * q2 * q3 * q4));

				var robotPositions = robots.Select(x => x.Position).ToHashSet();
				foreach (var robotPosition in robotPositions)
				{
					bool isSurrounded = true;
					
					foreach (var direction in Point2D.Directions)
					{
						if (!robotPositions.Contains(robotPosition + direction))
						{
							isSurrounded = false;
							break;
						}
					}

					if (isSurrounded)
					{
						foundChristmasTree = true;
						break;
					}
				}
			} while (!foundChristmasTree);
			
			//LogRobots(robots, seconds);
			
			return Task.FromResult<object>(
				seconds
			);
		}

		private BigInteger CalculateFewestTokensToWinAllPrizes()
		{
			return 1;
		}
	}
}
