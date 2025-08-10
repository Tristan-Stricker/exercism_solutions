using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class ParallelLetterFrequency
{
    public static Dictionary<char, int> Calculate(IEnumerable<string> texts)
    {
        if (!texts.Any())
        {
            return new Dictionary<char, int>();
        }

        var tasks = texts.Select(text => CalculateAsync(text)).ToArray();

        var result = new Dictionary<char, int>();

        var superTask = Task.WhenAll(tasks).ContinueWith(r => {

            result = DoTheThing(r.Result);
        });

        superTask.Wait();

        return result;
    }

    private static Dictionary<char, int> DoTheThing(IEnumerable<KeyValuePair<char, int>>[] results)
    {
        var dictionary = new Dictionary<char, int>();
        
        foreach(var result in results.SelectMany(x => x))
        {
            if(dictionary.TryGetValue(result.Key, out var existingCount))
            {
                dictionary[result.Key] = existingCount + result.Value;
            }
            else
            {
                dictionary[result.Key] = result.Value;
            }
        }

        return dictionary;
    }

    private static Task<IEnumerable<KeyValuePair<char, int>>> CalculateAsync(string input)
    {
        return Task.Run(() => CalculateTop3Counts(input));
    }

    private static IEnumerable<KeyValuePair<char, int>> CalculateTop3Counts(string input)
    {
        return input.Where(c => char.IsLetter(c))
            .Select(c => char.ToLowerInvariant(c))
            .ToLookup(x => x)
            .ToDictionary(c => c.Key, l => l.Count());
    }
}