using System.Text;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class TeamRecord : ICricketStat
    {
        public int Played
        {
            get;
            set;
        }

        public int Won
        {
            get;
            set;
        }

        public int Drew
        {
            get;
            set;
        }

        public int Lost
        {
            get;
            set;
        }

        public int Abandoned
        {
            get;
            set;
        }

        public int Tie
        {
            get;
            set;
        }

        public double WinRatio
        {
            get;
            set;
        }

        public TeamRecord()
        {
        }

        /// <inheritdoc/>
        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                Finalise);
        }

        /// <inheritdoc/>
        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: Finalise);
        }

        private void Finalise()
        {
            WinRatio = Won / (double)Played;
        }

        /// <inheritdoc/>
        public void UpdateStats(string teamName, ICricketMatch match)
        {
            Played++;
            if (match.Result == ResultType.Win)
            {
                Won++;
            }

            if (match.Result == ResultType.Loss)
            {
                Lost++;
            }
            if (match.Result == ResultType.Tie)
            {
                Tie++;
            }
            if (match.Result == ResultType.Draw)
            {
                Drew++;
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            Played = 0;
            Won = 0;
            Lost = 0;
            Drew = 0;
            Tie = 0;
            WinRatio = 0;
        }

        /// <inheritdoc/>
        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder sb = new StringBuilder();
            TextWriting.WriteTitle(sb, exportType, "Team Overall", headerElement);
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Games Played:", $"{Played}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Wins:", $"{Won}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Losses:", $"{Lost}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Draws:", $"{Drew}" });
            TextWriting.WriteParagraph(sb, exportType, new string[] { "Ties:", $"{Tie}" });
            return sb;
        }
    }
}
