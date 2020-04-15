using Cricket.Interfaces;
using Cricket.Match;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cricket
{
    public class CricketSeason : ICricketSeason
    {
        public override bool Equals(object obj)
        {
            if (obj is CricketSeason season)
            {
                return Name.Equals(season.Name) && Year.Equals(season.Year);
            }

            return false;
        }

        public bool SameSeason(DateTime year, string name)
        {
            return Year.Equals(year) && Name.Equals(name);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return Year.Year.ToString() + Name;
        }

        /// <inheritdoc/>
        public string Name
        { 
            get; 
            private set; 
        }

        /// <inheritdoc/>
        public DateTime Year
        { 
            get; 
            private set; 
        }

        /// <inheritdoc/>
        public List<ICricketPlayer> Players
        {
            get 
            {
                return new List<ICricketPlayer>();
            }
        }

        public List<CricketMatch> SeasonsMatches
        { 
            get; 
            private set; 
        }

        /// <inheritdoc/>
        public List<ICricketMatch> Matches
        {
            get 
            { 
                return SeasonsMatches.Select(match => (ICricketMatch)match).ToList(); 
            } 
        }

        /// <inheritdoc/>
        /// This is currently not implemented.
        public ICricketMatch GetMatch()
        {
            return new CricketMatch();
        }


        public CricketSeason()
        {
        }

        public CricketSeason(DateTime year, string name)
        {
            Name = name;
            Year = year;
        }
    }
}