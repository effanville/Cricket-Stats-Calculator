using Cricket.Player;

namespace Cricket.Match
{
    public class BattingEntry
    {
        public PlayerName Name
        {
            get;
            private set;
        }

        private int fRunsScored;
        public int RunsScored
        {
            get { return fRunsScored; }
            private set { fRunsScored = value; }
        }

        private BattingWicketLossType fMethodOut;
        public BattingWicketLossType MethodOut
        {
            get { return fMethodOut; }
            private set { fMethodOut = value; }
        }

        private PlayerName fFielder;
        public PlayerName Fielder
        {
            get { return fFielder; }
            private set { fFielder = value; }
        }

        private PlayerName fBowler;
        public PlayerName Bowler
        {
            get { return fBowler; }
            private set { fBowler = value; }
        }

        public bool Out()
        {
            return MethodOut != BattingWicketLossType.NotOut;
        }

        public void SetScores(BattingWicketLossType howOut, int runs, PlayerName fielder = null, PlayerName bowler = null)
        {
            MethodOut = howOut;
            RunsScored = runs;
            Fielder = fielder;
            Bowler = bowler;
        }

        public BattingEntry(PlayerName name)
        {
            Name = name;
        }
    }
}
