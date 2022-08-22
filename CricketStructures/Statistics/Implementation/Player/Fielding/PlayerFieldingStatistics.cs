﻿using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using Common.Structure.ReportWriting;
using System;
using System.Collections.Generic;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public sealed class PlayerFieldingRecord : ICricketStat
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

        public PlayerFieldingRecord()
        {
            StartYear = DateTime.MaxValue;
            EndYear = DateTime.MinValue;
        }

        public PlayerFieldingRecord(PlayerName name)
            : this()
        {
            Name = name;
        }
        public PlayerFieldingRecord(PlayerName name, ICricketTeam team, MatchType[] matchTypes)
            : this(name)
        {
            CalculateStats(team, matchTypes);
        }

        public PlayerFieldingRecord(PlayerName name, string teamName, ICricketSeason season, MatchType[] matchTypes)
            : this(name)
        {
            CalculateStats(teamName, season, matchTypes);
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
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            if (match.Played(teamName, Name))
            {
                if (match.MatchData.Date < StartYear)
                {
                    StartYear = match.MatchData.Date;
                }
                if (match.MatchData.Date > EndYear)
                {
                    EndYear = match.MatchData.Date;
                }

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
                .WriteTableFromEnumerable(Headers(true, false), new[] { Values(true, false) }, headerFirstColumn: false);
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

        public static IReadOnlyList<string> Headers(bool includeName, bool singleSeason, bool includeYear = true)
        {
            var headers = new List<string>();
            if (includeName)
            {
                headers.Add("Name");
            }

            if(includeYear)
            {
                if (singleSeason)
                {
                    headers.Add("Year");
                }
                else
                {
                    headers.Add("Start Year");
                    headers.Add("End Year");
                }
            }

            headers.Add("Catches");
            headers.Add("Run Outs");
            headers.Add($"Catches({CricketConstants.WicketKeeperSymbol})");
            headers.Add("Stumpings");
            headers.Add("Total");

            return headers;
        }

        public IReadOnlyList<string> Values(bool includeName, bool singleSeason, bool includeYear = true)
        {
            var values = new List<string>();
            if (includeName)
            {
                values.Add(Name.ToString());
            }

            if(includeYear)
            {
                if (singleSeason)
                {
                    values.Add(StartYear.Year.ToString());
                }
                else
                {
                    values.Add(StartYear.Year.ToString());
                    values.Add(EndYear.Year.ToString());
                }
            }
            values.Add(Catches.ToString());
            values.Add(RunOuts.ToString());
            values.Add(KeeperCatches.ToString());
            values.Add(KeeperStumpings.ToString());
            values.Add(TotalDismissals.ToString());

            return values;
        }
    }
}
