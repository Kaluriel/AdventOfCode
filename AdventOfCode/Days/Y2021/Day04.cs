using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day04 : DayBase2021
    {
        public struct BingoNumber
        {
            public int Number { get; set; }
            public bool Marked { get; set; }
        }

        public class BingoCard
        {
            public BingoNumber[][] Grid { get; } = new BingoNumber[][]
            {
                new BingoNumber[5],
                new BingoNumber[5],
                new BingoNumber[5],
                new BingoNumber[5],
                new BingoNumber[5],
            };

            public void MarkNumber(int num)
            {
                for (int y = 0; y < 5; ++y)
                {
                    for (int x = 0; x < 5; ++x)
                    {
                        if (Grid[x][y].Number == num)
                        {
                            Grid[x][y].Marked = true;
                        }
                    }
                }
            }

            public bool HasBingo()
            {
                for (int y = 0; y < 5; ++y)
                {
                    bool allMarked = true;

                    for (int x = 0; x < 5; ++x)
                    {
                        if (!Grid[x][y].Marked)
                        {
                            allMarked = false;
                            break;
                        }
                    }

                    if (allMarked)
                    {
                        return true;
                    }
                }

                for (int x = 0; x < 5; ++x)
                {
                    bool allMarked = true;

                    for (int y = 0; y < 5; ++y)
                    {
                        if (!Grid[x][y].Marked)
                        {
                            allMarked = false;
                            break;
                        }
                    }

                    if (allMarked)
                    {
                        return true;
                    }
                }

                return false;
            }

            public int GetSumUnmarkedNumbers()
            {
                int ret = 0;

                for (int y = 0; y < 5; ++y)
                {
                    for (int x = 0; x < 5; ++x)
                    {
                        if (!Grid[x][y].Marked)
                        {
                            ret += Grid[x][y].Number;
                        }
                    }
                }

                return ret;
            }
        }

        protected override Task<object> ExecutePart1Async(int testIndex)
        {
            var rows = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .ToArray();

            var numbers = rows[0].Split(new char[] { ',' }).Select(int.Parse).ToArray();
            var cards = new List<BingoCard>();

            for (int i = 1; i < rows.Length; i += 5)
            {
                var card = new BingoCard();

                for (int y = 0; y < 5; ++y)
                {
                    var cardRow = rows[i + y].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                                             .Select(int.Parse)
                                             .ToArray();

                    for (int x = 0; x < cardRow.Length; ++x)
                    {
                        card.Grid[x][y].Number = cardRow[x];
                    }
                }

                cards.Add(card);
            }

            int sumUnmarkedNumbers = -1;
            int lastNumberCalled = -1;

            for (int i = 0; i < numbers.Length; ++i)
            {
                lastNumberCalled = numbers[i];

                for (int c = 0; c < cards.Count; ++c)
                {
                    cards[c].MarkNumber(lastNumberCalled);
                }

                var bingoCard = cards.FirstOrDefault(c => c.HasBingo());
                if (bingoCard != null)
                {
                    sumUnmarkedNumbers = bingoCard.GetSumUnmarkedNumbers();
                    break;
                }
            }

            return Task.FromResult<object>(
                sumUnmarkedNumbers * lastNumberCalled
            );
        }

        protected override Task<object> ExecutePart2Async(int testIndex)
        {
            var rows = Source.SplitNewLine(StringSplitOptions.RemoveEmptyEntries)
                             .ToArray();

            var numbers = rows[0].Split(new char[] { ',' }).Select(int.Parse).ToArray();
            var cards = new List<BingoCard>();

            for (int i = 1; i < rows.Length; i += 5)
            {
                var card = new BingoCard();

                for (int y = 0; y < 5; ++y)
                {
                    var cardRow = rows[i + y].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();

                    for (int x = 0; x < cardRow.Length; ++x)
                    {
                        card.Grid[x][y].Number = cardRow[x];
                    }
                }

                cards.Add(card);
            }

            int sumUnmarkedNumbers = -1;
            int lastNumberCalled = -1;

            for (int i = 0; i < numbers.Length; ++i)
            {
                for (int c = 0; c < cards.Count; ++c)
                {
                    cards[c].MarkNumber(numbers[i]);
                }

                BingoCard bingoCard = null;

                do
                {
                    bingoCard = cards.FirstOrDefault(c => c.HasBingo());
                    if (bingoCard != null)
                    {
                        sumUnmarkedNumbers = bingoCard.GetSumUnmarkedNumbers();
                        lastNumberCalled = numbers[i];

                        cards.Remove(bingoCard);
                    }
                }
                while (bingoCard != null);
            }

            return Task.FromResult<object>(
                sumUnmarkedNumbers * lastNumberCalled
            );
        }
    }
}
