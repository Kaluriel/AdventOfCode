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
				["\r\n\r\n", "\n\n"],
				stringSplitOptions
			);
		}

		public static U SplitDoubleNewLine<U>(this string str, Func<string[], U> func, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			var split = str.Split(
				["\r\n\r\n", "\n\n"],
				stringSplitOptions
			);
			return func(split);
		}

		public static string[] SplitNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				["\r\n", "\n"],
				stringSplitOptions
			);
		}

		public static string[] SplitNewLineAndSpaces(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				["\r\n", "\n", " "],
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

		public static string Reverse(this string str)
		{
			return new string(
				str.ToCharArray()
					.Reverse()
					.ToArray()
			);
		}
	}
}
