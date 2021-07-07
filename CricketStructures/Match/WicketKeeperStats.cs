using Cricket.Player;

namespace Cricket.Match
{
    public class WicketKeeperStats
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Stumpings
        {
            get; set;
        }

        public int Catches
        {
            get; set;
        }

        public void SetScores(int stumpings, int catches)
        {
            Stumpings = stumpings;
            Catches = catches;
        }

        public WicketKeeperStats(PlayerName name)
        {
            Name = name;
        }

        public WicketKeeperStats()
        {
            Name = new PlayerName();
        }
    }
}
