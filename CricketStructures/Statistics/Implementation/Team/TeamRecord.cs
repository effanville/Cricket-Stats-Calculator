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
            get => Won/(double)Played;
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

        public void Finalise()
        {
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
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Team Overall", headerElement)
                .WriteParagraph(new string[] { "Games Played:", $"{Played}" })
                .WriteParagraph(new string[] { "Wins:", $"{Won}" })
                .WriteParagraph(new string[] { "Losses:", $"{Lost}" })
                .WriteParagraph(new string[] { "Draws:", $"{Drew}" })
                .WriteParagraph(new string[] { "Ties:", $"{Tie}" });
        }
    }
}
