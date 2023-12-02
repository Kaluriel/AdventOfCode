using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public class Day05 : DayBase2022
	{
		private struct Instruction
		{
			public int Count { get; set; }
			public int Start { get; set; }
			public int End { get; set; }
		}
		private List<List<char>> CrateStacks = new List<List<char>>();
		private List<Instruction> Instructions = new List<Instruction>();

		protected override Task ExecuteSharedAsync()
		{
			GetCreateStacks(ref CrateStacks, ref Instructions);
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			var stackList = DuplicateCrateStacks();

			foreach (var instr in Instructions)
			{
				int offset = stackList[instr.Start].Count - instr.Count;

				stackList[instr.End].AddRange(
					stackList[instr.Start].Skip(offset)
										  .Reverse()
				);

				stackList[instr.Start].RemoveRange(offset, instr.Count);
			}

			return Task.FromResult<object>(
				GetTopOfCrateStacks(stackList)
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			var stackList = DuplicateCrateStacks();

			foreach (var instr in Instructions)
			{
				int offset = stackList[instr.Start].Count - instr.Count;

				stackList[instr.End].AddRange(
					stackList[instr.Start].Skip(offset)
				);

				stackList[instr.Start].RemoveRange(offset, instr.Count);
			}

			return Task.FromResult<object>(
				GetTopOfCrateStacks(stackList)
			);
		}

		private void GetCreateStacks(ref List<List<char>> crateStacks, ref List<Instruction> instructions)
		{
			var sourceParts = Source.SplitDoubleNewLine();

			// Crate Stacks
			var sourceCrateStack = sourceParts[0].SplitNewLine();

			for (int j = 0; j < sourceCrateStack[^1].Length; j += 4)
			{
				crateStacks.Add(
					sourceCrateStack.Reverse()
									.Skip(1)
									.Select(x => x[j + 1])
									.Where(x => x != ' ')
									.ToList()
				);
			}

			// Instructions
			instructions.AddRange(
				sourceParts[1].SplitNewLine()
							  .Select(x => x.Split(' ')
											.Where((x, i) => ((i % 2) == 1))
											.Select(int.Parse))
							  .Select(
								  x => new Instruction()
							      {
								      Count = x.ElementAt(0),
								      Start = x.ElementAt(1) - 1,
								      End = x.ElementAt(2) - 1,
							      }
							  )
			);
		}

		private List<List<char>> DuplicateCrateStacks()
		{
			return new List<List<char>>(
				CrateStacks.Select(
					x => new List<char>(x)
				)
			);
		}

		private string GetTopOfCrateStacks(List<List<char>> crateStacks)
		{
			return crateStacks.Aggregate(
								  new StringBuilder(),
								  (x, y) => x.Append(y.Last())
							  )
							  .ToString();
		}
	}
}
