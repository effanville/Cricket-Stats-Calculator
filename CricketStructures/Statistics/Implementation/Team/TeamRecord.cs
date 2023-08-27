using System.Collections.Generic;

using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class TeamRecord : ICricketStat
    {
        private int SafeGet(ResultType resultType) => _matchRecord.ContainsKey(resultType) ? _matchRecord[resultType] : 0;
        private Dictionary<ResultType, int> _matchRecord = new Dictionary<ResultType, int>();
        public int Played
        {
            get;
            set;
        }

        public int Won => _matchRecord.ContainsKey(ResultType.Win) ? _matchRecord[ResultType.Win] : 0;
        public int Walkover => SafeGet(ResultType.Walkover);

        public int Drew => SafeGet(ResultType.Draw);

        public int Lost => _matchRecord.ContainsKey(ResultType.Loss) ? _matchRecord[ResultType.Loss] : 0;

        public int Abandoned => _matchRecord.ContainsKey(ResultType.Abandoned) ? _matchRecord[ResultType.Abandoned] : 0;

        public int Tie => _matchRecord.ContainsKey(ResultType.Tie) ? _matchRecord[ResultType.Tie] : 0;

        public int Cancelled => SafeGet(ResultType.Cancelled);

        public double WinRatio
        {
            get => Won / (double)Played;
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
            if (_matchRecord.ContainsKey(match.Result))
            {
                _matchRecord[match.Result]++;
            }
            else
            {
                _matchRecord[match.Result] = 1;
            }
        }

        /// <inheritdoc/>
        public void ResetStats()
        {
            _matchRecord.Clear();
        }

        /// <inheritdoc/>
        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            _ = rb.WriteTitle("Team Overall", headerElement)
                .WriteParagraph(new string[] { "Games Played:", $"{Played}" })
                .WriteParagraph(new string[] { "Wins:", $"{Won}" })
                .WriteParagraph(new string[] { "Losses:", $"{Lost}" })
                .WriteParagraph(new string[] { "Draws:", $"{Drew}" })
                .WriteParagraph(new string[] { "Ties:", $"{Tie}" })
                .WriteParagraph(new string[] { "Abandoned", $"{Abandoned}" })
                .WriteParagraph(new string[] { "Cancelled", $"{Cancelled}" })
                .WriteParagraph(new string[] { "Walkover", $"{Walkover}" });
        }
    }
}
