using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day10 : DayBase2023
	{
		private class PipeInfo
		{
			public char Value { get; }
			public Point2D Location { get; }
			public bool IsPipe => !IsGround;
			public bool IsStart { get; }
			public bool IsGround { get; }
			public Point2D[] Points { get; private set; }

			public PipeInfo(char c, Point2D location)
			{
				Value = c;
				Location = location;
				IsGround = c == '.';
				IsStart = c == 'S';
				if (IsPipe)
				{
					Points = new[]
					{
						c switch
						{
							'|' => new Point2D( 0, -1),
							'-' => new Point2D(-1,  0),
							'L' => new Point2D( 0, -1),
							'J' => new Point2D( 0, -1),
							'7' => new Point2D( 0,  1),
							'F' => new Point2D( 0,  1),
							_ => default
						},
						c switch
						{
							'|' => new Point2D( 0,  1),
							'-' => new Point2D( 1,  0),
							'L' => new Point2D( 1,  0),
							'J' => new Point2D(-1,  0),
							'7' => new Point2D(-1,  0),
							'F' => new Point2D( 1,  0),
							_ => default
						},
					};
				}
				else
				{
					Points = Array.Empty<Point2D>();
				}
			}

			public void UpdateStartPoint(PipeInfo[][] pipeMap)
			{
				List<Point2D> points = new List<Point2D>();
				
				foreach (var dir in Point2D.OrthoDirections)
				{
					Point2D p = Location + dir;
					
					if ((p.Y >= 0) && (p.X >= 0) && (p.Y < pipeMap.Length) && (p.X < pipeMap[p.Y].Length) &&
					    pipeMap[p.Y][p.X].Points.Contains(-dir))
					{
						points.Add(p);
					}
				}

				Points = points.ToArray();
			}
		}
		private PipeInfo[][] PipeMap;
		private PipeInfo Start;
		
		protected override Task ExecuteSharedAsync()
		{
			PipeMap = Source.SplitNewLine()
							.Select((l, y) => l.Select((c, x) => new PipeInfo(c, new Point2D(x, y)))
													   .ToArray())
							.ToArray();

			Start = PipeMap.First(pl => pl.Any(p => p.IsStart))
						   .First(p => p.IsStart);
			Start.UpdateStartPoint(PipeMap);

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			List<PipeInfo> visited = new List<PipeInfo>(PipeMap.Length * PipeMap[0].Length)
			{
				Start
			};

			List<PipeInfo> path = new List<PipeInfo>()
			{
				Start
			};
			
			PipeInfo next = PipeMap[Start.Points.First().Y][Start.Points.First().X];

			while (next != null)
			{
				path.Add(next);
				visited.Add(next);

				next = next.Points.Select(p => next.Location + p)
								  .Where(p => (p.Y >= 0) && (p.X >= 0) && (p.Y < PipeMap.Length) && (p.X < PipeMap[p.Y].Length))
								  .Select(p => PipeMap[p.Y][p.X])
								  .FirstOrDefault(p => !visited.Contains(p));
			}

			return Task.FromResult<object>(
				path.Count / 2
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				"-"
			);
		}
	}
}
