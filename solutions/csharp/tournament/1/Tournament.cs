using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class Tournament
{
    public static void Tally(Stream inStream, Stream outStream)
    {
        const string header = "Team                           | MP |  W |  D |  L |  P";

        var scores = new Dictionary<string, Score>();

        using var reader = new StreamReader(inStream);

        while (reader.Peek() >= 0)
        {
            var lineParts = reader.ReadLine().Split(';');

            if (lineParts.Count() != 3) throw new InvalidOperationException("");

            var leftTeam = lineParts[0];
            var rightTeam = lineParts[1];

            var result = GetResult(lineParts[2]);

            switch (result)
            {
                case Result.Win:
                    {
                        Record(scores, leftTeam, Result.Win);
                        Record(scores, rightTeam, Result.Loss);
                        break;
                    }
                case Result.Loss:
                    {
                        Record(scores, leftTeam, Result.Loss);
                        Record(scores, rightTeam, Result.Win);
                        break;
                    }
                case Result.Draw:
                    {
                        Record(scores, leftTeam, Result.Draw);
                        Record(scores, rightTeam, Result.Draw);
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
        }

        var lines = scores
            .OrderByDescending(x => x.Value.Points)
            .ThenBy(x => x.Key)
            .ToList()
            .Select(rank => {
                var team = rank.Key;
                var score = rank.Value;
                var cells = $"|  {score.Played} |  {score.Won} |  {score.Draws} |  {score.Lost} |  {score.Points}";

                var padding = header.Length - cells.Length;

                return team.PadRight(padding) + cells;
            });

        using var writer =  new StreamWriter(outStream);        

        if (!lines.Any())
            writer.Write(header);
        else
            writer.Write(header + "\n");

        var stringLines = string.Join("\n", lines);

        foreach(var line in stringLines)
        {
            writer.Write(line);
        }
    }

    private static void Record(
        Dictionary<string, Score> scores,
        string team,
        Result result)
    {
        if (scores.TryGetValue(team, out var currentScore))
        {
            scores[team] = Score.FromResult(currentScore, result); ;
        }
        else
        {
            scores[team] = Score.FromResult(Score.Nil, result);
        }
    }

    private class Score
    {

        public int Points => (Won * 3) + (Draws * 1);

        public int Played { get; }

        public int Won { get; }

        public int Draws { get; }

        public int Lost { get; }

        public static Score FromResult(Score score, Result result) => 
            new Score(
                played: score.Played + 1,
                won: score.Won + (result == Result.Win ? 1 : 0),
                drawn: score.Draws + (result == Result.Draw ? 1 : 0),
                lost: score.Lost + (result == Result.Loss ? 1 : 0)
                );

        public static Score Nil => new Score(0, 0, 0, 0);        

        private Score(int played, int won, int drawn, int lost)
        {
            Played = played;
            Won    = won;
            Draws  = drawn;
            Lost   = lost;
        }
    }

    private enum Result { Win, Draw, Loss };

    private static Result GetResult(string result) => result switch
    {
        "win" => Result.Win,
        "loss" => Result.Loss,
        "draw" => Result.Draw,
        _ => throw new NotImplementedException()
    };
}
