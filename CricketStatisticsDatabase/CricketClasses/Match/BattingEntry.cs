using System.Collections.Generic;
using System.Linq;
using Cricket.Player;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace Cricket.Match
{
    /// <summary>
    /// Represents an entry on the batting part of a cricket scorecard.
    /// </summary>
    public class BattingEntry : IValidity
    {
        public override string ToString()
        {
            if (Name != null)
            {
                return "Batsman-" + Name.ToString();
            }

            return "Batsman: No Name";
        }

        /// <summary>
        /// At what point in the innings did this batsman bat.
        /// This is the number the batsman went into bat.
        /// </summary>
        public int Order
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the player on this batting entry.
        /// </summary>
        public PlayerName Name
        {
            get;
            set;
        }

        /// <summary>
        /// How this batsman was out.
        /// </summary>
        public Wicket MethodOut
        {
            get;
            set;
        }

        /// <summary>
        /// Any possible fielder associated with the wicket.
        /// This could (and should) be null if not needed.
        /// </summary>
        public PlayerName Fielder
        {
            get;
            set;
        }

        /// <summary>
        /// Any possible bowler associated with the wicket.
        /// This could (and should) be null if not needed.
        /// </summary>
        public PlayerName Bowler
        {
            get;
            set;
        }

        /// <summary>
        /// The runs the batsman scored.
        /// </summary>
        public int RunsScored
        {
            get;
            set;
        }

        /// <summary>
        /// The Wicket of the team this batsman fell at. 
        /// </summary>
        public int WicketFellAt
        {
            get;
            set;
        }

        /// <summary>
        /// The number of runs the team had scored when the batsman was out.
        /// </summary>
        public int TeamScoreAtWicket
        {
            get;
            set;
        }

        /// <summary>
        /// Was this batsman out.
        /// </summary>
        public bool Out()
        {
            return !(MethodOut == Wicket.NotOut || MethodOut == Wicket.DidNotBat);
        }

        /// <summary>
        /// Input the values into this batting entry.
        /// </summary>
        public void SetScores(Wicket howOut, int runs, int order, int wicketFellAt, int teamScoreAtWicket, PlayerName fielder = null, PlayerName bowler = null)
        {
            MethodOut = howOut;
            RunsScored = runs;
            Order = order;
            WicketFellAt = wicketFellAt;
            TeamScoreAtWicket = teamScoreAtWicket;
            Fielder = fielder;
            Bowler = bowler;
        }

        /// <summary>
        /// Returns whether there are any errors with this entry.
        /// </summary>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <summary>
        /// Returns the errors with this entry.
        /// </summary>
        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = Name.Validation();
            results.AddIfNotNull(Validating.NotNegative(RunsScored, nameof(RunsScored), ToString()));
            if (MethodOut == Wicket.DidNotBat)
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldntBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} cannot be set with DidnotBat.");
                    results.Add(bowlerShouldntBeSet);
                }
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult fielderShouldntBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} cannot be set with DidnotBat.");
                    results.Add(fielderShouldntBeSet);
                }
                results.AddIfNotNull(Validating.NotEqualTo(RunsScored, 0, nameof(RunsScored), ToString()));
            }

            if (MethodOut == Wicket.Bowled || MethodOut == Wicket.LBW || MethodOut == Wicket.Caught || MethodOut == Wicket.HitWicket || MethodOut == Wicket.Stumped)
            {
                if (PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Bowler)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            if (MethodOut == Wicket.Caught || MethodOut == Wicket.RunOut || MethodOut == Wicket.Stumped)
            {
                if (PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult bowlerShouldBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    bowlerShouldBeSet.AddMessage($"{nameof(Fielder)} should be set with {MethodOut}.");
                    results.Add(bowlerShouldBeSet);
                }
            }

            if (MethodOut == Wicket.RunOut || MethodOut == Wicket.NotOut)
            {
                if (!PlayerName.IsNullOrEmpty(Bowler))
                {
                    ValidationResult bowlerShouldntBeSet = new ValidationResult(false, nameof(Bowler), ToString());
                    bowlerShouldntBeSet.AddMessage($"{nameof(Bowler)} should not be set with {MethodOut}.");
                    results.Add(bowlerShouldntBeSet);
                }
            }

            if (MethodOut == Wicket.Bowled || MethodOut == Wicket.NotOut || MethodOut == Wicket.HitWicket || MethodOut == Wicket.LBW)
            {
                if (!PlayerName.IsNullOrEmpty(Fielder))
                {
                    ValidationResult fielderShouldntBeSet = new ValidationResult(false, nameof(Fielder), ToString());
                    fielderShouldntBeSet.AddMessage($"{nameof(Fielder)} should not be set with {MethodOut}.");
                    results.Add(fielderShouldntBeSet);
                }
            }

            return results;
        }

        public BattingEntry(PlayerName name)
        {
            Name = name;
        }

        public BattingEntry()
        {
        }
    }
}
