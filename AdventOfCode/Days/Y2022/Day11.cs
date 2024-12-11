using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public sealed class Day11 : Day
	{
		private class Monkey
		{
			public List<UInt64> StartingItems { get; set; }
			public string[] Expression { get; set; }
			public UInt64 TestConstant { get; set; }
			public int TestTrueMonkey { get; set; }
			public int TestFalseMonkey { get; set; }
		}
		private class MonkeyData
		{
			public Monkey Monkey { get; }
			public List<UInt64> Items { get; } = new List<UInt64>();
			public UInt64 ItemsInspected { get; set; }

			public MonkeyData(Monkey monkey)
			{
				Monkey = monkey;
				Items = monkey.StartingItems.ToList();
			}
		}
		private IEnumerable<Monkey> Monkeys = Enumerable.Empty<Monkey>();

		protected override Task ExecuteSharedAsync()
		{
			Monkeys = GetMonkeys().ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			MonkeyData[] monkeyData = Monkeys.Select(x => new MonkeyData(x))
											 .ToArray();
			
			for (int r = 0; r < 20; ++r)
			{
				RunMonkeys(monkeyData, 3, true);
			}

			return Task.FromResult<object>(
				monkeyData.OrderByDescending(x => x.ItemsInspected)
						  .Take(2)
						  .Select(x => x.ItemsInspected)
						  .Aggregate((x, y) => x * y)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			MonkeyData[] monkeyData = Monkeys.Select(x => new MonkeyData(x))
											 .ToArray();

			UInt64 lcm = monkeyData.Select(x => x.Monkey.TestConstant)
								   .Aggregate((x, y) => x * y);

			for (int r = 0; r < 10000; ++r)
			{
				RunMonkeys(monkeyData, lcm, false);
			}

			return Task.FromResult<object>(
				monkeyData.OrderByDescending(x => x.ItemsInspected)
						  .Take(2)
						  .Select(x => x.ItemsInspected)
						  .Aggregate((x, y) => x * y)
			);
		}

		private void RunMonkeys(MonkeyData[] monkeyData, UInt64 worryReducer, bool useDivide)
		{
			// for each monkey
			for (int m = 0; m < monkeyData.Length; ++m)
			{
				// for each item
				for (int i = 0; i < monkeyData[m].Items.Count; ++i)
				{
					// change worry level
					monkeyData[m].Items[i] = ParseExpression(monkeyData[m].Monkey, monkeyData[m].Items[i]);

					// reduce worry
					if (useDivide)
					{
						monkeyData[m].Items[i] /= worryReducer;
					}
					else
					{
						monkeyData[m].Items[i] %= worryReducer;
					}

					// move item to target monkey
					int targetMonkey = ((monkeyData[m].Items[i] % monkeyData[m].Monkey.TestConstant) == 0)
						? monkeyData[m].Monkey.TestTrueMonkey
						: monkeyData[m].Monkey.TestFalseMonkey;

					monkeyData[targetMonkey].Items.Add(monkeyData[m].Items[i]);
				}

				// increment items inspected and reset item list
				monkeyData[m].ItemsInspected += (UInt64)monkeyData[m].Items.Count;
				monkeyData[m].Items.Clear();
			}
		}

		// this is just a sequence, it doesn't have an order of operations
		private UInt64 ParseExpression(Monkey monkey, UInt64 old)
		{
			UInt64 ret = GetValue(monkey.Expression[0], old);
			if (monkey.Expression.Length > 1)
			{
				string operation = monkey.Expression[1];

				for (int i = 2; i < monkey.Expression.Length; ++i)
				{
					if ((i % 2) == 0)
					{
						UInt64 val = GetValue(monkey.Expression[i], old);

						ret = operation switch
						{
							"*" => ret * val,
							"+" => ret + val,
							_ => throw new NotImplementedException(operation)
						};
					}
					else
					{
						operation = monkey.Expression[i];
					}
				}
			}

			return ret;
		}

		private UInt64 GetValue(string expressionPart, UInt64 old)
		{
			UInt64 ret = old;

			if (expressionPart != "old")
			{
				ret = UInt64.Parse(expressionPart);
			}

			return ret;
		}

		private IEnumerable<Monkey> GetMonkeys()
		{
			return Source.SplitDoubleNewLine()
						 .Select(x => x.SplitNewLine()
									   .Skip(1)
									   .Select(y => y.Split(": ")[1]
													 .Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries))
									   .ToArray())
						 .Select(
							 x => new Monkey()
							 {
								 StartingItems = x[0].Select(UInt64.Parse)
													 .ToList(),
								 Expression = x[1].Skip(2)
												  .ToArray(),
								 TestConstant = x[2].Skip(2)
													.Select(UInt64.Parse)
													.First(),
								 TestTrueMonkey = x[3].Skip(3)
													  .Select(int.Parse)
													  .First(),
								 TestFalseMonkey = x[4].Skip(3)
													   .Select(int.Parse)
													   .First()
							 }
						 );
		}
	}
}
