using System.Collections.Generic;
using System.IO;
using System.Linq;
using CricketStructures.Interfaces;
using CricketStructures.Match.Innings;
using StructureCommon.FileAccess;

namespace CricketStructures.Statistics.DetailedStats
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
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }
            foreach (List<Partnership> partList in PartnershipsByWicket)
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
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            List<Partnership> partnerships = match.Partnerships(teamName);
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

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            if (PartnershipsByWicket.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Partnerships By Wicket Over 100", HtmlTag.h2);

                for (int i = 0; i < PartnershipsByWicket.Count; i++)
                {
                    if (PartnershipsByWicket[i].Any())
                    {
                        FileWritingSupport.WriteTitle(writer, exportType, $"{i + 1}th Wicket", HtmlTag.h3);
                        FileWritingSupport.WriteTable(writer, exportType, PartnershipsByWicket[i], headerFirstColumn: false);
                    }
                }
            }

            if (MostPartnerships.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Most Partnerships", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, MostPartnerships, headerFirstColumn: false);
            }
            if (MostPartnershipsAsPair.Any())
            {
                FileWritingSupport.WriteTitle(writer, exportType, "Most Partnerships As a Pair", HtmlTag.h2);
                FileWritingSupport.WriteTable(writer, exportType, MostPartnershipsAsPair, headerFirstColumn: false);
            }
        }
    }
}
