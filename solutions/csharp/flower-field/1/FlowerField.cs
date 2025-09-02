using System.Text;

public static class FlowerField
{
    public static string[] Annotate(string[] input)
    {
        if (input.Length == 0)
        {
            return [];
        }

        var field = new Field(input);
        return field.CountSurroundingFlowers().ToArray();
    }
    
    
    private class Field
    {
        public Field(string[] input)
        {
            Rows = input.Length;
            Cols = input[0].Length;
            Matrix = new char[Rows, Cols];

            for (int row = 0; row < Rows; row++)
            {
                for (int col = 0; col < Cols; col++)
                {
                    Matrix[row, col] = input[row][col];
                }
            }
        }

        public IEnumerable<string> CountSurroundingFlowers()
        {
            for (int row = 0; row < Rows; row++)
            {
                var stringBuilder = new StringBuilder();
                for (int col = 0; col < Cols; col++)
                {
                    var cell = Matrix[row, col];

                    if (cell == ' ')
                    {
                        var sum = GetSurroundingFlowers(row, col).Sum();
                        if (sum > 0)
                        {
                            var convert = Convert.ToString(sum);
                            stringBuilder.Append(convert);
                        }
                        else
                        {
                            stringBuilder.Append(' ');
                        }
                    }
                    else
                    {
                        stringBuilder.Append(cell);
                    }
                }
                yield return stringBuilder.ToString();
            }
        }

        private IEnumerable<int> GetSurroundingFlowers(int row, int col)
        {
            int[] window = [-1, 0, 1];
            foreach (var slidingCol in window)
            {
                foreach (var slidingRow in window)
                {
                    var adjustedCol = col + slidingCol;
                    var adjustedRow = row + slidingRow;
                    
                    if (adjustedCol == col && adjustedRow == row)
                    {
                        continue;
                    }

                    if (IsOutsideBounds(adjustedRow, adjustedCol))
                    {
                        continue;
                    }
                    
                    var value =  Matrix[adjustedRow, adjustedCol];

                    if (value == '*')
                    {
                        yield return 1;
                    }
                }
            }
        }

        private bool IsOutsideBounds(int row, int col) =>
            col < 0 || 
            col >= Cols || 
            row < 0 || 
            row >= Rows;

        public char[,] Matrix { get; }

        private int Rows { get; }
        private int Cols { get; }
    }
    
}

