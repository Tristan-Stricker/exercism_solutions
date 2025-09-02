public static class Poker
{
    public static IEnumerable<string> BestHands(IEnumerable<string> hands)
    {
        var allHands = hands as string[] ?? hands.ToArray();

        if (allHands.Length == 1)
        {
            yield return allHands.First();
            yield break;
        }

        var categorize = Categorize(allHands).ToList();
        categorize.Sort();

        var grouped = categorize.GroupBy(c => c.Sequence);

        var first = grouped.First();
        foreach (var hand in first)
        {
            yield return hand.Input;
        }
    }

    private enum Category
    {
        StraightFlush,
        AceLowStraightFlush,
        FourOfAKind,
        FullHouse,
        Flush,
        Straight,
        AceLowStraight,
        ThreeOfAKind,
        TwoPair,
        OnePair,
        HighCard
    }

    private enum Card
    {
        Ace,
        King,
        Queen,
        Jack,
        Ten,
        Nine,
        Eight,
        Seven,
        Six,
        Five,
        Four,
        Three,
        Two,
        One
    }

    private static IEnumerable<Hand> Categorize(string[] allHands)
    {
        foreach (var hand in allHands)
        {
            var cards = CreateCards(hand).OrderBy(x => x).ToArray();
            var sequence = string.Join("", cards);

            var groupByKind = cards
                .GroupBy(x => x)
                .OrderByDescending(x => x.Count())
                .ThenBy(x => x.Key)
                .Select(grouping => new CardGroup(grouping.Key, grouping.Count()))
                .ToArray();

            Category category;

            var numberOfGroups = groupByKind.Length;

            var sameSuit = AreAllSameSuit(hand);
        
            if (numberOfGroups == 5 && sameSuit && HasSequentialRank(cards))
            {
                category =  Category.StraightFlush;
            }
            else if (numberOfGroups == 5 && sameSuit && IsAceLowStraight(cards))
            {
                category = Category.AceLowStraightFlush;
            }
            else if (numberOfGroups == 5 && HasSequentialRank(cards))
            {
                category = Category.Straight;
            }
            else if (numberOfGroups == 5 && IsAceLowStraight(cards))
            {
                category = Category.AceLowStraight;
            }
            else if (numberOfGroups == 2 && groupByKind[0].Count == 4)
            {
                category = Category.FourOfAKind;
            }
            else if (numberOfGroups == 2 && groupByKind[0].Count == 3 && groupByKind[1].Count == 2)
            {
                category = Category.FullHouse;
            }
            else if (sameSuit)
            {
                category = Category.Flush;
            }
            else if (numberOfGroups == 3 && groupByKind[0].Count == 3)
            {
                category = Category.ThreeOfAKind;
            }
            else if (numberOfGroups == 3 && groupByKind[0].Count == 2 && groupByKind[1].Count == 2)
            {
                category = Category.TwoPair;
            }
            else if (numberOfGroups == 4)
            {
                category = Category.OnePair;
            }
            else
            {
                category = Category.HighCard;
            }

            yield return new Hand(category, hand, groupByKind,sequence);
        }
    }

    private static bool AreAllSameSuit(string hand)
    {
        var split = hand.Split(' ');
        var suitGroups = split.Select(s => s.Last()).GroupBy(x => x);
        return suitGroups.Count() == 1;
    }

    private static bool IsAceLowStraight(Card[] cards) => 
        cards.First() == Card.Ace && HasSequentialRank(cards[1..]);

    private static bool HasSequentialRank(Card[] cards)
    {
        var hasSequentialRank = true;
        for (int i = 1; i < cards.Length; i++)
        {
            var prev = cards[i - 1];
            var curr = cards[i];

            if (curr - prev == 1) { continue; }

            hasSequentialRank = false;
            break;
        }
        
        return hasSequentialRank;
    }

    private static IEnumerable<Card> CreateCards(string hand)
    {
        var split = hand.Split(' ');

        foreach (var card in split)
        {
            if (card.Length == 3)
            {
                yield return Card.Ten;
            }
            else
            {
                var cardType = card[0];
                yield return cardType switch
                {
                    'A' => Card.Ace,
                    'K' => Card.King,
                    'Q' => Card.Queen,
                    'J' => Card.Jack,
                    // special case for ten
                    '9' => Card.Nine,
                    '8' => Card.Eight,
                    '7' => Card.Seven,
                    '6' => Card.Six,
                    '5' => Card.Five,
                    '4' => Card.Four,
                    '3' => Card.Three,
                    '2' => Card.Two,
                    '1' => Card.One,
                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }
    }

    private record CardGroup(Card Card, int Count) : IComparable<CardGroup>
    {
        public int CompareTo(CardGroup? other)
        {
            if (other == null)
            {
                return -1;
            }

            if (Card != other.Card)
            {
                return Card.CompareTo(other.Card);
            }
            
            if(Count != other.Count)
            {
                return Count.CompareTo(other.Count);
            }

            return 0;
        }
    }
    
    private record Hand : IComparable<Hand>
    {
        public Hand(Category category, string input, CardGroup[] cards, string sequence)
        {
            Category = category;
            Input = input;
            Cards = cards;
            Sequence = sequence;
        }

        private CardGroup[] Cards { get; }
        public string Sequence { get; } // For breaking ties

        public Category Category { get; }
        public string Input { get; }

        public int CompareTo(Hand? other)
        {
            if (other == null)
            {
                return -1;
            }

            if (Category != other.Category)
            {
                return Category.CompareTo(other.Category);
            }

            var equal = 0;

            foreach (var (card,otherCard) in Cards.Zip(other.Cards, (card, otherCard) => (card, otherCard)))
            {
                var camparison = card.CompareTo(otherCard);

                if (camparison != equal)
                {
                    return camparison;
                }
            }
            
            return equal;
        }
    }
}