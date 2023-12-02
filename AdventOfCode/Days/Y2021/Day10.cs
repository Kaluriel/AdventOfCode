using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day10 : DayBase2021
    {
        private static IReadOnlyDictionary<char, int> SyntaxErrorScore { get; } = new Dictionary<char, int>()
        {
            {')', 3 },
            {']', 57 },
            {'}', 1197 },
            {'>', 25137 },
        };

        private static IReadOnlyDictionary<char, int> AutoCompleteScore { get; } = new Dictionary<char, int>()
        {
            {'(', 1 },
            {'[', 2 },
            {'{', 3 },
            {'<', 4 },
        };

        private static IReadOnlyDictionary<char, char> ClosingPair { get; } = new Dictionary<char, char>()
        {
            {'(', ')' },
            {'[', ']' },
            {'{', '}' },
            {'<', '>' },
        };

        protected override Task<object> ExecutePart1Async()
        {
            var score = Source.SplitNewLine()
                              .Select(GetSyntaxErrorScore)
                              .Where(x => x != 0)
                              .Sum();

            return Task.FromResult<object>(
                score
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var scores = Source.SplitNewLine()
                               .Where(IsIncomplete)
                               .Select(GetMissingClosingCharacters)
                               .Select(x => x.Aggregate((long)0, (x, y) => (x * 5) + AutoCompleteScore[y]))
                               .OrderBy(x => x)
                               .ToArray();

            long middle = scores[scores.Length / 2];

            return Task.FromResult<object>(
                middle
            );
        }

        static bool IsIncomplete(string str)
        {
            return GetSyntaxErrorScore(str) == 0;
        }

        static Stack<char> GetMissingClosingCharacters(string str)
        {
            var queue = new Stack<char>();

            for (int i = 0; i < str.Length; ++i)
            {
                if (IsOpening(str[i]))
                {
                    queue.Push(str[i]);
                }
                else if (IsClosing(str[i]))
                {
                    queue.Pop();
                }
            }

            return queue;
        }

        static int GetSyntaxErrorScore(string str)
        {
            var queue = new Stack<char>();

            //Console.WriteLine(str);
            for (int i = 0; i < str.Length; ++i)
            {
                if (IsOpening(str[i]))
                {
                    queue.Push(str[i]);
                }
                else if (IsClosing(str[i]))
                {
                    var openingChar = queue.Pop();
                    var closingChar = ClosingPair[openingChar];

                    if (closingChar != str[i])
                    {
                        //Console.WriteLine(str[i]);
                        return SyntaxErrorScore[str[i]];
                    }
                }
            }

            return 0;
        }

        static bool IsOpening(char c)
        {
            return "[(<{".Contains(c);
        }

        static bool IsClosing(char c)
        {
            return "])>}".Contains(c);
        }
    }
}
