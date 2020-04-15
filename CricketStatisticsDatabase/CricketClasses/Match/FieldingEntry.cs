using Cricket.Player;

namespace Cricket.Match
{
    public class FieldingEntry
    {
        public PlayerName Name
        {
            get;
            private set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int RunOuts
        { 
            get; 
            set; 
        }

        public WicketKeeperStats keeperFielding
        { 
            get; 
            set; 
        }

        public void SetScores(int catches, int runOuts, int stumpings, int keeperCatches)
        {
            Catches = catches;
            RunOuts = runOuts;
            keeperFielding.SetScores(stumpings, keeperCatches);
        }

        public FieldingEntry(PlayerName name)
        {
            Name = name;
            keeperFielding = new WicketKeeperStats(name);
        }
    }
}
