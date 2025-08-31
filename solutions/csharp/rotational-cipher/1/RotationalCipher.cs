using System.Text;
public static class RotationalCipher
{
    public static string Rotate(string text, int shiftKey)
    {
        if (shiftKey is 0 or 26)
        {
            return text;
        }

        var stringBuilder = new StringBuilder();
        foreach (var character in text)
        {
            if (!char.IsAsciiLetter(character))
            {
                stringBuilder.Append(character);
                continue;
            }

            var isUpper = char.IsUpper(character);
            
            var lowerBound = isUpper ? 'A' : 'a';

            var offset = character - lowerBound;
            
            var modularShift = (offset + shiftKey) % 26;

            var newChar = lowerBound + modularShift;
            stringBuilder.Append((char)newChar);
        }

        return stringBuilder.ToString();
    }
}