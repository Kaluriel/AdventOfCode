using System;
using System.Diagnostics;
using System.Text;

namespace AdventOfCode.Utils;

public static class Logging
{
    [Conditional("TRACE")]
    public static void Log(string text)
    {
        System.Diagnostics.Debug.WriteLine(text);
        Console.WriteLine(text);
    }
    
    public static void LogMap(char[][] map)
    {
        StringBuilder sb = new StringBuilder();

        for (int y = 0; y < map.Length; ++y)
        {
            string line = "";
				
            for (int x = 0; x < map[y].Length; ++x)
            {
                sb.Append(map[y][x]);
            }

            sb.AppendLine();
        }
			
        Logging.Log(sb.ToString());
    }
}
