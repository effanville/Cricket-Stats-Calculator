using Cricket.Player;

namespace Cricket.Match
{
    public class FieldingEntry
    {
        public PlayerName Name
        {
            get;
            set;
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

        public int KeeperStumpings
        {
            get { return keeperFielding.Stumpings; }
            set { keeperFielding.Stumpings = value; } 
        }

        public int KeeperCatches
        {
            get {return keeperFielding.Catches; }
            set { keeperFielding.Catches = value; } 
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

        public FieldingEntry()
        { keeperFielding = new WicketKeeperStats(); }
    }
}
