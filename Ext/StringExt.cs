using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Ext
{
	public static class StringExt
	{
		public static string[] SplitDoubleNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				new string[] { "\r\n\r\n", "\n\n" },
				stringSplitOptions
			);
		}

		public static string[] SplitNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				new string[] { "\r\n", "\n" },
				stringSplitOptions
			);
		}

		public static string Replace(this string str, KeyValuePair<string, string>[] valuePairs)
		{
			StringBuilder strBuilder = new StringBuilder(str);
			
			for (int i = 0; i < valuePairs.Length; ++i)
			{
				strBuilder.Replace(valuePairs[i].Key, valuePairs[i].Value);
			}

			return strBuilder.ToString();
		}
	}
}
