﻿using Cricket.Interfaces;
using ExportHelpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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
            foreach (var season in team.Seasons)
            {
                CalculateStats(season);
            }

            YearByYearRecords.Sort((a, b) => a.Year.CompareTo(b.Year));
            TeamAgainstRecords.Sort((a, b) => a.OppositionName.CompareTo(b.OppositionName));
        }

        public void CalculateStats(ICricketSeason season)
        {
            YearByYearRecords.Add(new TeamYearRecord(season));

            foreach (var match in season.Matches)
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
                var oppo = TeamAgainstRecords.First(team => team.OppositionName.Equals(match.MatchData.Opposition));
                oppo.AddResult(match);
            }
            else
            {
                TeamAgainstRecords.Add(new TeamOppositionRecord(match));
            }
        }

        public void ExportStats(StreamWriter writer)
        {
            writer.WriteLine(GenericHeaderWriter.TableHeader(YearByYearRecords[0], ","));
            foreach (var record in YearByYearRecords)
            {
                writer.WriteLine(record.ToCSVLine());
            }

            writer.WriteLine("");
            writer.WriteLine(GenericHeaderWriter.TableHeader(TeamAgainstRecords[0], ","));
            foreach (var record in TeamAgainstRecords)
            {
                writer.WriteLine(record.ToCSVLine());
            }

            writer.WriteLine("");
            NotableScores.ExportStats(writer);

            writer.WriteLine("");
            BestResults.ExportStats(writer);

            writer.WriteLine("");
            WorstLosses.ExportStats(writer);
        }
    }
}
