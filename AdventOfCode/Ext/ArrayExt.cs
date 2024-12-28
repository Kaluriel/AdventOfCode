using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using AdventOfCode.DataTypes;

namespace AdventOfCode.Ext
{
	public static class ArrayExt
	{
		public static int IndexOf<T>(this T[] array, int startIndex, T[] subArray, int subArrayStartIndex, int subArrayLength)
		{
			int ret = -1;

			for (int i = startIndex; i < (array.Length - subArrayLength); ++i)
			{
				if (array.Skip(subArrayStartIndex).Take(subArray.Length).SequenceEqual(subArray))
				{
					ret = i;
					break;
				}
			}

			return ret;
		}

		public static int IndexOf<T>(this T[] array, T[] subArray)
		{
			int ret = -1;

			for (int i = 0; i < (array.Length - subArray.Length); ++i)
			{
				if (array.Skip(i).Take(subArray.Length).SequenceEqual(subArray))
				{
					ret = i;
					break;
				}
			}

			return ret;
		}
	}
}
