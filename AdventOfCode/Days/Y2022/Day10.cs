using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public class Day10 : DayBase2022
	{
		delegate void OperandAction(ref int registerX, int val);

		private struct Operand
		{
			public int Cycles { get; set; }
			public OperandAction Func { get; set; }

			public Operand(int cycles, OperandAction func)
			{
				Cycles = cycles;
				Func = func;
			}

			public static void noop(ref int registerX, int val)
			{
			}

			public static void addx(ref int registerX, int val)
			{
				registerX += val;
			}
		}

		private static readonly IReadOnlyDictionary<string, Operand> Operands = new Dictionary<string, Operand>()
		{
			{ "addx", new Operand(2, Operand.addx) },
			{ "noop", new Operand(1, Operand.noop) },
		};
		private IEnumerable<KeyValuePair<string, int>> Program = Enumerable.Empty<KeyValuePair<string, int>>();

		protected override Task ExecuteSharedAsync()
		{
			Program = GetProgram();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			int sumOfSignalStrengths = 0;
			int cycleCount = 0;
			int registerX = 1;

			foreach (var instr in Program)
			{
				for (int cycle = 0; cycle < Operands[instr.Key].Cycles; ++cycle)
				{
					++cycleCount;

					if (((cycleCount - 20) % 40) == 0)
					{
						sumOfSignalStrengths += cycleCount * registerX;
						if (cycleCount == 240)
						{
							break;
						}
					}
				}

				if (cycleCount == 240)
				{
					break;
				}

				Operands[instr.Key].Func.Invoke(ref registerX, instr.Value);
			}

			return Task.FromResult<object>(
				sumOfSignalStrengths
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			StringBuilder strBuilder = new StringBuilder();
			int cycleCount = 0;
			int registerX = 1;

			foreach (var instr in Program)
			{
				for (int cycle = 0; cycle < Operands[instr.Key].Cycles; ++cycle)
				{
					++cycleCount;

					int pixelX = (cycleCount % 40) - 1;
					if ((pixelX >= (registerX - 1)) && (pixelX <= (registerX + 1)))
					{
						strBuilder.Append("#");
					}
					else
					{
						strBuilder.Append(".");
					}

					if (cycleCount >= 240)
					{
						break;
					}

					if (pixelX == -1)
					{
						strBuilder.AppendLine();
					}
				}

				if (cycleCount == 240)
				{
					break;
				}

				Operands[instr.Key].Func.Invoke(ref registerX, instr.Value);
			}

			return Task.FromResult<object>(
				strBuilder.ToString()
			);
		}

		private IEnumerable<KeyValuePair<string, int>> GetProgram()
		{
			return Source.SplitNewLine()
						 .Select(x => x.Split(' '))
						 .Select(
							 y => new KeyValuePair<string, int>(
								 y[0],
								 (y.Length > 1) ? int.Parse(y[1]) : 0
							 )
						 );
		}
	}
}
