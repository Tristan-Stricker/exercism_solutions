public static class PascalsTriangle
{
    public static IEnumerable<int[]> Calculate(int rows)
    {
        var currentRow = new[] { 1 };

        for (var rowNumber = 1; rowNumber <= rows; rowNumber++)
        {
            yield return currentRow;
     
            var numberOfElements = rowNumber + 1;
            
            var priorRow = currentRow;
            currentRow = new int[numberOfElements];

            for (var col = 0; col < currentRow.Length; col++)
            {
                var isFirstElement = col == 0;
                var isLastElement = col  == numberOfElements - 1;

                if (isFirstElement || isLastElement)
                {
                    currentRow[col] = 1;
                }
                else
                {
                    currentRow[col] = priorRow[col-1] + priorRow[col];
                }
            }
        }
    }
}