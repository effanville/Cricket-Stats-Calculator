namespace CricketStructures.Match
{
    public static class WicketExtensions
    {
        public static bool IsBowlerWicket(this Wicket wicket)
        {
            return wicket == Wicket.Bowled
                || wicket == Wicket.LBW
                || wicket == Wicket.Caught
                || wicket == Wicket.HitWicket
                || wicket == Wicket.Stumped;
        }

        public static bool IsFielderWicket(this Wicket wicket)
        {
            return wicket == Wicket.Caught
                || wicket == Wicket.RunOut
                || wicket == Wicket.Stumped;
        }

        public static bool IsNotOut(this Wicket wicket)
        {
            return wicket == Wicket.NotOut
                || wicket == Wicket.RetiredNotOut
                || wicket == Wicket.DidNotBat;
        }

        public static bool MustBeKeeper(this Wicket wicket)
        {
            return wicket == Wicket.Stumped;
        }

        public static bool DidNotBat(this Wicket wicket)
        {
            return wicket == Wicket.DidNotBat;
        }

        public static bool DidBat(this Wicket wicket)
        {
            return !wicket.DidNotBat();
        }

        public static bool IsOut(this Wicket wicket)
        {
            return !wicket.IsNotOut();
        }
    }
}
