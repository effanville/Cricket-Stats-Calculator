namespace CricketStructures.Statistics
{
    public static class StatCollectionExtenstions
    {
        public static bool IsPlayerStat(this StatCollection stat)
        {
            return stat == StatCollection.PlayerBrief
                || stat == StatCollection.PlayerDetailed
                || stat == StatCollection.PlayerSeason;
        }

        public static bool IsSeasonStat(this StatCollection stat)
        {
            return stat == StatCollection.SeasonBrief
                || stat == StatCollection.PlayerSeason;
        }

        public static bool IsAllTimeStat(this StatCollection stat)
        {
            return stat == StatCollection.AllTimeBrief
                || stat == StatCollection.AllTimeDetailed;
        }
    }
}
