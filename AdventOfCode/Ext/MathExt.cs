using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Ext
{
	public static class MathExt
	{
		public static Int64 LowestCommonMultiple(this IEnumerable<Int64> values)
		{
			return values.Aggregate(LowestCommonMultiple);
		}
		
		public static Int64 LowestCommonMultiple(Int64 a, Int64 b)
		{
			return Math.Abs(a * b) / GreatestCommonDivider(a, b);
		}
		
		public static Int64 GreatestCommonDivider(Int64 a, Int64 b)
		{
			return (b == 0)
				? a
				: GreatestCommonDivider(b, a % b);
		}
		
		public static bool IsInsidePolygon(Point2D test, Point2D[] points)
		{
			bool result = false;

			for (int i = 0, j = points.Length - 1; i < points.Length; j = i++)
			{
				if ((((points[i].Y <= test.Y) && (test.Y < points[j].Y)) ||
				     ((points[j].Y <= test.Y) && (test.Y < points[i].Y))) &&
				    (test.X < (points[j].X - points[i].X) * (test.Y - points[i].Y) / (points[j].Y - points[i].Y) + points[i].X))
				{
					result = !result;
				}
			}

			return result;
		}
	}
}
