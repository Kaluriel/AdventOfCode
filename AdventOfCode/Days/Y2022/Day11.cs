using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public class Day11 : DayBase2022
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

		protected override Task<object> ExecutePart1Async()
		{
			MonkeyData[] monkeyData = Monkeys.Select(x => new MonkeyData(x))
											 .ToArray();
			
			for (int r = 0; r < 20; ++r)
			{
				for (int m = 0; m < monkeyData.Length; ++m)
				{
					monkeyData[m].ItemsInspected += (UInt64)monkeyData[m].Items.Count;

					for (int i = 0; i < monkeyData[m].Items.Count; ++i)
					{
						monkeyData[m].Items[i] = ParseExpression(monkeyData[m].Monkey, monkeyData[m].Items[i]) / 3;
						int targetMonkey = monkeyData[m].Monkey.TestFalseMonkey;

						if ((monkeyData[m].Items[i] % monkeyData[m].Monkey.TestConstant) == 0)
						{
							targetMonkey = monkeyData[m].Monkey.TestTrueMonkey;
						}

						monkeyData[targetMonkey].Items.Add(monkeyData[m].Items[i]);
					}

					monkeyData[m].Items.Clear();
				}
			}

			return Task.FromResult<object>(
				monkeyData.OrderByDescending(x => x.ItemsInspected)
						  .Select(x => x.ItemsInspected)
						  .Take(2)
						  .Aggregate((x, y) => x * y)
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			MonkeyData[] monkeyData = Monkeys.Select(x => new MonkeyData(x))
											 .ToArray();

			UInt64 lcm = monkeyData.Select(x => x.Monkey.TestConstant)
								   .Aggregate((x, y) => x * y);

			for (int r = 0; r < 10000; ++r)
			{
				for (int m = 0; m < monkeyData.Length; ++m)
				{
					monkeyData[m].ItemsInspected += (UInt64)monkeyData[m].Items.Count;

					for (int i = 0; i < monkeyData[m].Items.Count; ++i)
					{
						monkeyData[m].Items[i] = ParseExpression(monkeyData[m].Monkey, monkeyData[m].Items[i]) % lcm;
						int targetMonkey = monkeyData[m].Monkey.TestFalseMonkey;

						if ((monkeyData[m].Items[i] % monkeyData[m].Monkey.TestConstant) == 0)
						{
							targetMonkey = monkeyData[m].Monkey.TestTrueMonkey;
						}

						monkeyData[targetMonkey].Items.Add(monkeyData[m].Items[i]);
					}

					monkeyData[m].Items.Clear();
				}
			}

			return Task.FromResult<object>(
				monkeyData.OrderByDescending(x => x.ItemsInspected)
						  .Select(x => x.ItemsInspected)
						  .Take(2)
						  .Aggregate((x, y) => x * y)
			);
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

						switch (operation)
						{
							case "*":
								ret *= val;
								break;

							case "+":
								ret += val;
								break;

							default:
								throw new NotImplementedException(operation);
						}
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
