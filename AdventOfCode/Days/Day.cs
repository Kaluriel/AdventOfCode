using AdventOfCode.Ext;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days
{
	public abstract class Day
	{
		public string Source { get; private set; } = string.Empty;
		public int Year { get; }

		protected Day(int year)
		{
			Year = year;
		}

		private async Task LoadAsync(int index)
		{
			Source = await ReadDayDataFileAsync(index);
		}

		private Task CleanupAsync()
		{
			Source = string.Empty;
			return Task.CompletedTask;
		}

		public async Task ExecuteAsync()
		{
#if DEBUG
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
#endif
			StringBuilder strBuilder = new StringBuilder();
#if TEST
			int dataFileCount = GetTestFileCount();
#else
			int dataFileCount = 1;
#endif

			strBuilder.AppendLine($"{GetType().Name}");
			strBuilder.AppendLine($"------------------");

			for (int index = 0; index < dataFileCount; ++index)
			{
#if DEBUG
				sw.Restart();
#endif

#if TEST
				strBuilder.AppendLine($"Test {index + 1}");
#endif

				try
				{
#if TEST
					var testResults = await ReadDayResultsFileAsync(index);
#endif
					await LoadAsync(index);

					// Execute any shared tasks initially
					await ExecuteSharedAsync();

					// Now execute our part tasks
					var tasks = new[]
					{
						ExecutePart1Async(),
						ExecutePart2Async(),
					};

					// Wait for them to finish
					var results = await Task.WhenAll(tasks);

					// Output the results
					for (int i = 0; i < results.Length; ++i)
					{
						string? strResult = results[i].ToString();

						strBuilder.Append($"\tPart {i + 1}");
#if TEST
						bool success = strResult == testResults[i];
						strBuilder.AppendLine($" [{(success ? "success" : "failed")}]");
#else
						strBuilder.AppendLine();
#endif
						strBuilder.AppendLine($"\t\t{strResult?.Replace("\n", "\n\t\t")}");

#if TEST
						if (!success)
						{
							strBuilder.AppendLine($"\t[expected]:");
							strBuilder.AppendLine($"\t\t{testResults[i]?.Replace("\n", "\n\t\t")}");
						}
#endif
					}
				}
				finally
				{
					await CleanupAsync();
				}

#if DEBUG
				sw.Stop();
				strBuilder.AppendLine($"\tExecution Time: {sw.ElapsedMilliseconds}ms");
#endif
			}

			Log(strBuilder.ToString());
		}

		protected virtual Task ExecuteSharedAsync()
		{
			return Task.CompletedTask;
		}

		protected abstract Task<object> ExecutePart1Async();
		protected abstract Task<object> ExecutePart2Async();

		protected static void Log(string solution)
		{
			System.Diagnostics.Debug.WriteLine(solution);
		}

		protected Task<string> ReadDayDataFileAsync(int index = 0)
		{
			return System.IO.File.ReadAllTextAsync(
				GetDaySourceFilePath(
#if TEST
					$"_{index:00}_Data"
#endif
				)
			);
		}

#if TEST
		private async Task<string[]> ReadDayResultsFileAsync(int index = 0)
		{
			return (await System.IO.File.ReadAllTextAsync(
				GetDaySourceFilePath($"_{index:00}_Results")
			))
			.SplitDoubleNewLine()
			.Select(x => x.Replace("\r\n", "\n"))
			.ToArray();
		}

		private int GetTestFileCount()
		{
			int count = 0;
			
			for (; ; ++count)
			{
				string path = GetDaySourceFilePath($"_{count:00}_Results");

				if (!System.IO.File.Exists(path))
				{
					break;
				}
			}

			return count;
		}
#endif

		protected string GetDaySourceFolderPath()
		{
			return $"Data/Y{Year}/"
#if TEST
				+ "Test/"
#endif
				;
		}

		protected string GetDaySourceFileName(string suffix = "")
		{
			return $"{GetType().Name}{suffix}.txt";
		}

		protected string GetDaySourceFilePath(string suffix = "")
		{
			return GetDaySourceFolderPath() + GetDaySourceFileName(suffix);
		}
	}
}
