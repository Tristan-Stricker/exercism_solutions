using System;
using System.Collections.Generic;
using System.Linq;

public enum YachtCategory
{
    Ones = 1,
    Twos = 2,
    Threes = 3,
    Fours = 4,
    Fives = 5,
    Sixes = 6,
    FullHouse = 7,
    FourOfAKind = 8,
    LittleStraight = 9,
    BigStraight = 10,
    Choice = 11,
    Yacht = 12,
}

public static class YachtGame
{
    public static int Score(int[] dice, YachtCategory category)
    {
        IEnumerable<(int number, int count)> goupings = dice.GroupBy(x => x).Select(x => (x.Key, x.Count()));

        return category switch
        {
            var num when num <= YachtCategory.Sixes => ScoreNumbers((int)category, goupings),
            YachtCategory.Yacht                     => ScoreYacht(goupings),
            YachtCategory.FullHouse                 => ScoreFullHouse(category, goupings),
            YachtCategory.FourOfAKind               => ScoreFourOfAKind(goupings),
            YachtCategory.LittleStraight            => ScoreLittleStraight(goupings),
            YachtCategory.BigStraight               => ScoreBigStraight(goupings),
            YachtCategory.Choice                    => dice.Sum(),            
            _ =>                                       0
        };
    }

    private static int ScoreYacht(IEnumerable<(int number, int count)> goupings)
    {
        return goupings.Count() == 1 ? 50 : 0;
    }

    private static int ScoreBigStraight(IEnumerable<(int number, int count)> goupings)
    {
        var isLittleStraight = goupings.Count() == 5 && goupings.Select(x => x.number).OrderBy(x => x).SequenceEqual(new int[] { 2, 3, 4, 5, 6 });
        return isLittleStraight ? 30 : 0;
    }

    private static int ScoreLittleStraight(IEnumerable<(int number, int count)> goupings)
    {
        var isLittleStraight = goupings.Count() == 5 && goupings.Select(x => x.number).OrderBy(x => x).SequenceEqual(new int[] { 1, 2, 3, 4, 5 });
        return isLittleStraight ? 30 : 0;
    }

    private static int ScoreFourOfAKind(IEnumerable<(int number, int count)> goupings)
    {
        var fourOfAKind = goupings.Where(x => x.count >= 4);
        var isFourOfAKind = fourOfAKind.Count() == 1;
        return isFourOfAKind ? fourOfAKind.Single().number * 4 : 0;
    }

    private static int ScoreFullHouse(YachtCategory category, IEnumerable<(int number, int count)> goupings)
    {
        var hasOneGroupOfTwo = goupings.Where(x => x.count == 3).Count() == 1;
        var hasOneGroupOfThree = goupings.Where(x => x.count == 2).Count() == 1;
        var isFullHouse = hasOneGroupOfThree && hasOneGroupOfTwo;

        return isFullHouse ?
            goupings.Aggregate(seed: 0, (total, next) => total + (next.number * next.count)) :
            0;
    }

    private static int ScoreNumbers(int categoryAsNumber, IEnumerable<(int number, int count)> goupings)
    {
        var any = goupings.SingleOrDefault(g => g.number == categoryAsNumber);
        return any != default ? (any.count * categoryAsNumber) : 0;
    }
}

