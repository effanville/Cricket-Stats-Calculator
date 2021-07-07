using Cricket.Player;

namespace Cricket.Statistics.DetailedStats
{
    public class PartnershipPairNumber : PartnershipNumber
    {
        public PlayerName SecondPlayer
        {
            get;
            set;
        }

        public new string ToCSVLine()
        {
            return NumberPartnerships + "," + Player.ToString() + " & " + SecondPlayer.ToString();
        }
    }
}
