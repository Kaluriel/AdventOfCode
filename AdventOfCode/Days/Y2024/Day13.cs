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
	public sealed class Day13 : Day
	{
		private const int CostOfButtonA = 3;
		private const int CostOfButtonB = 1;
		private struct MachineInfo
		{
			public Point2DLarge ButtonA;
			public Point2DLarge ButtonB;
			public Point2DLarge Prize;
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			MachineInfo[] machines = Source.SplitDoubleNewLine()
				.Select(x => x.SplitNewLine())
				.Select(x => new MachineInfo
					{
						ButtonA = new Point2DLarge(x
							.First()
							.Split(["X+", ", Y+"], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							.Select(y => new BigInteger(y))
							.ToArray()),
						ButtonB = new Point2DLarge(x
							.Skip(1)
							.First()
							.Split(["X+", ", Y+"], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							.Select(y => new BigInteger(y))
							.ToArray()),
						Prize = new Point2DLarge(x
							.Skip(2)
							.First()
							.Split(["X=", ", Y="], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							.Select(y => new BigInteger(y))
							.ToArray())
					}
				)
				.ToArray();
			return Task.FromResult<object>(
				CalculateFewestTokensToWinAllPrizes(machines)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			MachineInfo[] machines = Source.SplitDoubleNewLine()
				.Select(x => x.SplitNewLine())
				.Select(x => new MachineInfo
					{
						ButtonA = new Point2DLarge(x
							.First()
							.Split(["X+", ", Y+"], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							.Select(y => new BigInteger(y))
							.ToArray()),
						ButtonB = new Point2DLarge(x
							.Skip(1)
							.First()
							.Split(["X+", ", Y+"], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							.Select(y => new BigInteger(y))
							.ToArray()),
						Prize = new Point2DLarge(x
							.Skip(2)
							.First()
							.Split(["X=", ", Y="], StringSplitOptions.RemoveEmptyEntries)
							.Skip(1)
							.Select(int.Parse)
							//.Select(y => y * new BigInteger(1000000) * new BigInteger(10000000))
							.Select(y => new BigInteger(y))
							.ToArray())
					}
				)
				.ToArray();
			return Task.FromResult<object>(
				CalculateFewestTokensToWinAllPrizes(machines)
			);
		}

		private BigInteger CalculateFewestTokensToWinAllPrizes(MachineInfo[] machines)
		{
			BigInteger cost = 0;

			foreach (var machine in machines)
			{
				BigInteger machineCost = int.MaxValue;

				var maxButtonAPresses = MathExt.Min(
					(machine.Prize.X / machine.ButtonA.X),
					(machine.Prize.Y / machine.ButtonA.Y)
				);
				for (var buttonAPresses = 0; buttonAPresses < maxButtonAPresses; ++buttonAPresses)
				{
					var remainingPrizeVector = machine.Prize - (machine.ButtonA * buttonAPresses);
					var buttonBPresses = MathExt.Min(
						(remainingPrizeVector.X / machine.ButtonB.X),
						(remainingPrizeVector.Y / machine.ButtonB.Y)
					);
					remainingPrizeVector -= buttonBPresses * machine.ButtonB;

					if (remainingPrizeVector == 0)
					{
						var newCost = (buttonBPresses * CostOfButtonB) + (buttonAPresses * CostOfButtonA);
						if (newCost < machineCost)
						{
							machineCost = newCost;
						}
						else if (newCost > machineCost)
						{
							break;
						}
					}
				}

				{
					var maxButtonBPresses = MathExt.Min(
						(machine.Prize.X / machine.ButtonB.X),
						(machine.Prize.Y / machine.ButtonB.Y)
					);
					for (var buttonBPresses = 0; buttonBPresses < maxButtonBPresses; ++buttonBPresses)
					{
						var remainingPrizeVector = machine.Prize - (machine.ButtonB * buttonBPresses);
						var buttonAPresses = MathExt.Min(
							(remainingPrizeVector.X / machine.ButtonA.X),
							(remainingPrizeVector.Y / machine.ButtonA.Y)
						);
						remainingPrizeVector -= buttonAPresses * machine.ButtonA;

						if (remainingPrizeVector == 0)
						{
							var newCost = (buttonBPresses * CostOfButtonB) + (buttonAPresses * CostOfButtonA);
							if (newCost < machineCost)
							{
								machineCost = newCost;
							}
							else if (newCost > machineCost)
							{
								break;
							}
						}
					}
				}

				if (machineCost == int.MaxValue)
				{
					machineCost = 0;
				}

				cost += machineCost;
			}

			return cost;
		}
	}
}
