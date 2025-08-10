using System.Text;

public static class OcrNumbers
{
    public static string Convert(string input)
    {
        var splitByNewline = input.Split('\n');

        if (splitByNewline.Length % 4 != 0)
        {
            throw new ArgumentException("Line numbers must be multiple of four lines");
        }

        if (splitByNewline.Any(line => line.Length % 3 != 0))
        {
            throw new ArgumentException("Column lengths must be multiple of 3");
        }
        
        StringBuilder sb = new StringBuilder();
        var linesChunkedIntoFourRowGroups = splitByNewline.Chunk(4).ToList();
        
        var counter = 0;
        foreach (var lines in linesChunkedIntoFourRowGroups)
        {
            var columnIndexes = Enumerable.Range(0, lines[0].Length);
            var columnIndexRanges = columnIndexes.Chunk(3).ToArray();
            foreach (var chunkIndex in columnIndexRanges)
            {
                var indexStart = chunkIndex[0];
                var indexEnd = chunkIndex[2] + 1;
                var row1 = lines[0][indexStart..indexEnd];
                var row2 = lines[1][indexStart..indexEnd];
                var row3 = lines[2][indexStart..indexEnd];

                var number = $"{row1}{row2}{row3}";
                sb.Append(GetNumber(number));
            }
            counter++;

            var hasMoreRows = counter != linesChunkedIntoFourRowGroups.Count;
            
            if (hasMoreRows)
            {
                sb.Append(',');
            }
        }
        
        return sb.ToString();
    }

    private static string GetNumber(string input) =>
        input switch
        {
            Zero => "0",
            One => "1",
            Two => "2",
            Three => "3",
            Four => "4",
            Five => "5",
            Six => "6",
            Seven => "7",
            Eight => "8",
            Nine => "9",
            _ => "?"
        };
    
    private const string Zero = 
        " _ " +
        "| |" +
        "|_|";
    
    private const string One = 
        "   " +
        "  |" +
        "  |";
    
    private const string Two = 
        " _ " +
        " _|" +
        "|_ ";
    
    private const string Three = 
        " _ " +
        " _|" +
        " _|";
    
    private const string Four = 
        "   " +
        "|_|" +
        "  |";
    
    private const string Five = 
        " _ " +
        "|_ " +
        " _|";
    
    private const string Six = 
        " _ " +
        "|_ " +
        "|_|";
    
    private const string Seven = 
        " _ " +
        "  |" +
        "  |";
    
    private const string Eight = 
        " _ " +
        "|_|" +
        "|_|";
    
    private const string Nine = 
        " _ " +
        "|_|" +
        " _|";
}