using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using Common.Structure.ReportWriting;
using System.Text;
using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Partnerships
{
    internal sealed class PlayerPartnershipStats : ICricketStat
    {
        PlayerName Name
        {
            get; set;
        }

        public List<Partnership> PartnershipsByWicket
        {
            get;
            set;
        } = new List<Partnership>(new Partnership[10]);

        public PlayerPartnershipStats()
        {
        }

        public PlayerPartnershipStats(PlayerName player)
            : this()
        {
            Name = player;
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes));
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
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
                for (int i = 0; i < partnerships.Count; i++)
                {
                    if (partnerships[i] != null)
                    {
                        if (PartnershipsByWicket[i] == null)
                        {
                            if ((Name == null || partnerships[i].ContainsPlayer(Name)))
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                        }
                        else
                        {
                            if ((Name == null || partnerships[i].ContainsPlayer(Name)) && partnerships[i].CompareTo(PartnershipsByWicket[i]) > 0)
                            {
                                PartnershipsByWicket[i] = partnerships[i];
                            }
                        }
                    }
                }
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            PartnershipsByWicket = new List<Partnership>();
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder writer = new StringBuilder();
            TextWriting.WriteTitle(writer, exportType, "Highest Partnerships", headerElement);
            TableWriting.WriteTable(writer, exportType, PartnershipsByWicket.Where(ship => ship != null), headerFirstColumn: false);

            return writer;
        }
    }
}
