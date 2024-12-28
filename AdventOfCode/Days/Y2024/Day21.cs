using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day21 : Day
	{
		private string[][] NumberKeypad => [
			["7", "8", "9"],
			["4", "5", "6"],
			["1", "2", "3"],
			["X", "0", "A"],
		];
		private Point2D NumberKeypadStart = new Point2D(2, 3);
		private Dictionary<Point2D, DijkstraAlgorithm.DijkstraNode> NumberKeypadNodes;
		private Dictionary<(char, char), List<DijkstraAlgorithm.DijkstraNode>> NumberKeypadRoutePoints;
		
		private string[][] DirectionKeypad => [
			["X", "^", "A"],
			["<", "v", ">"],
		];
		private Point2D DirectionKeypadStart = new Point2D(2, 0);
		private Dictionary<Point2D, DijkstraAlgorithm.DijkstraNode> DirectionKeypadNodes;
		private Dictionary<(char, char), List<DijkstraAlgorithm.DijkstraNode>> DirectionKeypadRoutePoints;

		private static readonly Dictionary<Point2D, string> DirectionInput = new Dictionary<Point2D, string>()
		{
			{ Point2D.Up, "^" },
			{ Point2D.Right, ">" },
			{ Point2D.Down, "v" },
			{ Point2D.Left, "<" },
		};

		private static readonly Dictionary<(char, char), string> RouteLUT = new Dictionary<(char, char), string>();
		private static readonly StringBuilder TempStringBuilder = new StringBuilder();
		
		private string[] Codes;

		protected override Task ExecuteSharedAsync()
		{
			NumberKeypadNodes = DijkstraAlgorithm.CreateNodes(NumberKeypad, (x) => x != "X");
			NumberKeypadRoutePoints = new Dictionary<(char, char), List<DijkstraAlgorithm.DijkstraNode>>();
			for (int yS = 0; yS < NumberKeypad.Length; ++yS)
			{
				for (int xS = 0; xS < NumberKeypad[yS].Length; ++xS)
				{
					if (NumberKeypad[yS][xS] == "X")
					{
						continue;
					}
					
					for (int yE = 0; yE < NumberKeypad.Length; ++yE)
					{
						for (int xE = 0; xE < NumberKeypad[yS].Length; ++xE)
						{
							if ((xS == xE) && (yS == yE) || (NumberKeypad[yE][xE] == "X"))
							{
								continue;
							}

							DijkstraAlgorithm.Build(NumberKeypadNodes, new Point2D(xS, yS), true);
							var route = DijkstraAlgorithm.FindShortestManhattanRoute(NumberKeypadNodes, new Point2D(xS, yS), new Point2D(xE, yE));
							NumberKeypadRoutePoints[(NumberKeypad[yS][xS][0], NumberKeypad[yE][xE][0])] = route;
						}
					}
				}
			}
			
			DirectionKeypadNodes = DijkstraAlgorithm.CreateNodes(DirectionKeypad, (x) => x != "X");
			DirectionKeypadRoutePoints = new Dictionary<(char, char), List<DijkstraAlgorithm.DijkstraNode>>();
			for (int yS = 0; yS < DirectionKeypad.Length; ++yS)
			{
				for (int xS = 0; xS < DirectionKeypad[yS].Length; ++xS)
				{
					if (DirectionKeypad[yS][xS] == "X")
					{
						continue;
					}

					for (int yE = 0; yE < DirectionKeypad.Length; ++yE)
					{
						for (int xE = 0; xE < DirectionKeypad[yS].Length; ++xE)
						{
							if ((xS == xE) && (yS == yE) || (DirectionKeypad[yE][xE] == "X"))
							{
								continue;
							}
							
							DijkstraAlgorithm.Build(DirectionKeypadNodes, new Point2D(xS, yS), true);
							var route = DijkstraAlgorithm.FindShortestManhattanRoute(DirectionKeypadNodes, new Point2D(xS, yS), new Point2D(xE, yE));
							DirectionKeypadRoutePoints[(DirectionKeypad[yS][xS][0], DirectionKeypad[yE][xE][0])] = route;
						}
					}
				}
			}
			
			Codes = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			Point2D[] directionRobotLocations = new Point2D[2];
			Point2D numpadRobotLocation = NumberKeypadStart;

			for (int i = 0; i < directionRobotLocations.Length; ++i)
			{
				directionRobotLocations[i] = DirectionKeypadStart;
			}

			StringBuilder prev_code = new StringBuilder();
			StringBuilder next_code = new StringBuilder();
			int ret = 0;

			foreach (var code in Codes)
			{
				prev_code.Clear();
				prev_code.Append(code);
				next_code = NumberKeypadNav(prev_code, next_code, ref numpadRobotLocation);

				for (int i = 0; i < directionRobotLocations.Length; ++i)
				{
					(prev_code, next_code) = (next_code, prev_code);
					next_code = DirectionKeypadNav(prev_code, next_code, ref directionRobotLocations[i]);
				}

				ret += next_code.Length * int.Parse(code[..^1]);
			}

			return Task.FromResult<object>(
				ret
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			Point2D[] directionRobotLocations = new Point2D[25];
			Point2D numpadRobotLocation = NumberKeypadStart;

			for (int i = 0; i < directionRobotLocations.Length; ++i)
			{
				directionRobotLocations[i] = DirectionKeypadStart;
			}

			StringBuilder prev_code = new StringBuilder();
			StringBuilder next_code = new StringBuilder();
			int ret = 0;

			foreach (var code in Codes)
			{
				prev_code.Clear();
				prev_code.Append(code);
				next_code = NumberKeypadNav(prev_code, next_code, ref numpadRobotLocation);

				for (int i = 0; i < directionRobotLocations.Length; ++i)
				{
					(prev_code, next_code) = (next_code, prev_code);
					next_code = DirectionKeypadNav(prev_code, next_code, ref directionRobotLocations[i]);
				}

				ret += next_code.Length * int.Parse(code[..^1]);
			}

			return Task.FromResult<object>(
				1
			);
		}

		private StringBuilder NumberKeypadNav(StringBuilder code, StringBuilder ret, ref Point2D startLocation)
		{
			return NavigateKeypad(code, ret, NumberKeypad, NumberKeypadRoutePoints, ref startLocation);
		}

		private StringBuilder DirectionKeypadNav(StringBuilder code, StringBuilder ret, ref Point2D startLocation)
		{
			return NavigateKeypad(code, ret, DirectionKeypad, DirectionKeypadRoutePoints, ref startLocation);
		}

		private static StringBuilder NavigateKeypad(StringBuilder code, StringBuilder ret, string[][] keypad, Dictionary<(char, char), List<DijkstraAlgorithm.DijkstraNode>> routes, ref Point2D startLocation)
		{
			ret.Clear();

			for (int codeIndex = 0; codeIndex < code.Length; ++codeIndex)
			{
				var sourceButton = keypad[startLocation.Y][startLocation.X][0];
				var destButton = code[codeIndex];

				if (sourceButton != destButton)
				{
					var routeKey = (sourceButton, destButton);
					var route = routes[routeKey];
					
					if (!RouteLUT.TryGetValue((sourceButton, destButton), out var finalRoute))
					{
						TempStringBuilder.Clear();

						for (int i = 1; i < route.Count; ++i)
						{
							var dir = route[i].Position - route[i - 1].Position;
							TempStringBuilder.Append(DirectionInput[dir]);
						}

						finalRoute = TempStringBuilder.ToString();
						RouteLUT.Add((sourceButton, destButton), finalRoute);
					}

					startLocation = route[^1].Position;
					ret.Append(finalRoute);
				}

				ret.Append("A");
			}

			return ret;
		}
	}
}
