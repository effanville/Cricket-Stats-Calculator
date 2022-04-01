namespace CricketStructures.Match
{
    public sealed class MatchResult
    {
        public bool HasResult
        {
            get;
            set;
        }

        public bool IsDraw
        {
            get;
            set;
        }

        public bool IsNoResult
        {
            get;
            set;
        }

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

        public static MatchResult IsNoResultMatch(string firstTeam, string secondTeam)
        {
            var result = new MatchResult(firstTeam, secondTeam);
            result.IsNoResult = true;
            return result;
        }

        public override string ToString()
        {
            if (IsNoResult)
            {
                return $"{WinningTeam} v {LosingTeam} was abandonded.";
            }
            if (!WinningRunMargin.HasValue && !WinningWicketMargin.HasValue)
            {
                return $"{WinningTeam} beat {LosingTeam}.";
            }

            string winningMargin = WinningRunMargin.HasValue ? $"{WinningRunMargin.Value} runs" : $"{WinningWicketMargin.Value} wickets";
            return $"{WinningTeam} beat {LosingTeam} by {winningMargin}.";
        }
    }
}
