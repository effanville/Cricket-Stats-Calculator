using Cricket.Player;

namespace Cricket.Match
{
    public class BowlingEntry
    {
        public PlayerName Name
        {
            get;
            private set;
        }

        private int fOversBowled;
        public int OversBowled
        {
            get { return fOversBowled; }
            set { fOversBowled = value; }
        }

        private int fMaidens;
        public int Maidens
        {
            get { return fMaidens; }
            set { fMaidens = value; }
        }

        private int fRunsConceded;
        public int RunsConceded
        {
            get { return fRunsConceded; }
            set { fRunsConceded = value; }
        }

        private int fWickets;
        public int Wickets
        {
            get { return fWickets; }
            set { fWickets = value; }
        }

        public void SetBowling(int overs, int maidens, int runsConceded, int wickets)
        {
            OversBowled = overs;
            Maidens = maidens;
            RunsConceded = runsConceded;
            Wickets = wickets;
        }

        public BowlingEntry(PlayerName name)
        {
            Name = name;
        }
    }
}
