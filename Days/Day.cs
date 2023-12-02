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

			strBuilder.AppendLine($"{GetType().Name}");
			strBuilder.AppendLine($"------------------");

			for (int index = 0; index < dataFileCount; ++index)
			{
#if TEST
				strBuilder.AppendLine($"Test {index + 1}");
#endif

				try
				{
#if TEST
					string[] testResults = await ReadDayResultsFileAsync(index);
#endif
					await LoadAsync(index);

					// Execute any shared tasks initially
					await ExecuteSharedAsync();

					// Output the results
					for (int part = 0; part < 2; ++part)
					{
						string? strResult = null;
						bool exception = false;

						strBuilder.Append($"\tPart {part + 1}");

						sw.Restart();

#if TEST
						bool skip = (part >= testResults.Length) || testResults[part] == "-";
						if (!skip)
#endif
						{
							try
							{
								var task = part switch
								{
									0 => ExecutePart1Async(),
									1 => ExecutePart2Async(),
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
								strResult = ex.Message;
								exception = true;
							}
						}

#if TEST
						bool success = skip || (strResult == testResults[part]);
#endif
						string? report = null;

						if (exception)
						{
							report = "exception";
						}
#if TEST
						else if (skip)
						{
							report = "skipped";
						}
						else if (success)
						{
							report = "success";
						}
						else
						{
							report = "failed";
						}
#endif

						if (!string.IsNullOrWhiteSpace(report))
						{
							strBuilder.Append($" [{report}]");
						}

						if (sw.ElapsedMilliseconds == 0)
						{
							strBuilder.AppendLine($" - {(int)(sw.Elapsed.TotalMilliseconds * 1000.0)}ns");
						}
						else
						{
							strBuilder.AppendLine($" - {sw.ElapsedMilliseconds}ms");
						}

#if TEST
						if (skip)
						{
							continue;
						}
#endif
						strBuilder.AppendLine($"\t\t{strResult?.Replace("\n", "\n\t\t")}");

#if TEST
						if (!success)
						{
							strBuilder.AppendLine($"\t[expected]:");
							strBuilder.AppendLine($"\t\t{testResults[part]?.Replace("\n", "\n\t\t")}");
						}
#endif
					}
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

		protected abstract Task<object> ExecutePart1Async();
		protected abstract Task<object> ExecutePart2Async();

		protected static void Log(string text)
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
	}
}
