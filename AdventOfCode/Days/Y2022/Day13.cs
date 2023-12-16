using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.DataTypes;
using System.Text;
using System.Text.Json;


namespace AdventOfCode.Days.Y2022
{
	public class Day13 : DayBase2022
	{
		private class PacketComparer : IComparer<JsonElement>
		{
			public int Compare(JsonElement x, JsonElement y)
			{
				return CompareJsonElements(x, y);
			}

			public static int CompareJsonElements(JsonElement left, JsonElement right)
			{
				if ((left.ValueKind == JsonValueKind.Number) && (right.ValueKind == JsonValueKind.Number))
				{
					return left.GetInt32().CompareTo(right.GetInt32());
				}
				else if ((left.ValueKind == JsonValueKind.Array) && (right.ValueKind == JsonValueKind.Number))
				{
					return CompareJsonElements(
						left,
						JsonSerializer.Deserialize<JsonElement>($"[{right.GetInt32()}]")
					);
				}
				else if ((left.ValueKind == JsonValueKind.Number) && (right.ValueKind == JsonValueKind.Array))
				{
					return CompareJsonElements(
						JsonSerializer.Deserialize<JsonElement>($"[{left.GetInt32()}]"),
						right
					);
				}
				else if ((left.ValueKind == JsonValueKind.Array) && (right.ValueKind == JsonValueKind.Array))
				{
					var leftArrayIter = left.EnumerateArray();
					var rightArrayIter = right.EnumerateArray();
					int arrayComp = leftArrayIter.Count() - rightArrayIter.Count();

					while (leftArrayIter.MoveNext() && rightArrayIter.MoveNext())
					{
						int comp = CompareJsonElements(leftArrayIter.Current, rightArrayIter.Current);
						if (comp != 0)
						{
							return comp;
						}
					}

					if (arrayComp == 0)
					{
						return 0;
					}

					return Math.Sign(arrayComp);
				}

				throw new NotImplementedException();
			}
	}

		private struct PacketPair
		{
			public JsonElement LeftData { get; set; }
			public JsonElement RightData { get; set; }

			public bool IsRightOrder()
			{
				var ret = PacketComparer.CompareJsonElements(LeftData, RightData);
				return ret < 0;
			}
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				Source.SplitDoubleNewLine()
					  .Select(x => x.SplitNewLine())
					  .Select(
						  x => new PacketPair()
						  {
						      LeftData = JsonSerializer.Deserialize<JsonElement>(x.First()),
							  RightData = JsonSerializer.Deserialize<JsonElement>(x.Skip(1).First())
						  }
					  )
					  .Select((x, i) => new { Index = i + 1, Data = x })
					  .Where(x => x.Data.IsRightOrder())
					  .Sum(x => x.Index)
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			const string twoElement = "[[2]]";
			const string sixElement = "[[6]]";

			return Task.FromResult<object>(
				Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
					  .Concat(new[] { twoElement, sixElement })
					  .OrderBy(x => JsonSerializer.Deserialize<JsonElement>(x), new PacketComparer())
					  .Select((x, i) => new { Index = i + 1, Data = x })
					  .Where(x => (x.Data == twoElement) || (x.Data == sixElement))
					  .Select(x => x.Index)
					  .Aggregate((x, y) => x * y)
			);
		}

	}
}
