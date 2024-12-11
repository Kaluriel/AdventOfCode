using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2024
{
	public sealed class Day07 : Day
	{
		private (Int64 TestValue, Int64[] Values)[] Calibrations;

		protected override Task ExecuteSharedAsync()
		{
			Calibrations = Source.SplitNewLine()
				.Select(line => line
					.Split([':', ' '], StringSplitOptions.RemoveEmptyEntries)
					.Select(Int64.Parse)
					.ToArray())
				.Select(x => (TestValue: x.First(), Values: x.Skip(1).ToArray()))
				.ToArray();

			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfTestValuesP1()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				SumOfTestValuesP2()
			);
		}

		private Int64 SumOfTestValuesP1()
		{
			Int64 ret = 0;

			foreach (var calibration in Calibrations)
			{
				int permutations = (int)Math.Pow(2, calibration.Values.Length - 1);

				for (int permutation = 0; permutation < permutations; ++permutation)
				{
					Int64 totalCalibrationResult = calibration.Values[0];

					for (int shift = 0; shift < (calibration.Values.Length - 1); ++shift)
					{
						if ((permutation & (1 << shift)) == 0)
						{
							totalCalibrationResult += calibration.Values[shift + 1];
						}
						else
						{
							totalCalibrationResult *= calibration.Values[shift + 1];
						}

						if (totalCalibrationResult > calibration.TestValue)
						{
							break;
						}
					}

					if (totalCalibrationResult == calibration.TestValue)
					{
						ret += totalCalibrationResult;
						break;
					}
				}
			}

			return ret;
		}

		private Int64 SumOfTestValuesP2()
		{
			Int64 ret = 0;

			Parallel.ForEach(
				Calibrations,
				(calibration) =>
				{
					int permutations = (int)Math.Pow(3, calibration.Values.Length - 1);

					for (int permutation = 0; permutation < permutations; ++permutation)
					{
						Int64 totalCalibrationResult = calibration.Values[0];
						int v = permutation;

						for (int i = 1; i < calibration.Values.Length; ++i, v /= 3)
						{
							totalCalibrationResult = (v % 3) switch
							{
								0 => totalCalibrationResult + calibration.Values[i],
								1 => totalCalibrationResult * calibration.Values[i],
								2 => Int64.Parse($"{totalCalibrationResult}{calibration.Values[i]}"),
								_ => throw new ArgumentOutOfRangeException()
							};

							if (totalCalibrationResult > calibration.TestValue)
							{
								break;
							}
						}

						if (totalCalibrationResult == calibration.TestValue)
						{
							Interlocked.Add(ref ret, totalCalibrationResult);
							break;
						}
					}
				}
			);

			return ret;
		}
	}
}
