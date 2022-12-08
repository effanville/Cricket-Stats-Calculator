﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

using Common.Structure.Extensions;
using Common.Structure.Validation;

namespace CricketStructures.Match.Innings
{
    public sealed class Extras : IValidity, IEquatable<Extras>
    {
        [XmlAttribute(AttributeName = "B")]
        public int Byes
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "LB")]
        public int LegByes
        {
            get;
            set;
        }


        [XmlAttribute(AttributeName = "W")]
        public int Wides
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "NB")]
        public int NoBalls
        {
            get;
            set;
        }

        [XmlAttribute(AttributeName = "P")]
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

        public Extras Copy()
        {
            return new Extras(Byes, LegByes, Wides, NoBalls, Penalties);
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

        /// <inheritdoc/>
        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public bool Equals(Extras other)
        {
            return Byes.Equals(other.Byes)
                && LegByes.Equals(other.LegByes)
                && Wides.Equals(other.Wides)
                && NoBalls.Equals(other.NoBalls)
                && Penalties.Equals(other.Penalties);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Byes, LegByes, Wides, NoBalls, Penalties);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as Extras);
        }
    }
}
