using System;
using System.Collections.Generic;
using System.Linq;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Ext
{
	public static class ListExt
	{
		public static bool IsAdjacent(this List<Point2D> list, Point2D point)
		{
			return list.Any(x => x.IsAdjacent(point));
		}
	}
}
