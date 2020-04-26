using Cricket.Player;

namespace Cricket.Match
{
    public class BattingEntry
    {
        public PlayerName Name
        {
            get;
            set;
        }

        private BattingWicketLossType fMethodOut;
        public BattingWicketLossType MethodOut
        {
            get { return fMethodOut; }
            set { fMethodOut = value; }
        }

        private PlayerName fFielder;
        public PlayerName Fielder
        {
            get { return fFielder; }
            set { fFielder = value; }
        }

        private PlayerName fBowler;
        public PlayerName Bowler
        {
            get { return fBowler; }
            set { fBowler = value; }
        }

        private int fRunsScored;
        public int RunsScored
        {
            get { return fRunsScored; }
            set { fRunsScored = value; }
        }

        public bool Out()
        {
            return !(MethodOut == BattingWicketLossType.NotOut || MethodOut == BattingWicketLossType.DidNotBat);
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

        public BattingEntry()
        { }
    }
}
