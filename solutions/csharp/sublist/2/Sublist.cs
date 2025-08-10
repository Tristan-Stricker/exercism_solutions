using System;
using System.Collections.Generic;
using System.Linq;

public enum SublistType
{
    Equal,
    Unequal,
    Superlist,
    Sublist
}

public static class Sublist
{
    public static SublistType Classify<T>(List<T> list1, List<T> list2)
        where T : IComparable
    {
        var fistIsEmpty = !list1.Any();
        var secondIsEmpty = !list2.Any();

        if (fistIsEmpty && secondIsEmpty)
        {
            return SublistType.Equal;
        }
        else if(fistIsEmpty && !secondIsEmpty)
        {
            return SublistType.Sublist;
        }
        else if (!fistIsEmpty && secondIsEmpty)
        {
            return SublistType.Superlist;
        }
        else if(list2.Count() == list1.Count())
        {
            return CompareEqualSizedLists(list1, list2);
        }
        else if (list2.Count() > list1.Count())
        {
            return CompareLists(list1, list2, SublistType.Sublist);
        }
        else if (list1.Count() > list2.Count())
        {
            return CompareLists(list2, list1, SublistType.Superlist);
        }

        return SublistType.Unequal;
    }

    private static SublistType CompareLists<T>(List<T> list1, List<T> list2, SublistType type) where T : IComparable
    {
        var leftCursor = 0;
        var subRangeLength = list1.Count();
        var rightCursor = leftCursor + subRangeLength;

        while (rightCursor <= list2.Count())
        {
            var subRange = list2.GetRange(leftCursor, subRangeLength);

            if (CompareEqualSizedLists(subRange, list1) == SublistType.Equal)
            {
                return type;
            }

            leftCursor++;
            rightCursor++;
        }

        return SublistType.Unequal;
    }

    private static SublistType CompareEqualSizedLists<T>(List<T> list1, List<T> list2) where T : IComparable
    {
        if(list1.Count() != list2.Count())
        {
            throw new Exception("Not same size");
        }

        for (var i = 0; i < list2.Count(); i++)
        {
            var left = list2[i];
            var right = list1[i];

            if (left.CompareTo(right) != 0)
            {
                return SublistType.Unequal;
            }
        }

        return SublistType.Equal;
    }
}