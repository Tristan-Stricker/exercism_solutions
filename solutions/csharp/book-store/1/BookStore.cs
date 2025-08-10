using System;
using System.Collections.Generic;
using System.Linq;

public static class BookStore
{
    public static decimal Total(IEnumerable<int> books)
    {
        var bookCount = books.Count();
        var distinctBookCount = books.Distinct().Count();

        if (!books.Any()){
            return 0m;
        }

        if(bookCount > 0 && bookCount <= 5)
        {
            var discount = GetDiscount(distinctBookCount);
            var leftOvers = (bookCount - distinctBookCount) * 8m;
            return (distinctBookCount * 8m) - (discount * (distinctBookCount * 8)) + leftOvers;
        }

        var groupCount = books.GroupBy(x => x).Count();
        if(groupCount< 1 || groupCount > 5)
        {
            throw new Exception("");
        }

        var prices = new List<decimal>();

        foreach(var count in Enumerable.Range(1, groupCount).Reverse())
        {
            var numberOfGroups = bookCount / count;
            var remainder = bookCount % (numberOfGroups * count);

            var discount = GetDiscount(count);
            var discountForRemainder = GetDiscount(remainder);

            var withOutDiscount = numberOfGroups * 8m * count;
            var withDiscount = withOutDiscount - (withOutDiscount * discount);

            var remainderPrice = remainder * 8m;
            var remainderWithDiscount = remainderPrice - (remainderPrice * discountForRemainder);

            prices.Add(withDiscount + remainderWithDiscount);
        }


        return prices.Min();
    }

    private static decimal GetDiscount(int groupCount)
    {
        var discount = groupCount switch
        {
            2 => 0.05m,
            3 => 0.10m,
            4 => 0.20m,
            5 => 0.25m,
            _ => 0,  // default
        };
        return discount;
    }
}
