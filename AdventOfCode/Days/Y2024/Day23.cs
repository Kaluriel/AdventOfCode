using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day23 : Day
	{
		private Dictionary<string, HashSet<string>> Connections;

		protected override Task ExecuteSharedAsync()
		{
			var connections = Source
				.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
				.Select(line => line.Split('-', StringSplitOptions.RemoveEmptyEntries))
				.ToArray();

			Connections = new Dictionary<string, HashSet<string>>();

			foreach (var connection in connections)
			{
				Connections.TryAdd(connection[0], new HashSet<string>());
				Connections.TryAdd(connection[1], new HashSet<string>());
				Connections[connection[0]].Add(connection[1]);
				Connections[connection[1]].Add(connection[0]);
			}

			var ret = Connections.OrderByDescending(x => x.Value.Count)
				.First();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			HashSet<string> computerConnections = new HashSet<string>();

			foreach (var connection in Connections)
			{
				foreach (var combination in connection.Value.Combinations())
				{
					
				}
			}

			return Task.FromResult<object>(
				computerConnections.Count
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			List<HashSet<string>> computerGroups = new List<HashSet<string>>();

			foreach (var connection in Connections)
			{
				int index = computerGroups.FindIndex(group => group.Contains(connection.Key) || connection.Value.Any(group.Contains));
				if (index == -1)
				{
					index = computerGroups.Count;
					computerGroups.Add(new HashSet<string>(1 + connection.Value.Count));
				}
				
				computerGroups[index].Add(connection.Key);
				foreach (var primaryConnection in connection.Value)
				{
					computerGroups[index].Add(primaryConnection);
				}
			}
			
			return Task.FromResult<object>(
				String.Join(
					",",
					computerGroups
						.OrderByDescending(x => x.Count)
						.First()
						.OrderBy(x => x)
				)
			);
		}
	}
}
