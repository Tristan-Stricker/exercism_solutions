using System;

public static class Diamond
{
    public static string Make(char target)
    {
        const char startChar = 'A';

        if(target == startChar)
        {
            return "A";
        }

        var subTriangleHeight = target - startChar;
        var lineLength = (2 * subTriangleHeight) + 1;
        var lines = new string[lineLength];

        var midPoint = subTriangleHeight + 1;

        for (int lineNumber = 0; lineNumber < lines.Length; lineNumber++)
        {
            var line = new char[lineLength];
            Array.Fill(line, ' ');

            var isFirst = lineNumber == 0;
            var isLast = lineNumber == lines.Length - 1;

            if (isFirst || isLast)
            {
                line[midPoint - 1] = startChar;
                lines[lineNumber] = new string(line);
                continue;
            }

            var charOnLine = (char)((65) + lineNumber);
            var charIndex = lineNumber + 1;
            var leftIndex = midPoint - charIndex  - 1;
            var rightIndex = midPoint + charIndex - 1;

            if(lineNumber < subTriangleHeight)
            {
                line[leftIndex] = charOnLine;
                line[rightIndex] = charOnLine;
            }
            else if(lineNumber == subTriangleHeight)
            {
                line[0] = charOnLine;
                line[lineLength - 1] = charOnLine;
            }
            else
            {
                var diff = Math.Abs(subTriangleHeight - lineNumber);
                var upIndex = subTriangleHeight - diff;
                lines[lineNumber] = lines[upIndex];
                continue;
            }

            lines[lineNumber] = new string(line);
        }

        var outPut = string.Join("\n", lines);
        return outPut;
    }
}