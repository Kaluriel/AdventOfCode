using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading.Tasks;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day17 : Day
	{
		enum OpCode
		{
			adv = 0,
			bxl = 1,
			bst = 2,
			jnz = 3,
			bxc = 4,
			@out = 5,
			bdv = 6,
			cdv = 7,
		}

		private long RegisterA;
		private long RegisterB;
		private long RegisterC;
		private long[] Program;

		protected override Task ExecuteSharedAsync()
		{
			var lines = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries);
			RegisterA = long.Parse(lines[0].Substring(lines[0].IndexOf(": ", StringComparison.Ordinal) + 2));
			RegisterB = long.Parse(lines[1].Substring(lines[1].IndexOf(": ", StringComparison.Ordinal) + 2));
			RegisterC = long.Parse(lines[2].Substring(lines[2].IndexOf(": ", StringComparison.Ordinal) + 2));
			Program = lines[3]
				.Substring(lines[3].IndexOf(": ", StringComparison.Ordinal) + 2)
				.Split(',', StringSplitOptions.RemoveEmptyEntries)
				.Select(long.Parse)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				String.Join(",", RunProgram(Program, RegisterA, RegisterB, RegisterC))
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			long numPossibleRegisterA = (long)Math.Pow(8, Program.Length); // every 8 increments of the last number increases the output length
			long registerA = 0;

			for (; registerA < numPossibleRegisterA; ++registerA)
			{
				long[] program = RunProgram(Program, registerA, RegisterB, RegisterC);
				
				// check if arrays match
				if (program.SequenceEqual(Program))
				{
					break;
				}

				// check if output program matches the end numbers in our original program
				bool allMatch = Program
					.TakeLast(program.Length)					// take last values of original Program to the length of the output program
					.Zip(program, (x, y) => x == y)	// interlace with the output program and compare
					.All(x => x);							// check they all match

				if (allMatch)
				{
					// prevent infinite loop is register matches on 0
					if (registerA == 0)
					{
						registerA = 1;
					}

					// scale up by 8
					registerA = (registerA * 8);
					
					// decrement so the next cycle's increment brings it back,
					--registerA;
				}
			}
			
			return Task.FromResult<object>(
				registerA
			);
		}

		private static long[] RunProgram(long[] program, long registerA, long registerB, long registerC)
		{
			long ReadComboOperand(long pc)
			{
				return program[pc] switch
				{
					<= 3 => program[pc],
					4 => registerA,
					5 => registerB,
					6 => registerC,
					_ => throw new Exception("Unknown combo operand")
				};
			}

			List<long> @out = new List<long>();

			for (long pc = 0; pc < program.Length; pc += 2)
			{
				switch ((OpCode)program[pc])
				{
					case OpCode.adv:
						registerA /= (long)Math.Pow(2, ReadComboOperand(pc + 1));
						break;
					case OpCode.bxl:
						registerB ^= program[pc + 1];
						break;
					case OpCode.bst:
						registerB = ReadComboOperand(pc + 1) % 8;
						break;
					case OpCode.jnz:
						pc = (registerA != 0) ? program[pc + 1] - 2 : pc;
						break;
					case OpCode.bxc:
						registerB ^= registerC;
						break;
					case OpCode.@out:
						@out.Add(ReadComboOperand(pc + 1) % 8);
						break;
					case OpCode.bdv:
						registerB = registerA / (long)Math.Pow(2, ReadComboOperand(pc + 1));
						break;
					case OpCode.cdv:
						registerC = registerA / (long)Math.Pow(2, ReadComboOperand(pc + 1));
						break;
				}
			}

			return @out.ToArray();
		}
	}
}
