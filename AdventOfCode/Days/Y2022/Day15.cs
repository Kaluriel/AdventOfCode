using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.Ext;
using AdventOfCode.DataTypes;
using System.Text;
using System.Text.Json;


namespace AdventOfCode.Days.Y2022
{
	public class Day15 : DayBase2022
	{
		private class SensorInfo
		{
			public Point2D Sensor { get; }
			public Point2D SensorMin { get; }
			public Point2D SensorMax { get; }
			public Point2D ClosestBeacon { get; }
			public int ManhattenDistance { get; }

			public SensorInfo(Point2D sensor, Point2D beacon)
			{
				Sensor = sensor;
				ClosestBeacon = beacon;
				ManhattenDistance = Sensor.GetManhattenDistance(ClosestBeacon);
				SensorMin = sensor - new Point2D(ManhattenDistance, ManhattenDistance);
				SensorMax = sensor + new Point2D(ManhattenDistance, ManhattenDistance);
			}

			public int GetRowExtent(int row)
			{
				return ManhattenDistance - Math.Abs(row - Sensor.Y);
			}

			public bool IsInRange(Point2D point)
			{
				return Sensor.GetManhattenDistance(point) <= ManhattenDistance;
			}

			public bool Contains(Area2D area)
			{
				foreach (Point2D corner in area.GetCorners())
				{
					if (!IsInRange(corner))
					{
						return false;
					}
				}

				return true;
			}

			public bool OverlapEstimate(Area2D area)
			{
				Area2D a = new Area2D(
					SensorMin,
					SensorMax
				);

				return a.Overlaps(area);
			}
		}

#if TEST
		private const int Part1Row = 10;
		private const int LowestCoordinate = 0;
		private const int HighestCoordinate = 20;
#else
		private const int Part1Row = 2000000;
		private const int LowestCoordinate = 0;
		private const int HighestCoordinate = 4000000;
#endif

		private IEnumerable<SensorInfo> Sensors = Enumerable.Empty<SensorInfo>();

		protected override Task ExecuteSharedAsync()
		{
			Sensors = GetSensors();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async()
		{
			SensorInfo[] sensorsTrackingRow = Sensors.Where(x => (x.SensorMin.Y <= Part1Row) && (x.SensorMax.Y >= Part1Row))
													 .OrderBy(x => x.Sensor.X - x.GetRowExtent(Part1Row))
													 .ThenByDescending(x => x.GetRowExtent(Part1Row))
													 .ToArray();
			int beaconsOnRow = sensorsTrackingRow.Where(x => x.ClosestBeacon.Y == Part1Row)
												 .Select(x => x.ClosestBeacon)
												 .Distinct()
												 .Count();

			int extent = sensorsTrackingRow[0].GetRowExtent(Part1Row);
			int x = sensorsTrackingRow[0].Sensor.X + extent;
			int noBeaconCount = extent * 2;

			for (int index = 1; index < sensorsTrackingRow.Length; ++index)
			{
				extent = sensorsTrackingRow[index].GetRowExtent(Part1Row);
				int rowMaxX = sensorsTrackingRow[index].Sensor.X + extent;
				int stride = Math.Max(rowMaxX - x + 1, 0);
				noBeaconCount += stride;
				x += stride;
			}

			return Task.FromResult<object>(
				noBeaconCount - beaconsOnRow
			);
		}

		protected override Task<object> ExecutePart2Async()
		{
			int minX = Math.Max(Sensors.Min(x => x.SensorMin.X), LowestCoordinate);
			int maxX = Math.Min(Sensors.Max(x => x.SensorMax.X), HighestCoordinate);
			int minY = Math.Max(Sensors.Min(x => x.SensorMin.Y), LowestCoordinate);
			int maxY = Math.Min(Sensors.Max(x => x.SensorMax.Y), HighestCoordinate);
			UInt64 signalFrequency = 0;

			Point2D? point = FindZeroCoveragePoint(
				new Area2D(
					new Point2D(minX, minY),
					new Point2D(maxX, maxY)
				),
				Sensors.ToArray()
			);
			if (point.HasValue)
			{
				signalFrequency = (UInt64)point.Value.X * 4000000 + (UInt64)point.Value.Y;
			}

			return Task.FromResult<object>(
				signalFrequency
			);
		}

		private Point2D? FindZeroCoveragePoint(Area2D area, SensorInfo[] sensors)
		{
			if (!sensors.Any(x => x.Contains(area)))
			{
				foreach (var sub in area.Subdivide())
				{
					if (sub.Size == Point2D.Zero)
					{
						if (sensors.Any(x => x.IsInRange(sub.Min)))
						{
							return null;
						}

						return sub.Min;
					}
					else
					{
						var subSensors = sensors.Where(x => sub.Overlaps(new Area2D(x.SensorMin, x.SensorMax))).ToArray();
						if (subSensors.Length > 0)
						{
							var point = FindZeroCoveragePoint(sub, subSensors);
							if (point.HasValue)
							{
								return point;
							}
						}
					}
				}
			}

			return null;
		}

		private IEnumerable<SensorInfo> GetSensors()
		{
			return Source.SplitNewLine()
						 .Select(x => x.Split(new char[] { '=', ',', ':' })
									   .Where((y, i) => (i % 2) == 1)
									   .Select(int.Parse)
									   .Chunk(2)
									   .Select(x => new Point2D(x.First(), x.Skip(1).First())))
						 .Select(
							 x => new SensorInfo(x.First(), x.Skip(1).First())
						 )
						 .ToArray();
		}
	}
}
