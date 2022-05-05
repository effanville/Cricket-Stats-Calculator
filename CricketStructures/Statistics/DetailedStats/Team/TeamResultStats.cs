using System.Collections.Generic;
using System.Linq;
using CricketStructures.Match;
using CricketStructures.Season;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;
using System.Text;

namespace CricketStructures.Statistics.DetailedStats
{
    public class TeamResultStats
    {
        /// <summary>
        /// Year by year record of the performance of the team.
        /// </summary>
        public List<TeamYearRecord> YearByYearRecords
        {
            get;
            set;
        } = new List<TeamYearRecord>();

        /// <summary>
        /// Record by opposition of the performance of the team
        /// </summary>
        public List<TeamOppositionRecord> TeamAgainstRecords
        {
            get;
            set;
        } = new List<TeamOppositionRecord>();

        public ExtremeScores NotableScores
        {
            get;
            set;
        } = new ExtremeScores();

        public LargestVictories BestResults
        {
            get;
            set;
        } = new LargestVictories();

        public HeaviestDefeats WorstLosses
        {
            get;
            set;
        } = new HeaviestDefeats();

        public TeamResultStats()
        {

        }

        public void CalculateStats(ICricketTeam team)
        {
            foreach (ICricketSeason season in team.Seasons)
            {
                CalculateStats(team.TeamName, season);
            }

            YearByYearRecords.Sort((a, b) => a.Year.CompareTo(b.Year));
            TeamAgainstRecords.Sort((a, b) => a.OppositionName.CompareTo(b.OppositionName));
        }

        public void CalculateStats(string teamName, ICricketSeason season)
        {
            YearByYearRecords.Add(new TeamYearRecord(season));

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(teamName, match);
            }
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            NotableScores.UpdateStats(teamName, match);
            BestResults.UpdateStats(teamName, match);
            WorstLosses.UpdateStats(teamName, match);

            if (TeamAgainstRecords.Any(team => team.OppositionName.Equals(match.MatchData.OppositionName(teamName))))
            {
                TeamOppositionRecord oppo = TeamAgainstRecords.First(team => team.OppositionName.Equals(match.MatchData.OppositionName(teamName)));
                oppo.AddResult(match);
            }
            else
            {
                TeamAgainstRecords.Add(new TeamOppositionRecord(teamName, match));
            }
        }

        public void ExportStats(StringBuilder writer, DocumentType exportType)
        {
            TextWriting.WriteTitle(writer, exportType, "Yearly Records", DocumentElement.h2);
            TableWriting.WriteTable(writer, exportType, YearByYearRecords, headerFirstColumn: false);

            TextWriting.WriteTitle(writer, exportType, "Record against each team", DocumentElement.h2);
            TableWriting.WriteTable(writer, exportType, TeamAgainstRecords, headerFirstColumn: false);

            NotableScores.ExportStats(writer, exportType);

            BestResults.ExportStats(writer, exportType);

            WorstLosses.ExportStats(writer, exportType);
        }
    }
}
