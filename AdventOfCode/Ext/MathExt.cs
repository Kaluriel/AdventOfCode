using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
	}
}
