using AdventOfCode.Ext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.Days.Y2021
{
    public class Day18 : DayBase2021
    {
        static (SnailfishMathPair, long)[] day18_mag_test = new[]
        {
            (new SnailfishMathPair("[[1,2],[[3,4],5]]"), 143L),
            (new SnailfishMathPair("[[[[0,7],4],[[7, 8],[6, 0]]],[8, 1]]"), 1384),
            (new SnailfishMathPair("[[[[1, 1],[2,2]],[3,3]],[4,4]]"), 445),
            (new SnailfishMathPair("[[[[3, 0],[5,3]],[4,4]],[5,5]]"), 791),
            (new SnailfishMathPair("[[[[5, 0],[7,4]],[5,5]],[6,6]]"), 1137),
            (new SnailfishMathPair("[[[[8, 7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"), 3488),
            (new SnailfishMathPair("[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]"), 4140),
        };

        static (SnailfishMathPair, string)[] day18_reduce_test = new[]
        {
            (
                new SnailfishMathPair("[[[[[9,8],1],2],3],4]", false),
                "[[[[0,9],2],3],4]"
            ),
            (
                new SnailfishMathPair("[7,[6,[5,[4,[3,2]]]]]", false),
                "[7,[6,[5,[7,0]]]]"
            ),
            (
                new SnailfishMathPair("[[6,[5,[4,[3,2]]]],1]", false),
                "[[6,[5,[7,0]]],3]"
            ),
            (
                new SnailfishMathPair("[[3,[2,[1,[7,3]]]],[6,[5,[4,[3,2]]]]]", false),
                "[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]"
            ),
            (
                new SnailfishMathPair("[[3,[2,[8,0]]],[9,[5,[4,[3,2]]]]]", false),
                "[[3,[2,[8,0]]],[9,[5,[7,0]]]]"
            ),
        };

        static (SnailfishMathPair[], string)[] day18_sum_test = new[]
        {
            (//0
                new []
                {
                    new SnailfishMathPair("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"),
                    new SnailfishMathPair("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"),
                },
                "[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"
            ),
            (//1
                new []
                {
                    new SnailfishMathPair("[[[[4,0],[5,4]],[[7,7],[6,0]]],[[8,[7,7]],[[7,9],[5,0]]]]"),
                    new SnailfishMathPair("[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]"),
                },
                "[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]"
            ),
            (//2
                new []
                {
                    new SnailfishMathPair("[[[[6,7],[6,7]],[[7,7],[0,7]]],[[[8,7],[7,7]],[[8,8],[8,0]]]]"),
                    new SnailfishMathPair("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"),
                },
                "[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]"
            ),
            (//3
                new []
                {
                    new SnailfishMathPair("[[[[7,0],[7,7]],[[7,7],[7,8]]],[[[7,7],[8,8]],[[7,7],[8,7]]]]"),
                    new SnailfishMathPair("[7,[5,[[3,8],[1,4]]]]"),
                },
                "[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[[7,7],[7,8]],[[9,5],[8,7]]],[[[6,8],[0,8]],[[9,9],[9,0]]]]"),
                    new SnailfishMathPair("[[2,[2,2]],[8,[8,1]]]"),
                },
                "[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[[6,6],[6,6]],[[6,0],[6,7]]],[[[7,7],[8,9]],[8,[8,1]]]]"),
                    new SnailfishMathPair("[2,9]"),
                },
                "[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]"
            ),
            /*(
                new []
                {
                    new SnailfishMathPair("[[[[6,6],[7,7]],[[0,7],[7,7]]],[[[5,5],[5,6]],9]]"),
                    new SnailfishMathPair("[1,[[[9,3],9],[[9,0],[0,7]]]]"),
                },
                "[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[[7,8],[6,7]],[[6,8],[0,8]]],[[[7,7],[5,0]],[[5,5],[5,6]]]]"),
                    new SnailfishMathPair("[[[5,[7,4]],7],1]"),
                },
                "[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[[7,7],[7,7]],[[8,7],[8,7]]],[[[7,0],[7,7]],9]]"),
                    new SnailfishMathPair("[[[[4,2],2],6],[8,7]]"),
                },
                "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"
            ),*/

            (
                new []
                {
                    new SnailfishMathPair("[[[[4,3],4],4],[7,[[8,4],9]]]"),
                    new SnailfishMathPair("[1,1]"),
                },
                "[[[[0,7],4],[[7,8],[6,0]]],[8,1]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[1,1]"),
                    new SnailfishMathPair("[2,2]"),
                    new SnailfishMathPair("[3,3]"),
                    new SnailfishMathPair("[4,4]"),
                },
                "[[[[1,1],[2,2]],[3,3]],[4,4]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[1,1]"),
                    new SnailfishMathPair("[2,2]"),
                    new SnailfishMathPair("[3,3]"),
                    new SnailfishMathPair("[4,4]"),
                    new SnailfishMathPair("[5,5]"),
                },
                "[[[[3,0],[5,3]],[4,4]],[5,5]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[1,1]"),
                    new SnailfishMathPair("[2,2]"),
                    new SnailfishMathPair("[3,3]"),
                    new SnailfishMathPair("[4,4]"),
                    new SnailfishMathPair("[5,5]"),
                    new SnailfishMathPair("[6,6]"),
                },
                "[[[[5,0],[7,4]],[5,5]],[6,6]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]"),
                    new SnailfishMathPair("[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]"),
                    new SnailfishMathPair("[[2,[[0,8],[3,4]]],[[[6, 7],1],[7,[1,6]]]]"),
                    new SnailfishMathPair("[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]"),
                    new SnailfishMathPair("[7,[5,[[3,8],[1,4]]]]"),
                    new SnailfishMathPair("[[2,[2,2]],[8,[8,1]]]"),
                    new SnailfishMathPair("[2,9]"),
                    new SnailfishMathPair("[1,[[[9,3],9],[[9,0],[0,7]]]]"),
                    new SnailfishMathPair("[[[5,[7,4]],7],1]"),
                    new SnailfishMathPair("[[[[4,2],2],6],[8,7]]"),
                },
                "[[[[8,7],[7,7]],[[8,6],[7,7]]],[[[0,7],[6,6]],[8,7]]]"
            ),
            (
                new []
                {
                    new SnailfishMathPair("[[[0,[5,8]],[[1,7],[9,6]]],[[4,[1,2]],[[1,4],2]]]"),
                    new SnailfishMathPair("[[[5,[2,8]],4],[5,[[9,9],0]]]"),
                    new SnailfishMathPair("[6,[[[6, 2],[5, 6]],[[7,6],[4,7]]]]"),
                    new SnailfishMathPair("[[[6,[0,7]],[0,9]],[4,[9,[9,0]]]]"),
                    new SnailfishMathPair("[[[7,[6,4]],[3,[1,3]]],[[[5,5],1],9]]"),
                    new SnailfishMathPair("[[6,[[7,3],[3,2]]],[[[3,8],[5,7]],4]]"),
                    new SnailfishMathPair("[[[[5,4],[7,7]],8],[[8,3],8]]"),
                    new SnailfishMathPair("[[9,3],[[9,9],[6,[4,9]]]]"),
                    new SnailfishMathPair("[[2,[[7,7],7]],[[5,8],[[9,3],[0,2]]]]"),
                    new SnailfishMathPair("[[[[5,2],5],[8,[3,7]]],[[5,[7,5]],[4,4]]]"),
                },
                "[[[[6,6],[7,6]],[[7,7],[7,0]]],[[[7,7],[7,7]],[[7,8],[9,9]]]]"
            ),
        };

        class SnailfishMathPair
        {
            public enum ReductionType
            {
                none,
                explode,
                split,
            }

            public SnailfishMathPair? Parent;
            public bool IsLeftChild => Parent?.LeftPair == this;
            public bool IsRightChild => Parent?.RightPair == this;

            public SnailfishMathPair? LeftPair;
            public SnailfishMathPair? RightPair;
            public int? Left;
            public int? Right;

            public SnailfishMathPair(string str, bool reduceAll = true)
                : this(null, str, reduceAll)
            {
            }

            public SnailfishMathPair(SnailfishMathPair? parent, string str, bool reduceAll = false)
                : this(parent, str, 0, str.Length, reduceAll)
            {
            }

            private SnailfishMathPair(SnailfishMathPair? parent, string str, int start, int end, bool reduceAll)
            {
                Parent = parent;

                int pairSeparator = FindPairDividerIndex(str, start, end);
                int pairEnd = FindPairEndIndex(str, pairSeparator + 1, end);

                int leftStrStart = start + 1;
                int leftStrEnd = pairSeparator;
                int rightStrStart = pairSeparator + 1;
                int rightStrEnd = pairEnd;

                // left pair / regular
                if (str[leftStrStart] == '[')
                {
                    LeftPair = new SnailfishMathPair(this, str, leftStrStart, leftStrEnd, false);
                }
                else
                {
                    string leftStr = str[leftStrStart..leftStrEnd].Trim();
                    Left = int.Parse(leftStr);
                }

                // right pair / regular
                if (str[rightStrStart] == '[')
                {
                    RightPair = new SnailfishMathPair(this, str, rightStrStart, rightStrEnd, false);
                }
                else
                {
                    string rightStr = str[rightStrStart..rightStrEnd].Trim();
                    Right = int.Parse(rightStr);
                }

                if (reduceAll)
                {
                    while (Reduce() != ReductionType.none) ;
                }
            }

            private static int FindPairDividerIndex(string str, int start, int end)
            {
                int openCount = 0;

                for (int i = start; i < end; ++i)
                {
                    if (str[i] == '[')
                    {
                        ++openCount;
                    }
                    else if (str[i] == ']')
                    {
                        --openCount;
                    }

                    if ((openCount == 1) && (str[i] == ','))
                    {
                        return i;
                    }
                }

                return -1;
            }

            private static int FindPairEndIndex(string str, int start, int end)
            {
                int openCount = 0;

                for (int i = start; i < end; ++i)
                {
                    if (str[i] == '[')
                    {
                        ++openCount;
                    }
                    else if (str[i] == ']')
                    {
                        if (openCount == 0)
                        {
                            return i;
                        }

                        --openCount;
                    }
                }

                return -1;
            }

            public ReductionType Reduce()
            {
                if (TryExplode(0))
                {
                    return ReductionType.explode;
                }

                if (TrySplit())
                {
                    return ReductionType.split;
                }

                return ReductionType.none;
            }

            public bool TryExplode(int depth)
            {
                if (depth != 4)
                {
                    return (LeftPair?.TryExplode(depth + 1) ?? false) ||
                           (RightPair?.TryExplode(depth + 1) ?? false);
                }

                if ((Left == null) || (Right == null))
                {
                    throw new NotSupportedException();
                }

                (SnailfishMathPair, int) leftNode = GetRegularToLeft();
                (SnailfishMathPair, int) rightNode = GetRegularToRight();

                if (leftNode.Item1 != null)
                {
                    if (leftNode.Item2 == 0)
                    {
                        leftNode.Item1.Left += Left;
                    }
                    else
                    {
                        leftNode.Item1.Right += Left;
                    }
                }

                if (rightNode.Item1 != null)
                {
                    if (rightNode.Item2 == 0)
                    {
                        rightNode.Item1.Left += Right;
                    }
                    else
                    {
                        rightNode.Item1.Right += Right;
                    }
                }

                if (IsLeftChild)
                {
                    this.Parent.Left = 0;
                    this.Parent.LeftPair = null;
                }
                else if (IsRightChild)
                {
                    this.Parent.Right = 0;
                    this.Parent.RightPair = null;
                }
                else
                {
                    throw new NotSupportedException();
                }

                return true;
            }

            public bool TrySplit()
            {
                if (Left.HasValue && (Left >= 10))
                {
                    LeftPair = Split(this, Left.Value);
                    Left = null;
                    return true;
                }
                else if (LeftPair?.TrySplit() ?? false)
                {
                    return true;
                }

                if (Right.HasValue && (Right >= 10))
                {
                    RightPair = Split(this, Right.Value);
                    Right = null;
                    return true;
                }
                else if (RightPair?.TrySplit() ?? false)
                {
                    return true;
                }

                return false;
            }

            private static SnailfishMathPair Split(SnailfishMathPair parent, long value)
            {
                long left = value / 2;
                long right = (value + 1) / 2;
                return new SnailfishMathPair(parent, $"[{left},{right}]");
            }

            private (SnailfishMathPair, int) GetRegularToLeft()
            {
                SnailfishMathPair node = this;

                // go up the hierarchy to find where we can branch to the left
                while (node != null)
                {
                    if (node.IsRightChild)
                    {
                        node = node.Parent;
                        break;
                    }

                    node = node.Parent;
                }

                // If the found node has a left regular, then return that
                if (node?.Left.HasValue ?? false)
                {
                    return (node, 0);
                }

                // Otherwise go down the left hierarchy to find a leaf with a right regular
                node = node?.LeftPair;

                while ((node != null) && !node.Right.HasValue)
                {
                    node = node.RightPair;
                }

                // If we found a right regular, return it
                if (node != null)
                {
                    return (node, 1);
                }

                // Otherwise we didn't find a regular
                return (null, -1);
            }

            private (SnailfishMathPair, int) GetRegularToRight()
            {
                SnailfishMathPair node = this;

                // go up the hierarchy to find where we can branch to the right
                while (node != null)
                {
                    if (node.IsLeftChild)
                    {
                        node = node.Parent;
                        break;
                    }

                    node = node.Parent;
                }

                // If the found node has a right regular, then return that
                if (node?.Right.HasValue ?? false)
                {
                    return (node, 1);
                }

                // Otherwise go down the right hierarchy to find a leaf with a left regular
                node = node?.RightPair;

                while ((node != null) && !node.Left.HasValue)
                {
                    node = node.LeftPair;
                }

                // If we found a left regular, return it
                if (node != null)
                {
                    return (node, 0);
                }

                // Otherwise we didn't find a regular
                return (null, -1);
            }

            public long Magnitude
            {
                get
                {
                    long left = 3 * (LeftPair?.Magnitude ?? Left.Value);
                    long right = 2 * (RightPair?.Magnitude ?? Right.Value);
                    return left + right;
                }
            }

            public override string ToString()
            {
                string left = LeftPair?.ToString() ?? Left.Value.ToString();
                string right = RightPair?.ToString() ?? Right.Value.ToString();

                return $"[{left},{right}]";
            }
        }

        protected override Task<object> ExecutePart1Async()
        {
            for (int i = 0; i < day18_reduce_test.Length; ++i)
            {
                day18_reduce_test[i].Item1.Reduce();

                System.Diagnostics.Debug.Assert(day18_reduce_test[i].Item1.ToString() == day18_reduce_test[i].Item2);
            }

            for (int i = 0; i < day18_sum_test.Length; ++i)
            {
                var initial = day18_sum_test[i].Item1[0];
                var sumFinal = initial;

                for (int s = 1; s < day18_sum_test[i].Item1.Length; ++s)
                {
                    sumFinal = new SnailfishMathPair($"[{sumFinal.ToString()},{day18_sum_test[i].Item1[s].ToString()}]");
                }

                System.Diagnostics.Debug.Assert(sumFinal.ToString() == day18_sum_test[i].Item2);
            }

            for (int i = 0; i < day18_mag_test.Length; ++i)
            {
                System.Diagnostics.Debug.Assert(day18_mag_test[i].Item1.Magnitude == day18_mag_test[i].Item2);
            }

            //
            var parts = Source.SplitNewLine()
                              .Select(x => new SnailfishMathPair(x))
                              .ToArray();
            var sum = parts[0];

            for (int s = 1; s < parts.Length; ++s)
            {
                sum = new SnailfishMathPair($"[{sum.ToString()},{parts[s].ToString()}]");
            }
             
            return Task.FromResult<object>(
                sum.Magnitude
            );
        }

        protected override Task<object> ExecutePart2Async()
        {
            var parts = Source.SplitNewLine()
                              .Select(x => new SnailfishMathPair(x))
                              .ToArray();
            long largestMagnitude = 0;

            for (int i = 0; i < parts.Length; ++i)
            {
                for (int j = 0; j < parts.Length; ++j)
                {
                    if (i == j)
                    {
                        continue;
                    }

                    var sumFinal = new SnailfishMathPair($"[{parts[i].ToString()},{parts[j].ToString()}]");
                    largestMagnitude = Math.Max(largestMagnitude, sumFinal.Magnitude);
                }
            }

            //
            return Task.FromResult<object>(
                largestMagnitude
            );
        }
    }
}
