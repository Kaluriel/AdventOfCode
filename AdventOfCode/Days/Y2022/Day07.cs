using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using System.Text;

namespace AdventOfCode.Days.Y2022
{
	public class Day07 : DayBase2022
	{
		private struct FileInfo
		{
			public string Name { get; set; }
			public int Size { get; set; }
		}

		private class DirectoryInfo
		{
			public DirectoryInfo? Parent { get; }
			public string Name { get; }
			public List<FileInfo> Files { get; } = new List<FileInfo>();
			public List<DirectoryInfo> Directories { get; } = new List<DirectoryInfo>();

			public DirectoryInfo(string name, DirectoryInfo? parent)
			{
				Parent = parent;
				Name = name;
			}

			public int DirectorySize => Files.Sum(x => x.Size) + Directories.Sum(x => x.DirectorySize);
		}

		private List<DirectoryInfo> Directories = new List<DirectoryInfo>();
		private DirectoryInfo? RootDirectory = null;

		protected override Task ExecuteSharedAsync()
		{
			Directories = GetDirectories();
			RootDirectory = Directories.First();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			return Task.FromResult<object>(
				Directories.Select(x => x.DirectorySize)
				           .Where(x => x <= 100000)
						   .Sum()
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			int freeSpace = 70000000 - (RootDirectory?.DirectorySize ?? 0);
			int spaceNeeded = 30000000 - freeSpace;

			return Task.FromResult<object>(
				Directories.Where(x => x.DirectorySize >= spaceNeeded)
						   .OrderBy(x => x.DirectorySize)
						   .First()
						   .DirectorySize
			);
		}

		private List<DirectoryInfo> GetDirectories()
		{
			DirectoryInfo rootDirectory = new DirectoryInfo("/", null);
			List<DirectoryInfo> directories = new List<DirectoryInfo>()
			{
				rootDirectory
			};
			DirectoryInfo? currentDirectory = null;

			var commands = new Queue<string[]>(
				Source.SplitNewLine()
					  .Select(x => x.Split(' '))
					  .Where(x => x[0] != "ls")
			);

			string[]? commandParts;

			while (commands.TryDequeue(out commandParts))
			{
				if (commandParts[0] == "$")
				{
					if (commandParts[1] == "cd")
					{
						if (commandParts[2] == "/")
						{
							currentDirectory = rootDirectory;
						}
						else if (commandParts[2] == "..")
						{
							currentDirectory = currentDirectory?.Parent;
						}
						else
						{
							var directory = currentDirectory?.Directories.Find(x => x.Name == commandParts[2]);
							if (directory == null)
							{
								directory = new DirectoryInfo(commandParts[2], currentDirectory);
								currentDirectory?.Directories.Add(directory);
								directories.Add(directory);
							}
							currentDirectory = directory;
						}
					}
				}
				else if (commandParts[0] == "dir")
				{
					var directory = new DirectoryInfo(commandParts[1], currentDirectory);
					currentDirectory?.Directories.Add(directory);
					directories.Add(directory);
				}
				else
				{
					currentDirectory?.Files.Add(
						new FileInfo()
						{
							Name = commandParts[1],
							Size = int.Parse(commandParts[0])
						}
					);
				}
			}

			return directories;
		}
	}
}
