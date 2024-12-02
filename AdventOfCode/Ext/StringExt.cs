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
		
		/*public static IEnumerable<string> SplitDoubleNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.SplitAsEnumerable(
				new string[] { "\r\n\r\n", "\n\n" },
				stringSplitOptions
			);
		}*/

		public static string[] SplitNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				new string[] { "\r\n", "\n" },
				stringSplitOptions
			);
		}

		public static string[] SplitNewLineAndSpaces(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.Split(
				new string[] { "\r\n", "\n", " " },
				stringSplitOptions
			);
		}

		/*public static IEnumerable<string> SplitNewLine(this string str, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			return str.SplitAsEnumerable(
				new string[] { "\r\n", "\n" },
				stringSplitOptions
			);
		}*/

		/*public static IEnumerable<string> SplitAsEnumerable(this string str, IEnumerable<string> separators, StringSplitOptions stringSplitOptions = StringSplitOptions.None)
		{
			int startPosition = 0;
			
			separators = separators.Where(s => !string.IsNullOrEmpty(s));

			for (int i = 0; i < str.Length; i++)
			{
				int remainingLength = str.Length - i;
				
				foreach (string separator in separators)
				{
					int currentSepLength = separator.Length;

					if ((str[i] == separator[0]) && (currentSepLength < remainingLength))
					{
						if ((currentSepLength == 1) || str.AsSpan(i, currentSepLength).SequenceEqual(separator))
						{
							string subStr = str.Substring(startPosition, i - startPosition);

							if ((stringSplitOptions != StringSplitOptions.RemoveEmptyEntries) || !subStr.All(char.IsWhiteSpace))
							{
								yield return subStr;
							}

							startPosition = i + currentSepLength;
							i += currentSepLength - 1;
							break;
						}
					}
				}
			}

			if (startPosition != str.Length)
			{
				string subStr = str.Substring(startPosition, str.Length - startPosition);

				if ((stringSplitOptions != StringSplitOptions.RemoveEmptyEntries) || !subStr.All(char.IsWhiteSpace))
				{
					yield return subStr;
				}
			}
		}*/

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
