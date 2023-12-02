using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day08 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var sequences = Source.SplitNewLine()
                                  .Select(
                                      x =>
                                      {
                                          var split = x.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                                          return new
                                          {
                                              Left = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries),
                                              Right = split[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                          };
                                      }
                                  )
                                  .ToArray();

            int appearanceCount = 0;

            for (int s = 0; s < sequences.Length; ++s)
            {
                appearanceCount += sequences[s].Right.Count(x => (x.Length == 2) || (x.Length == 3) || (x.Length == 4) || (x.Length == 7));
            }

            return Task.FromResult<object>(
                appearanceCount
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var sequences = Source.SplitNewLine()
                .Select(
                    x =>
                    {
                        var split = x.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
                        return new
                        {
                            Left = split[0].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => OrderStr(x)),
                            Right = split[1].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(x => OrderStr(x))
                        };
                    }
                );

            int total = 0;

            foreach (var seq in sequences)
            {
                var sequenceList = new List<string>(seq.Left);
                var mapping = new Dictionary<string, int>();

                // 1
                string one = sequenceList.Single(x => x.Length == 2);
                sequenceList.Remove(one);
                mapping.Add(one, 1);

                // 7
                string seven = sequenceList.Single(x => x.Length == 3);
                sequenceList.Remove(seven);
                mapping.Add(seven, 7);

                // 4
                string four = sequenceList.Single(x => x.Length == 4);
                sequenceList.Remove(four);
                mapping.Add(four, 4);

                // 8
                string eight = sequenceList.Single(x => x.Length == 7);
                sequenceList.Remove(eight);
                mapping.Add(eight, 8);


                // 3
                string three = sequenceList.Single(x => (x.Length == 5) && one.All(x.Contains));
                sequenceList.Remove(three);
                mapping.Add(three, 3);

                // 9
                string nine = sequenceList.Single(x => (x.Length == 6) && three.All(x.Contains));
                sequenceList.Remove(nine);
                mapping.Add(nine, 9);

                // 0
                string zero = sequenceList.Single(x => (x.Length == 6) && seven.All(x.Contains));
                sequenceList.Remove(zero);
                mapping.Add(zero, 0);

                // 6
                string six = sequenceList.Single(x => x.Length == 6);
                sequenceList.Remove(six);
                mapping.Add(six, 6);

                // 5
                string five = sequenceList.Single(x => (x.Length == 5) && (x.Count(six.Contains) == (six.Length - 1)));
                sequenceList.Remove(five);
                mapping.Add(five, 5);

                // 2
                string two = sequenceList.Single(x => (x.Length == 5));
                sequenceList.Remove(two);
                mapping.Add(two, 2);

                string segNumber = string.Empty;

                foreach(var val in seq.Right)
                {
                    segNumber += mapping[val];
                }

                //Console.WriteLine($"{val}");
                total += int.Parse(segNumber);
            }

            return Task.FromResult<object>(
                total
            );
        }

        private static string OrderStr(string str)
        {
            return new string(str.OrderBy(x => x).ToArray());
        }
    }
}
