using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cricket.Interfaces;
using Common.Structure.FileAccess;
using Common.Structure.ReportWriting;

namespace Cricket.Statistics.DetailedStats
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
                CalculateStats(season);
            }

            YearByYearRecords.Sort((a, b) => a.Year.CompareTo(b.Year));
            TeamAgainstRecords.Sort((a, b) => a.OppositionName.CompareTo(b.OppositionName));
        }

        public void CalculateStats(ICricketSeason season)
        {
            YearByYearRecords.Add(new TeamYearRecord(season));

            foreach (ICricketMatch match in season.Matches)
            {
                UpdateStats(match);
            }
        }

        public void UpdateStats(ICricketMatch match)
        {
            NotableScores.UpdateStats(match);
            BestResults.UpdateStats(match);
            WorstLosses.UpdateStats(match);

            if (TeamAgainstRecords.Any(team => team.OppositionName.Equals(match.MatchData.Opposition)))
            {
                TeamOppositionRecord oppo = TeamAgainstRecords.First(team => team.OppositionName.Equals(match.MatchData.Opposition));
                oppo.AddResult(match);
            }
            else
            {
                TeamAgainstRecords.Add(new TeamOppositionRecord(match));
            }
        }

        public void ExportStats(StreamWriter writer, ExportType exportType)
        {
            FileWritingSupport.WriteTitle(writer, exportType, "Yearly Records", HtmlTag.h2);
            FileWritingSupport.WriteTable(writer, exportType, YearByYearRecords, headerFirstColumn: false);

            FileWritingSupport.WriteTitle(writer, exportType, "Record against each team", HtmlTag.h2);
            FileWritingSupport.WriteTable(writer, exportType, TeamAgainstRecords, headerFirstColumn: false);

            NotableScores.ExportStats(writer, exportType);

            BestResults.ExportStats(writer, exportType);

            WorstLosses.ExportStats(writer, exportType);
        }
    }
}
