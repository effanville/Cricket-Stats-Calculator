using System;
using System.Collections.Generic;
using System.Linq;
using StructureCommon.Extensions;
using StructureCommon.Validation;

namespace Cricket.Match
{
    public class MatchInfo : IValidity
    {
        public override string ToString()
        {
            return Date.ToUkDateString() + "-" + Opposition + "-" + HomeOrAway;
        }

        public string Opposition
        {
            get;
            set;
        }

        public DateTime Date
        {
            get;
            set;
        }

        public string Place
        {
            get;
            set;
        }

        public Location HomeOrAway
        {
            get;
            set;
        }

        public MatchType Type
        {
            get;
            set;
        }

        public MatchInfo()
        {
        }

        public MatchInfo(string opposition, DateTime date, string place, MatchType matchType)
        {
            Opposition = opposition;
            Date = date;
            Place = place;
            Type = matchType;
        }

        public bool Validate()
        {
            return !Validation().Any(validation => !validation.IsValid);
        }

        public List<ValidationResult> Validation()
        {
            List<ValidationResult> results = new List<ValidationResult>();
            results.AddIfNotNull(Validating.IsNotNullOrEmpty(Opposition, nameof(Opposition), ToString()));
            return results;
        }
    }
}
