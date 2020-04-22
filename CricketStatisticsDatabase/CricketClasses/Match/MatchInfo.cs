using System;

namespace Cricket.Match
{
    public class MatchInfo
    {
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
            get { return date; }
            set { date = value; }
        }

        private string fPlace;
        public string Place
        {
            get { return fPlace; }
            set { fPlace = value; }
        }

        private MatchType fType;
        public MatchType Type
        {
            get { return fType; }
            set { fType = value; }
        }

        public MatchInfo()
        { }

        public MatchInfo(string opposition, DateTime date, string place, MatchType matchType)
        {
            Opposition = opposition;
            Date = date;
            Place = place;
            Type = matchType;
        }
    }
}
