namespace CricketStructures.Match
{
    public sealed class MatchResult
    {
        public string WinningTeam
        {
            get;
            set;
        }

        public string LosingTeam
        {
            get;
            set;
        }


        public int? WinningRunMargin
        {
            get;
            set;
        }

        public int? WinningWicketMargin
        {
            get;
            set;
        }

        public MatchResult(string winningTeam, string losingTeam, int? winningRunMargin = null, int? winningWicketMargin = null)
        {
            WinningTeam = winningTeam;
            LosingTeam = losingTeam;
            WinningRunMargin = winningRunMargin;
            WinningWicketMargin = winningWicketMargin;
        }

        public override string ToString()
        {
            if (!WinningRunMargin.HasValue && !WinningWicketMargin.HasValue)
            {
                return $"{WinningTeam} beat {LosingTeam}.";
            }

            string winningMargin = WinningRunMargin.HasValue ? $"{WinningRunMargin.Value} runs" : $"{WinningWicketMargin.Value} wickets";
            return $"{WinningTeam} beat {LosingTeam} by {winningMargin}.";
        }
    }
}
