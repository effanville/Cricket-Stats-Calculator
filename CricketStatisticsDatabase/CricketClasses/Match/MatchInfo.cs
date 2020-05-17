using StructureCommon.Extensions;
using StructureCommon.Validation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cricket.Match
{
    public class MatchInfo : IValidity
    {
        public override string ToString()
        {
            return Date.ToUkDateString() + "-" + Opposition;
        }

        private string fOpposition;
        public string Opposition
        {
            get
            {
                return fOpposition;
            }
            set
            {
                fOpposition = value;
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get
            {
                return date;
            }
            set
            {
                date = value;
            }
        }

        private string fPlace;
        public string Place
        {
            get
            {
                return fPlace;
            }
            set
            {
                fPlace = value;
            }
        }

        private MatchType fType;
        public MatchType Type
        {
            get
            {
                return fType;
            }
            set
            {
                fType = value;
            }
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
            var results = new List<ValidationResult>();
            results.AddIfNotNull(Validating.IsNotNullOrEmpty(Opposition, nameof(Opposition), ToString()));
            return results;
        }
    }
}
