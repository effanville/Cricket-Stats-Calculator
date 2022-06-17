using CricketStructures.Match;
using CricketStructures.Match.Innings;
using CricketStructures.Season;
using CricketStructures.Player;
using System.Text;
using Common.Structure.ReportWriting;

namespace CricketStructures.Statistics.Implementation.Player.Fielding
{
    public class PlayerFieldingStatistics : ICricketStat
    {
        public PlayerName Name
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
            CricketStatsHelpers.MatchIterator(
                season,
                matchTypes,
                match => UpdateStats(teamName, match));
        }

        public void UpdateStats(string teamName, ICricketMatch match)
        {
            FieldingEntry fielding = match.GetFielding(teamName, Name);
            if (fielding != null)
            {
                Catches += fielding.Catches;
                RunOuts += fielding.RunOuts;
                KeeperCatches += fielding.KeeperCatches;
                KeeperStumpings += fielding.KeeperStumpings;
            }
        }

        public StringBuilder ExportStats(DocumentType exportType, DocumentElement headerElement)
        {
            StringBuilder sb = new StringBuilder();
            TextWriting.WriteTitle(sb, exportType, "Fielding Stats", headerElement);
            TableWriting.WriteTable(sb, exportType, new PlayerFieldingStatistics[] { this }, headerFirstColumn: false);
            return sb;
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
