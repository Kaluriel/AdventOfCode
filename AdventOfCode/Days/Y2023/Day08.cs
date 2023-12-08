using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day08 : DayBase2023
	{
		private int[] Directions;
		private Dictionary<string, string[]> Maps;
		private string[] StartLocations;
		private string[] EndLocations;
		
		protected override Task ExecuteSharedAsync()
		{
			var data = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);
				
            Directions = data.First()
							 .Select(c => c == 'L' ? 0 : 1)
							 .ToArray();
            
            Maps = data.Skip(1)
					   .First()
					   .SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
			           .Select(x => x.Split(new[]{ ' ', '=', ',', '(', ')'}, StringSplitOptions.RemoveEmptyEntries))
			           .ToDictionary(
						   x => x.First(),
						   x => x.Skip(1)
									   .ToArray()
					   );

            StartLocations = Maps.Keys.Where(x => x.EndsWith("A"))
									  .ToArray();
            EndLocations = Maps.Keys.Where(x => x.EndsWith("Z"))
									.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			string currentMap = "AAA";
			int steps = 0;

			while (currentMap != "ZZZ")
			{
				int directionIndex = steps % Directions.Length;
				int direction = Directions[directionIndex];
				currentMap = Maps[currentMap][direction];
				++steps;
			}
			
			return Task.FromResult<object>(
				steps
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			string[] currentMaps = (string[])StartLocations.Clone();
			Int64[] steps = new Int64[currentMaps.Length];

			for (int location = 0; location < currentMaps.Length; ++location)
			{
				while (!EndLocations.Contains(currentMaps[location]))
				{
					Int64 directionIndex = steps[location] % Directions.Length;
					int direction = Directions[directionIndex];
					string currentMap = currentMaps[location];

					currentMaps[location] = Maps[currentMap][direction];
					++steps[location];
				}
			}

			return Task.FromResult<object>(
				steps.LowestCommonMultiple()
			);
		}
	}
}
