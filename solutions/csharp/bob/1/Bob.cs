public static class Bob
{
    public static string Response(string statement)
    {
        var trimmed = statement.TrimEnd();
        
        var endsWithQuestionMark = trimmed.EndsWith('?');
        
        var lenght = trimmed.Length;
        var takeLenght = endsWithQuestionMark ? lenght -1 :lenght;
        var letters = trimmed
            .Take(takeLenght)
            .Where(char.IsLetter)
            .ToArray();
            
        var isAllCaps = letters.Any() && letters.All(char.IsUpper);

        if (isAllCaps && endsWithQuestionMark)
        {
            return "Calm down, I know what I'm doing!";
        }
        
        if (endsWithQuestionMark)
        {
            return "Sure.";
        }

        if (isAllCaps)
        {
            return "Whoa, chill out!";
        }

        if (string.IsNullOrWhiteSpace(trimmed))
        {
            return "Fine. Be that way!";
        }
        
        return "Whatever.";
        //throw new NotImplementedException("You need to implement this method.");
    }
}