using System;

using CricketStructures.Player;

namespace CricketStructures.Statistics.Implementation.Player.Model
{
    public sealed class BattingAverageList
    {
        private int fNumberRuns;
        private int fNumberNotOut;
        private int fNumberInnings;
        public PlayerName Name
        {
            get;
            private set;
        }

        public DateTime Start
        {
            get;
            private set;
        }
        public DateTime End
        {
            get;
            private set;
        }
        public double Average => fNumberRuns / (fNumberInnings - (double)fNumberNotOut);

        public BattingAverageList(PlayerName name, DateTime firstDate, int numberRuns, bool notOut)
        {
            fNumberRuns = numberRuns;
            fNumberNotOut = notOut ? 1 : 0;
            fNumberInnings = 1;
            Name = name;
            Start = firstDate;
            End = firstDate;
        }

        public void UpdateValues(DateTime newDate, int numberRuns, bool notOut)
        {
            fNumberInnings++;
            fNumberRuns += numberRuns;
            if (notOut)
            {
                fNumberNotOut++;
            }
            End = End < newDate ? newDate : End;
            Start = Start > newDate ? newDate : Start;
        }
    }
}
