using Cricket.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Statistics.DetailedStats
{
    public class PartnershipStats
    {
        public string Info = "All Partnerships of value over 100 are recorded";
        public List<List<Partnership>> PartnershipsByWicket
        {
            get;
            set;
        } = new List<List<Partnership>>(new List<Partnership>[10]);

        public List<PartnershipNumber> MostPartnerships
        {
            get;
            set;
        } = new List<PartnershipNumber>();

        public List<PartnershipPairNumber> MostPartnershipsAsPair
        {
            get;
            set;
        } = new List<PartnershipPairNumber>();

        public void CalculateStats(ICricketTeam team)
        {
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }
        }

        public void CalculateStats(ICricketSeason season)
        {
            foreach (var match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        /// <summary>
        /// Updates the holdings of partnerships from the specified match.
        /// This updates and only stores partnerships where runs involved were over 100.
        /// </summary>
        public void UpdateStats(ICricketMatch match)
        {
            var partnerships = match.Partnerships();
            foreach (var ship in partnerships)
            {
                if (ship != null)
                {
                    if (ship.Runs >= 100)
                    {
                        PartnershipsByWicket[ship.Wicket - 1].Add(ship);
                        var partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerOne));
                        if (partnershipPlayer != null)
                        {
                            partnershipPlayer.NumberPartnerships++;
                        }
                        else
                        {
                            MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerOne, NumberPartnerships = 1 });
                        }

                        partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerTwo));
                        if (partnershipPlayer != null)
                        {
                            partnershipPlayer.NumberPartnerships++;
                        }
                        else
                        {
                            MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerTwo, NumberPartnerships = 1 });
                        }

                        var pair = MostPartnershipsAsPair.First(partnershipPair => ship.SamePair(partnershipPair.Player, partnershipPair.SecondPlayer));
                        if (pair != null)
                        {
                            pair.NumberPartnerships++;
                        }
                        else
                        {
                            MostPartnershipsAsPair.Add(new PartnershipPairNumber() { Player = ship.PlayerOne, SecondPlayer = ship.PlayerTwo, NumberPartnerships = 1 });
                        }
                    }
                }
            }
        }
    }
}
