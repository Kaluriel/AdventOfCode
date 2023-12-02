using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Area2D
	{
		public Point2D Min { get; set; }
		public Point2D Max { get; set; }

		public Area2D(Point2D min, Point2D max)
		{
			Min = min;
			Max = max;
		}

		public IEnumerable<Point2D> GetCorners()
		{
			yield return Min;
			yield return new Point2D(Min.X, Max.Y);
			yield return Max;
			yield return new Point2D(Max.X, Min.Y);
		}

		public Point2D Size => Max - Min;
		public bool IsZero => Size == Point2D.Zero;
		public bool IsOne => Size == Point2D.One;

		public IEnumerable<Area2D> Subdivide()
		{
			Point2D halfSize = Size / 2;
			Point2D mid = Min + halfSize;

			yield return new Area2D(Min, mid);

			if (Size.X > 1)
			{
				yield return new Area2D(
					new Point2D(mid.X, Min.Y),
					new Point2D(Max.X, mid.Y)
				);
			}

			if (Size.Y > 1)
			{
				yield return new Area2D(
					new Point2D(Min.X, mid.Y),
					new Point2D(mid.X, Max.Y)
				);
			}

			if ((Size.X > 1) && (Size.Y > 1))
			{
				yield return new Area2D(
					mid,
					Max
				);
			}
		}

		public bool Overlaps(Area2D area)
		{
			bool xOverlap = ((this.Min.X >= area.Min.X) && (this.Min.X <= area.Max.X)) ||
							((area.Min.X >= this.Min.X) && (area.Min.X <= this.Max.X));

			bool yOverlap = ((this.Min.Y >= area.Min.Y) && (this.Min.Y <= area.Max.Y)) ||
							((area.Min.Y >= this.Min.Y) && (area.Min.Y <= this.Max.Y));

			return xOverlap && yOverlap;
		}
	}
}
