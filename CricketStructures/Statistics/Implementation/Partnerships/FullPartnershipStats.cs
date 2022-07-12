using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using CricketStructures.Statistics.Implementation.Partnerships.Model;

namespace CricketStructures.Statistics.Implementation.Partnerships
{
    internal sealed class FullPartnershipStats : ICricketStat
    {
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

        public FullPartnershipStats()
        {
            for (int i = 0; i < PartnershipsByWicket.Count; i++)
            {
                PartnershipsByWicket[i] = new List<Partnership>();
            }
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: Update);

            void Update()
            {
                foreach (List<Partnership> partList in PartnershipsByWicket)
                {
                    partList.Sort((a, b) => b.Runs.CompareTo(a.Runs));
                }

                MostPartnerships.Sort((a, b) => b.NumberPartnerships.CompareTo(a.NumberPartnerships));
                MostPartnershipsAsPair.Sort((a, b) => b.NumberPartnerships.CompareTo(a.NumberPartnerships));
            }
        }

        /// <summary>
        /// Updates the holdings of partnerships from the specified match.
        /// This updates and only stores partnerships where runs involved were over 100.
        /// </summary>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            List<Partnership> partnerships = match.Partnerships(teamName);
            if (partnerships != null)
            {
                foreach (Partnership ship in partnerships)
                {
                    if (ship != null)
                    {
                        if (ship.Runs >= 100)
                        {
                            PartnershipsByWicket[ship.Wicket - 1].Add(ship);

                            if (MostPartnerships.Any(player => player.Player.Equals(ship.PlayerOne)))
                            {
                                PartnershipNumber partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerOne));
                                partnershipPlayer.NumberPartnerships++;
                            }
                            else
                            {
                                MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerOne, NumberPartnerships = 1 });
                            }

                            if (MostPartnerships.Any(player => player.Player.Equals(ship.PlayerTwo)))
                            {
                                PartnershipNumber partnershipPlayer = MostPartnerships.First(player => player.Player.Equals(ship.PlayerTwo));
                                partnershipPlayer.NumberPartnerships++;
                            }
                            else
                            {
                                MostPartnerships.Add(new PartnershipNumber() { Player = ship.PlayerTwo, NumberPartnerships = 1 });
                            }

                            if (MostPartnershipsAsPair.Any(partnershipPair => ship.SamePair(partnershipPair.Player, partnershipPair.SecondPlayer)))
                            {
                                PartnershipPairNumber pair = MostPartnershipsAsPair.First(partnershipPair => ship.SamePair(partnershipPair.Player, partnershipPair.SecondPlayer));
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
        public void ResetStats()
        {
            PartnershipsByWicket = new List<List<Partnership>>();
            MostPartnerships = new List<PartnershipNumber>();
            MostPartnershipsAsPair = new List<PartnershipPairNumber>();
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            DocumentElement lowerLevelElement = headerElement++;
            _ = rb.WriteTitle("Partnership Records", headerElement);
            if (PartnershipsByWicket.Any())
            {
                _ = rb.WriteTitle("Partnerships By Wicket Over 100", lowerLevelElement);

                for (int i = 0; i < PartnershipsByWicket.Count; i++)
                {
                    if (PartnershipsByWicket[i].Any())
                    {
                        _ = rb.WriteTitle($"{i + 1}th Wicket", lowerLevelElement)
                            .WriteTable(PartnershipsByWicket[i], headerFirstColumn: false);
                    }
                }
            }

            if (MostPartnerships.Any())
            {
                _ = rb.WriteTitle("Most Partnerships", lowerLevelElement)
                    .WriteTable(MostPartnerships, headerFirstColumn: false);
            }
            if (MostPartnershipsAsPair.Any())
            {
                _ = rb.WriteTitle("Most Partnerships As a Pair", lowerLevelElement)
                    .WriteTable(MostPartnershipsAsPair, headerFirstColumn: false);
            }
        }
    }
}
