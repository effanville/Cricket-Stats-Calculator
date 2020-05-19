using Cricket.Interfaces;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
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

        public PartnershipStats()
        {
            for (int i = 0; i < PartnershipsByWicket.Count; i++)
            {
                PartnershipsByWicket[i] = new List<Partnership>();
            }
        }

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
            foreach (var partList in PartnershipsByWicket)
            {
                partList.Sort((a, b) => b.Runs.CompareTo(a.Runs));
            }

            MostPartnerships.Sort((a, b) => b.NumberPartnerships.CompareTo(a.NumberPartnerships));
            MostPartnershipsAsPair.Sort((a, b) => b.NumberPartnerships.CompareTo(a.NumberPartnerships));
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

                        if (MostPartnerships.Any(player => player.Player.Equals(ship.PlayerOne)))
                        {
                            var partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerOne));
                            partnershipPlayer.NumberPartnerships++;
                        }
                        else
                        {
                            MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerOne, NumberPartnerships = 1 });
                        }

                        if (MostPartnerships.Any(player => player.Player.Equals(ship.PlayerTwo)))
                        {
                            var partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerTwo));
                            partnershipPlayer.NumberPartnerships++;
                        }
                        else
                        {
                            MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerTwo, NumberPartnerships = 1 });
                        }

                        if (MostPartnershipsAsPair.Any(partnershipPair => ship.SamePair(partnershipPair.Player, partnershipPair.SecondPlayer)))
                        {
                            var pair = MostPartnershipsAsPair.First(partnershipPair => ship.SamePair(partnershipPair.Player, partnershipPair.SecondPlayer));
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

        public void ExportStats(StreamWriter writer)
        {
            if (PartnershipsByWicket.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Partnerships By Wicket Over 100");
                writer.WriteLine("");

                for (int i = 0; i < PartnershipsByWicket.Count; i++)
                {
                    if (PartnershipsByWicket[i].Any())
                    {
                        writer.WriteLine($"{i + 1}th Wicket");
                        writer.WriteLine(GenericHeaderWriter.TableHeader(new Partnership(), ","));
                        foreach (var record in PartnershipsByWicket[i])
                        {
                            writer.WriteLine(record.ToCSVLine());
                        }
                    }
                }
            }

            if (MostPartnerships.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Most Partnerships");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new PartnershipNumber(), ","));
                foreach (var record in MostPartnerships)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
            if (MostPartnershipsAsPair.Any())
            {
                writer.WriteLine("");
                writer.WriteLine("Most Partnerships As a Pair");
                writer.WriteLine("");

                writer.WriteLine(GenericHeaderWriter.TableHeader(new PartnershipPairNumber(), ","));
                foreach (var record in MostPartnershipsAsPair)
                {
                    writer.WriteLine(record.ToCSVLine());
                }
            }
        }
    }
}
