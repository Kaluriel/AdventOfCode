using System;
using System.Diagnostics;

namespace AdventOfCode.Utils;

public static class Logging
{
    [Conditional("TRACE")]
    public static void Log(string text)
    {
        System.Diagnostics.Debug.WriteLine(text);
        Console.WriteLine(text);
    }
}
