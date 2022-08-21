using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using System;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public sealed class PlayerFieldingStatistics : ICricketStat
    {
        public PlayerName Name
        {
            get;
            set;
        }

        public DateTime StartYear
        {
            get;
            private set;
        }

        public DateTime EndYear
        {
            get;
            private set;
        }

        public int MatchesPlayed
        {
            get;
            set;
        }

        public int Catches
        {
            get;
            set;
        }

        public int RunOuts
        {
            get;
            set;
        }

        public int KeeperStumpings
        {
            get;
            set;
        }

        public int KeeperCatches
        {
            get;
            set;
        }

        public int TotalDismissals => Catches + RunOuts + KeeperCatches + KeeperStumpings;

        internal int TotalKeeperDismissals => KeeperCatches + KeeperStumpings;

        internal int TotalNonKeeperDismissals => Catches + RunOuts;

        public PlayerFieldingStatistics()
        {
        }

        public PlayerFieldingStatistics(PlayerName name)
        {
            Name = name;
        }

        public void CalculateStats(ICricketTeam team, MatchType[] matchTypes)
        {
            CricketStatsHelpers.SeasonIterator(
                team.Seasons,
                season => CalculateStats(team.TeamName, season, matchTypes),
                preCycleAction: ResetStats);
        }

        public void CalculateStats(string teamName, ICricketSeason season, MatchType[] matchTypes)
        {
            if (season.Year < StartYear)
            {
                StartYear = season.Year;
            }
            if (season.Year > EndYear)
            {
                EndYear = season.Year;
            }

            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.Played(teamName, Name))
            {
                MatchesPlayed++;
                FieldingEntry fielding = match.GetFielding(teamName, Name);
                if (fielding != null)
                {
                    Catches += fielding.Catches;
                    RunOuts += fielding.RunOuts;
                    KeeperCatches += fielding.KeeperCatches;
                    KeeperStumpings += fielding.KeeperStumpings;
                }
            }
        }

        public void ExportStats(ReportBuilder rb, DocumentElement headerElement)
        {
            
            _ = rb.WriteTitle("Fielding Stats", headerElement)
                .WriteTable(new PlayerFieldingStatistics[] { this }, headerFirstColumn: false);
        }

        public void Finalise()
        {
        }

        public void ResetStats()
        {
            Catches = 0;
            RunOuts = 0;
            KeeperStumpings = 0;
            KeeperCatches = 0;
        }
    }
}
