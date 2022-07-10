using System;
using System.Linq;
using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Player;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Player
{
    public class PlayerAttendanceStatistics : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public int TotalGamesPlayed
        {
            get;
            set;
        }

        public int TotalGamesWon
        {
            get;
            set;
        }

        public int TotalGamesLost
        {
            get;
            set;
        }

        public int TotalMom
        {
            get;
            set;
        }

        public double WinRatio => Math.Round(TotalGamesWon / (double)TotalGamesPlayed, 2);

        public PlayerAttendanceStatistics()
        {
        }

        public PlayerAttendanceStatistics(PlayerName name)
        {
            Name = name;
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.PlayNotPlay(teamName, Name))
            {
                TotalGamesPlayed += 1;
                if (match.MenOfMatch != null && match.MenOfMatch.Contains(Name))
                {
                    TotalMom += 1;
                }
                if (match.Result == ResultType.Win)
                {
                    TotalGamesWon += 1;
                }
                if (match.Result == ResultType.Loss)
                {
                    TotalGamesLost += 1;
                }
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            TotalGamesWon = 0;
            TotalGamesPlayed = 0;
            TotalGamesLost = 0;
            TotalMom = 0;
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            var sb = new StringBuilder();
            TextWriting.WriteTitle(sb, exportType, "Appearances", headerElement);

            TableWriting.WriteTable(sb, exportType, new PlayerAttendanceStatistics[] { this }, headerFirstColumn: false);
            return sb;
        }
    }
}
