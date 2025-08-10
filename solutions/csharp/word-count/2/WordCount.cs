using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class WordCount
{
    public static IDictionary<string, int> CountWords(string phrase)
    {
        var counts = new Dictionary<string, int>();
        var currentWord = new StringBuilder();
        var escapeChars = new char[] { '\n' };
        var beginApostrophe = false;

        var i = 0;
        foreach(var character in phrase)
        {
            i++;
            var isPunctuationOrSeperator = char.IsPunctuation(character) || char.IsSeparator(character);
            var isApostrophe = character == '\'';

            // begin quote
            if (currentWord.Length == 0 && isApostrophe) 
            {
                beginApostrophe = true;
                continue;
            }
            // end quote
            else if (currentWord.Length > 0 && beginApostrophe && isApostrophe) 
            {
                beginApostrophe = false;
                AddWord(counts, currentWord);
                continue;
            }
            // escape chars
            else if (escapeChars.Contains(character))
            {
                continue;
            }
            // punctuation or seperator
            else if(currentWord.Length > 0 && isPunctuationOrSeperator && !isApostrophe)
            {
                AddWord(counts, currentWord);
            }
            // the end of sequence
            else if (i == phrase.Length && currentWord.Length > 0)
            {
                currentWord.Append(character);
                AddWord(counts, currentWord);
            }
            // just letters, chars or apostrophe inbetween letters
            else if(char.IsLetterOrDigit(character) || isApostrophe)
            {
                currentWord.Append(character);
            }
        }

        return counts;
    }

    private static void AddWord(Dictionary<string, int> counts, StringBuilder currentWord)
    {
        var word = currentWord.ToString().ToLowerInvariant();

        if(counts.TryGetValue(word, out var value))
        {
            counts[word] = value + 1;
        }
        else
        {
            counts[word] = 1;
        }

        currentWord.Clear();
    }
}