using System;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.DataTypes
{
	public struct Line1D
	{
		public int Start { get; set; }
		public int End { get; set; }

		public Line1D(int start, int end)
		{
			Start = start;
			End = end;
		}

		public bool Contains(int val)
		{
			return (val >= this.Start) && (val <= this.End);
		}

		public bool Contains(Line1D line)
		{
			return (this.Start >= line.Start) && (this.End <= line.End);
		}

		public bool Overlaps(Line1D line)
		{
			return (this.Start <= line.End) && (line.Start <= this.End);
		}
	}
}
