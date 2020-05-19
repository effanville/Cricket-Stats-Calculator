using Cricket.Interfaces;

namespace Cricket.Statistics.DetailedStats
{
    public class TeamOppositionRecord : TeamRecord
    {
        public string OppositionName
        {
            get;
            set;
        }

        public TeamOppositionRecord(string opposition)
            : base()
        {
            OppositionName = opposition;
        }

        public TeamOppositionRecord(ICricketMatch match)
            : base()
        {
            OppositionName = match.MatchData.Opposition;
            AddResult(match);
        }

        public void AddResult(ICricketMatch match)
        {
            Played++;
            if (match.Result == Match.ResultType.Win)
            {
                Won++;
            }

            if (match.Result == Match.ResultType.Loss)
            {
                Lost++;
            }
            if (match.Result == Match.ResultType.Tie)
            {
                Tie++;
            }
            if (match.Result == Match.ResultType.Draw)
            {
                Drew++;
            }

            WinRatio = (double)Won / (double)Played;
        }

        public new string ToCSVLine()
        {
            return OppositionName + "," + base.ToCSVLine();
        }
    }
}
