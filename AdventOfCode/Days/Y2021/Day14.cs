using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day14 : DayBase2021
    {
        protected override Task<object> ExecutePart1Async()
        {
            var data = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .Select((x, i) => new { Index = i, Item = x })
                             .GroupBy(x => x.Index == 0)
                             .Aggregate(
                                 new { PolymerTemplate = string.Empty, PairInsertionRules = new Dictionary<string, string>() },
                                 (x, y) =>
                                 {
                                     if (y.Key)
                                     {
                                         x = new { PolymerTemplate = y.First().Item, PairInsertionRules = x.PairInsertionRules };
                                     }
                                     else
                                     {
                                         foreach (var item in y)
                                         {
                                             var parts = item.Item.Split(new char[] { ' ' });
                                             x.PairInsertionRules.Add(parts[0], parts[2]);
                                         }
                                     }
                             
                                     return x;
                                 }
                             );

            string polymerTemplate = data.PolymerTemplate;

            //Console.WriteLine($"Template:     {polymerTemplate}");

            for (int i = 0; i < 10; ++i)
            {
                for (int c = 0; c < (polymerTemplate.Length - 1); c += 2)
                {
                    var pair = polymerTemplate[c..(c + 2)];
                    var insert = data.PairInsertionRules[pair];
                    polymerTemplate = polymerTemplate.Insert(c + 1, insert);
                }

                //Console.WriteLine($"After step {i + 1}: {polymerTemplate}");
            }

            var elementCounts = polymerTemplate.GroupBy(x => x)
                .Select(g => new KeyValuePair<char, int>(g.Key, g.Count()))
                .OrderBy(x => x.Value);

            /*foreach (var element in elementCounts)
            {
                Console.WriteLine($"{element.Key}: {element.Value}");
            }*/

            //
            return Task.FromResult<object>(
                elementCounts.Last().Value - elementCounts.First().Value
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var data = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .Select(x => x.Split(new char[] { ' ' }))
                             .GroupBy(x => x.Length == 1)
                             .Aggregate(
                                 new { PolymerPairs = new Dictionary<string, long>(), LastElement = 'a', PairInsertionRules = new Dictionary<string, char>() },
                                 (x, y) =>
                                 {
                                     if (y.Key)
                                     {
                                         var polymerTemplate = y.First().First();
                                         x = new
                                         {
                                             PolymerPairs = polymerTemplate.SelectWithNext((yx, yy) => $"{yx}{yy}")
                                                                           .GroupBy(yx => yx)
                                                                           .ToDictionary(
                                                                               yg => yg.Key,
                                                                               yg => (long)yg.Count()
                                                                           ),
                                             LastElement = polymerTemplate.Last(),
                                             PairInsertionRules = x.PairInsertionRules
                                         };
                                     }
                                     else
                                     {
                                         x.PairInsertionRules.AddRange(
                                             y.ToDictionary(
                                                 yg => yg[0],
                                                 yg => yg[2][0]
                                             )
                                         );
                                     }
                             
                                     return x;
                                 }
                             );

            // polymerize those pairs
            var polymerPairCounts = data.PolymerPairs;

            for (int i = 0; i < 40; ++i)
            {
                var newPairCount = new Dictionary<string, long>();

                foreach (var pair in polymerPairCounts)
                {
                    char insert = data.PairInsertionRules[pair.Key];

                    string newPair = $"{pair.Key[0]}{insert}";
                    newPairCount.TryGetValue(newPair, out long count);
                    newPairCount[newPair] = count + pair.Value;

                    newPair = $"{insert}{pair.Key[1]}";
                    newPairCount.TryGetValue(newPair, out count);
                    newPairCount[newPair] = count + pair.Value;
                }

                polymerPairCounts = newPairCount;
            }

            // remove second element
            var dict = new Dictionary<char, long>();

            foreach (var pair in polymerPairCounts)
            {
                char key = pair.Key[0];
                dict.TryGetValue(key, out long count);
                dict[key] = count + pair.Value;
            }

            // make sure to readd the last element
            {
                dict.TryGetValue(data.LastElement, out long count);
                dict[data.LastElement] = count + 1;
            }

            // sort our elements
            var elementCounts = dict.OrderBy(x => x.Value);

            return Task.FromResult<object>(
                elementCounts.Last().Value - elementCounts.First().Value
            );
        }
    }
}
