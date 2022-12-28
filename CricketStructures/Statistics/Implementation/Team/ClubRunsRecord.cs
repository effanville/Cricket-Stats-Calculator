using Common.Structure.ReportWriting;

using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;

namespace CricketStructures.Statistics.Implementation.Team
{
    internal sealed class ClubRunsRecord : ICricketStat
    {
        public int NumberGames{get;private set;}
        public int NumberRuns{get; private set;}
        public int NumberWickets{get; private set;}
        public Over NumberOvers{get; private set;}

        public double RunsPerGame => (double)NumberRuns / NumberGames;
        public double RunsPerWicket => (double)NumberRuns / NumberWickets;
        public double RunsPerOver => (double)NumberRuns / (double)NumberOvers;

        public ClubRunsRecord()
        {
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {            
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                ResetStats,
                Finalise);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {            
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match),
                postCycleAction: Finalise);
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {            
            _ = rb.WriteTitle("Team Overall", headerElement)
                .WriteParagraph(new string[] { "Games Played:", $"{NumberGames}" })
                .WriteParagraph(new string[] { "RunsScored:", $"{NumberRuns}" })
                .WriteParagraph(new string[] { "RunsPerGame:", $"{RunsPerGame}" })
                .WriteParagraph(new string[] { "RunsPerWicket:", $"{RunsPerWicket}" })
                .WriteParagraph(new string[] { "RunsPerOver:", $"{RunsPerOver}" });
        }

        public void Finalise()
        {
        }

        public void ResetStats()
        {
            NumberRuns = 0;
            NumberGames = 0;
            NumberWickets = 0;
            NumberOvers = 0;
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            var innings = match.GetInnings(teamName, batting: true);
            var teamScore = innings?.Score();
            if(teamScore != null)
            {
                NumberGames++;
                NumberRuns += teamScore.Runs;
                NumberWickets += teamScore.Wickets;
                NumberOvers += teamScore.Overs == Over.Unknown() ? match.MaximumNumberOvers : teamScore.Overs;
            }
        }
    }
}