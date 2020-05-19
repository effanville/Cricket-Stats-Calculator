using Cricket.Player;

namespace Cricket.Statistics.DetailedStats
{
    public class PartnershipNumber
    {
        public int NumberPartnerships
        {
            get;
            set;
        }

        public PlayerName Player
        {
            get;
            set;
        }

        public string ToCSVLine()
        {
            return NumberPartnerships + "," + Player.ToString();
        }
    }
}
