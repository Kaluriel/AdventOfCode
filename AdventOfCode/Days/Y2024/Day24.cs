using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.Utils;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day24 : Day
	{
		enum BitwiseOp
		{
			And = 0,
			Or = 1,
			Xor = 2,
		}
		private Dictionary<string, bool> InitialWireValues;
		private Dictionary<string, (string, BitwiseOp, string)> WireGates;
		private string[] OutputNames;

		protected override Task ExecuteSharedAsync()
		{
			var parts = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);

			InitialWireValues = parts[0]
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(": ", StringSplitOptions.RemoveEmptyEntries))
				.ToDictionary(line => line[0], line => line[1] == "1");

			WireGates = parts[1]
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split(" ", StringSplitOptions.RemoveEmptyEntries))
				.ToDictionary(x => x[4], x => (x[0], Enum.Parse<BitwiseOp>(x[1], true), x[2]));

			OutputNames = WireGates
				.Where(x => x.Key.StartsWith("z"))
				.Select(x => x.Key)
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			var wireValues = new Dictionary<string, bool>(InitialWireValues);
			Int64 val = 0;

			foreach (var wireName in OutputNames)
			{
				bool wireValue = GetWireValue(wireName, wireValues);
				if (wireValue)
				{
					int shiftAmount = int.Parse(wireName.Substring(1));
					val |= (Int64)1 << shiftAmount;
				}
			}

			return Task.FromResult<object>(
				val
			);
		}

		private bool GetWireValue(string wireName, Dictionary<string, bool> wires)
		{
			if (wires.TryGetValue(wireName, out var value))
			{
				return value;
			}

			if (!WireGates.ContainsKey(wireName))
			{
				throw new NotImplementedException();
			}

			bool val1 = GetWireValue(WireGates[wireName].Item1, wires);
			bool val2 = GetWireValue(WireGates[wireName].Item3, wires);
			bool val = WireGates[wireName].Item2 switch
			{
				BitwiseOp.And => val1 && val2,
				BitwiseOp.Or => val1 || val2,
				BitwiseOp.Xor => val1 ^ val2,
				_ => throw new NotImplementedException(),
			};

			wires.Add(wireName, val);

			return val;
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				1
			);
		}
	}
}
