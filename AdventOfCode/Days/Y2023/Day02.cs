using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public class Day02 : DayBase2023
	{
		enum ECubeColor
		{
			Red,
			Green,
			Blue
		}
		
		struct BatchInfo
		{
			public KeyValuePair<ECubeColor, int>[] CubeCount { get; set; }
			public int Red => CubeCount.FirstOrDefault(x => x.Key == ECubeColor.Red).Value;
			public int Green => CubeCount.FirstOrDefault(x => x.Key == ECubeColor.Green).Value;
			public int Blue => CubeCount.FirstOrDefault(x => x.Key == ECubeColor.Blue).Value;
		}
		
		struct Game
		{
			public int ID { get; set; }
			public BatchInfo[] Batches;
		}

		private Game[] Games;

		protected override Task ExecuteSharedAsync()
		{
			Games = Source.SplitNewLine()
						  .Select(x => x.Split(new []{ ": ", "; " }, StringSplitOptions.RemoveEmptyEntries))
						  .Select(
							  x => new Game
							  {
								  ID = int.Parse(x[0].Substring(x[0].IndexOf(' ') + 1)),
								  Batches = x.Skip(1)
											 .Select(
	                                             y => new BatchInfo()
	                                             {
											 		CubeCount = y.Split(new [] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries)
											 					 .Chunk(2)
											 					 .Select(
											 						 z => new KeyValuePair<ECubeColor, int>(
											 							 Enum.Parse<ECubeColor>(z.Skip(1).First(), true),
											 							 int.Parse(z.First())
                                                                      )
											 					 )
											 					 .ToArray()
											 	}
											 )
											 .ToArray()
							  }
						  )
						  .ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				Games.Where(x => x.Batches.All(y => y.Red <= 12 && y.Green <= 13 && y.Blue <= 14))
					 .Select(x => x.ID)
					 .Sum()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				Games.Select(
					x => x.Batches.Max(y => y.Red) * x.Batches.Max(y => y.Green) * x.Batches.Max(y => y.Blue) 
				)
				.Sum()
			);
		}
	}
}
