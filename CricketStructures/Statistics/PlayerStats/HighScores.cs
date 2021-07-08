using CricketStructures.Player;

namespace CricketStructures.Statistics.PlayerStats
{
    public class HighScores
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int Centuries
        {
            get;
            set;
        }

        public int Fifties
        {
            get;
            set;
        }

        public HighScores()
        {
        }

        public HighScores(PlayerName name, int century, int fifty)
        {
            Name = name;
            Centuries = century;
            Fifties = fifty;
        }
    }
}
