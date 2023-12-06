using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day05 : DayBase2023
	{
		enum ECategory
		{
			Seed,
			Soil,
			Fertilizer,
			Water,
			Light,
			Temperature,
			Humidity,
			Location,
		}

		class Conversion
		{
			public struct ConvertMap
			{
				public Int64 DestinationStart { get; set; }
				public Int64 SourceStart { get; set; }
				public Int64 RangeLength { get; set; }
				public Int64 SourceEnd => SourceStart + (RangeLength - 1);
			}
			
			public ECategory From { get; set; }
			public ECategory To { get; set; }
			public ConvertMap[] Mapping { get; set; }

			public Int64 Convert(Int64 val)
			{
				for (int i = 0; i < Mapping.Length; ++i)
				{
					if ((val >= Mapping[i].SourceStart) && (val <= Mapping[i].SourceEnd))
					{
						return Mapping[i].DestinationStart + (val - Mapping[i].SourceStart);
					}
				}

				return val;
			}
		}

		private Int64[] Seeds;
		private Conversion[] Conversions;

		protected override Task ExecuteSharedAsync()
		{
			var data = Source.SplitDoubleNewLine(StringSplitOptions.RemoveEmptyEntries);

			Seeds = data.First()
						.Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)
						.Skip(1)
						.Select(Int64.Parse)
						.ToArray();
			
			Conversions = data.Skip(1)
							  .Select(x => x.SplitNewLine()
												 .Select(y => y.Split(new[]{'-', ' '}, StringSplitOptions.RemoveEmptyEntries)))
							  .Select(
							      x => new Conversion()
							      {
							      	  From = Enum.Parse<ECategory>(x.First()
																		 .First(), true),
							          To = Enum.Parse<ECategory>(x.First()
																	   .Skip(2)
																	   .First(), true),
							          Mapping = x.Skip(1)
												 .Select(y => y.Select(Int64.Parse))
												 .Select(
													 y => new Conversion.ConvertMap()
													 {
														 DestinationStart = y.First(),
														 SourceStart = y.Skip(1).First(),
														 RangeLength = y.Skip(2).First(),
													 }
												 )
												 .ToArray()
							      }
							  )
							  .ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			Int64 ret = int.MaxValue;

			for (int i = 0; i < Seeds.Length; ++i)
			{
				Int64 newVal = Seeds[i];

				for (int j = 0; j < Conversions.Length; ++j)
				{
					newVal = Conversions[j].Convert(newVal);
				}

				ret = Math.Min(newVal, ret);
			}
			
			return Task.FromResult<object>(
				ret
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			Int64[] results = new Int64[Seeds.Length / 2];

			Parallel.For(
				0,
				results.Length,
				(int index) =>
				{
					Int64 ret = int.MaxValue;
					int i = index * 2;
					
					for (int r = 0; r < Seeds[i + 1]; ++r)
					{
						Int64 newVal = Seeds[i] + r;

						for (int j = 0; j < Conversions.Length; ++j)
						{
							newVal = Conversions[j].Convert(newVal);
						}

						ret = Math.Min(newVal, ret);
					}
					
					results[index] = ret;
				}
			);
			
			return Task.FromResult<object>(
				results.Min()
			);
		}
	}
}
