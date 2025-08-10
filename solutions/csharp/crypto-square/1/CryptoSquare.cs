using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class CryptoSquare
{
    private static string FixedLength(string input, int length)
    {
        if (input.Length > length)
            return input.Substring(0, length);
        else
            return input.PadRight(length, ' ');
    }

    private static string NormalizedPlaintext(string plaintext)
    {
        return new string(plaintext.Where(c => !char.IsPunctuation(c) && !char.IsWhiteSpace(c)).Select(c => char.ToLower(c)).ToArray());
    }

    private static IEnumerable<string> PlaintextSegments(string plaintext)
    {
        var sqrt    = Math.Sqrt(plaintext.Length);
        var rows    = Convert.ToInt32(Math.Round(sqrt));
        var columns = Convert.ToInt32(Math.Ceiling(sqrt));
        foreach (var rowNumber in Enumerable.Range(0, rows))
        {
            var row = new string(plaintext.Skip(rowNumber * columns).Take(columns).ToArray());
            var padded = FixedLength(row, columns);
            yield return padded;
        }

        yield break;
    }

    private static string Encoded(IEnumerable<string> segments)
    {
        var colLength = segments.FirstOrDefault()?.Length ?? 0;
        var encoded = new List<string>();

        foreach(var col in Enumerable.Range(0, colLength))
        {
            var verticalSlice = new StringBuilder();
            foreach (var segment in segments)
            {
                var character = segment.ElementAt(col);
                verticalSlice.Append(character);
            }

            encoded.Add(verticalSlice.ToString());
        }        

        return string.Join(' ', encoded);
    }

    public static string Ciphertext(string plaintext)
    {
        var normalized = NormalizedPlaintext(plaintext);
        var segments   = PlaintextSegments(normalized);
        var encoded    = Encoded(segments);
        return encoded;
    }
}