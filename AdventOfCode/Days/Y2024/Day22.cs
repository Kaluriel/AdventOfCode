using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day22 : Day
	{
		sealed class ArrayEqualityComparer : IEqualityComparer<int[]>
		{
			public bool Equals(int[]? x, int[]? y)
			{
				if (x is null && y is null) return true;
				if (x is null || y is null) return false;
				return x.SequenceEqual(y);
			}

			public int GetHashCode(int[] obj)
			{
				return String.Join(",", obj.Select(x => x.ToString())).GetHashCode();
			}
		}
		
		private const int NumberOfSecretsToGenerate = 2000;
		private const int SequenceMatchCount = 4;
		
		class Monkey
		{
			public Int64[] SecretNumbers = new Int64[NumberOfSecretsToGenerate + 1];
			public Int64[] Prices = new Int64[NumberOfSecretsToGenerate + 1];
			public Int64[] Changes = new Int64[NumberOfSecretsToGenerate + 1];
			public int[] ChangeHash = new int[NumberOfSecretsToGenerate + 1];
		}
		
		private Int64[] SecretNumbers;

		protected override Task ExecuteSharedAsync()
		{
			SecretNumbers = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(Int64.Parse)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				SecretNumbers
					.Sum(
						x =>
						{
							for (int i = 0; i < NumberOfSecretsToGenerate; ++i)
							{
								x = NextSecretNumber(x);
							}

							return x;
						}
					)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			var monkeys = new Monkey[SecretNumbers.Length];

			for (int i = 0; i < monkeys.Length; ++i)
			{
				monkeys[i] = new Monkey();

				monkeys[i].SecretNumbers[0] = SecretNumbers[i];
				monkeys[i].Prices[0] = SecretNumbers[i] % 10;
				var previousPrice = monkeys[i].Prices[0];
				int sequence = 0;

				for (int j = 1; j < monkeys[i].SecretNumbers.Length; ++j)
				{
					var secretNumber = NextSecretNumber(monkeys[i].SecretNumbers[j - 1]);

					monkeys[i].SecretNumbers[j] = secretNumber;
					monkeys[i].Prices[j] = secretNumber % 10;

					monkeys[i].Changes[j] = monkeys[i].Prices[j] - previousPrice;
					previousPrice = monkeys[i].Prices[j];
					
					sequence = (sequence << 8) | (byte)(monkeys[i].Changes[j] & 0xff);
					monkeys[i].ChangeHash[j] = sequence;
				}
			}

			Int64 bestSum = 0;
			
			for (int bestNumber = 9; bestNumber >= 0; --bestNumber)
			//int bestNumber = 7;
			{
				HashSet<int> foundSequences = new HashSet<int>();
				int bestPossiblePrice = bestNumber * monkeys.Length;

				for (int monkeyIndex = 0; monkeyIndex < monkeys.Length; ++monkeyIndex)
				{
					int startIndex = 0;

					do
					{
						startIndex = Array.IndexOf(monkeys[monkeyIndex].Prices, bestNumber, startIndex);
						if (startIndex != -1)
						{
							foundSequences.Add(monkeys[monkeyIndex].ChangeHash[startIndex]);
							++startIndex;
						}
					}
					while (startIndex != -1);
				}

				foreach (var sequence in foundSequences)
				{
					Int64 currentBestSum = 0;

					Parallel.For(
						0,
						monkeys.Length,
						(monkeyIndex) =>
						{
							int index = Array.IndexOf(monkeys[monkeyIndex].ChangeHash, sequence, SequenceMatchCount + 1);
							if (index != -1)
							{
								var price = monkeys[monkeyIndex].Prices[index];
								Interlocked.Add(ref currentBestSum, price);
							}
						}
					);

					if (currentBestSum > bestSum)
					{
						bestSum = currentBestSum;
						if (bestSum == bestPossiblePrice)
						{
							break;
						}
					}
				}
			}
			
			return Task.FromResult<object>(
				bestSum
			);
		}

		private static Int64 NextSecretNumber(Int64 previousSecretNumber)
		{
			previousSecretNumber = MixAndPrune(previousSecretNumber * 64, previousSecretNumber);
			previousSecretNumber = MixAndPrune(previousSecretNumber / 32, previousSecretNumber);
			return MixAndPrune(previousSecretNumber * 2048, previousSecretNumber);
		}

		private static Int64 MixAndPrune(Int64 value1, Int64 value2)
		{
			return (value1 ^ value2) & 16777215;
		}
	}
}
