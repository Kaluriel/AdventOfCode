using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using AdventOfCode.DataTypes;
using AdventOfCode.Ext;

namespace AdventOfCode.Days.Y2023
{
	public sealed class Day07 : Day
	{
		class Hand
		{
			public char[] Cards { get; set; }
			public int Bid { get; set; }
		}

		enum EType
		{
			HighCard,
			OnePair,
			TwoPair,
			ThreeOfAKind,
			FullHouse,
			FourOfAKind,
			FiveOfAKind,
		}

		private KeyValuePair<EType, int[]>[] TypeRules = new KeyValuePair<EType, int[]>[]
		{
			new KeyValuePair<EType, int[]>(EType.HighCard, new []{ 1, 1, 1, 1, 1 }),
			new KeyValuePair<EType, int[]>(EType.OnePair, new []{ 2, 1, 1, 1 }),
			new KeyValuePair<EType, int[]>(EType.TwoPair, new []{ 2, 2, 1 }),
			new KeyValuePair<EType, int[]>(EType.ThreeOfAKind, new []{ 3, 1, 1 }),
			new KeyValuePair<EType, int[]>(EType.FullHouse, new []{ 3, 2 }),
			new KeyValuePair<EType, int[]>(EType.FourOfAKind, new []{ 4, 1 }),
			new KeyValuePair<EType, int[]>(EType.FiveOfAKind, new []{ 5 }),
		};

		private List<char> CardStrength = new List<char>()
		{
			'2', '3', '4', '5', '6', '7', '8', '9', 'T', 'J', 'Q', 'K', 'A',
		};
		
		private List<char> CardStrengthP2 = new List<char>()
		{
			'J', '2', '3', '4', '5', '6', '7', '8', '9', 'T', 'Q', 'K', 'A',
		};

		private Hand[] Hands;

		protected override Task ExecuteSharedAsync()
		{
			Hands = Source.SplitNewLine()
						  .Select(x => x.Split(new[]{' '}))
						  .Select(
							  x => new Hand()
							  {
								  Cards = x.First()
										   .ToArray(),
								  Bid = x.Skip(1)
										 .Select(int.Parse)
										 .First(),
							  }
						  )
						  .ToArray();
			return base.ExecuteSharedAsync();
		}

		protected override Task<object> ExecutePart1Async(int testIndex)
		{
			return Task.FromResult<object>(
				Hands.OrderBy(hand => DetermineHandType(hand.Cards))
					 .ThenBy(hand => CalcHandValue(hand.Cards, CardStrength))
					 .Select((hand, i) => (i + 1) * hand.Bid)
					 .Sum()
			);
		}

		protected override Task<object> ExecutePart2Async(int testIndex)
		{
			return Task.FromResult<object>(
				Hands.OrderBy(hand => DetermineHandTypeWithWildcards(hand.Cards))
					 .ThenBy(hand => CalcHandValue(hand.Cards, CardStrengthP2))
					 .Select((hand, i) => (i + 1) * hand.Bid)
					 .Sum()
			);
		}

		private EType DetermineHandType(IEnumerable<char> cards)
		{
			var groupedCards = cards.GroupBy(card => card)
													  .OrderByDescending(card => card.Count())
													  .ToArray();
			EType ret = EType.HighCard;

			for (int i = 0; i < TypeRules.Length; ++i)
			{
				if (TypeRules[i].Value.Length != groupedCards.Length)
				{
					continue;
				}

				bool allMatch = true;
				
				for (int g = 0; g < groupedCards.Length; ++g)
				{
					if (TypeRules[i].Value[g] != groupedCards[g].Count())
					{
						allMatch = false;
						break;
					}
				}

				if (allMatch)
				{
					ret = TypeRules[i].Key;
					break;
				}
			}

			return ret;
		}

		private EType DetermineHandTypeWithWildcards(char[] handCards)
		{
			EType ret = DetermineHandType(handCards);

			// contains a joker, but not all jokers
			if (handCards.Any(card => card == 'J') && handCards.Any(card => card != 'J'))
			{
				EType newHighestType = handCards.Where(card => card != 'J')
											    .Select(joker => handCards.Select(card => card == 'J' ? joker : card))
								   			    .Select(DetermineHandType)
								   			    .OrderByDescending(highestType => highestType)
								   			    .First();
				if (newHighestType > ret)
				{
					ret = newHighestType;
				}
			}

			return ret;
		}

		private int CalcHandValue(char[] cards, IList<char> cardStrengths)
		{
			return cards.Select((card, i) => new { Card = card, Index = i })
						.Sum(card => cardStrengths.IndexOf(card.Card) << (26 - (card.Index * 4)));
		}
	}
}
