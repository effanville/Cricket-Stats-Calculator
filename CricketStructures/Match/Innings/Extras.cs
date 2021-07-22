using System.Collections.Generic;
using System.Linq;
using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match.Innings
{
    public sealed class Extras : IValidity
    {
        public int Byes
        {
            get;
            set;
        }

        public int LegByes
        {
            get;
            set;
        }

        public int Wides
        {
            get;
            set;
        }

        public int NoBalls
        {
            get;
            set;
        }

        public int Penalties
        {
            get;
            set;
        }

        public Extras()
        {
        }

        public Extras(int byes, int legByes, int wides, int noBalls, int penalties = 0)
        {
            Byes = byes;
            LegByes = legByes;
            Wides = wides;
            NoBalls = noBalls;
            Penalties = penalties;
        }

        public void SetExtras(int byes, int legByes, int wides, int noBalls, int penalties = 0)
        {
            Byes = byes;
            LegByes = legByes;
            Wides = wides;
            NoBalls = noBalls;
            Penalties = penalties;
        }

        public int Runs()
        {
            return Byes + LegByes + Wides + NoBalls + Penalties;
        }

        public int NonBowlerRuns()
        {
            return Byes + LegByes + Penalties;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            results.AddIfNotNull(Validating.NotNegative(Byes, nameof(Byes), ToString()));
            results.AddIfNotNull(Validating.NotNegative(LegByes, nameof(LegByes), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Wides, nameof(Wides), ToString()));
            results.AddIfNotNull(Validating.NotNegative(NoBalls, nameof(NoBalls), ToString()));
            results.AddIfNotNull(Validating.NotNegative(Penalties, nameof(Penalties), ToString()));
            return results;
        }
    }
}
