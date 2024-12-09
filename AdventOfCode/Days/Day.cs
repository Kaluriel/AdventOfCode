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
			System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
			StringBuilder strBuilder = new StringBuilder();
#if TEST
			int dataFileCount = GetTestFileCount();
#else
			int dataFileCount = 1;
#endif

			strBuilder.AppendLine($"--- [ {GetType().Name} ] -----------------------");

			for (int testIndex = 0; testIndex < dataFileCount; ++testIndex)
			{
#if TEST
				strBuilder.AppendLine($"Test {testIndex + 1}");
#endif

				try
				{
#if TEST
					string[] testResults = await ReadDayResultsFileAsync(testIndex);
#endif
					await LoadAsync(testIndex);

					// Execute any shared tasks initially
					await ExecuteSharedAsync();

					// Output the results
					for (int part = 0; part < 2; ++part)
					{
#if TEST
						if ((part >= testResults.Length) || testResults[part] == "-")
						{
							continue;
						}
#endif

						string? strResult = null;
						bool exception = false;

						strBuilder.Append($"\tPart {part + 1}");

						sw.Restart();

						try
						{
							var task = part switch
							{
								0 => ExecutePart1Async(testIndex),
								1 => ExecutePart2Async(testIndex),
								_ => throw new NotImplementedException(part.ToString())
							};

							task = task.ContinueWith(
								x =>
								{
									sw.Stop();
									return x;
								}
							).Unwrap();

							strResult = (await task).ToString();
						}
						catch (System.Exception ex)
						{
							strResult = ex.Message + Environment.NewLine + ex.StackTrace;
							exception = true;
						}

#if TEST
						bool success = (strResult == testResults[part]);
#endif
						string? report = null;

						if (exception)
						{
							report = "exception";
						}
#if TEST
						else if (success)
						{
							report = "success";
						}
						else
						{
							report = "failed";
						}
#endif

						strBuilder.Append($" [");
						if (!string.IsNullOrWhiteSpace(report))
						{
							strBuilder.Append($"{report} in ");
						}
						if (sw.ElapsedMilliseconds == 0)
						{
							strBuilder.Append($"{(int)(sw.Elapsed.TotalMilliseconds * 1000.0)}ns");
						}
						else
						{
							strBuilder.Append($"{sw.ElapsedMilliseconds}ms");
						}

						strBuilder.Append($"] => {strResult?.Replace("\n", "\n\t\t")}");

#if TEST
						if (!success)
						{
							strBuilder.Append($" (Expected {testResults[part]?.Replace("\n", "\n\t\t")})");
						}
#endif

						strBuilder.AppendLine();
					}
				}
				catch (Exception ex)
				{
					Log(ex.Message);
				}
				finally
				{
					await CleanupAsync();
				}
			}

			Log(strBuilder.ToString());
		}

		protected virtual Task ExecuteSharedAsync()
		{
			return Task.CompletedTask;
		}

		protected abstract Task<object> ExecutePart1Async(int testIndex);
		protected abstract Task<object> ExecutePart2Async(int testIndex);

		public static void Log(string text)
		{
			System.Diagnostics.Debug.WriteLine(text);
			Console.WriteLine(text);
		}

		protected Task<string> ReadDayDataFileAsync(int index = 0)
		{
			string srcPath = GetDaySourceFilePath(
#if TEST
				$"_{index:00}_Data"
#endif
			);
			return System.IO.File.ReadAllTextAsync(srcPath);
		}

#if TEST
		private async Task<string[]> ReadDayResultsFileAsync(int index = 0)
		{
			return (await System.IO.File.ReadAllTextAsync(
				GetDaySourceFilePath($"_{index:00}_Results")
			))
			.SplitDoubleNewLine()
			.Select(x => x.Replace("\r\n", "\n")
						       .Replace("\n", Environment.NewLine))
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
		
		public static string DrawGrid(char[][] grid)
		{
			StringBuilder strBuilder = new StringBuilder();

			for (int y = 0; y < grid.Length; ++y)
			{
				if (y > 0)
				{
					strBuilder.AppendLine();
				}

				for (int x = 0; x < grid[y].Length; ++x)
				{
					strBuilder.Append($"{grid[y][x]}");
				}
			}

			return strBuilder.ToString();
		}
	}
}
