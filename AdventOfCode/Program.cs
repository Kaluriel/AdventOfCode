﻿using AdventOfCode.Days;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AdventOfCode.Utils;

namespace AdventOfCode
{
	class Program
	{
		static readonly int? YearOverride = null;
		static readonly int? DayOverride = null;
		private static int? NumDaysFromEnd = 1;

		static int Year => YearOverride ?? DateTime.UtcNow.Year;

		static readonly IEnumerable<Day> Days = ReflectionExt.GetTypeSubclasses<Day>(new object[0])
															 .Where(x => x.Year == Year)
															 .OrderBy(x => x.Year)
															 .ThenBy(x => x.GetType().Name);

		static void Main(string[] args)
		{
			TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

			MainAsync().Wait();

#if !DEBUG
			Console.ReadLine();
#endif
		}

		static async Task MainAsync()
		{
			Logging.Log($"--- [ YEAR {Year} ] -------------------");

			var days = Days;

			if (DayOverride.HasValue)
			{
				days = days.Where(day => day.DayNumber == DayOverride.Value);
			}
			else if (NumDaysFromEnd.HasValue)
			{
				days = days.TakeLast(NumDaysFromEnd.Value);
			}

			foreach (Day day in days)
			{
				try
				{
					await ExecuteDayAsync(day);
				}
				catch (System.Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
				}

				System.Diagnostics.Debug.WriteLine("");
				Console.WriteLine("");
			}
		}

		static async Task ExecuteDayAsync(Day day)
		{
			try
			{
				await day.ExecuteAsync();
			}
			catch (System.Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}

		private static void TaskScheduler_UnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
		{
			e.SetObserved();

			if (e.Exception is AggregateException ae)
			{
				ae.Handle(
					ex =>
					{
						Console.WriteLine("Exception type: {0}", ex.GetType());
						return true;
					}
				);
			}
		}
	}
}
