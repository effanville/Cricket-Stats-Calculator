using Cricket.Player;
using ExtensionMethods;
using System.Collections.Generic;
using System.Linq;
using Validation;

namespace Cricket.Match
{
    public class BattingEntry : IValidity
    {
        public PlayerName Name
        {
            get;
            set;
        }

        private BattingWicketLossType fMethodOut;
        public BattingWicketLossType MethodOut
        {
            get { return fMethodOut; }
            set { fMethodOut = value; }
        }

        private PlayerName fFielder;
        public PlayerName Fielder
        {
            get { return fFielder; }
            set { fFielder = value; }
        }

        private PlayerName fBowler;
        public PlayerName Bowler
        {
            get { return fBowler; }
            set { fBowler = value; }
        }

        private int fRunsScored;
        public int RunsScored
        {
            get { return fRunsScored; }
            set { fRunsScored = value; }
        }

        public bool Out()
        {
            return !(MethodOut == BattingWicketLossType.NotOut || MethodOut == BattingWicketLossType.DidNotBat);
        }

        public void SetScores(BattingWicketLossType howOut, int runs, PlayerName fielder = null, PlayerName bowler = null)
        {
            MethodOut = howOut;
            RunsScored = runs;
            Fielder = fielder;
            Bowler = bowler;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            var results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(RunsScored, nameof(RunsScored)));
            if (MethodOut == BattingWicketLossType.DidNotBat)
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    var bowlerShouldntBeSet = new ValidationResult();
                    bowlerShouldntBeSet.IsValid = false;
                    bowlerShouldntBeSet.PropertyName = nameof(Bowler);
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} cannot be set with DidnotBat.");
                    results.Add(bowlerShouldntBeSet);
                }
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    var fielderShouldntBeSet = new ValidationResult();
                    fielderShouldntBeSet.IsValid = false;
                    fielderShouldntBeSet.PropertyName = nameof(Fielder);
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} cannot be set with DidnotBat.");
                    results.Add(fielderShouldntBeSet);
                }
            }

            if (MethodOut == BattingWicketLossType.Bowled || MethodOut == BattingWicketLossType.LBW || MethodOut == BattingWicketLossType.Caught || MethodOut == BattingWicketLossType.HitWicket)
            {
                if (PlayerName.IsNullOrEmpty(Bowler))
                {
                    var bowlerShouldBeSet = new ValidationResult();
                    bowlerShouldBeSet.IsValid = false;
                    bowlerShouldBeSet.PropertyName = nameof(Bowler);
                    bowlerShouldBeSet.AddMessage($"{nameof(Bowler)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            if (MethodOut == BattingWicketLossType.Caught || MethodOut == BattingWicketLossType.RunOut || MethodOut == BattingWicketLossType.Stumped)
            {
                if (PlayerName.IsNullOrEmpty(Fielder))
                {
                    var bowlerShouldBeSet = new ValidationResult();
                    bowlerShouldBeSet.IsValid = false;
                    bowlerShouldBeSet.PropertyName = nameof(Fielder);
                    bowlerShouldBeSet.AddMessage($"{nameof(Fielder)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            return results;
        }

        public BattingEntry(PlayerName name)
        {
            Name = name;
        }

        public BattingEntry()
        { }
    }
}
