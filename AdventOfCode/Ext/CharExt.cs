using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Ext
{
	public static class CharExt
	{
		public static int AsInt(this char val)
		{
			return val - '0';
		}
		
		public static ulong AsULong(this char val)
		{
			return (ulong)(val - '0');
		}
	}
}
